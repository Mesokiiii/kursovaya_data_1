@echo off
echo ========================================
echo Запуск тестов с покрытием кода
echo ========================================
echo.

echo [1/3] Запуск тестов...
dotnet test FootballLeague.Tests/FootballLeague.Tests.csproj --collect:"XPlat Code Coverage" --results-directory ./TestResults

echo.
echo [2/3] Генерация HTML отчета...
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:"TestResults/CoverageReport" -reporttypes:Html

echo.
echo [3/3] Открытие отчета в браузере...
start TestResults/CoverageReport/index.html

echo.
echo ========================================
echo Готово! Отчет открыт в браузере.
echo ========================================
pause
