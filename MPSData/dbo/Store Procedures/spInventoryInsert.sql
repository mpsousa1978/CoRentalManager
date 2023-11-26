CREATE PROCEDURE [dbo].[spInventoryInsert]
@ProductId int,
@Quantaty int,
@PurchasePrice money,
@PurchaseDate datetime2

AS
begin
	set nocount on

	insert into dbo.Inventory (ProductId,Quantaty,PurchasePrice,PurchaseDate)
	values(@ProductId,@Quantaty,@PurchasePrice,@PurchaseDate)

end
