WScript.Echo "========================" 
WScript.Echo "Send e-mail notification" 
WScript.Echo "------------------------" 

set args = WScript.Arguments 
num = args.Count 

Set WSHNetwork = CreateObject("WScript.Network")
execited_name =  WSHNetwork.UserName

if num < 1 then 
   WScript.Echo "ERROR: No command line parameters! Please select user name, password and e-mail address" 
   WScript.Quit 1 
end if 

user_mail=args(0)

IF user_mail<>"" THEN

   sch = "http://schemas.microsoft.com/cdo/configuration/" 
   Set cdoConfig = CreateObject("CDO.Configuration")  
 
   With cdoConfig.Fields  
       .Item(sch & "sendusing") = 2  
       .Item(sch & "smtpserverport") = 25  
       .Item(sch & "smtpserver") = "tfsat01"  
       .Update  
   End With 


  Set objEmail = CreateObject("CDO.Message")
  Set objEmail.Configuration = cdoConfig 
  objEmail.MimeFormatted = TRUE
  objEmail.From = "reply@acme.org"
  objEmail.To = user_mail
  objEmail.Subject = "SharedBinaries deploy perfromed by user " & execited_name 
  objEmail.Textbody = "Hello" & vbCrLf
  objEmail.Textbody =  objEmail.Textbody & "" & vbCrLf
  objEmail.Textbody =  objEmail.Textbody & "SharedBinaries were deployed." & vbCrLf
  objEmail.Textbody =  objEmail.Textbody & "Please take latest version" & vbCrLf
  objEmail.Textbody =  objEmail.Textbody & "" & vbCrLf
  objEmail.Textbody =  objEmail.Textbody & execited_name

  objEmail.Send

  WScript.Echo "Notification Send" 
END IF

WScript.Quit