packages/OpenCover.4.6.166/tools/OpenCover.Console.exe -target:packages/xunit.runner.console.2.0.0/tools/xunit.console.x86.exe -targetargs:"Wikibase.Net.Tests/bin/Debug/Wikibase.Net.Tests.dll -noshadow" -register:user -filter:"-[Wikibase.Net.Test]* +[Wikibase.Net]*" -output:coverage.xml
REPO_COMMIT=$(git rev-parse HEAD)
REPO_BRANCH=$(git rev-parse --abbrev-ref HEAD)
REPO_COMMIT_AUTHOR=$(git show -s --pretty=format:"%cn")
REPO_COMMIT_AUTHOR_EMAIL=$(git show -s --pretty=format:"%ce")
REPO_COMMIT_MESSAGE=$(git show -s --pretty=format:"%s")
echo $REPO_COMMIT
echo $REPO_BRANCH
echo $REPO_COMMIT_AUTHOR
echo $REPO_COMMIT_AUTHOR_EMAIL
echo $REPO_COMMIT_MESSAGE
packages/coveralls.net.0.6.0/tools/csmacnz.Coveralls.exe --opencover -i coverage.xml --repoToken $1 --commitId $REPO_COMMIT --commitBranch $REPO_BRANCH --commitAuthor "$REPO_COMMIT_AUTHOR" --commitEmail "$REPO_COMMIT_AUTHOR_EMAIL" --commitMessage "$REPO_COMMIT_MESSAGE" --useRelativePaths
