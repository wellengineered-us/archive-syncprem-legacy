@echo off

REM
REM	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
REM	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
REM

CALL set_ps_env.bat

REM Required for successful test coverage execution under .NET Core
SET COMPLUS_ReadyToRun=0
SET COREHOST_TRACE=0
SET DOTNET_CLI_CAPTURE_TIMING=0

"%POWERSHELL_CORE_EXE_PATH%" -command .\run_unit_test_coverage
PAUSE > NUL