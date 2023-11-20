CREATE PROCEDURE [dbo].[spUserLookup]
	@Id varchar(128)
AS
BEGIN
	set nocount on;
	
	SELECT Id, FirstName, LastName, EmailAddress, CreateDate
	FROM [dbo].[User]
	WHERE Id = @Id;

END;