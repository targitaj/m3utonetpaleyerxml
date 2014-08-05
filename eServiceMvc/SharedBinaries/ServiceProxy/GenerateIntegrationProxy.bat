call "%VS100COMNTOOLS%vsvars32.bat"

cd integraatioproxy

del *.*
SET MACHINE=AS04
SET BRANCH=MAIN
SET BTS=BTS11
rem copy C:\Projektit\UMAEN\Main\Source\SharedBinaries\Migri.Uma.Data\Migri.Uma.Data.DataContracts.Registry.dll

rem http://BTS11.dev.uma.gov/ASMTESTAPP_Main_Uma_VTJResidencePermitInfo/Uma_VTJResidencePermitInfo.svc

svcutil  /config:UmaIntegration.config /o:UmaIntegrationVTJ.cs /n:*,UmaIntegration /ct:System.Collections.Generic.List`1 http://%BTS%.dev.uma.gov/%MACHINE%_%BRANCH%_UMA_VTJ/UMA_VTJ.svc?wsdl
svcutil /mergeconfig /config:UmaIntegration.config /o:UmaIntegrationIPost.cs /n:*,UmaIntegration /ct:System.Collections.Generic.List`1 http://%BTS%.dev.uma.gov/%MACHINE%_%BRANCH%_UMA_iPost/UMA_iPostService.svc?wsdl 
svcutil /mergeconfig /config:UmaIntegration.config /o:UmaIntegrationTAX.cs /n:*,UmaIntegration /ct:System.Collections.Generic.List`1 http://%BTS%.dev.uma.gov/%MACHINE%_%BRANCH%_UMA_TAX/UMA_Tax.svc?wsdl
svcutil /mergeconfig /config:UmaIntegration.config /o:UmaIntegrationHekopassi.cs /n:*,UmaIntegration /ct:System.Collections.Generic.List`1 http://%BTS%.dev.uma.gov/%MACHINE%_%BRANCH%_UMA_Hekopassi/UMA_Hekopassi.svc?wsdl
svcutil /mergeconfig /config:UmaIntegration.config /o:UmaIntegrationVTJUpdate.cs /n:*,UmaIntegration /ct:System.Collections.Generic.List`1 http://bts05.dev.uma.gov/UMA_VTJCitizenshipUpdate/UMA_UpdateCitizenship.svc?wsdl
svcutil /mergeconfig /config:UmaIntegration.config /o:UmaIntegrationKela.cs /n:*,UmaIntegration /ct:System.Collections.Generic.List`1 http://%BTS%.dev.uma.gov/%MACHINE%_%BRANCH%_UMA_Kela/UMA_Kela.svc?wsdl
svcutil /mergeconfig /config:UmaIntegration.config /o:UmaIntegrationSIS2Way.cs /n:*,UmaIntegration /ct:System.Collections.Generic.List`1 http://%BTS%.dev.uma.gov/%MACHINE%_%BRANCH%_UMA_SIS2Way/UMA_SIS2Way.svc?wsdl
svcutil /mergeconfig /config:UmaIntegration.config /o:UmaIntegrationSIS.cs /n:*,UmaIntegration /ct:System.Collections.Generic.List`1 http://%BTS%.dev.uma.gov/%MACHINE%_%BRANCH%_UMA_SIS/UMA_SIS.svc?wsdl
svcutil /mergeconfig /config:UmaIntegration.config /o:UmaIntegrationSMS.cs /n:*,UmaIntegration /ct:System.Collections.Generic.List`1 http://%BTS%.dev.uma.gov/%MACHINE%_%BRANCH%_UMA_SMS/UMA_SMS.svc?wsdl
svcutil /mergeconfig /config:UmaIntegration.config /o:UmaIntegrationEmail.cs /n:*,UmaIntegration /ct:System.Collections.Generic.List`1 http://%BTS%.dev.uma.gov/%MACHINE%_%BRANCH%_UMA_Email/UMA_EmailService.svc?wsdl
svcutil /mergeconfig /config:UmaIntegration.config /o:UmaIntegrationUlkonetRC.cs /n:*,UmaIntegration /ct:System.Collections.Generic.List`1 http://%BTS%.dev.uma.gov/%MACHINE%_%BRANCH%_UMA_UlkonetRegCheck/UMA_UlkonetRegCheck.svc?wsdl
svcutil /mergeconfig /config:UmaIntegration.config /o:UmaIntegrationKelaRC.cs /n:*,UmaIntegration /ct:System.Collections.Generic.List`1 http://%BTS%.dev.uma.gov/%MACHINE%_%BRANCH%_UMA_KelaRC/UMA_KelaRegistryCheck.svc?wsdl
svcutil /mergeconfig /config:UmaIntegration.config /o:UmaIntegrationCompanyInformationRC.cs /n:*,UmaIntegration /ct:System.Collections.Generic.List`1 http://%BTS%.dev.uma.gov/%MACHINE%_%BRANCH%_UMA_CompanyInfo_Service/UMA_CompanyInfo.svc?wsdl
svcutil /mergeconfig /config:UmaIntegration.config /o:UmaIntegrationPatjaRC.cs /n:*,UmaIntegration.PatjaRC /ct:System.Collections.Generic.List`1 http://%BTS%.dev.uma.gov/%MACHINE%_%BRANCH%_UMA_PatjaRC/UMA_PatjaRC.svc?wsdl
svcutil /mergeconfig /config:UmaIntegration.config /o:UmaIntegrationGemaltoCardOrder.cs /n:*,UmaIntegration /ct:System.Collections.Generic.List`1 http://%BTS%.dev.uma.gov/%MACHINE%_%BRANCH%_UMA_GemaltoCardOrder/UMA_GemaltoCardOrder.svc?wsdl
svcutil /mergeconfig /config:UmaIntegration.config /o:UmaIntegrationGemaltoStatusCheck.cs /n:*,UmaIntegration /ct:System.Collections.Generic.List`1 http://%BTS%.dev.uma.gov/%MACHINE%_%BRANCH%_UMA_GemaltoCheckDeliveredCards/UMA_GemaltoCheckDeliveredCards.svc?wsdl
svcutil /mergeconfig /config:UmaIntegration.config /o:UmaIntegrationEsrvPaymentInfo.cs /n:*,UmaIntegration /ct:System.Collections.Generic.List`1 http://bts08.dev.uma.gov/UMA_EsrvPaymentInfoService/UMA_EsrvPaymentInfoService.svc?wsdl
svcutil /mergeconfig /config:UmaIntegration.config /o:UmaIntegrationHaoKho.cs /n:*,UmaIntegration /ct:System.Collections.Generic.List`1 http://%BTS%.dev.uma.gov/%MACHINE%_%BRANCH%_HaoKhoIntegration/HaoKhoIntegration.svc?wsdl
svcutil /mergeconfig /config:UmaIntegration.config /o:UmaIntegrationSIS2.cs /n:*,UmaIntegration.SIS2 /ct:System.Collections.Generic.List`1 http://%BTS%.dev.uma.gov/%MACHINE%_%BRANCH%_UMA_SIS2/UMA_SIS2.svc?wsdl
svcutil /mergeconfig /config:UmaIntegration.config /o:UmaIntegrationVTJResidencePermit.cs /n:*,UmaIntegration /ct:System.Collections.Generic.List`1 http://bts10.dev.uma.gov/Uma_VTJResidencePermitInfo/Uma_VTJResidencePermitInfo.svc?wsdl
svcutil /mergeconfig /config:UmaIntegration.config /o:UmaIntegrationKieku.cs /n:*,UmaIntegration /ct:System.Collections.Generic.List`1 http://bts10.dev.uma.gov/UMA_Kieku/UMA_Kieku.svc?wsdl

c:\windows\microsoft.net\framework\v2.0.50727\csc /target:library /out:UmaIntegrationProxy.dll /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.runtime.serialization.dll" /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.runtime.serialization.dll" /reference:"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\system.servicemodel.dll" *.cs


del *.cs

dir *.*

cd..
