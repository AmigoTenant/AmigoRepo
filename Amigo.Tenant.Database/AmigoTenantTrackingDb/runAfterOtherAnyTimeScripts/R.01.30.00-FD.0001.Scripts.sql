-- ENTITY STATUS

if not exists(Select * from EntityStatus Where Code          ='EXPIRED' ) Insert into EntityStatus ( Code           ,Name           ,EntityCode     ,Sequence       ,CreatedBy      ,CreationDate   ,UpdatedBy      ,UpdatedDate  ) values ('EXPIRED','Expired','CO','5','1', GETDATE() ,'1',GETDATE())
if not exists(Select * from EntityStatus Where Code          ='CANCELED' ) Insert into EntityStatus ( Code           ,Name           ,EntityCode     ,Sequence       ,CreatedBy      ,CreationDate   ,UpdatedBy      ,UpdatedDate  ) values ('CANCELED','Cancelado','CO','6','1', GETDATE() ,'1',GETDATE())
