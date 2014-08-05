cd documentumproxy

del *.*

rem copy C:\Projektit\UMA\Main\Source\SharedBinaries\Migri.Uma.Data\Migri.Uma.Data.DataContracts.Rekisteri.dll


svcutil /config:UmaDocumentum.config /o:UmaDocumentumProxy.cs /n:*,UmaDocumentum /ct:System.Collections.Generic.List`1 net.tcp://localhost/documentservice/service.svc/mex


c:\windows\microsoft.net\framework\v2.0.50727\csc /target:library /out:UmaDocumentumProxy.dll /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.runtime.serialization.dll" /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.runtime.serialization.dll" /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.servicemodel.dll" *.cs


del *.cs

dir *.*

cd..