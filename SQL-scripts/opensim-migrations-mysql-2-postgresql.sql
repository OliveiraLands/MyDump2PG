
begin transaction;

alter table assets alter column id drop default;
alter table assets alter column id type uuid using cast(id as uuid);
alter table assets alter column id set default '00000000-0000-0000-0000-000000000000'::uuid;

--alter table assets alter column "CreatorID" type uuid using cast( (case when "CreatorID" = '' then '00000000-0000-0000-0000-000000000000' else "CreatorID" end) as uuid );

alter table auth alter column "UUID" type uuid using cast("UUID" as uuid);

alter table avatars alter column "PrincipalID" type uuid using cast("PrincipalID" as uuid);

alter table estate_groups alter column "uuid" type uuid using cast("uuid" as uuid);

alter table estate_managers alter column "uuid" type uuid using cast("uuid" as uuid);

alter table estate_map alter column "RegionID" drop default;
alter table estate_map alter column "RegionID" type uuid using cast( (case when "RegionID" = '' then '00000000-0000-0000-0000-000000000000' else "RegionID" end) as uuid );
alter table estate_map alter column "RegionID" set default '00000000-0000-0000-0000-000000000000'::uuid;

alter table estate_settings alter column "EstateOwner" type uuid using cast("EstateOwner" as uuid);

alter table estate_users alter column "uuid" type uuid using cast("uuid" as uuid);

alter table estateban alter column "bannedUUID" type uuid using cast("bannedUUID" as uuid);

alter table friends alter column "PrincipalID" drop default;
alter table friends alter column "PrincipalID" type uuid using cast("PrincipalID" as uuid), alter column "Friend" type uuid using cast("Friend" as uuid);
alter table friends alter column "PrincipalID" set default '00000000-0000-0000-0000-000000000000'::uuid;

-- griduser dont use UserID as uuid because hypergrid address
-- alter table griduser alter column "UserID" drop default, alter column "LastRegionID" drop default;
-- alter table griduser alter column "UserID" type uuid using cast("UserID" as uuid), alter column "LastRegionID" type uuid using cast("LastRegionID" as uuid);
-- alter table griduser alter column "UserID" set default '00000000-0000-0000-0000-000000000000'::uuid, alter column "LastRegionID" set default '00000000-0000-0000-0000-000000000000'::uuid;

alter table inventoryfolders alter column "folderID" drop default;
alter table inventoryfolders alter column "folderID" type uuid using cast("folderID" as uuid);
alter table inventoryfolders alter column "folderID" set default '00000000-0000-0000-0000-000000000000'::uuid;

alter table inventoryfolders alter column "agentID"  type uuid using cast("agentID" as uuid);
alter table inventoryfolders alter column "parentFolderID" type uuid using cast("parentFolderID" as uuid);

alter table inventoryitems alter column "assetID" type uuid using cast("assetID" as uuid);

alter table inventoryitems alter column "groupID" drop default;
alter table inventoryitems alter column "groupID" type uuid using cast("groupID" as uuid);
alter table inventoryitems alter column "groupID" set default '00000000-0000-0000-0000-000000000000'::uuid;

alter table inventoryitems alter column "inventoryID"    drop default;
alter table inventoryitems alter column "inventoryID"    type uuid using cast("inventoryID" as uuid);
alter table inventoryitems alter column "inventoryID"    set default '00000000-0000-0000-0000-000000000000'::uuid;

alter table inventoryitems alter column "avatarID"       type uuid using cast("avatarID" as uuid);
alter table inventoryitems alter column "parentFolderID" type uuid using cast("parentFolderID" as uuid);


rollback;

