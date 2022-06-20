# Contributing to Automation Brew PowerShell

This project contains PowerShell cmdlets for administrators and developers to automate different scenarios such as data generation and testing.

## Code of conduct

Help us keep this project open and inclusive. Please read and follow our [code of conduct](CODE_OF_CONDUCT.md).

## GitHub Basics

### GitHub Workflow

If you don't have much experience using GitHub or Git, [here is a guide to understanding the GitHub flow](https://guides.github.com/introduction/flow/) and [here is a guide to understanding the basic Git commands](https://education.github.com/git-cheat-sheet-education.pdf).

### Forking the autobrew-powershell repository

Unless you are working with multiple contributors on the same file, we ask that you fork the repository and submit your pull request from there. [Here is a guide to forks in GitHub](https://guides.github.com/activities/forking/).

## Filing issues

You can find all of the issues that have been filed in the [issues](https://github.com/automationbrew/autobrew-powershell/issues) section of the repository.

To file an issue, first select one of the [provided templates](https://github.com/automationbrew/autobrew-powershell/issues/new/choose) to ensure that the proper information is provided. The following are a few of the templates we have:

- [Documentation bug](https://github.com/automationbrew/autobrew-powershell/issues/new?assignees=&labels=needs-triage&template=DOC_BUG.yml&title=%5BDoc%5D%3A+)
- [Feature request](https://github.com/automationbrew/autobrew-powershell/issues/new?assignees=&labels=feature-request%2Cneeds-triage&template=FEATURE_REQUEST.yml&title=%5BFeature%5D%3A+)
- [Module bug](https://github.com/automationbrew/autobrew-powershell/issues/new?assignees=&labels=needs-triage%2Cbug&template=BUG_REPORT.yml)

## Submitting changes

### Pull requests

You can find all of the pull requests that have been opened in the [pull requests](https://github.com/automationbrew/autobrew-powershell/pulls) section of the repository.

When creating a pull request, keep the following in mind:

- Make sure you are pointing to the fork and branch that your changes were made in
- Choose the correct branch you want your pull request to be merged into
  - Deleting or ignoring the template will elongate the time it takes for your pull request to be reviewed.
  - The **main** branch is for active development; changes in this branch will be in the next Automation Brew PowerShell release.

### Pull request guidelines

A pull request template will automatically be included as a part of your pull request. Please fill out the checklist as specified. Pull requests **will not be reviewed** unless they include a properly completed checklist.

The following is a list of guidelines that pull requests opened in the Automation Brew PowerShell repository must adhere to. You can find a more complete discussion of the design guidelines [here](docs/design-guidelines.md).

#### General guidelines

The following guidelines must be followed in **EVERY** pull request that is opened.

- Title of the pull request is clear and informative
- The [change log](CHANGELOG.md) file has been updated:
  - A snippet outlining the change(s) made in the pull request should be written under the `## Upcoming release` header -- no new version header should be added
- There are a [small number of commits](docs/cleaning-commits.md) that each have an informative message

#### Testing guidelines

The following guidelines must be followed in **EVERY** pull request that is opened.

- Changes made have corresponding test coverage.
- No existing tests should be skipped
- Tests should have proper setup of resources to ensure any user can re-record the test if necessary
- Tests should not include any hardcoded values.
