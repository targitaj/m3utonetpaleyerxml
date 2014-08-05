SET IDENTITY_INSERT [UserRole] ON
MERGE INTO [UserRole] AS Target
USING (VALUES
  (998,'Translator')
 ,(999,'Manager')
) AS Source ([RoleID],[RoleName])
ON (Target.[RoleID] = Source.[RoleID])
WHEN MATCHED AND (Target.[RoleName] <> Source.[RoleName]) THEN
 UPDATE SET
 [RoleName] = Source.[RoleName]
WHEN NOT MATCHED BY TARGET THEN
 INSERT([RoleID],[RoleName])
 VALUES(Source.[RoleID],Source.[RoleName])
WHEN NOT MATCHED BY SOURCE THEN 
 DELETE;
SET IDENTITY_INSERT [UserRole] OFF
GO

DECLARE @mergeError int, @mergeCount int
SELECT @mergeError = @@ERROR, @mergeCount = @@ROWCOUNT
IF @mergeError != 0
 BEGIN
 PRINT 'ERROR OCCURRED IN MERGE FOR [UserRole]. Rows affected: ' + CAST(@mergeCount AS VARCHAR(100)); -- SQL should always return zero rows affected
 END
ELSE
 BEGIN
 PRINT '[UserRole] rows affected by MERGE: ' + CAST(@mergeCount AS VARCHAR(100));
 END
GO
