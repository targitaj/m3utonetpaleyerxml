call "%VS100COMNTOOLS%vsvars32.bat"

cd ErrorLoggingProxy

del *.*

copy ..\..\AutoMapper\AutoMapper.dll
copy ..\..\Migri.Uma.Data\Migri.Uma.Data.DataContracts.Registry.dll
copy ..\..\Migri.Uma.Data\Migri.Uma.Data.DataContracts.Enums.dll

svcutil /config:ErrorLogging.config /o:ErrorLoggingProxy.cs /n:*,UmaErrorLogging /r:Migri.Uma.Data.DataContracts.Registry.dll /r:Migri.Uma.Data.DataContracts.Enums.dll /ct:System.Collections.Generic.List`1 net.tcp://localhost/ErrorLoggingService/service.svc/mex

c:\windows\microsoft.net\framework\v2.0.50727\csc /target:library /out:ErrorLoggingProxy.dll /r:Migri.Uma.Data.DataContracts.Registry.dll /r:Migri.Uma.Data.DataContracts.Enums.dll  /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.runtime.serialization.dll" /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.servicemodel.dll" *.cs

del AutoMapper.dll

del *.cs

dir *.*

cd..
