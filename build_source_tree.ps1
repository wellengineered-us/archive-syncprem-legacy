#
#	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
#	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
#

cls
$psvt_git_commit_id = $PSVersionTable.GitCommitId

if ($psvt_git_commit_id -eq $null)
{ echo "An error occurred during the operation (GIT commit ID)."; return; }

echo "Using PowerShell: $psvt_git_commit_id"

$this_dir_path = [System.Environment]::CurrentDirectory

$using_vs = $false

$src_dir_name = "src"
$src_dir_path = "$this_dir_path\$src_dir_name"

$sln_file_name = $null # can be explicitly set here or $null for auto-discovery

$build_flavor = "debug"
$build_verbosity = "quiet"

echo "SOURCE DIR PATH: $src_dir_path"

if ($sln_file_name -eq $null)
{
	$sln_files = Get-ChildItem "$src_dir_path\*.sln" | Select-Object -First 1 -Property Name

	if ($sln_files -eq $null)
	{ echo "An error occurred during the operation (solution file discovery)."; return; }

	$sln_file_name = $sln_files[0].Name
}

$sln_file_path = "$src_dir_path\$sln_file_name"

echo "SOLUTION FILE NAME: $sln_file_name"
echo "SOLUTION FILE PATH: $sln_file_path"

if (-not $using_vs)
{
	$msbuild_dir_path = "C:\Program Files\dotnet"
	$msbuild_file_name = "dotnet.exe"
	$msbuild_command = "msbuild"
	$msbuild_exe = "$msbuild_dir_path\$msbuild_file_name"
}
else
{
	$msbuild_dir_path = "C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin"
	$msbuild_file_name = "msbuild.exe"
	$msbuild_command = ""
	$msbuild_exe = "$msbuild_dir_path\$msbuild_file_name"
}

echo "The operation is starting..."

&"$msbuild_exe" $msbuild_command /verbosity:$build_verbosity /consoleloggerparameters:ErrorsOnly "$sln_file_path" /t:clean /p:Configuration="$build_flavor"

if (!($LastExitCode -eq $null -or $LastExitCode -eq 0))
{ echo "An error occurred during the operation."; return; }

&"$msbuild_exe" $msbuild_command /verbosity:$build_verbosity /consoleloggerparameters:ErrorsOnly "$sln_file_path" /t:restore /p:Configuration="$build_flavor"

if (!($LastExitCode -eq $null -or $LastExitCode -eq 0))
{ echo "An error occurred during the operation."; return; }

&"$msbuild_exe" $msbuild_command /verbosity:$build_verbosity /consoleloggerparameters:ErrorsOnly "$sln_file_path" /t:build /p:Configuration="$build_flavor"

if (!($LastExitCode -eq $null -or $LastExitCode -eq 0))
{ echo "An error occurred during the operation."; return; }

echo "The operation completed successfully."