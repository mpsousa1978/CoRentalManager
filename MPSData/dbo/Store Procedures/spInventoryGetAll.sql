CREATE PROCEDURE [dbo].[spInventoryGetAll]
AS
begin
	set nocount on

	select [Id], [ProductId], [Quantaty], [PurchasePrice], [PurchaseDate] 
	from dbo.Inventory;

end
