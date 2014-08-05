/****
***** This script is one-time script to convert existing eService users in ASP.NET Membership tables int new ASP.NET Identity table structures.
***** More info here: http://stackoverflow.com/questions/7315284/sql-aspnet-profile
***** Also password hash reuse is described here: http://www.asp.net/identity/overview/migrations/migrating-an-existing-website-from-sql-membership-to-aspnet-identity
****/


/****
***** This is actual statement which extracts data from Oracle ESRV (old) account table
***** and creates INSERT statements for Identity table in new eServices
****/

/* -- commented out as it is Oracle Query

--ALTER SESSION SET current_schema="UMA_PRODUCTION";
select 'INSERT INTO [User] ([CustomerId], [FirstName], [LastName], [PersonCode]
 ,[BirthDate], [IsStronglyAuthenticated], [Email], [EmailConfirmed]
 ,[PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp]
 ,[TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount]
 ,[UserName], [CreatedDate]) VALUES (' || 
  nvl2(usr.uma_customer_number, TO_CHAR(usr.uma_customer_number), 'NULL') || ',' ||
  '''' || replace(usr.first_names,'''','''''') || '''' || ',' ||
  '''' || replace(usr.last_name,'''','''''') || '''' || ',' ||
  nvl2(usr.personal_id_number, '''' || usr.personal_id_number || '''', 'NULL') || ',' ||
  nvl2(usr.birth_date, '''' || TO_CHAR(usr.birth_date,'YYYY-MM-DD') || '''', 'NULL') || ',' ||
  CASE usr.authentication_type_enum WHEN 'Strong' THEN '1' ELSE '0' END || ',' ||
  '''' || replace(usr.email,'''','''''') || '''' || ',' ||
  '1' || ',' ||
  nvl2(usr.mobile_phone, '''' || usr.mobile_phone || '''', 'NULL') || ',' ||
  nvl2(usr.mobile_phone, '1', '0') || ',' ||
  '''' || REGEXP_REPLACE(sys_guid(), '(........)(....)(....)(................)', '\1-\2-\3-\4') || '''' || ',' ||
  CASE usr.authentication_type_enum WHEN 'Strong' THEN '0' ELSE '1' END || ',' ||
  '1' || ',' ||
  '0' || ',' ||
  '''' || replace(usr.email,'''','''''') || '''' || ',' ||
  '''' || TO_CHAR(usr.creation_time,'YYYY-MM-DD HH24:MI:SS') || '''' ||
  ');' AS Statements
FROM
  esrv_customer_account usr
WHERE 
  usr.email IS NOT NULL

*/
GO


/****
***** Next two functions are to be executed in old eService database to create procedures necessary for extracting Custom properties out of profile
****/

CREATE FUNCTION dbo.fn_GetElement
(
@ord AS INT,
@str AS VARCHAR(8000),
@delim AS VARCHAR(1) )

RETURNS INT
AS
BEGIN
  -- If input is invalid, return null.
  IF @str IS NULL
      OR LEN(@str) = 0
      OR @ord IS NULL
      OR @ord < 1
      -- @ord > [is the] expression that calculates the number of elements.
      OR @ord > LEN(@str) - LEN(REPLACE(@str, @delim, '')) + 1
    RETURN NULL
  DECLARE @pos AS INT, @curord AS INT
  SELECT @pos = 1, @curord = 1
  -- Find next element's start position and increment index.
  WHILE @curord < @ord
    SELECT
      @pos    = CHARINDEX(@delim, @str, @pos) + 1,
      @curord = @curord + 1
  RETURN
  CAST(SUBSTRING(@str, @pos, CHARINDEX(@delim, @str + @delim, @pos) - @pos) AS INT)
END
GO

CREATE FUNCTION dbo.fn_GetProfileElement
(
@fieldName AS NVARCHAR(100),
@fields AS NVARCHAR(4000),
@values AS NVARCHAR(4000))

RETURNS NVARCHAR(4000)
AS
BEGIN
  -- If input is invalid, return null.
  IF @fieldName IS NULL
      OR LEN(@fieldName) = 0
      OR @fields IS NULL
      OR LEN(@fields) = 0
      OR @values IS NULL
      OR LEN(@values) = 0

    RETURN NULL

-- locate FieldName in Fields
DECLARE @fieldNameToken AS NVARCHAR(20)
DECLARE @fieldNameStart AS INTEGER,
@valueStart AS INTEGER,
@valueLength AS INTEGER

-- Only handle string type fields (:S:)
SET @fieldNameStart = CHARINDEX(@fieldName + ':S',@Fields,0)

-- If field is not found, return null
IF @fieldNameStart = 0 RETURN NULL
SET @fieldNameStart = @fieldNameStart + LEN(@fieldName) + 3

-- Get the field token which I've defined as the start of the
-- field offset to the end of the length
SET @fieldNameToken = SUBSTRING(@Fields,@fieldNameStart,LEN(@Fields)-@fieldNameStart)

-- Get the values for the offset and length
SET @valueStart = dbo.fn_getelement(1,@fieldNameToken,':')
SET @valueLength = dbo.fn_getelement(2,@fieldNameToken,':')

