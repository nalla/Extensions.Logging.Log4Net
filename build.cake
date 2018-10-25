/*
 * Load additional tools
 */
#tool xunit.runner.console&version=2.3.1
#tool GitVersion.CommandLine&version=3.6.5

/*
 * Commandline argument handling
 */
string target = Argument("target", "Default");
string configuration = Argument("configuration", "Release");

/*
 * Constants, initial variables
 */
string projectName = "Extensions.Logging.Log4Net";
FilePath project = $"./src/{projectName}/{projectName}.csproj";
DirectoryPath artifacts = "./artifacts";
GitVersion version = GitVersion();

/*
 * Helper: GetReleaseNotes
 *
 * Parses the git commits since the last tag to render some release notes
 * that will be taken into account when publishing the repository.
 */
string[] GetReleaseNotes()
{
	string tag = null;

	try
	{
		StartProcess("git", new ProcessSettings
		{
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			Silent = true,
			Arguments = "describe --tags --abbrev=0 head~"
		}, out IEnumerable<string> output);
		tag = output.First();
	} catch
	{
		// Ignore describe call. Maybe head~1 is not present.
	}

	string commitRange = !string.IsNullOrWhiteSpace(tag) ? $"{tag}..HEAD": null;

	StartProcess("git", new ProcessSettings
	{
		RedirectStandardOutput = true,
		Silent = true,
		Arguments = $"log {commitRange} --no-merges --format=\"- [%h] %s\""
	}, out IEnumerable<string> changes);

	if (changes.Any())
	{
		changes = changes.Select(x => x.Replace("\"", "\\\""));
	}

	return changes.ToArray();
}

/*
 * Task: Clean
 */
Task("Clean")
	.Does(() =>
{	
	CleanDirectories($"./src/**/bin/{configuration}");
	CleanDirectories("./src/**/obj");
	CleanDirectory(artifacts);
});

/*
 * Task: Build
 */
Task("Build")
	.IsDependentOn("Clean")
	.Does(() =>
{
	DotNetCoreBuild(project.FullPath, new DotNetCoreBuildSettings {
		Configuration = configuration
	});
});

/*
 * Task: Pack
 */
Task("Pack")
	.IsDependentOn("Build")
	.Does (() =>
{
	var nugetDirectory = artifacts.Combine("nuget");

	EnsureDirectoryExists(nugetDirectory);

	DotNetCorePack(project.FullPath, new DotNetCorePackSettings {
		Configuration = configuration,
		IncludeSymbols = true,
		NoBuild = true,
		OutputDirectory = nugetDirectory,
		ArgumentCustomization = args =>
		{
			return args
				.Append($"/p:PackageVersion={version.NuGetVersion}")
				.AppendQuoted("/p:PackageReleaseNotes=" + string.Join("\n", GetReleaseNotes()));
		}
	});
});

/*
 * Task: Default
 */
Task("Default").IsDependentOn("Pack");

/*
 * Script Execution
 */
RunTarget(target);
