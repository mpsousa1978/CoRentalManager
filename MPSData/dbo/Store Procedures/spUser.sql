CREATE PROCEDURE [dbo].[spUser]
AS
BEGIN
	set nocount on;
	
	SELECT Id, FirstName, LastName, EmailAddress, CreateDate
	FROM [dbo].[User];

END;
