cd CacheProxy

del *.*

copy ..\..\Migri.Uma.Data\Migri.Uma.Data.DataContracts.Enums.dll
copy ..\..\Migri.Uma.Data\Migri.Uma.Data.DataContracts.Registry.dll
copy ..\..\Migri.Uma.Data\Migri.Uma.Data.Repository.dll
copy ..\..\Migri.Uma.Data\Migri.Uma.EnterpriseLibrary.Data.dll

svcutil /config:UmaCache.config /o:UmaCacheProxy.cs /n:*,UmaCache /r:Migri.Uma.Data.DataContracts.Enums.dll /r:Migri.Uma.Data.Repository.dll /r:Migri.Uma.Data.DataContracts.Registry.dll /ct:System.Collections.Generic.List`1 net.pipe://localhost/CacheService/mex

c:\windows\microsoft.net\framework\v2.0.50727\csc /target:library /out:UmaCacheProxy.dll /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.runtime.serialization.dll" /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.runtime.serialization.dll" /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.servicemodel.dll" /reference:Migri.Uma.Data.Repository.dll /reference:Migri.Uma.Data.DataContracts.Registry.dll /reference:Migri.Uma.EnterpriseLibrary.Data.dll /reference:Migri.Uma.Data.DataContracts.Enums.dll *.cs

del *.cs
del Migri.Uma.Data.DataContracts.Enums.dll
del Migri.Uma.Data.DataContracts.Registry.dll
del Migri.Uma.Data.Repository.dll
del Migri.Uma.EnterpriseLibrary.Data.dll

dir *.*

cd..