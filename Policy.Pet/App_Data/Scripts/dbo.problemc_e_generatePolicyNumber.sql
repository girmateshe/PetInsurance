/*
IF EXISTS ( SELECT Id FROM sysObjects WHERE Name like  'generatePolicyNumber' ) 
BEGIN           
    DROP proc generatePolicyNumber
END   
*/
create procedure generatePolicyNumber
	@countrycode char(3),
	@policyNumber char(13) out
as 
begin
 
  DECLARE @Number int = 1;
  SELECT @number = Id 
  FROM PetOwner With (Tablock, Holdlock)

  if(@Number is null) 
  begin
	select @Number = 1
  end

  select @policyNumber = left(@countrycode + '_00000000'+ CONVERT(VARCHAR, @Number), 13)

end 