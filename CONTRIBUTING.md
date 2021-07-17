# Contributing

## Introduction
Please follow the guidelines in this document if you want to contribute to Pamaxie<br>
Please remember that to contribute to our source code you will also have to follow our [code of conduct](https://github.com/pamaxie/Pamaxie/blob/main/CODE_OF_CONDUCT.md)!

## How To Contribute
TO BE ADDED

## Guidelines
Only work on issues that you know you can fix or are willing to do the research to fix.

Only work on problems that are documented. If you find a new issue create an issue first. Core contributors will validate the issue and then say if it's worth working on and if it should be worked on at all.

Planning is key. If a issue is flagged as a "Complex Problem" it usually requires planning and we expect contributors to have done that planning and explain their train of thought as a comment in the issue. We will then agree or disagree with the implementation. After that, a contribution can be done.

When working on a issue, create a branch for the issue with the format ``dev/IssueNumber/SumUp``<br>
Example:
```
dev/2/Test_For_API
```

### Formatting
TO BE ADDED

### Commit Messages
Pamaxie uses a consistent structure for commit messages with the following pattern:
```
#ISSUE Sum_Up_Of_Current_Change
```

#### Example
```
#1 Fixed Bug in AuthController.cs where a null reference was not checked for.
#2 Implemented Unit Tests for Pamaxie.API.
#3 Improved run-time for method DownloadFile in ImageProcessing.cs.
```