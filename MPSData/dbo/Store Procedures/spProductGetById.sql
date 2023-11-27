CREATE PROCEDURE dbo.spProductGetById
	@Id int = 0
AS
begin
	set nocount on;
	SELECT id,ProductName,[Description],RetailPrice,QuantatyInStock,IsTaxable
	from dbo.Product 
	where id = @Id
	order by ProductName;
end;
