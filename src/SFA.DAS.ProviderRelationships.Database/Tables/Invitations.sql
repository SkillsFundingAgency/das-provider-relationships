﻿CREATE TABLE [dbo].[Invitations]
(
	[Id] BIGINT NOT NULL IDENTITY,
    [Reference] UNIQUEIDENTIFIER NOT NULL,
    [Ukprn] VARCHAR(255) NOT NULL,
    [UserRef] VARCHAR(255),
    [EmployerOrganisation] VARCHAR(255) NOT NULL,
    [EmployerFirstName] VARCHAR(255) NOT NULL,
    [EmployerLastName] VARCHAR(255)  NOT NULL,
    [EmployerEmail] VARCHAR(255) NOT NULL,
    [Status] INT NOT NULL,
    [CreatedDate] DATETIME,
    [UpdatedDate] DATETIME, 
    CONSTRAINT [PK_Invitations] PRIMARY KEY ([Id])
)
