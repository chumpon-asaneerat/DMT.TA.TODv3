/* USE IN UserCreditBorrowSummaryView */
CREATE VIEW UserCreditBorrowSummaryView
AS
	SELECT UserCreditBalance.* 
		 , TSB.TSBNameEN
		 , TSB.TSBNameTH
		 , PlazaGroup.PlazaGroupNameEN, PlazaGroup.PlazaGroupNameTH, PlazaGroup.Direction 
		 , (
			 SELECT IFNULL(SUM(AmountST25), 0) 
			   FROM UserCreditTransaction 
			  WHERE UserCreditTransaction.TransactionType = 1 -- Borrow = 1
				AND UserCreditTransaction.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountST25
		 , (
			 SELECT IFNULL(SUM(AmountST50), 0) 
			   FROM UserCreditTransaction 
			  WHERE UserCreditTransaction.TransactionType = 1 -- Borrow = 1
				AND UserCreditTransaction.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountST50
		 , (
			 SELECT IFNULL(SUM(AmountBHT1), 0) 
			   FROM UserCreditTransaction 
			  WHERE UserCreditTransaction.TransactionType = 1 -- Borrow = 1
				AND UserCreditTransaction.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountBHT1
		 , (
			 SELECT IFNULL(SUM(AmountBHT2), 0) 
			   FROM UserCreditTransaction 
			  WHERE UserCreditTransaction.TransactionType = 1 -- Borrow = 1
				AND UserCreditTransaction.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountBHT2
		 , (
			 SELECT IFNULL(SUM(AmountBHT5), 0) 
			   FROM UserCreditTransaction 
			  WHERE UserCreditTransaction.TransactionType = 1 -- Borrow = 1
				AND UserCreditTransaction.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountBHT5
		 , (
			 SELECT IFNULL(SUM(AmountBHT10), 0) 
			   FROM UserCreditTransaction 
			  WHERE UserCreditTransaction.TransactionType = 1 -- Borrow = 1
				AND UserCreditTransaction.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountBHT10
		 , (
			 SELECT IFNULL(SUM(AmountBHT20), 0) 
			   FROM UserCreditTransaction 
			  WHERE UserCreditTransaction.TransactionType = 1 -- Borrow = 1
				AND UserCreditTransaction.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountBHT20
		 , (
			 SELECT IFNULL(SUM(AmountBHT50), 0) 
			   FROM UserCreditTransaction 
			  WHERE UserCreditTransaction.TransactionType = 1 -- Borrow = 1
				AND UserCreditTransaction.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountBHT50
		 , (
			 SELECT IFNULL(SUM(AmountBHT100), 0) 
			   FROM UserCreditTransaction 
			  WHERE UserCreditTransaction.TransactionType = 1 -- Borrow = 1
				AND UserCreditTransaction.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountBHT100
		 , (
			 SELECT IFNULL(SUM(AmountBHT500), 0) 
			   FROM UserCreditTransaction 
			  WHERE UserCreditTransaction.TransactionType = 1 -- Borrow = 1
				AND UserCreditTransaction.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountBHT500
		 , (
			 SELECT IFNULL(SUM(AmountBHT1000), 0) 
			   FROM UserCreditTransaction 
			  WHERE UserCreditTransaction.TransactionType = 1 -- Borrow = 1
				AND UserCreditTransaction.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountBHT1000
	  FROM UserCreditBalance, TSB, PlazaGroup
	 WHERE PlazaGroup.TSBId = TSB.TSBId 
	   AND UserCreditBalance.TSBId = TSB.TSBId 
	   AND UserCreditBalance.PlazaGroupId = PlazaGroup.PlazaGroupId 
