#Requires -Modules platyPS

function New-AbMarkdownHelp
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String]$HelpFolderPath
    )

    $HelpFolder = Get-Item $HelpFolderPath
    $ModuleFolder = $HelpFolder.Parent.Parent
    $ModuleFolderPath = "$($ModuleFolder.FullName)\src\PowerShell\"

    $NewHelpFolderPath = "$ModuleFolderPath\temp_help"
    $psd1 = Get-ChildItem $ModuleFolderPath | Where-Object { $_.Name -eq "Ab.psd1" }

    Import-Module $psd1.FullName -Scope Global

    New-Item -Path $NewHelpFolderPath -ItemType Directory -Force | Out-Null
    Copy-Item -Path "$HelpFolderPath\*" -Destination $NewHelpFolderPath

    Update-MarkdownHelpModule -Path $NewHelpFolderPath -AlphabeticParamsOrder -RefreshModulePage -UpdateInputOutput -UseFullTypeName | Out-Null

    $errors = Compare-AbMarkdownHelp $HelpFolderPath $NewHelpFolderPath

    if ($errors.Length -gt 0)
    {
        $errorMessage = @()
        $errorMessage += "ERROR: The following files have not been updated with the latest module changes.`n"
        $errorMessage += "Please make sure to run Update-MarkdownHelpModule to update the markdown files.`n"
        $errors | ForEach-Object { $errorMessage += "- $_`n" }
        throw $errorMessage
    }

    Remove-Item $NewHelpFolderPath -Recurse
}

function Compare-AbMarkdownHelp
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String]$OldHelpFolderPath,
        [Parameter(Mandatory = $true, Position = 1)]
        [String]$NewHelpFolderPath
    )

    $comparatorInstance = New-Object MarkdownComparator.Comparator
    $OldHelpFolder = Get-Item $OldHelpFolderPath
    $errors = @()

    foreach ($oldFile in Get-ChildItem $OldHelpFolder)
    {
        $newFilePath = "$NewHelpFolderPath\$($oldFile.Name)"
        if ($oldFile.Name -notlike "*-*")
        {
            continue;
        }

        try
        {
            $result = $comparatorInstance.Compare($oldFile.FullName, $newFilePath)
            if ($result -ne 0)
            {
                $errors += $oldFile.Name
            }
        }
        catch
        {
            $_.Exception.InnerException
        }
    }

    return $errors
}

