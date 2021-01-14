CREATE VIEW PlazaGroupView
AS
	SELECT PlazaGroup.* 
		 , TSB.TSBNameEN, TSB.TSBNameTH
	  FROM PlazaGroup
		 , TSB
	 WHERE PlazaGroup.TSBId = TSB.TSBId
