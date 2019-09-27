CREATE TABLE [dbo].[Invitations]
(
	[Id] INT NOT NULL PRIMARY KEY,
    [Reference] UNIQUEIDENTIFIER NOT NULL,
    [Ukprn] VARCHAR(255) NOT NULL,
    [UserRef] VARCHAR(255),
    [EmployerOrganisation] VARCHAR(255) NOT NULL,
    [EmployerFirstName] VARCHAR(255) NOT NULL,
    [EmployerLastName] VARCHAR(255)  NOT NULL,
    [EmployerEmail] VARCHAR(255) NOT NULL,
    [Status] INT NOT NULL,
    [CreatedDate] DATETIME,
    [UpdatedDate] DATETIME
)
