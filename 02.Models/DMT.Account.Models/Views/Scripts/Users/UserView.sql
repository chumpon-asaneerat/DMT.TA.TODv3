CREATE VIEW UserView
AS
	SELECT [User].*
		 , TRIM(PRINTF("%s %s"
		          , [User].PrefixEN
			      , TRIM(PRINTF("%s %s"
				            , [User].FirstNameEN
			                , TRIM(PRINTF("%s %s"
				                   , [User].MiddleNameEN
			                       , [User].LastNameEN))
			          ))
			   )) AS FullNameEN
		 , TRIM(PRINTF("%s %s"
		          , [User].PrefixTH
			      , TRIM(PRINTF("%s %s"
				            , [User].FirstNameTH
			                , TRIM(PRINTF("%s %s"
				                   , [User].MiddleNameTH
			                       , [User].LastNameTH))
			          ))
			   )) AS FullNameTH
		 , [Role].RoleNameEN, [Role].RoleNameTH, [Role].GroupId
	  FROM [User]
	     , [Role]
	 WHERE [User].RoleId = [Role].RoleId