-- Check for sane values, 0 length means the profile item was
-- stored, just no data
IF @valueLength = 0 RETURN ''

-- Return the string
RETURN SUBSTRING(@values, @valueStart+1, @valueLength)

END
GO



/******
******* This is can be used to update [User] in identity from Membership, if both databases are available on one server
*******/


UPDATE 
    [User] 
SET 
    [User].[PasswordHash] = (mem.Password+'|'+CAST(mem.PasswordFormat as varchar)+'|'+mem.PasswordSalt)
FROM
    aspnet_Users usr
LEFT OUTER JOIN aspnet_Membership mem 
    ON mem.ApplicationId = usr.ApplicationId AND usr.UserId = mem.UserId
LEFT OUTER JOIN aspnet_Profile prof
    ON prof.UserId = usr.UserId
WHERE 
    dbo.fn_GetProfileElement('CustomerId', prof.PropertyNames, prof.PropertyValuesString) IS NOT NULL
    AND [User].[CustomerId] = dbo.fn_GetProfileElement('CustomerId', prof.PropertyNames, prof.PropertyValuesString)



/******
******* This is can be used to create Password import into identity Table for users who have matching Customer Id
*******/


SELECT 
'UPDATE [User] SET [PasswordHash] = ''' + (mem.Password+'|'+CAST(mem.PasswordFormat as varchar)+'|'+mem.PasswordSalt) + ''' ' +
'WHERE [User].[CustomerId] = ' + dbo.fn_GetProfileElement('CustomerId', prof.PropertyNames, prof.PropertyValuesString)
FROM
    aspnet_Users usr
LEFT OUTER JOIN aspnet_Membership mem 
    ON mem.ApplicationId = usr.ApplicationId AND usr.UserId = mem.UserId
LEFT OUTER JOIN aspnet_Profile prof
    ON prof.UserId = usr.UserId
WHERE 
    dbo.fn_GetProfileElement('CustomerId', prof.PropertyNames, prof.PropertyValuesString) IS NOT NULL


  /******
  ******* This is reference for other information if needed
  *******/

--INSERT INTO [User] (--[UserID],
--      [CustomerId]
--      ,[FirstName]
--      ,[LastName]
--      ,[PersonCode]
--      ,[BirthDate]
--      ,[IsStronglyAuthenticated]
--      ,[Email]
--      ,[EmailConfirmed]
--      ,[PhoneNumber]
--      ,[PhoneNumberConfirmed]
--      ,[PasswordHash]
--      ,[SecurityStamp]
--      ,[TwoFactorEnabled]
--      ,[LockoutEndDateUtc]
--      ,[LockoutEnabled]
--      ,[AccessFailedCount]
--      ,[UserName]
--      ,[CreatedDate])
SELECT 
    --aspnet_Users.UserId AS [UserId],
    CAST(dbo.fn_GetProfileElement('CustomerId', prof.PropertyNames, prof.PropertyValuesString) AS BIGINT) AS [CustomerId],
    'First name' AS [FirstName],
    'Last name' AS [LastName],
    'Person code' AS [PersonCode],
    NULL AS [BirthDate],
    CAST(0 AS BIT) AS [IsStronglyAuthenticated], -- As "False" for Strongly authenticated,
    CASE dbo.fn_GetProfileElement('UserNameType', PropertyNames, PropertyValuesString) WHEN 'Email' THEN usr.UserName ELSE NULL END AS [Email],
    CASE dbo.fn_GetProfileElement('UserNameType', PropertyNames, PropertyValuesString) WHEN 'Email' THEN CAST(1 AS BIT) ELSE NULL END AS [EmailConfirmed],
    CASE dbo.fn_GetProfileElement('UserNameType', PropertyNames, PropertyValuesString) WHEN 'Phone' THEN usr.UserName ELSE NULL END AS [PhoneNumber],
    CASE dbo.fn_GetProfileElement('UserNameType', PropertyNames, PropertyValuesString) WHEN 'Phone' THEN CAST(1 AS BIT) ELSE NULL END AS [PhoneNumberConfirmed],
    (mem.Password+'|'+CAST(mem.PasswordFormat as varchar)+'|'+mem.PasswordSalt) AS [PasswordHash],
    NewID() AS [SecurityStamp],
    CAST(1 AS BIT) AS [TwoFactorEnabled],
    NULL AS [LockoutEndDateUtc],
    CAST(1 AS BIT) AS [LockoutEnabled],
    0 AS [AccessFailedCount],
    CASE dbo.fn_GetProfileElement('UserNameType', PropertyNames, PropertyValuesString) WHEN 'Email' THEN usr.UserName ELSE NULL END AS [UserName],
    mem.CreateDate AS [CreatedDate]
FROM aspnet_Users usr
LEFT OUTER JOIN aspnet_Membership mem 
    ON mem.ApplicationId = usr.ApplicationId AND usr.UserId = mem.UserId
LEFT OUTER JOIN aspnet_Profile prof
    ON prof.UserId = usr.UserId