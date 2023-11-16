CREATE TABLE [dbo].[User]
(
    [Id] VARCHAR(128) NOT NULL PRIMARY KEY , 
    [FirstName] VARCHAR(50) NOT NULL, 
    [LastName] VARCHAR(50) NOT NULL, 
    [EmailAddres] VARCHAR(255) NOT NULL, 
    [CreateDate] DATETIME NOT NULL DEFAULT getutcdate(),

)
