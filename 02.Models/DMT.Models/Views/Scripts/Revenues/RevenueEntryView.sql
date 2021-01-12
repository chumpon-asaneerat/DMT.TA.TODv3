CREATE VIEW RevenueEntryView
AS
	SELECT RevenueEntry.*
		 , TSB.TSBNameEN, TSB.TSBNameTH
		 , PlazaGroup.PlazaGroupNameEN, PlazaGroup.PlazaGroupNameTH, PlazaGroup.Direction
		 , [Shift].ShiftNameEN, [Shift].ShiftNameTH
	  FROM RevenueEntry
		 , TSB
	     , PlazaGroup
		 , [Shift]
	 WHERE PlazaGroup.TSBId = TSB.TSBId
	   AND RevenueEntry.TSBId = TSB.TSBId
	   AND RevenueEntry.PlazaGroupId = PlazaGroup.PlazaGroupId
	   AND RevenueEntry.ShiftId = [Shift].ShiftId
