[CmdletBinding()]
Param(
    [Parameter(Position = 0, Mandatory = $false, ValueFromRemainingArguments = $true)]
    [string[]]$ScriptArgs
)

# Get the script root folder.
if (!$PSScriptRoot) {
    $PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent
}

# Create the tools folder.
$Tools = Join-Path $PSScriptRoot "tools"
if (!(Test-Path $Tools)) {
    New-Item -Path $Tools -ItemType Directory | Out-Null
}

# Install Cake
&dotnet "tool" "install" "Cake.Tool" "--version=0.30.0" "--tool-path=$Tools"

# Execute Cake
&"$Tools\dotnet-cake" $ScriptArgs

# Return the exit code from Cake.
exit $LASTEXITCODE;
