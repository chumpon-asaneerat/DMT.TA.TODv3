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
		 , TSBExchangeGroup.RequestDate
		 , req.UserId AS  RequestUserId
		 , req.FullNameEN AS  RequestFullNameEN
		 , req.FullNameTH AS  RequestFullNameTH
	  FROM TSBExchangeTransaction
		 , TSB
		 --, UserView
		 , TSBExchangeGroup
		 , TSBExchangeTransaction req
	 WHERE TSBExchangeTransaction.TSBId = TSB.TSBId
	   AND TSBExchangeGroup.TSBId = TSB.TSBId
	   --AND TSBExchangeTransaction.UserId = UserView.UserId
	   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
	   AND req.TSBId = TSB.TSBId
	   AND req.GroupId = TSBExchangeGroup.GroupId
	   AND req.TransactionType = 1 -- Request = 1
