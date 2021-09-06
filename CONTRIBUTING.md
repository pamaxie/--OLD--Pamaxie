# Contributing


## Introduction
Please follow the guidelines in this document if you want to contribute to Pamaxie<br>
Please remember that to contribute to our source code you will also have to follow our [code of conduct](https://github.com/pamaxie/Pamaxie/blob/main/CODE_OF_CONDUCT.md)!


## Guidelines
Only work on issues that you know you can fix or are willing to do the research to fix.

Only work on problems that are documented. If you find a new issue create an issue first. Core contributors will validate the issue and then say if it's worth working on and if it should be worked on at all.

Planning is key. If a issue is flagged as a "Complex Problem" it usually requires planning and we expect contributors to have done that planning and explain their train of thought as a comment in the issue. We will then agree or disagree with the implementation. After that, a contribution can be done.

When working on a issue, create a branch for the issue with the format:
```
dev/issue/summary

Example:
dev/2/test_for_api
dev/5/website_performance
```

### Code Style
Normal .NET coding guidelines apply. See the [Framework Design Guidelines](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/) for more information.

### Documentation
All work needs to be documented. This includes the Wiki and in the code.

The wiki needs to be updated after new changes have been made and a pull request have been accepted.

Everything with a Public or Internal Access Modifier needs to have a summary explaining what they do, including the parameter or return value.
``` c#
Example:
/// <summary>
/// description
/// </summary>
/// <param name="name">description</param>
/// <returns>description</returns>
public string Method(string name) { }
```

### Unit Tests
Make sure to run all unit tests before creating a pull request. Any new code should also have reasonable unit test coverage.


## How To Contribute

### Preparation
- Through the GitHub discussions, you talk about a enhancement or a bug you would like to see/fixed, and why it should be in Pamaxie.
  - If approved through the GitHub discussions, ensure an accompanying GitHub issue is created with information and a link back to the discussion. 
- Once the issue have been approved, you can start working on it.
- If you see a issue without any assignee(s), then you can assign yourself, as long as you have the research needed, or are willingly to gather it before working on the issue. Gathered information will be discussed in the issue and once approved, you can start.

### Set up your environment
- You create, or update, a fork of pamaxie/Pamaxie under your GitHub account.
- Create a branch from dev, named relating to your issue. (See guideline for more information)
  
### Working on the issue.
- Please also observe the following:
  - No reformatting.
  - No changing files that are not specific to the issue.
- Test your changes.
- Document your work. (See guideline)

### Prepare commits
Pamaxie uses a consistent structure for commit messages with the following pattern:
```
#ISSUE Summary

Example:
#1 Fixed Bug in AuthController.cs where a null reference was not checked for.
#2 Implemented Unit Tests for Pamaxie.API.
#3 Improved run-time for the method DownloadFile in ImageProcessing.cs.
```

#### Before commiting:
- Should include new or changed tests relevant to the changes you are making.
- Everything should follow the Code Style. (See guideline)
- No unnecessary whitespace. Check for whitespace with ``git diff --check`` and ``git diff --cached --check`` before commit.
- Double check, to make sure everything is right.

### Submit pull request
#### Prerequisites:
- You are making commits in a issue branch.
- All code should compile without errors or warnings.
- All tests should be passing.

#### Submitting Pull Request:
- Once you feel it is ready, submit the pull request to the pamaxie/Pamaxie repository against the dev branch unless specifically requested to submit it against another branch.
  - In the case of a larger change that is going to require more discussion, please submit a pull request sooner. Waiting until you are ready may mean more changes than you are interested in if the changes are taking things in a direction the maintainers do not want to go.
- In the pull request, outline what you did and point to specific conversations, with a url, and issues that you are resolving. This is a tremendous help for the evaluation and acceptance.
- Once a pull request is in, do not delete the branch or close the pull request.
- One of the core Pamaxie team members will evaluate it within a reasonable time period. Some things get evaluated faster or fast tracked. We are human and we have active lives outside of open source so don't fret if you haven't seen any activity on your pull request within a month or two. We don't have a Service Level Agreement (SLA) for pull requests. Just know that we will evaluate your pull request.
- If issue is related to anything documented in the Wiki, then include snippet of changes that will be made. If the pull request is accepted, then please add your changes to the wiki.

### Respond to feedback on pull request
There may be feedback for you to fix or change. These changes must be pushed against the same branch, the pull request will automatically get updated.

If comments or questions have not received any response, it will eventually mean the pull request will be closed and not accepted. This does not mean that your contribution is not appreciated, just that things go stale. If you want to pick it back up, feel free to address the concerns, questions or feedback and reopen the pull request referencing the old one.

Sometimes there is a need for you to rebase your commit against the latest code before it can get reviewed further.

The only reason a pull request should be closed and resubmitted, is when a pull request is targeting the wrong branch.

### Code Style
We follow the Google code style guidelines for developing our source code. This means if someone wants to contribute to our codebase we would like if they adhere to the rules too. These rules can be found [here](https://google.github.io/styleguide/). If something is not defined in the code guidelines by google (usually most scenarios should be) or something is unclear please open an issue with "question" as a tag. We will then try to help that person as soon as possible. If there is any deviations from the guidelines we will note them here as well as in our wiki (see: contribution guide)

## Other general information
If you reformat code or hit core functionality without an approval from a person on the Pamaxie Team, it's likely that no matter how awesome it looks afterwards, it will probably not get accepted. Reformatting code makes it harder for the Pamaxie team to evaluate exactly what was changed.

If you do these things, it will be make evaluation and acceptance easy. Now if you stray outside of the guidelines above, it doesn't mean that your pull request will be ignored. It will just make things harder. Harder roughly translates to a longer SLA for your pull request.




