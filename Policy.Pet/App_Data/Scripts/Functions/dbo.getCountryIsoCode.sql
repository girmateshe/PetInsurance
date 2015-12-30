/*
IF EXISTS ( SELECT Id FROM sysObjects WHERE Name like  'getCountryIsoCode' ) 
BEGIN           
    DROP function getCountryIsoCode
END   
*/

USE [InsurancePolicy]
GO

CREATE FUNCTION [dbo].[getCountryIsoCode] 
(
	@ID int
)
RETURNS nvarchar(max)
AS
BEGIN
	
	DECLARE @CountryIsoCode nvarchar(max)

    select @CountryIsoCode = (select IsoCode from Country C where ID = @ID)

	RETURN isnull(@CountryIsoCode,'')

END 