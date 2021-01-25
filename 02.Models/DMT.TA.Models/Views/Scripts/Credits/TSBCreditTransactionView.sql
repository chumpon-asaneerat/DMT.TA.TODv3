CREATE VIEW TSBCreditTransactionView
AS
	SELECT TSBCreditTransaction.*
	     , TSB.TSBNameEN, TSB.TSBNameTH
		 , TSB.MaxCredit
		 , TSB.LowLimitST25, TSB.LowLimitST50
		 , TSB.LowLimitBHT1, TSB.LowLimitBHT2, TSB.LowLimitBHT5
		 , TSB.LowLimitBHT10, TSB.LowLimitBHT20, TSB.LowLimitBHT50
		 , TSB.LowLimitBHT100, TSB.LowLimitBHT500, TSB.LowLimitBHT1000
	  FROM TSBCreditTransaction
	     , TSB
	 WHERE TSBCreditTransaction.TSBId = TSB.TSBId