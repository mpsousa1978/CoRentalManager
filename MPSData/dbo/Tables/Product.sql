CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL PRIMARY KEY identity, 
    [ProductName] VARCHAR(100) NOT NULL, 
    [Description] VARCHAR(MAX) NOT NULL, 
    [RetailPrice] MONEY NOT NULL, 
    [QuantatyInStock] INT NULL,
    [CreateDate] DATETIME2 NOT NULL default getutcDate(), 
    [LastModified] DATETIME2 NOT NULL DEFAULT getutcdate()

)
