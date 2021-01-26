CREATE VIEW UserShiftRevenueView
AS
	SELECT UserShiftRevenue.*
		 , TSB.TSBNameEN, TSB.TSBNameTH
		 , PlazaGroup.PlazaGroupNameEN, PlazaGroup.PlazaGroupNameTH, PlazaGroup.Direction
		 , [Shift].ShiftNameEN, [Shift].ShiftNameTH
	  FROM UserShiftRevenue
		 , TSB
	     , PlazaGroup
		 , [Shift]
		 , UserShift
	 WHERE PlazaGroup.TSBId = TSB.TSBId
	   AND UserShift.TSBId = TSB.TSBId
	   AND UserShift.ShiftId = [Shift].ShiftId
	   AND UserShiftRevenue.TSBId = TSB.TSBId
	   AND UserShiftRevenue.PlazaGroupId = PlazaGroup.PlazaGroupId
	   AND UserShiftRevenue.ShiftId = [Shift].ShiftId
	   AND UserShiftRevenue.UserId = UserShift.UserId
  ORDER BY UserShift.[Begin], [Shift].ShiftId, PlazaGroup.PlazaGroupId
