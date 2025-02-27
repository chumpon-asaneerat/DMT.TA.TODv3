﻿CREATE VIEW TSBExchangeGroupView
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
		 -- REQUEST
		 , (
		    SELECT UserId 
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 1 -- Request = 1
		   ) AS RequestUserID
		 , (
		    SELECT FullNameEN 
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 1 -- Request = 1
		   ) AS RequestFullNameEN
		 , (
		    SELECT FullNameTH 
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 1 -- Request = 1
		   ) AS RequestFullNameTH
		 , (
		    SELECT ExchangeBHT 
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 1 -- Request = 1
		   ) AS RequestExchangeBHT
		 , (
		    SELECT BorrowBHT
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 1 -- Request = 1
		   ) AS RequestBorrowBHT
		 , (
		    SELECT AdditionalBHT
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 1 -- Request = 1
		   ) AS RequestAdditionalBHT
		 , (
		    SELECT PeriodBegin
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 1 -- Request = 1
		   ) AS RequestPeriodBegin
		 , (
		    SELECT PeriodEnd
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 1 -- Request = 1
		   ) AS RequestPeriodEnd
		 , (
		    SELECT Remark
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 1 -- Request = 1
		   ) AS RequestRemark
		 -- APPROVE
		 , (
		    SELECT ExchangeBHT 
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 3 -- Approve = 3
		   ) AS ApproveExchangeBHT
		 , (
		    SELECT BorrowBHT
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 3 -- Approve = 3
		   ) AS ApproveBorrowBHT
		 , (
		    SELECT AdditionalBHT
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 3 -- Approve = 3
		   ) AS ApproveAdditionalBHT
		 , (
		    SELECT PeriodBegin
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 3 -- Approve = 3
		   ) AS ApprovePeriodBegin
		 , (
		    SELECT PeriodEnd
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 3 -- Approve = 3
		   ) AS ApprovePeriodEnd
		 , (
		    SELECT Remark
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 3 -- Approve = 3
		   ) AS ApproveRemark
		 -- RECEIVE
		 , (
		    SELECT ExchangeBHT 
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 5 -- Received = 5
		   ) AS ReceiveExchangeBHT
		 , (
		    SELECT BorrowBHT
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 5 -- Received = 5
		   ) AS ReceiveBorrowBHT
		 , (
		    SELECT AdditionalBHT
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 5 -- Received = 5
		   ) AS ReceiveAdditionalBHT
		 , (
		    SELECT PeriodBegin
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 5 -- Received = 5
		   ) AS ReceivePeriodBegin
		 , (
		    SELECT PeriodEnd
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 5 -- Received = 5
		   ) AS ReceivePeriodEnd
		 , (
		    SELECT Remark
		      FROM TSBExchangeTransaction 
		     WHERE TSBExchangeTransaction.TSBId = TSB.TSBId 
			   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
			   AND TSBExchangeTransaction.TransactionType = 5 -- Received = 5
		   ) AS ReceiveRemark
	  FROM TSB
		 , TSBExchangeGroup
		 , TSBExchangeTransaction
		 --, UserView
	 WHERE TSBExchangeGroup.TSBId = TSB.TSBId
	   AND TSBExchangeTransaction.TSBId = TSB.TSBId
	   --AND TSBExchangeTransaction.UserId = UserView.UserId
	   AND TSBExchangeTransaction.GroupId = TSBExchangeGroup.GroupId
	   --AND TSBExchangeTransaction.TransactionType = 1 -- Request = 1
	   AND TSBExchangeTransaction.TransactionType = TSBExchangeGroup.[State]
