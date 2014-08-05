:r .\Roles.sql
/*
[UserID]                INT IDENTITY(10000,1) NOT NULL,
[CustomerId]            BIGINT           NULL,
[FirstName]             NVARCHAR(70)     NULL,
[LastName]              NVARCHAR(70)     NULL,
[PersonCode]            VARCHAR(20)      NULL,
[BirthDate]             DATE             NULL,
[Email]                 NVARCHAR(100)    NOT NULL,
[EmailConfirmed]        BIT              NOT NULL,
[PhoneNumber]           NVARCHAR (20)    NULL,
[PhoneNumberConfirmed]  BIT              NOT NULL,
[PasswordHash]          VARCHAR (70)     NULL,
[SecurityStamp]         VARCHAR (64)     NULL,
[TwoFactorEnabled]      BIT              NOT NULL,
[LockoutEndDateUtc]     DATETIME         NULL,
[LockoutEnabled]        BIT              NOT NULL,
[AccessFailedCount]     INT              NOT NULL,
[UserName]              NVARCHAR (128)   NULL,
[CreatedDate]           DATETIME NOT NULL
*/
SET IDENTITY_INSERT [User] ON
MERGE INTO [User] AS Target
USING (VALUES 
  (9999,NULL,'Migri','Admin',NULL,NULL,'admin@migri.fi',1,NULL,0,
     'AK2X7spy9oHVARcrTz5vp7ANapAfBXkAws0YfaPmVGB4k4PQKU8ZbPVwUeRakTRxkQ==','0553d78d-71bc-4fd9-a5df-d6e2389d48d7',
     1,NULL,1,0,'admin@migri.fi','2014-04-03T13:36:57.903')
 ,(9998,NULL,'Migri','Editor',NULL,NULL,'translator@migri.fi',1,NULL,0,
     'ACtqTxva76h2myYV0TIsj19Mn7yOS2EbG3tkOv1wZbOeXO7WI4VGZs7M+QDJ/hZrWw==','ab7cc477-4d86-4d08-a633-14c20653fe91',
     1,NULL,1,0,'translator@migri.fi','2014-04-04T09:16:45.677')
 ,(9997,NULL,'Migri','Tester',NULL,NULL,'tester@migri.fi',1,NULL,0,
     'ANe4YQoNa0l5r+9Rf0tJs/6faGBZm+D5gvKdLC0CKdIVtvaj6kxuJdJqsHyiAvIbOQ==','dff4be58-d9f9-4adc-9a21-28493b78fe22',
     1,NULL,1,0,'tester@migri.fi','2014-04-03T13:35:04.007')
) AS Source ([UserID],[CustomerId],[FirstName],[LastName],[PersonCode],[BirthDate],[Email],[EmailConfirmed],[PhoneNumber],[PhoneNumberConfirmed]
      ,[PasswordHash],[SecurityStamp]
      ,[TwoFactorEnabled],[LockoutEndDateUtc],[LockoutEnabled],[AccessFailedCount],[UserName],[CreatedDate])
ON (Target.[UserID] = Source.[UserID])
WHEN MATCHED AND 
(Target.[CustomerId] <> Source.[CustomerId] OR Target.[Email] <> Source.[Email] OR Target.[UserName] <> Source.[UserName] 
  OR Target.[PasswordHash] <> Source.[PasswordHash] OR Target.[SecurityStamp] <> Source.[SecurityStamp] OR Target.[EmailConfirmed] <> Source.[EmailConfirmed]) THEN
 UPDATE SET
    [CustomerId] = Source.[CustomerId], 
    [FirstName] = Source.[FirstName], 
    [LastName] = Source.[LastName], 
    [PersonCode] = Source.[PersonCode], 
    [BirthDate] = Source.[BirthDate], 
    [Email] = Source.[Email], 
    [EmailConfirmed] = Source.[EmailConfirmed], 
    [PhoneNumber] = Source.[PhoneNumber], 
    [PhoneNumberConfirmed] = Source.[PhoneNumberConfirmed], 
    [PasswordHash] = Source.[PasswordHash], 
    [SecurityStamp] = Source.[TwoFactorEnabled], 
    [TwoFactorEnabled] = Source.[LastName], 
    [LockoutEndDateUtc] = Source.[LockoutEndDateUtc], 
    [LockoutEnabled] = Source.[LockoutEnabled], 
    [AccessFailedCount] = Source.[AccessFailedCount], 
    [UserName] = Source.[UserName], 
    [CreatedDate] = Source.[CreatedDate]
WHEN NOT MATCHED BY TARGET THEN
 INSERT([UserID],[CustomerId],[FirstName],[LastName],[PersonCode],[BirthDate],[Email],[EmailConfirmed],[PhoneNumber],[PhoneNumberConfirmed],
     [PasswordHash],[SecurityStamp],[TwoFactorEnabled],[LockoutEndDateUtc],[LockoutEnabled],[AccessFailedCount],[UserName],[CreatedDate])
 VALUES(Source.[UserID],Source.[CustomerId],Source.[FirstName],Source.[LastName],Source.[PersonCode],Source.[BirthDate],Source.[Email],
     Source.[EmailConfirmed],Source.[PhoneNumber],Source.[PhoneNumberConfirmed],Source.[PasswordHash],Source.[SecurityStamp],Source.[TwoFactorEnabled],
     Source.[LockoutEndDateUtc],Source.[LockoutEnabled],Source.[AccessFailedCount],Source.[UserName],Source.[CreatedDate])
WHEN NOT MATCHED BY SOURCE THEN 
 DELETE;
SET IDENTITY_INSERT [User] OFF
GO

DECLARE @mergeError int, @mergeCount int
SELECT @mergeError = @@ERROR, @mergeCount = @@ROWCOUNT
IF @mergeError != 0
 BEGIN
 PRINT 'ERROR OCCURRED IN MERGE FOR [User]. Rows affected: ' + CAST(@mergeCount AS VARCHAR(100)); -- SQL should always return zero rows affected
 END
ELSE
 BEGIN
 PRINT '[User] rows affected by MERGE: ' + CAST(@mergeCount AS VARCHAR(100));
 END
GO

MERGE INTO [UserInRole] AS Target
USING (VALUES
  (9998,998)
 ,(9999,999)
) AS Source ([UserID],[RoleID])
ON (Target.[RoleID] = Source.[RoleID] AND Target.[UserID] = Source.[UserID])
WHEN NOT MATCHED BY TARGET THEN
 INSERT([UserID],[RoleID])
 VALUES(Source.[UserID],Source.[RoleID])
WHEN NOT MATCHED BY SOURCE THEN 
 DELETE;
GO

DECLARE @mergeError int, @mergeCount int
SELECT @mergeError = @@ERROR, @mergeCount = @@ROWCOUNT
IF @mergeError != 0
 BEGIN
 PRINT 'ERROR OCCURRED IN MERGE FOR [UserInRole]. Rows affected: ' + CAST(@mergeCount AS VARCHAR(100)); -- SQL should always return zero rows affected
 END
ELSE
 BEGIN
 PRINT '[UserInRole] rows affected by MERGE: ' + CAST(@mergeCount AS VARCHAR(100));
 END
GO

