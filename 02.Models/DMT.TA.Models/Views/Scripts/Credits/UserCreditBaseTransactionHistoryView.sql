CREATE VIEW UserCreditBaseTransactionHistoryView
AS
	SELECT strftime('%Y-%m-%d', UCTB.TransactionDate) AS TransDate
		 , UCTB.UserId
		 , UCTB.FullNameEN
		 , UCTB.FullNameTH
		 , UCB.BagNo
		 , UCB.BeltNo
		 , UCTB.UserCreditId
		 , UCB.CreditDate
		 , UCB.UserCreditDate
		 , UCB.TSBId
		 , UCB.TSBNameEN
		 , UCB.TSBNameTH
		 , UCB.PlazaGroupId
		 , UCB.PlazaGroupNameEN
		 , UCB.PlazaGroupNameTH
		 , UCB.ShiftId
		 , UCB.ShiftNameEN
		 , UCB.ShiftNameTH
	  FROM UserCreditTransaction UCTB
		 , UserCreditBalanceSSView UCB
	 WHERE UCB.UserCreditId = UCTB.UserCreditId
	 GROUP BY TransDate
			, UCTB.UserId
			, UCB.BagNo
			, UCB.BeltNo
			, UCTB.UserCreditId
			, UCB.CreditDate
			, UCB.UserCreditDate
			, UCB.TSBId
			, UCB.TSBNameEN
			, UCB.TSBNameTH
			, UCB.PlazaGroupId
			, UCB.PlazaGroupNameEN
			, UCB.PlazaGroupNameTH
		    , UCB.ShiftId
		    , UCB.ShiftNameEN
		    , UCB.ShiftNameTH
	 ORDER BY UCTB.TransactionDate
