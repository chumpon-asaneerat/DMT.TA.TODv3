CREATE VIEW LaneView
AS
	SELECT DISTINCT Lane.*
		 , TSB.TSBNameEN, TSB.TSBNameTH
		 , PlazaGroup.PlazaGroupNameEN, PlazaGroup.PlazaGroupNameTH, PlazaGroup.Direction
		 , Plaza.SCWPlazaId, Plaza.PlazaNameEN, Plaza.PlazaNameTH
	  FROM Lane
	     , Plaza
	     , PlazaGroup
		 , TSB
	 WHERE PlazaGroup.TSBId = TSB.TSBId
	   AND Plaza.TSBId = TSB.TSBId
	   AND Plaza.PlazaGroupId = PlazaGroup.PlazaGroupId
	   AND Lane.TSBId = TSB.TSBId
	   AND Lane.PlazaGroupId = Plaza.PlazaGroupId
	   AND Lane.PlazaId = Plaza.PlazaId
