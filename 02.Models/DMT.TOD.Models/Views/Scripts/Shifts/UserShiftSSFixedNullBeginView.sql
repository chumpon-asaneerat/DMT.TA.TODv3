CREATE VIEW UserShiftSSFixedNullBeginView
AS
	-- Fixed row that Begin Time is null but has End Time
	SELECT UserShift.UserShiftId
		 , UserShift.TSBId
		 , UserShift.ShiftId
		 , UserShift.UserId
		 , UserShift.FullNameEN
		 , UserShift.FullNameTH
		 , IFNULL(UserShift.[Begin], 
		   (
				-- Find last end from previous row but no previous row used current End time
				SELECT IFNULL(MAX(US1.[End]), 
							 IFNULL(
							 (SELECT MAX(US2.[Begin])
							    FROM UserShift US2
							   WHERE US2.UserId = UserShift.UserId
								 AND US2.[Begin] IS NOT NULL
								 AND US2.[Begin] <= UserShift.[End]
							     AND US2.UserShiftId <> UserShift.UserShiftId
							 ), UserShift.[End])) 
				  FROM UserShift US1
				 WHERE US1.UserId = UserShift.UserId
				   AND US1.[End] IS NOT NULL
				   AND US1.[End] <= UserShift.[End]
				   AND US1.UserShiftId <> UserShift.UserShiftId
		   )) AS [Begin]
		 , UserShift.[End]
		 , UserShift.ToTAServer
		 , UserShift.[Begin] AS BeginOriginal
		 , UserShift.[End] AS EndOriginal
	  FROM UserShift
