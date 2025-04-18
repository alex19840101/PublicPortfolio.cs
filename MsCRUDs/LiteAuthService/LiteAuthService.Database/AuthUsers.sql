﻿CREATE TABLE [dbo].[AuthUsers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Login] NVARCHAR(32) NOT NULL,
	[UserName] NVARCHAR(255) NOT NULL,
	[Email] NVARCHAR(255) NOT NULL,
	[PasswordHash] NVARCHAR(255) NOT NULL,

	[Nick] NVARCHAR(255),
	[Phone] NVARCHAR(255),
	[Role] NVARCHAR(255),

	[GranterId] INT,
	[CreatedDt] datetime2 NOT NULL,
	[LastUpdateDt] datetime2
	

	CONSTRAINT AL_Login UNIQUE(Login)
)
