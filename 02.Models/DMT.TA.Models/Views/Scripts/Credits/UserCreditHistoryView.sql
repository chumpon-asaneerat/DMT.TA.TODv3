CREATE VIEW UserCreditHistoryView
AS
	SELECT C.UserCreditId
		 , C.TransDate
		 , C.UserId
		 , C.FullNameEN
		 , C.FullNameTH
		 , C.BagNo
		 , C.BeltNo
		 -- TOTAL BORROW/RETURN
		 , IFNULL(SUM(B.BorrowBHTTotal), 0) AS BorrowBHTTotal
		 , IFNULL(SUM(R.ReturnBHTTotal), 0) AS ReturnBHTTotal
		 -- BORROWS
		 , IFNULL(SUM(B.BorrowST25), 0) AS BorrowST25
		 , IFNULL(SUM(B.BorrowST50), 0) AS BorrowST50
		 , IFNULL(SUM(B.BorrowBHT1), 0) AS BorrowBHT1
		 , IFNULL(SUM(B.BorrowBHT2), 0) AS BorrowBHT2
		 , IFNULL(SUM(B.BorrowBHT5), 0) AS BorrowBHT5
		 , IFNULL(SUM(B.BorrowBHT10), 0) AS BorrowBHT10
		 , IFNULL(SUM(B.BorrowBHT20), 0) AS BorrowBHT20
		 , IFNULL(SUM(B.BorrowBHT50), 0) AS BorrowBHT50
		 , IFNULL(SUM(B.BorrowBHT100), 0) AS BorrowBHT100
		 , IFNULL(SUM(B.BorrowBHT500), 0) AS BorrowBHT500
		 , IFNULL(SUM(B.BorrowBHT1000), 0) AS BorrowBHT1000
		 -- RETURNS
		 , IFNULL(SUM(R.ReturnST25), 0) AS ReturnST25
		 , IFNULL(SUM(R.ReturnST50), 0) AS ReturnST50
		 , IFNULL(SUM(R.ReturnBHT1), 0) AS ReturnBHT1
		 , IFNULL(SUM(R.ReturnBHT2), 0) AS ReturnBHT2
		 , IFNULL(SUM(R.ReturnBHT5), 0) AS ReturnBHT5
		 , IFNULL(SUM(R.ReturnBHT10), 0) AS ReturnBHT10
		 , IFNULL(SUM(R.ReturnBHT20), 0) AS ReturnBHT20
		 , IFNULL(SUM(R.ReturnBHT50), 0) AS ReturnBHT50
		 , IFNULL(SUM(R.ReturnBHT100), 0) AS ReturnBHT100
		 , IFNULL(SUM(R.ReturnBHT500), 0) AS ReturnBHT500
		 , IFNULL(SUM(R.ReturnBHT1000), 0) AS ReturnBHT1000
	     , C.CreditDate AS BagCreateDate
		 , C.TSBId
		 , C.TSBNameEN
		 , C.TSBNameTH
		 , C.PlazaGroupId
		 , C.PlazaGroupNameEN
		 , C.PlazaGroupNameTH
		 , C.ShiftId
		 , C.ShiftNameEN
		 , C.ShiftNameTH
	  FROM UserCreditBaseTransactionHistoryView C 
	       LEFT JOIN UserCreditBorrowTransactionHistoryView B 
		         ON (
				          B.UserCreditId = C.UserCreditId 
					  AND B.TransDate = C.TransDate
					)
	       LEFT JOIN UserCreditReturnTransactionHistoryView R 
		         ON (
				          R.UserCreditId = C.UserCreditId 
					  AND R.TransDate = C.TransDate
				    )
	 GROUP BY C.TransDate
		 , C.UserCreditId
	 ORDER BY C.TransDate
	        , C.UserCreditId
