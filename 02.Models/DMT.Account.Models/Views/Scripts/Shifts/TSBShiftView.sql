CREATE VIEW TSBShiftView
AS
	SELECT TSBShift.*
		 , TSB.TSBNameEN, TSB.TSBNameTH
		 , [Shift].ShiftNameEN, [Shift].ShiftNameTH
	  FROM TSBShift
		 , TSB
	     , [Shift]
	 WHERE TSBShift.TSBId = TSB.TSBId
	   AND TSBShift.ShiftId = [Shift].ShiftId
	 ORDER BY TSBShift.[Begin], [Shift].ShiftId
