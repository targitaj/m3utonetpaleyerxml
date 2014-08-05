CREATE TABLE [dbo].[UserRole] (
    [RoleID]   INT          IDENTITY(1000, 1) NOT NULL,
    [RoleName] NVARCHAR(50) NOT NULL
    CONSTRAINT [PK_UserRole_RoleID] PRIMARY KEY CLUSTERED ([RoleID] ASC)
);