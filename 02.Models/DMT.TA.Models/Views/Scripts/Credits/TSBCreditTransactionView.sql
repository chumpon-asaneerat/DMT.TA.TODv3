CREATE VIEW TSBCreditTransactionView
AS
	SELECT TSBCreditTransaction.*
	     , TSB.TSBNameEN, TSB.TSBNameTH
	  FROM TSBCreditTransaction
	     , TSB
	 WHERE TSBCreditTransaction.TSBId = TSB.TSBId