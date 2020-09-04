Code from https://www.nuget.org/packages/LambdaExpressionBuilder
The latest version 2.1.0-rc "supports" .NET Core, but has runtime bugs in the Contains and DoesNotContain classes.
The bugs were fixed in the public repository, but never released in a new nuget package.
So, running it locally for now, since there is no way to just override those two classes specifically.