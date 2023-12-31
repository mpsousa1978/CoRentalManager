﻿CREATE PROCEDURE [dbo].[spSaleInsert]
	@Id int,
	@CashierId nvarchar(128),
	@SaleDate datetime2,
	@SubTotal money,
	@Tax money,
	@Total money
AS
begin
	set nocount on;

	insert into dbo.Sale(CashierId,SaleDate,SubTotal,Tax,Total)
	values (@CashierId,@SaleDate,@SubTotal,@Tax,@Total);

	select @Id = Scope_IDENTITY();
end
