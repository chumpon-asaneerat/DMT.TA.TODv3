-- Stock = 1, Lane = 2, SoldByLane = 3, SoldByTSB = 4
CREATE VIEW TSBCouponSoldByTSBTransactionView
AS
	SELECT TransactionType, TransactionDate
	     , CouponId, CouponType, Price
		 , FinishFlag
		 , UserId , FullNameEN, FullNameTH
		 , UserReceiveDate
		 , SoldBy, SoldByFullNameEN, SoldByFullNameTH
		 , SoldDate
	 FROM TSBCouponTransaction 
	WHERE TransactionType = 4 -- SoldByTSB = 4
	  AND FinishFlag = 0 -- Completed
