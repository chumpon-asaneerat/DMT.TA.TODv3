/* Use in TSBCreditSummaryView */
CREATE VIEW TSBCreditBHT2SummaryView
AS
	SELECT TSB.TSBId
		 , TSB.TSBNameEN
		 , TSB.TSBNameTH
		 /* TSB Credit Initial (0) + Received (1) +  ReplaceIn (12) - Exchange (2) - Return (3) - ReplaceOut (11) */
		 , ((
			 SELECT IFNULL(SUM(AmountBHT2), 0) 
			   FROM TSBCreditTransaction 
			  WHERE (   TSBCreditTransaction.TransactionType = 0 
					 OR TSBCreditTransaction.TransactionType = 1
					 OR TSBCreditTransaction.TransactionType = 12
					) -- Initial = 0, Received = 1, Replace In = 12
				AND TSBCreditTransaction.TSBId = TSB.TSBId
			) -
			(
			 SELECT IFNULL(SUM(AmountBHT2), 0) 
			   FROM TSBCreditTransaction 
			  WHERE (   TSBCreditTransaction.TransactionType = 2
					 OR TSBCreditTransaction.TransactionType = 3
					 OR TSBCreditTransaction.TransactionType = 11
			        ) -- Exchange = 2, Returns = 3, Replace Out = 11
				AND TSBCreditTransaction.TSBId = TSB.TSBId
			) - 
			(
			 SELECT IFNULL(SUM(UserCreditTransaction.AmountBHT2), 0) 
			   FROM UserCreditTransaction, UserCreditBalance 
			  WHERE UserCreditBalance.TSBId = TSB.TSBId
				AND UserCreditTransaction.TransactionType = 1 -- Borrow
				AND UserCreditTransaction.UserCreditId = UserCreditBalance.UserCreditId
			) + 
			(
			 SELECT IFNULL(SUM(UserCreditTransaction.AmountBHT2), 0) 
			   FROM UserCreditTransaction, UserCreditBalance 
			  WHERE UserCreditBalance.TSBId = TSB.TSBId
				AND UserCreditTransaction.TransactionType = 2 -- Return
				AND UserCreditTransaction.UserCreditId = UserCreditBalance.UserCreditId
			)) AS AmountBHT2
	  FROM TSB
