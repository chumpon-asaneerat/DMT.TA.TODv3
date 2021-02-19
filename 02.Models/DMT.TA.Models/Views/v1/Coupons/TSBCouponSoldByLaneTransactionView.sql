-- Stock = 1, Lane = 2, SoldByLane = 3, SoldByTSB = 4
CREATE VIEW TSBCouponSoldByLaneTransactionView
AS
	SELECT TransactionType, TransactionDate
	     , CouponId, CouponType, Price
		 , FinishFlag
		 , UserId , FullNameEN, FullNameTH
		 , UserReceiveDate
		 , SoldBy, SoldByFullNameEN, SoldByFullNameTH
		 , SoldDate
	 FROM TSBCouponTransaction 
	WHERE TransactionType = 3 -- SoldByLane = 3
	  AND FinishFlag = 0 -- Completed
