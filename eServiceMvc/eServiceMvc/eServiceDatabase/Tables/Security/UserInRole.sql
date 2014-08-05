CREATE TABLE [dbo].[UserInRole]
(
    [UserID] INT NOT NULL,
    [RoleID] INT NOT NULL,
    CONSTRAINT [PK_UserInRole_UserID_RoleID] PRIMARY KEY CLUSTERED ([UserID] ASC, [RoleID] ASC),
    CONSTRAINT [FK_UserInRole_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID]),
    CONSTRAINT [FK_UserInRole_UserRole] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[UserRole] ([RoleID])
);