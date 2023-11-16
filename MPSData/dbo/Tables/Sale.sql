CREATE TABLE [dbo].[Sale]
(
	[Id] INT NOT NULL PRIMARY KEY identity, 
    [CashierId] VARCHAR(128) NOT NULL, 
    [SaleDate] DATETIME2 NOT NULL, 
    [SubTotal] MONEY NOT NULL, 
    [Tax] MONEY NOT NULL, 
    [Total] MONEY NOT NULL
)