function Test-AbMarkdownHelp
{
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, Position = 0)]
        [String]$HelpFolderPath,
        [Parameter(Mandatory = $true, Position = 1)]
        [String]$SuppressedExceptionsPath,
        [Parameter(Mandatory = $true, Position = 2)]
        [String]$NewExceptionsPath
    )

    PROCESS
    {
        $HelpFolder = Get-Item $HelpFolderPath
        $Exceptions = Import-Csv "$SuppressedExceptionsPath\ValidateHelpIssues.csv"
        [String[]]$errors = @()
        $MarkdownFiles = Get-ChildItem -Path $HelpFolder
        $ModuleName = ($MarkdownFiles | Where-Object { $_.Name -notlike "*-*" }).Name -replace ".md",""
        
        foreach ($file in $MarkdownFiles)
        {
            # Ignore the module page
            if ($file.Name -notlike "*-*")
            {
                continue
            }

            $CmdletName = $file.BaseName

            $fileErrors = @()
            $content = Get-Content $file.FullName
            for ($idx = 0; $idx -lt $content.Length; $idx++)
            {
                switch -Regex ($content[$idx])
                {
                    "## SYNOPSIS"
                    {
                        $foundSynopsis = $False
                        $idx++

                        # Check each line between SYNOPSIS and SYNTAX for any text
                        for (;;)
                        {
                            $foundSynopsis = $foundSynopsis -or (!([string]::IsNullOrWhiteSpace("$($content[$idx])")))
                            if ($content[$idx+1] -notcontains "## SYNTAX")
                            {
                                $idx++
                                if ($idx -ge $content.Length)
                                {
                                    Write-Error "Could not find SYNTAX header in file $($file.Name)"
                                    return
                                }
                            }
                            elseif ($content[$idx] -contains "{{Fill in the Synopsis}}")
                            {
                                $foundSynopsis = $false
                                break
                            }
                            else
                            {
                                break
                            }
                        }

                        if (!($foundSynopsis))
                        {
                            $fileErrors += "No synopsis found"
                        }
                    }
                    "## DESCRIPTION"
                    {
                        $foundDescription = $False
                        $idx++

                        # Check each line between DESCRIPTION and EXAMPLES for any text
                        for (;;)
                        {
                            $foundDescription = $foundDescription -or (!([string]::IsNullOrWhiteSpace("$($content[$idx])")))
                            if ($content[$idx+1] -notcontains "## EXAMPLES")
                            {
                                $idx++
                                if ($idx -ge $content.Length)
                                {
                                    Write-Error "Could not find EXAMPLES header in file $($file.Name)"
                                    return
                                }
                            }
                            elseif ($content[$idx] -contains "{{Fill in the Description}}")
                            {
                                $foundDescription = $false
                                break
                            }
                            else
                            {
                                break
                            }
                        }

                        if (!($foundDescription))
                        {
                            $fileErrors += "No description found"
                        }
                    }
                    "## EXAMPLES"
                    {
                        # Move the index to the start of the PowerShell code
                        while ($content[$idx] -notcontains "``````" -and $content[$idx] -notcontains "``````powershell")
                        {
                            $idx++
                            if ($idx -ge $content.Length)
                            {
                                Write-Error "Could not find start of PowerShell example in file $($file.Name)"
                                return
                            }
                        }

                        # Check for the platyPS example template
                        #
                        # ```
                        # PS C:\> {{ Add example code here }}
                        # ```
                        #
                        if ($content[$idx+1] -contains "PS C:\> {{ Add example code here }}")
                        {
                            $fileErrors += "No examples found"
                        }

                        # Check for other missing example formats (such as empty)
                        #
                        # ```
                        #
                        # ```
                        #
                        if ([string]::IsNullOrWhiteSpace("$($content[$idx+1])"))
                        {
                            $fileErrors += "No examples found"
                        }
                    }
                    "@{Text=}"
                    {
                        # This case occurs when there is no description provided for a parameter
                        $parameter = $content[$idx-1].Substring(5)
                        $fileErrors += "No description found for parameter $parameter"
                    }
                    ".``````yaml"
                    {
                        $parameter = $content[$idx-1].Substring(5)
                        $fileErrors += "Trailing yaml string found in description for parameter $parameter. Please move the trailing yaml string to a line by itself."
                    }
                    "online version:"
                    {
                       $onlineString = "https://github.com/automationbrew/autobrew-powershell/blob/main/docs/help/$($CmdletName.ToLower()).md"
                       $split = $content[$idx] -split "online version:"
                       if ([string]::IsNullOrWhiteSpace($split[1]) -or $split[1] -notlike "*$onlineString*")
                       {
                           $fileErrors += "Online version in the header of the file is incorrect. The corresponding URL should be the following: $onlineString"
                       }
                    }
                    default
                    {

                    }
                }
            }

            # If the markdown file had any missing help, add them to the list to be printed later
            if ($fileErrors.Count -gt 0)
            {
                $fileExceptions = $Exceptions | Where-Object { $_.Target -eq "$($file.Name)" }
                $fileErrors | ForEach-Object {
                    $exception = $_

                    if($null -eq ($fileExceptions | Where-Object { $_.Description -eq "$error" }))
                    {
                        $errors += "$($file.name),$($exception)"
                    }
                }
            }
        }

        # If there were any errors recorded, print them out and throw
        if ($errors.Count -gt 0)
        {
            $errors | ForEach-Object { Add-Content "$NewExceptionsPath\ValidateHelpIssues.csv" $_ }
        }
    }
}

function New-AbMamlHelp
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String]$HelpFolderPath,
        [Parameter(Mandatory = $true, Position = 1)]
        [String]$OutputPath
    )

    # Generate the MAML help from the markdown files
    New-ExternalHelp -Path $HelpFolderPath -OutputPath $OutputPath -Force
    $dir = Get-ChildItem $OutputPath
    $MAML = $dir | Where-Object { $_.FullName -like "*.dll-Help.xml" }

    # Modify the MAML (add spaces between examples)
    $MAML | ForEach-Object { Edit-AbMamlHelp $_ }
}

function Edit-AbMamlHelp
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [System.IO.FileInfo]$MAML
    )

    $content = Get-Content $MAML

    # Keep track of the number of examples we find so we can add enough space in the new array
    $exampleCount = 0
    for ($idx = 0; $idx -lt $content.Length; $idx++)
    {
        if ($content[$idx] -like "*<command:example>*")
        {
            $exampleCount++
        }
    }

    # Since we will be adding two <maml:para></maml:para> tags to the MAML, we need to adjust the size of the array
    $newContentLength = $content.Length + (2 * $exampleCount)
    $newContent = New-Object string[] $newContentLength
    $buffer = 0

    for ($idx = 0; $idx -lt $content.Length; $idx++)
    {
        $newContent[$idx + $buffer] = $content[$idx]

        # If the next line is going to be the end of a remark in an example, add the two new lines for spacing
        if ($content[$idx+1] -like "*</dev:remarks>*")
        {
            $newContent[$idx + $buffer + 1] = "<maml:para></maml:para>"
            $newContent[$idx + $buffer + 2] = "<maml:para></maml:para>"

            $buffer += 2
        }
    }

    # Replace the contents of the current MAML with the new contents
    $result = $newContent -join "`r`n"
    $tempFile = Get-Item $MAML
    [System.IO.File]::WriteAllText($tempFile.FullName, $result)

    # Check to ensure that the MAML file is still valid XML
    [Reflection.Assembly]::LoadWithPartialName("System.Xml.Linq") | Out-Null
    [System.Xml.Linq.XDocument]::Load($tempFile.FullName) | Out-Null
}

# ------------
# Start
# ------------

Import-Module -Name platyPS -Scope Global