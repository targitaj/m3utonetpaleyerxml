cd authorizationproxy

del *.*

rem copy C:\Projektit\UMA\Main\Source\SharedBinaries\Migri.Uma.Data\Migri.Uma.Data.DataContracts.Rekisteri.dll


svcutil /config:UmaAuthorization.config /o:UmaAuthorizationProxy.cs /n:*,UmaAuthorization /ct:System.Collections.Generic.List`1 net.tcp://localhost:50001/mex


c:\windows\microsoft.net\framework\v2.0.50727\csc /target:library /out:UmaAuthorizationProxy.dll /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.runtime.serialization.dll" /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.runtime.serialization.dll" /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.servicemodel.dll" *.cs


del *.cs

dir *.*

cd..