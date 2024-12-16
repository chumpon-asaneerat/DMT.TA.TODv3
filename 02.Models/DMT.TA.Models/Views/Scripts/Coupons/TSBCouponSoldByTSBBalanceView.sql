CREATE VIEW TSBCouponSoldByTSBBalanceView
AS
	-- Stock = 1, Lane = 2, SoldByLane = 3, SoldByTSB = 4
	SELECT TSB.TSBId, TSB.TSBNameEN, TSB.TSBNameTH
		 , (
		     -- Count No of Coupon 35
			 SELECT IFNULL(COUNT(*), 0) 
			   FROM TSBCouponTransaction 
			  WHERE TSBCouponTransaction.TransactionType = 4 -- SoldByTSB = 4
				AND TSBCouponTransaction.TSBId = TSB.TSBId
				AND TSBCouponTransaction.CouponType = 35
				AND TSBCouponTransaction.FinishFlag = 0 -- Completed
		   ) AS CouponBHT35
		 , (
		     -- Count No of Coupon 40
			 SELECT IFNULL(COUNT(*), 0) 
			   FROM TSBCouponTransaction 
			  WHERE TSBCouponTransaction.TransactionType = 4 -- SoldByTSB = 4
				AND TSBCouponTransaction.TSBId = TSB.TSBId
				AND TSBCouponTransaction.CouponType = 40
				AND TSBCouponTransaction.FinishFlag = 0 -- Completed
		   ) AS CouponBHT40
		 , (
		     -- Count No of Coupon 80
			 SELECT IFNULL(COUNT(*), 0) 
			   FROM TSBCouponTransaction 
			  WHERE TSBCouponTransaction.TransactionType = 4 -- SoldByTSB = 4
				AND TSBCouponTransaction.TSBId = TSB.TSBId
				AND TSBCouponTransaction.CouponType = 80
				AND TSBCouponTransaction.FinishFlag = 0 -- Completed
		   ) AS CouponBHT80
		 , (
		     -- Count No of Coupon 90
			 SELECT IFNULL(COUNT(*), 0) 
			   FROM TSBCouponTransaction 
			  WHERE TSBCouponTransaction.TransactionType = 4 -- SoldByTSB = 4
				AND TSBCouponTransaction.TSBId = TSB.TSBId
				AND TSBCouponTransaction.CouponType = 90
				AND TSBCouponTransaction.FinishFlag = 0 -- Completed
		   ) AS CouponBHT90
		 , (
		     -- Calc Price of Coupon 35
			 SELECT IFNULL(SUM(Price), 0) 
			   FROM TSBCouponTransaction 
			  WHERE TSBCouponTransaction.TransactionType = 4 -- SoldByTSB = 4
				AND TSBCouponTransaction.TSBId = TSB.TSBId
				AND TSBCouponTransaction.CouponType = 35
				AND TSBCouponTransaction.FinishFlag = 0 -- Completed
		   ) AS PriceBHT35
		 , (
		     -- Calc Price of Coupon 40
			 SELECT IFNULL(SUM(Price), 0) 
			   FROM TSBCouponTransaction 
			  WHERE TSBCouponTransaction.TransactionType = 4 -- SoldByTSB = 4
				AND TSBCouponTransaction.TSBId = TSB.TSBId
				AND TSBCouponTransaction.CouponType = 40
				AND TSBCouponTransaction.FinishFlag = 0 -- Completed
		   ) AS PriceBHT40
		 , (
		     -- Calc Price of Coupon 80
			 SELECT IFNULL(SUM(Price), 0) 
			   FROM TSBCouponTransaction 
			  WHERE TSBCouponTransaction.TransactionType = 4 -- SoldByTSB = 4
				AND TSBCouponTransaction.TSBId = TSB.TSBId
				AND TSBCouponTransaction.CouponType = 80
				AND TSBCouponTransaction.FinishFlag = 0 -- Completed
		   ) AS PriceBHT80
		 , (
		     -- Calc Price of Coupon 90
			 SELECT IFNULL(SUM(Price), 0) 
			   FROM TSBCouponTransaction 
			  WHERE TSBCouponTransaction.TransactionType = 4 -- SoldByTSB = 4
				AND TSBCouponTransaction.TSBId = TSB.TSBId
				AND TSBCouponTransaction.CouponType = 90
				AND TSBCouponTransaction.FinishFlag = 0 -- Completed
		   ) AS PriceBHT90
		 , (
		     -- Calc Price of Coupon all types
			 SELECT IFNULL(SUM(Price), 0) 
			   FROM TSBCouponTransaction 
			  WHERE TSBCouponTransaction.TransactionType = 4 -- SoldByTSB = 4
				AND TSBCouponTransaction.TSBId = TSB.TSBId
				AND TSBCouponTransaction.FinishFlag = 0 -- Completed
		   ) AS CouponBHTTotal
	  FROM TSB
