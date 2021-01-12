/* Use in TSBCreditSummaryView */
CREATE VIEW TSBCreditUserBHTTotalSummaryView
AS
	SELECT TSB.TSBId
		 , TSB.TSBNameEN
		 , TSB.TSBNameTH
		 /* User Credit Borrow (1) - Return (2) */
		 , ((
			 SELECT (IFNULL(SUM(UserCreditTransaction.AmountST25), 0) +
					 IFNULL(SUM(UserCreditTransaction.AmountST50), 0) +
					 IFNULL(SUM(UserCreditTransaction.AmountBHT1), 0) +
					 IFNULL(SUM(UserCreditTransaction.AmountBHT2), 0) +
					 IFNULL(SUM(UserCreditTransaction.AmountBHT5), 0) +
					 IFNULL(SUM(UserCreditTransaction.AmountBHT10), 0) +
					 IFNULL(SUM(UserCreditTransaction.AmountBHT20), 0) +
					 IFNULL(SUM(UserCreditTransaction.AmountBHT50), 0) +
					 IFNULL(SUM(UserCreditTransaction.AmountBHT100), 0) +
					 IFNULL(SUM(UserCreditTransaction.AmountBHT500), 0) +
					 IFNULL(SUM(UserCreditTransaction.AmountBHT1000), 0))
			   FROM UserCreditTransaction, UserCreditBalance
			  WHERE UserCreditTransaction.TransactionType = 1 -- Borrow = 1
				AND UserCreditTransaction.UserCreditId = UserCreditBalance.UserCreditId
				AND UserCreditBalance.TSBId = TSB.TSBId
			) -
			(
			 SELECT (IFNULL(SUM(UserCreditTransaction.AmountST25), 0) +
					 IFNULL(SUM(UserCreditTransaction.AmountST50), 0) +
					 IFNULL(SUM(UserCreditTransaction.AmountBHT1), 0) +
					 IFNULL(SUM(UserCreditTransaction.AmountBHT2), 0) +
					 IFNULL(SUM(UserCreditTransaction.AmountBHT5), 0) +
					 IFNULL(SUM(UserCreditTransaction.AmountBHT10), 0) +
					 IFNULL(SUM(UserCreditTransaction.AmountBHT20), 0) +
					 IFNULL(SUM(UserCreditTransaction.AmountBHT50), 0) +
					 IFNULL(SUM(UserCreditTransaction.AmountBHT100), 0) +
					 IFNULL(SUM(UserCreditTransaction.AmountBHT500), 0) +
					 IFNULL(SUM(UserCreditTransaction.AmountBHT1000), 0))
			   FROM UserCreditTransaction, UserCreditBalance 
			  WHERE UserCreditTransaction.TransactionType = 2 -- Returns = 2
				AND UserCreditTransaction.UserCreditId = UserCreditBalance.UserCreditId
				AND UserCreditBalance.TSBId = TSB.TSBId
			)) AS UserBHTTotal
	  FROM TSB
