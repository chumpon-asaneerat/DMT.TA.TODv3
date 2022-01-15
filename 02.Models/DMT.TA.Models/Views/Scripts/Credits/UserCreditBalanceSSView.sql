CREATE VIEW UserCreditBalanceSSView
AS
	SELECT UCB.UserCreditId
		 , UCB.UserCreditDate
		 , UCB.UserId
		 , UCB.FullNameEN
		 , UCB.FullNameTH
		 , UCB.BagNo
		 , UCB.BeltNo
		 , UCB.RevenueId
		 , TSS.UserId AS ChiefId
		 , TSS.FullNameEN AS ChiefNameEN
		 , TSS.FullNameTH AS ChiefNameTH
		 , TSS.ShiftId AS ChiefShiftId
		 , TSS.[Begin] AS ChiefShiftBegin
		 , TSS.[End] AS ChiefShiftEnd
		 , UCB.TSBId
		 , TSB.TSBNameEN
		 , TSB.TSBNameTH
		 , UCB.PlazaGroupId
		 , PlazaGroup.PlazaGroupNameEN
		 , PlazaGroup.PlazaGroupNameTH
		 , TSS.TSBShiftId
	  FROM TSB
		 , PlazaGroup
		 , UserCreditBalance UCB LEFT JOIN 
		   TSBShiftSSView TSS ON 
		   (
				 UCB.UserCreditDate >= TSS.[Begin]
			 AND UCB.UserCreditDate <  TSS.[End]
			 AND UCB.TSBId = TSS.TSBId
		   )
	 WHERE UCB.TSBId = TSB.TSBId
	   AND UCB.PlazaGroupId = PlazaGroup.PlazaGroupId
