@ECHO OFF

set OLDDIR=%CD%

copy "..\..\UmaDA\Source\Migri.Uma.EnterpriseLibrary.Data\bin\Debug\Migri.Uma.EnterpriseLibrary.Data.dll"
copy "..\..\UmaDA\Source\Migri.Uma.Data.BaseRegistry\bin\Debug\Migri.Uma.Data.BaseRegistry.dll"
copy "..\..\UmaDA\Source\Migri.Uma.Data.CaseManagement\bin\Debug\Migri.Uma.Data.CaseManagement.dll"
copy "..\..\UmaDA\Source\Migri.Uma.Data.CustomerRegistry\bin\Debug\Migri.Uma.Data.CustomerRegistry.dll"
copy "..\..\UmaDA\Source\Migri.Uma.Data.DataContracts.Enums\bin\Debug\Migri.Uma.Data.DataContracts.Enums.dll"
copy "..\..\UmaDA\Source\Migri.Uma.Data.DataContracts.Eservice\bin\Debug\Migri.Uma.Data.DataContracts.Eservice.dll"
copy "..\..\UmaDA\Source\Migri.Uma.Data.DataContracts.Registry\bin\Debug\Migri.Uma.Data.DataContracts.Registry.dll"
copy "..\..\UmaDA\Source\Migri.Uma.Data.EserviceRegistry\bin\Debug\Migri.Uma.Data.EserviceRegistry.dll"
copy "..\..\UmaDA\Source\Migri.Uma.Data.Repository\bin\Debug\Migri.Uma.Data.Repository.dll"

cd %OLDDIR%

pause
