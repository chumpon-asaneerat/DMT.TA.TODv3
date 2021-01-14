CREATE VIEW TSBExchangeGroupView
AS
	-- State Types:
	-- Request = 1
	-- Canceled = 2
	-- Approve = 3
	-- Reject = 4
	-- Received = 5
	-- Exchange = 6 
	-- Return = 7
	-- Completed = 9
	SELECT TSBExchangeGroup.* 
		 , TSB.TSBNameEN
		 , TSB.TSBNameTH
		 --, UserView.FullNameEN, UserView.FullNameTH
		 , TSBExchangeTransaction.TransactionId
		 , TSBExchangeTransaction.TransactionType
		 , TSBExchangeTransaction.TransactionDate
		 , TSBExchangeTransaction.UserId
		 , TSBExchangeTransaction.FullNameEN
		 , TSBExchangeTransaction.FullNameTH
		 , TSBExchangeTransaction.ExchangeBHT
		 , TSBExchangeTransaction.BorrowBHT
		 , TSBExchangeTransaction.AdditionalBHT
		 , TSBExchangeTransaction.PeriodBegin
		 , TSBExchangeTransaction.PeriodEnd
		 , TSBExchangeTransaction.Remark
	  FROM TSB
		 , TSBExchangeGroup
		 , TSBExchangeTransaction
		 --, UserView
	 WHERE TSBExchangeGroup.TSBId = TSB.TSBId
	   AND TSBExchangeTransaction.TSBId = TSB.TSBId
	   --AND TSBExchangeTransaction.UserId = UserView.UserId
	   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
	   AND TSBExchangeTransaction.TransactionType = 1 -- Request = 1

