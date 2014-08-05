call "%VS100COMNTOOLS%vsvars32.bat"

cd proxy

del *.*

svcutil /config:UmaAsyncTasks.config /o:UmaAsyncTasksProxy.cs /n:*,UmaAsyncTasks /ct:System.Collections.Generic.List`1 http://localhost/AsyncTasksService/Service.svc/mex
c:\windows\microsoft.net\framework\v2.0.50727\csc /target:library /out:UmaAsyncTasksProxy.dll /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.runtime.serialization.dll" /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.runtime.serialization.dll" /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.servicemodel.dll" UmaAsyncTasksProxy.cs

del UmaAsyncTasksProxy.cs


svcutil /config:AffectoScan.config /o:AffectoScanServiceProxy.cs /n:*,AffectoScanService /ct:System.Collections.Generic.List`1 http://scan01/ADCPool/PdfTransferService/DCPdfServiceReference.asmx
c:\windows\microsoft.net\framework\v2.0.50727\csc /target:library /out:AffectoScanServiceProxy.dll /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.runtime.serialization.dll" /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.runtime.serialization.dll" /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.servicemodel.dll" AffectoScanServiceProxy.cs

del AffectoScannServiceProxy.cs 

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

svcutil /config:UmaServices.config /o:CommonServicesProxy.cs /n:*,UmaServices /async /r:Migri.Uma.Data.DataContracts.Registry.dll /r:Migri.Uma.Services.DataContracts.dll /r:Migri.Uma.Services.Measures.dll /ct:System.Collections.Generic.List`1 http://localhost/commonservices/service.svc/mex

svcutil /mergeConfig /config:UmaServices.config /o:CustomerRegistryProxy.cs /n:*,UmaServices /async /r:Migri.Uma.Data.DataContracts.Registry.dll /r:Migri.Uma.Data.DataContracts.Enums.dll /r:Migri.Uma.Data.DataContracts.Eservice.dll /r:Migri.Uma.Services.DataContracts.Measure.dll /r:Migri.Uma.Services.DataContracts.dll /ct:System.Collections.Generic.List`1 http://localhost/customerregistry/service.svc/mex

svcutil /mergeConfig /config:UmaServices.config /o:CaseManagementProxy.cs /n:*,UmaServices /async /r:Migri.Uma.Data.DataContracts.Registry.dll /r:Migri.Uma.Data.DataContracts.Enums.dll  /r:Migri.Uma.Data.DataContracts.Eservice.dll /r:Migri.Uma.Services.DataContracts.Measure.dll /r:Migri.Uma.Services.DataContracts.dll /r:UmaIntegrationProxy.dll /ct:System.Collections.Generic.List`1 http://localhost/casemanagement/service.svc/mex

svcutil /mergeConfig /config:UmaServices.config /o:BaseRegistryProxy.cs /n:*,UmaServices /async /r:Migri.Uma.Data.DataContracts.Registry.dll /r:Migri.Uma.Data.DataContracts.Enums.dll  /r:Migri.Uma.Data.DataContracts.Eservice.dll /r:Migri.Uma.Services.DataContracts.Measure.dll /r:Migri.Uma.Services.DataContracts.dll /ct:System.Collections.Generic.List`1 http://localhost/baseregistry/service.svc/mex

svcutil /mergeConfig /config:UmaServices.config /o:FileServiceProxy.cs /n:*,UmaServices /async /r:Migri.Uma.Data.DataContracts.Registry.dll /r:Migri.Uma.Data.DataContracts.Enums.dll  /r:Migri.Uma.Data.DataContracts.Eservice.dll /r:Migri.Uma.Services.DataContracts.Measure.dll /r:Migri.Uma.Services.DataContracts.dll /ct:System.Collections.Generic.List`1 http://localhost/fileservice/service.svc/mex

svcutil /mergeConfig /config:UmaServices.config /o:MaintenanceToolProxy.cs /n:*,UmaServices /async /r:Migri.Uma.Data.DataContracts.Registry.dll /r:Migri.Uma.Data.DataContracts.Enums.dll /r:Migri.Uma.Data.DataContracts.Eservice.dll /r:Migri.Uma.Services.DataContracts.Measure.dll /r:Migri.Uma.Services.DataContracts.dll /ct:System.Collections.Generic.List`1 http://localhost/maintenanceservice/service.svc/mex

c:\windows\microsoft.net\framework\v2.0.50727\csc /target:library /out:UmaServicesProxy.dll /reference:Migri.Uma.Data.DataContracts.Registry.dll /reference:Migri.Uma.Data.DataContracts.Enums.dll  /r:Migri.Uma.Data.DataContracts.Eservice.dll /reference:Migri.Uma.Services.DataContracts.Measure.dll /reference:Migri.Uma.Services.DataContracts.dll /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.runtime.serialization.dll" /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.servicemodel.dll" *.cs


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
