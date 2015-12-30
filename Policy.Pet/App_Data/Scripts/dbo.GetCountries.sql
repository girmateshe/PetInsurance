/*
IF EXISTS ( SELECT Id FROM sysObjects WHERE Name like  'GetCountries' ) 
BEGIN           
    DROP proc GetCountries
END   
*/

CREATE PROCEDURE [dbo].[GetCountries]
AS
	select Name, 
		   IsoCode
	from Country