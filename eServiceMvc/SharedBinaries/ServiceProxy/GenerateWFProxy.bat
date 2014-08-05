cd wfproxy

del *.*

rem copy C:\Projektit\UMA\Main\Source\SharedBinaries\Migri.Uma.Data\Migri.Uma.Palvelut.DataContracts.dll


svcutil /config:UmaWf.config /o:UmaWfProxy.cs /n:*,UmaWf /ct:System.Collections.Generic.List`1 http://localhost/CPService/CaseserviceService.svc?wsdl

c:\windows\microsoft.net\framework\v2.0.50727\csc /target:library /out:UmaWfProxy.dll /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.runtime.serialization.dll" /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.runtime.serialization.dll" /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.servicemodel.dll" *.cs


del *.cs

dir *.*

cd..