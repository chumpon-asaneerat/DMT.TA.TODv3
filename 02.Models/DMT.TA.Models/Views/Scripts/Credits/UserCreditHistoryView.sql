CREATE VIEW UserCreditHistoryView
AS
	SELECT C.UserCreditId
	     -- Note:
		 -- no need 'localtime' here because the UserCreateDate is already in localtime.
		 -- so strftime will convert UserCreditDate to match format that is YYYY-MM-DD
		 -- without time correctly without shift timezone.
	     , strftime('%Y-%m-%d', C.UserCreditDate) AS BagCreateDate
		 , C.UserId
		 , C.FullNameEN
		 , C.FullNameTH
		 , C.BagNo
		 , C.BeltNo
		 -- TOTAL BORROW/RETURN
		 , SUM(  B.AmountST25 + B.AmountST50
		       + B.AmountBHT1 + B.AmountBHT2 + B.AmountBHT5
		       + B.AmountBHT10 + B.AmountBHT20 + B.AmountBHT50
		       + B.AmountBHT100 + B.AmountBHT500
		       + B.AmountBHT1000) AS BorrowBHTTotal
		 , SUM(  R.AmountST25 + R.AmountST50
		       + R.AmountBHT1 + R.AmountBHT2 + R.AmountBHT5
		       + R.AmountBHT10 + R.AmountBHT20 + R.AmountBHT50
		       + R.AmountBHT100 + R.AmountBHT500
		       + R.AmountBHT1000) AS ReturnBHTTotal
		 -- BORROWS
		 , SUM(B.AmountST25) AS BorrowST25
		 , SUM(B.AmountST50) AS BorrowST50
		 , SUM(B.AmountBHT1) AS BorrowBHT1
		 , SUM(B.AmountBHT2) AS BorrowBHT2
		 , SUM(B.AmountBHT5) AS BorrowBHT5
		 , SUM(B.AmountBHT10) AS BorrowBHT10
		 , SUM(B.AmountBHT20) AS BorrowBHT20
		 , SUM(B.AmountBHT50) AS BorrowBHT50
		 , SUM(B.AmountBHT100) AS BorrowBHT100
		 , SUM(B.AmountBHT500) AS BorrowBHT500
		 , SUM(B.AmountBHT1000) AS BorrowBHT1000
		 -- RETURNS
		 , SUM(R.AmountST25) AS ReturnST25
		 , SUM(R.AmountST50) AS ReturnST50
		 , SUM(R.AmountBHT1) AS ReturnBHT1
		 , SUM(R.AmountBHT2) AS ReturnBHT2
		 , SUM(R.AmountBHT5) AS ReturnBHT5
		 , SUM(R.AmountBHT10) AS ReturnBHT10
		 , SUM(R.AmountBHT20) AS ReturnBHT20
		 , SUM(R.AmountBHT50) AS ReturnBHT50
		 , SUM(R.AmountBHT100) AS ReturnBHT100
		 , SUM(R.AmountBHT500) AS ReturnBHT500
		 , SUM(R.AmountBHT1000) AS ReturnBHT1000
		 , C.TSBId
		 , C.TSBNameEN
		 , C.TSBNameTH
		 , C.PlazaGroupId
		 , C.PlazaGroupNameEN
		 , C.PlazaGroupNameTH
	  FROM UserCreditBalanceSSView C
	     , UserCreditBorrowSummaryView B
	     , UserCreditReturnSummaryView R
	 WHERE B.UserCreditId = C.UserCreditId
	   AND R.UserCreditId = C.UserCreditId
	 GROUP BY C.UserCreditId
	     , BagCreateDate
	 ORDER BY C.UserCreditId
	        , BagCreateDate
