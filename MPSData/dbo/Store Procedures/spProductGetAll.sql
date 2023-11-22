CREATE PROCEDURE [dbo].[spProductGetAll]
AS
begin
	set nocount on;
	SELECT id,ProductName,[Description],RetailPrice,QuantatyInStock,IsTaxable
	from dbo.Product 
	order by ProductName;
end;

