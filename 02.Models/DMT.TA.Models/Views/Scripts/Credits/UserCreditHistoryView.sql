CREATE VIEW UserCreditHistoryView
AS
	SELECT STRFTIME('%Y-%m-%d', B.UserCreditDate) AS CreditDate
		 , B.UserId
		 , B.FullNameEN
		 , B.FullNameTH
		 , B.TSBId
		 , B.TSBNameEN
		 , B.TSBNameTH
		 , B.PlazaGroupId
		 , B.PlazaGroupNameEN, B.PlazaGroupNameTH, B.Direction 
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
	  FROM UserCreditBorrowSummaryView B,
		   UserCreditReturnSummaryView R
	 WHERE B.TSBId = R.TSBId
	   AND B.PlazaGroupId = R.PlazaGroupId
	   AND B.UserId = R.UserId
	   AND STRFTIME('%Y-%m-%d %H:%M:%S.%f', B.UserCreditDate) >= STRFTIME('%Y-%m-%d 00:00:00.000', B.UserCreditDate)
	   AND STRFTIME('%Y-%m-%d %H:%M:%S.%f', B.UserCreditDate) <= STRFTIME('%Y-%m-%d 23:59:59.999', B.UserCreditDate)
	   AND STRFTIME('%Y-%m-%d %H:%M:%S.%f', R.UserCreditDate) >= STRFTIME('%Y-%m-%d 00:00:00.000', R.UserCreditDate)
	   AND STRFTIME('%Y-%m-%d %H:%M:%S.%f', R.UserCreditDate) <= STRFTIME('%Y-%m-%d 23:59:59.999', R.UserCreditDate)
	 GROUP BY B.TSBId
		 , B.PlazaGroupId
		 , STRFTIME('%Y-%m-%d', B.UserCreditDate)
		 , B.UserId
	 ORDER BY B.TSBId
		 , B.PlazaGroupId
		 , B.UserCreditDate
		 , B.UserId
   