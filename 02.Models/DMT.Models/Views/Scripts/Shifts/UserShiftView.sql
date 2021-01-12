CREATE VIEW UserShiftView
AS
	SELECT UserShift.*
		 , TSB.TSBNameEN, TSB.TSBNameTH
		 , [Shift].ShiftNameEN, [Shift].ShiftNameTH
	  FROM UserShift
		 , TSB
	     , [Shift]
	 WHERE UserShift.TSBId = TSB.TSBId
	   AND UserShift.ShiftId = [Shift].ShiftId
