call "%VS90COMNTOOLS%vsvars32.bat"

cd IntegrationCommonProxy

del *.*


svcutil /config:IntegrationCommon.config /o:IntegrationCommonProxy.cs /n:*,IntegrationCommon /ct:System.Collections.Generic.List`1 http://localhost/IntegrationCommonService/Service.svc


c:\windows\microsoft.net\framework\v2.0.50727\csc /target:library /out:IntegrationCommonProxy.dll /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.runtime.serialization.dll" /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.runtime.serialization.dll" /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.servicemodel.dll" *.cs


del *.cs

dir *.*

cd..

pause