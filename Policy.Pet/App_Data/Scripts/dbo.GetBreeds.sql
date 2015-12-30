/*
IF EXISTS ( SELECT Id FROM sysObjects WHERE Name like  'GetBreeds' ) 
BEGIN           
    DROP proc GetBreeds
END   
*/

CREATE PROCEDURE [dbo].[GetBreeds]
AS
	select Id, 
		   Name
	from Breed