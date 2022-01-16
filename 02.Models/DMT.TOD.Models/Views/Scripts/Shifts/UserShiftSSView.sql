CREATE VIEW UserShiftSSView
AS
	-- Find row that Begin is null but has End
	SELECT USEV.UserShiftId
		 , USEV.TSBId
		 , USEV.ShiftId
		 , USEV.UserId
		 , USEV.FullNameEN
		 , USEV.FullNameTH
		 , USEV.ToTAServer
		 , USEV.[Begin]
		 /* 'now' is in UTC required 'localtime' */
		 , IFNULL(USEV.[End], strftime('%Y-%m-%dT%H:%M:%f', 'now', 'localtime')) AS [End]
		 , USEV.BeginOriginal
		 , USEV.EndOriginal
	  FROM UserShiftSSFixedNullEndView USEV
