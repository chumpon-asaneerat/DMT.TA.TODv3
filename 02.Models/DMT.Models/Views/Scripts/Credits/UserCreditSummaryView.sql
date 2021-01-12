CREATE VIEW UserCreditSummaryView
AS
	SELECT UserCreditBalance.* 
		 , TSB.TSBNameEN
		 , TSB.TSBNameTH
		 , PlazaGroup.PlazaGroupNameEN, PlazaGroup.PlazaGroupNameTH, PlazaGroup.Direction 
		 , (
			 SELECT UserCreditBorrowSummaryView.AmountST25 - UserCreditReturnSummaryView.AmountST25
			   FROM UserCreditBorrowSummaryView, UserCreditReturnSummaryView 
			  WHERE UserCreditBorrowSummaryView.UserCreditId = UserCreditBalance.UserCreditId
				AND UserCreditReturnSummaryView.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountST25
		 , (
			 SELECT UserCreditBorrowSummaryView.AmountST50 - UserCreditReturnSummaryView.AmountST50
			   FROM UserCreditBorrowSummaryView, UserCreditReturnSummaryView 
			  WHERE UserCreditBorrowSummaryView.UserCreditId = UserCreditBalance.UserCreditId
				AND UserCreditReturnSummaryView.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountST50
		 , (
			 SELECT UserCreditBorrowSummaryView.AmountBHT1 - UserCreditReturnSummaryView.AmountBHT1
			   FROM UserCreditBorrowSummaryView, UserCreditReturnSummaryView 
			  WHERE UserCreditBorrowSummaryView.UserCreditId = UserCreditBalance.UserCreditId
				AND UserCreditReturnSummaryView.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountBHT1
		 , (
			 SELECT UserCreditBorrowSummaryView.AmountBHT2 - UserCreditReturnSummaryView.AmountBHT2
			   FROM UserCreditBorrowSummaryView, UserCreditReturnSummaryView 
			  WHERE UserCreditBorrowSummaryView.UserCreditId = UserCreditBalance.UserCreditId
				AND UserCreditReturnSummaryView.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountBHT2
		 , (
			 SELECT UserCreditBorrowSummaryView.AmountBHT5 - UserCreditReturnSummaryView.AmountBHT5
			   FROM UserCreditBorrowSummaryView, UserCreditReturnSummaryView 
			  WHERE UserCreditBorrowSummaryView.UserCreditId = UserCreditBalance.UserCreditId
				AND UserCreditReturnSummaryView.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountBHT5
		 , (
			 SELECT UserCreditBorrowSummaryView.AmountBHT10 - UserCreditReturnSummaryView.AmountBHT10
			   FROM UserCreditBorrowSummaryView, UserCreditReturnSummaryView 
			  WHERE UserCreditBorrowSummaryView.UserCreditId = UserCreditBalance.UserCreditId
				AND UserCreditReturnSummaryView.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountBHT10
		 , (
			 SELECT UserCreditBorrowSummaryView.AmountBHT20 - UserCreditReturnSummaryView.AmountBHT20
			   FROM UserCreditBorrowSummaryView, UserCreditReturnSummaryView 
			  WHERE UserCreditBorrowSummaryView.UserCreditId = UserCreditBalance.UserCreditId
				AND UserCreditReturnSummaryView.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountBHT20
		 , (
			 SELECT UserCreditBorrowSummaryView.AmountBHT50 - UserCreditReturnSummaryView.AmountBHT50
			   FROM UserCreditBorrowSummaryView, UserCreditReturnSummaryView 
			  WHERE UserCreditBorrowSummaryView.UserCreditId = UserCreditBalance.UserCreditId
				AND UserCreditReturnSummaryView.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountBHT50
		 , (
			 SELECT UserCreditBorrowSummaryView.AmountBHT100 - UserCreditReturnSummaryView.AmountBHT100
			   FROM UserCreditBorrowSummaryView, UserCreditReturnSummaryView 
			  WHERE UserCreditBorrowSummaryView.UserCreditId = UserCreditBalance.UserCreditId
				AND UserCreditReturnSummaryView.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountBHT100
		 , (
			 SELECT UserCreditBorrowSummaryView.AmountBHT500 - UserCreditReturnSummaryView.AmountBHT500
			   FROM UserCreditBorrowSummaryView, UserCreditReturnSummaryView 
			  WHERE UserCreditBorrowSummaryView.UserCreditId = UserCreditBalance.UserCreditId
				AND UserCreditReturnSummaryView.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountBHT500
		 , (
			 SELECT UserCreditBorrowSummaryView.AmountBHT1000 - UserCreditReturnSummaryView.AmountBHT1000
			   FROM UserCreditBorrowSummaryView, UserCreditReturnSummaryView 
			  WHERE UserCreditBorrowSummaryView.UserCreditId = UserCreditBalance.UserCreditId
				AND UserCreditReturnSummaryView.UserCreditId = UserCreditBalance.UserCreditId
			) AS AmountBHT1000
	  FROM UserCreditBalance, TSB, PlazaGroup
	 WHERE PlazaGroup.TSBId = TSB.TSBId 
	   AND UserCreditBalance.TSBId = TSB.TSBId 
	   AND UserCreditBalance.PlazaGroupId = PlazaGroup.PlazaGroupId 
