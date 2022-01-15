CREATE VIEW UserShiftSSFixedNullEndView
AS
	-- Find row that already fixed Begin time but has End time is null in row that is not last opened shift
	SELECT USF.UserShiftId
		 , USF.TSBId
		 , USF.ShiftId
		 , USF.UserId
		 , USF.FullNameEN
		 , USF.FullNameTH
		 , USF.[Begin]
		 , IFNULL(USF.[End], 
		   (
			 	SELECT MIN(USF1.[Begin])
			 	  FROM UserShiftSSFixedNullBeginView USF1 
			 	 WHERE USF1.UserId = USF.UserId
				   AND USF1.[Begin] IS NOT NULL
				   AND USF1.[End] IS NOT NULL
				   AND USF1.[Begin] >= IFNULL(USF.[Begin], USF.[End])
				   AND USF1.UserShiftId <> USF.UserShiftId
		   )) AS [End]
		 , USF.ToTAServer
		 , USF.BeginOriginal
		 , USF.EndOriginal
	  FROM UserShiftSSFixedNullBeginView USF
