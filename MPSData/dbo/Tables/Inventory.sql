CREATE TABLE [dbo].[Inventory]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [ProductId] INT NOT NULL, 
    [Quantaty] INT NOT NULL, 
    [PurchasePrice] MONEY NOT NULL, 
    [PorchaseDate] DATETIME2 NOT NULL DEFAULT getutcdate()
)
