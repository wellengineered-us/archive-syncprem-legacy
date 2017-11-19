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

$src_dir_path = "$this_dir_path\src"
$doc_dir_path = "$this_dir_path\doc"
$imports_dir_path = "$this_dir_path\imports"
$output_dir_path = "$this_dir_path\output"
$pkg_dir_path = "$this_dir_path\pkg"
$templates_dir_path = "$this_dir_path\templates"
$tools_dir_path = "$this_dir_path\tools"

$sln_file_name = "TextMetal.sln"
$sln_file_path = "$src_dir_path\$sln_file_name"

$visual_studio_internal_version = "14.0"
$msbuild_dir_path = "C:\Program Files (x86)\MSBuild\$visual_studio_internal_version\bin\amd64"
$msbuild_file_name = "msbuild.exe"
$msbuild_exe = "$msbuild_dir_path\$msbuild_file_name"

$build_flavor = "debug"
$build_verbosity = "quiet"

$robocopy_exe = "robocopy.exe"
$artifacts_dir_path = "$src_dir_path\artifacts"

echo "The operation is starting..."

if ((Test-Path -Path $pkg_dir_path))
{
	Remove-Item $pkg_dir_path -recurse -force
}

New-Item -ItemType directory -Path $pkg_dir_path

Copy-Item "$this_dir_path\trunk.bat" "$pkg_dir_path\."
Copy-Item "$doc_dir_path\LICENSE.txt" "$pkg_dir_path\."

$argz = @("$templates_dir_path",
	"$pkg_dir_path\templates",
	"/MIR",
	"/e",
	"/xd", "*!git*", "output",
	"/xf", "*!git*")

&"$robocopy_exe" $argz

if (!($LastExitCode -eq $null -or $LastExitCode -eq 1))
{ echo "An error occurred during the operation."; return; }

$argz = @("$artifacts_dir_path\bin",
	"$pkg_dir_path\packages",
	"/MIR",
	"/e",
	"/xd", "*!git*", "output",
	"/xf", "*!git*")

&"$robocopy_exe" $argz

if (!($LastExitCode -eq $null -or $LastExitCode -eq 1))
{ echo "An error occurred during the operation."; return; }

echo "The operation completed successfully."