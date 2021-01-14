CREATE VIEW PlazaView
AS
	SELECT Plaza.*
		 , TSB.TSBNameEN, TSB.TSBNameTH
		 , PlazaGroup.PlazaGroupNameEN, PlazaGroup.PlazaGroupNameTH, PlazaGroup.Direction
	  FROM Plaza
	     , PlazaGroup
		 , TSB
	 WHERE PlazaGroup.TSBId = TSB.TSBId
	   AND Plaza.TSBId = TSB.TSBId
	   AND Plaza.PlazaGroupId = PlazaGroup.PlazaGroupId
