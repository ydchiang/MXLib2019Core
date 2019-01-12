@echo off
REM ========================================================
REM = MXLib2019Core Nunit Test command line                =
REM = Author: YD Chiang                                    =
REM = Date  : 2019/01/12                                   =
REM =                                                      =
REM ========================================================

set ReportGenerator=ReportGenerator-4.0.2
set LibPath=.\MXLib2019CoreTest
Set Toolkits=.\Toolkits

if not exist %LibPath%\MakeReport\History mkdir %LibPath%\MakeReport\History

dotnet test --logger "trx;LogFileName=%LibPath%\MakeReport\TestResult.trx" --no-build /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=%LibPath%\MakeReport\MXLib2019Core_CodeCoverage.xml
%Toolkits%\%ReportGenerator%\ReportGenerator.exe "-reports:%LibPath%\MakeReport\MXLib2019Core_CodeCoverage.xml" "-targetdir:%LibPath%\MakeReport" "-historydir:%LibPath%\MakeReport\History" -reporttypes:HTML;TextSummary -verbosity:Off
