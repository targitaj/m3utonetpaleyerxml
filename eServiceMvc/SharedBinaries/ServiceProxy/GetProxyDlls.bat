call "%VS90COMNTOOLS%vsvars32.bat"

cd proxy

copy ..\..\AutoMapper\AutoMapper.dll
copy ..\..\Migri.Uma.Data\Migri.Uma.Data.BaseRegistry.dll
copy ..\..\Migri.Uma.Data\Migri.Uma.Data.Repository.dll
copy ..\..\Migri.Uma.Data\Migri.Uma.Data.CaseManagement.dll
copy ..\..\Migri.Uma.Data\Migri.Uma.Data.DataContracts.Registry.dll
copy ..\..\Migri.Uma.Data\Migri.Uma.Data.DataContracts.Enums.dll
copy ..\..\Migri.Uma.Data\Migri.Uma.Data.DataContracts.Eservice.dll
copy ..\..\..\UMAServices\Source\Migri.Uma.Services.DataContracts.Measure\bin\Debug\Migri.Uma.Services.DataContracts.Measure.dll
copy ..\..\..\UMAServices\Source\Migri.Uma.Services.DataContracts\bin\Debug\Migri.Uma.Services.DataContracts.dll

copy ..\..\..\UMAServices\Source\Migri.Uma.Services.Measures\bin\Debug\Migri.Uma.Services.Measures.dll
copy ..\..\..\UMAServices\Source\Migri.Uma.Services.RegistryLogic\bin\Debug\Migri.Uma.Services.RegistryLogic.dll
copy ..\..\..\UMAServices\Source\Migri.Uma.Services.Common\bin\Debug\Migri.Uma.Services.Common.dll
copy ..\IntegraatioProxy\UmaIntegrationProxy.dll


del Migri.Uma.Services.RegistryLogic.dll
del Migri.Uma.Services.BaseRegistry.dll
del Migri.Uma.Services.Common.dll
del AutoMapper.dll
del Migri.Uma.Data.BaseRegistry.dll
del Migri.Uma.Data.Repository.dll
del Migri.Uma.Data.CaseManagement.dll


REM del *.cs

dir *.*

cd..

pause
