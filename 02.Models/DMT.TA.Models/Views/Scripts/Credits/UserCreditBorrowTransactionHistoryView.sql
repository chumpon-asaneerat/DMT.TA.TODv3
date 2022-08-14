CREATE VIEW UserCreditBorrowTransactionHistoryView
AS
	SELECT strftime('%Y-%m-%d', UCTB.TransactionDate) AS TransDate
		 , UCTB.UserId
		 , UCTB.FullNameEN
		 , UCTB.FullNameTH
		 , UCB.BagNo
		 , UCB.BeltNo
		 , UCB.ReceivedDate
		 , UCB.Canceled
		 , UCB.CancelDate
		 , UCB.CancelUserId
		 , UCB.CancelFullNameEN
		 , UCB.CancelFullNameTH
		 , UCB.RevenueId
		 , UCB.RevenueBagNo
		 , UCB.RevenueBeltNo
		 , (
			  IFNULL(SUM(UCTB.AmountST25), 0)
			+ IFNULL(SUM(UCTB.AmountST50), 0)
			+ IFNULL(SUM(UCTB.AmountBHT1), 0)
			+ IFNULL(SUM(UCTB.AmountBHT2), 0)
			+ IFNULL(SUM(UCTB.AmountBHT5), 0)
			+ IFNULL(SUM(UCTB.AmountBHT10), 0)
			+ IFNULL(SUM(UCTB.AmountBHT20), 0)
			+ IFNULL(SUM(UCTB.AmountBHT50), 0)
			+ IFNULL(SUM(UCTB.AmountBHT100), 0)
			+ IFNULL(SUM(UCTB.AmountBHT500), 0)
			+ IFNULL(SUM(UCTB.AmountBHT1000), 0)
		   ) AS BorrowBHTTotal
		 , IFNULL(SUM(UCTB.AmountST25), 0) AS BorrowST25
		 , IFNULL(SUM(UCTB.AmountST50), 0) AS BorrowST50
		 , IFNULL(SUM(UCTB.AmountBHT1), 0) AS BorrowBHT1
		 , IFNULL(SUM(UCTB.AmountBHT2), 0) AS BorrowBHT2
		 , IFNULL(SUM(UCTB.AmountBHT5), 0) AS BorrowBHT5
		 , IFNULL(SUM(UCTB.AmountBHT10), 0) AS BorrowBHT10
		 , IFNULL(SUM(UCTB.AmountBHT20), 0) AS BorrowBHT20
		 , IFNULL(SUM(UCTB.AmountBHT50), 0) AS BorrowBHT50
		 , IFNULL(SUM(UCTB.AmountBHT100), 0) AS BorrowBHT100
		 , IFNULL(SUM(UCTB.AmountBHT500), 0) AS BorrowBHT500
		 , IFNULL(SUM(UCTB.AmountBHT1000), 0) AS BorrowBHT1000
		 , UCTB.UserCreditId
		 , UCB.CreditDate
		 , UCB.UserCreditDate
		 , UCB.TSBId
		 , UCB.TSBNameEN
		 , UCB.TSBNameTH
		 , UCB.PlazaGroupId
		 , UCB.PlazaGroupNameEN
		 , UCB.PlazaGroupNameTH
		 , UCB.ShiftId
		 , UCB.ShiftNameEN
		 , UCB.ShiftNameTH
	  FROM UserCreditTransaction UCTB
		 , UserCreditBalanceSSView UCB
	 WHERE UCTB.TransactionType = 1 -- Borrow = 1
	   AND UCB.UserCreditId = UCTB.UserCreditId
	 GROUP BY TransDate
			, UCTB.UserId
			, UCB.BagNo
			, UCB.BeltNo
		    , UCB.ReceivedDate
		    , UCB.Canceled
		    , UCB.CancelDate
		    , UCB.CancelUserId
		    , UCB.CancelFullNameEN
		    , UCB.CancelFullNameTH
		    , UCB.RevenueId
		    , UCB.RevenueBagNo
		    , UCB.RevenueBeltNo
			, UCTB.UserCreditId
			, UCB.CreditDate
			, UCB.UserCreditDate
			, UCB.TSBId
			, UCB.TSBNameEN
			, UCB.TSBNameTH
			, UCB.PlazaGroupId
			, UCB.PlazaGroupNameEN
			, UCB.PlazaGroupNameTH
		    , UCB.ShiftId
		    , UCB.ShiftNameEN
		    , UCB.ShiftNameTH
	 ORDER BY UCTB.TransactionDate
