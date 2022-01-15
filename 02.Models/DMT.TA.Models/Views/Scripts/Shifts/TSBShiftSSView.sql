CREATE VIEW TSBShiftSSView
AS
	SELECT TSBShift.TSBShiftId
	     , TSBShift.TSBId
		 , TSBShift.ShiftId
		 , IFNULL(TSBShift.[Begin], (SELECT MAX([End]) FROM TSBShift)) AS [Begin]
		 , IFNULL(TSBShift.[End], strftime('%Y-%m-%dT%H:%M:%f', 'now', 'localtime')) AS [End]
		 , TSBShift.UserId
		 , TSBShift.FullNameEN
		 , TSBShift.FullNameTH
		 , TSBShift.ToTAServer
	  FROM TSBShift
