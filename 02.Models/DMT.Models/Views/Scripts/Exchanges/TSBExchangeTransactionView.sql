CREATE VIEW TSBExchangeTransactionView
AS
	-- Transaction Types:
	-- Request = 1
	-- Canceled = 2
	-- Approve = 3
	-- Reject = 4
	-- Received = 5
	-- Exchange = 6 
	-- Return = 7
	-- Completed = 9
	SELECT TSBExchangeTransaction.* 
		 , TSB.TSBNameEN
		 , TSB.TSBNameTH
		 --, UserView.FullNameEN, UserView.FullNameTH
	  FROM TSBExchangeTransaction
		 , TSB
		 --, UserView
	 WHERE TSBExchangeTransaction.TSBId = TSB.TSBId
	   --AND TSBExchangeTransaction.UserId = UserView.UserId
