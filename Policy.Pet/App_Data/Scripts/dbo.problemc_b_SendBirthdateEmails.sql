/*
IF EXISTS ( SELECT Id FROM sysObjects WHERE Name like  'SendBirthdateEmails' ) 
BEGIN           
    DROP proc SendBirthdateEmails
END   
*/

CREATE PROCEDURE [dbo].[SendBirthdateEmails]
	@date datetime
AS

	declare @email TABLE(
		email varchar(100) NOT NULL
	)
	
	insert into @email
	select po.Email
	from Pet p
	join PetOwner po
	on p.PetOwnerId = po.Id
	where p.DateOfBirth = @date

	select * from @email

	/*
	  Send email for all emails in @email table
	*/
