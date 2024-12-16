CREATE VIEW TSBCouponSummarryView
AS
    -- 1 = Stock
    -- 2 = Lane
    -- 3 = SoldByLane
    -- 4 = SoldByTSB
    SELECT A.UserId, A.FullNameEN, A.FullNameTH
    	 --, A.UserReceiveDate
         -- C35
    	 , IFNULL((SELECT COUNT(C35.CouponType) 
    	      FROM TSBCouponTransactionView C35 
    		 WHERE C35.CouponType = 35 
    		   AND C35.TSBId = A.TSBId
    		   AND C35.UserId = A.UserId
    		   AND C35.FinishFlag = A.FinishFlag
               AND (C35.TransactionType = 2 OR C35.TransactionType = 3) -- Lane or SoldByLane
             GROUP BY C35.TSBId
                    , C35.UserId
    	   ), 0) AS CountCouponBHT35
    	 , IFNULL((SELECT SUM(C35.Price) 
    	      FROM TSBCouponTransactionView C35 
    		 WHERE C35.CouponType = 35 
    		   AND C35.TSBId = A.TSBId
    		   AND C35.UserId = A.UserId
    		   AND C35.FinishFlag = A.FinishFlag
               AND (C35.TransactionType = 2 OR C35.TransactionType = 3) -- Lane or SoldByLane
             GROUP BY C35.TSBId
                    , C35.UserId
    	   ), 0) AS AmountCouponBHT35
         -- C40
    	 , IFNULL((SELECT COUNT(C40.CouponType) 
    	      FROM TSBCouponTransactionView C40 
    		 WHERE C40.CouponType = 40 
    		   AND C40.TSBId = A.TSBId
    		   AND C40.UserId = A.UserId
    		   AND C40.FinishFlag = A.FinishFlag
               AND (C40.TransactionType = 2 OR C40.TransactionType = 3) -- Lane or SoldByLane
             GROUP BY C40.TSBId
                    , C40.UserId
    	   ), 0) AS CountCouponBHT40
    	 , IFNULL((SELECT SUM(C40.Price) 
    	      FROM TSBCouponTransactionView C40 
    		 WHERE C40.CouponType = 40 
    		   AND C40.TSBId = A.TSBId
    		   AND C40.UserId = A.UserId
    		   AND C40.FinishFlag = A.FinishFlag
               AND (C40.TransactionType = 2 OR C40.TransactionType = 3) -- Lane or SoldByLane
             GROUP BY C40.TSBId
                    , C40.UserId
    	   ), 0) AS AmountCouponBHT40
         -- C80
    	 , IFNULL((SELECT COUNT(C80.CouponType)
    	      FROM TSBCouponTransactionView C80
    		 WHERE C80.CouponType = 80
    		   AND C80.TSBId = A.TSBId
    		   AND C80.UserId = A.UserId
    		   AND C80.FinishFlag = A.FinishFlag
               AND (C80.TransactionType = 2 OR C80.TransactionType = 3) -- Lane or SoldByLane
             GROUP BY C80.TSBId
                    , C80.UserId
    	   ), 0)  AS CountCouponBHT80
    	 , IFNULL((SELECT SUM(C80.Price) 
    	      FROM TSBCouponTransactionView C80
    		 WHERE C80.CouponType = 80
    		   AND C80.TSBId = A.TSBId
    		   AND C80.UserId = A.UserId
    		   AND C80.FinishFlag = A.FinishFlag
               AND (C80.TransactionType = 2 OR C80.TransactionType = 3) -- Lane or SoldByLane
             GROUP BY C80.TSBId
                    , C80.UserId
    	   ), 0) AS AmountCouponBHT80
         -- C90
    	 , IFNULL((SELECT COUNT(C90.CouponType)
    	      FROM TSBCouponTransactionView C90
    		 WHERE C90.CouponType = 90
    		   AND C90.TSBId = A.TSBId
    		   AND C90.UserId = A.UserId
    		   AND C90.FinishFlag = A.FinishFlag
               AND (C90.TransactionType = 2 OR C90.TransactionType = 3) -- Lane or SoldByLane
             GROUP BY C90.TSBId
                    , C90.UserId
    	   ), 0)  AS CountCouponBHT90
    	 , IFNULL((SELECT SUM(C90.Price) 
    	      FROM TSBCouponTransactionView C90
    		 WHERE C90.CouponType = 90
    		   AND C90.TSBId = A.TSBId
    		   AND C90.UserId = A.UserId
    		   AND C90.FinishFlag = A.FinishFlag
               AND (C90.TransactionType = 2 OR C90.TransactionType = 3) -- Lane or SoldByLane
             GROUP BY C90.TSBId
                    , C90.UserId
    	   ), 0) AS AmountCouponBHT90
         , A.TSBId, A.TSBNameEN, A.TSBNameTH
    	 --, A.SoldBy, A.SoldByFullNameEN, A.SoldByFullNameTH
      FROM TSBCouponTransactionView A
     WHERE (    A.UserId IS NOT NULL
            AND A.UserId <> '')
       AND A.FinishFlag = 1 -- Avaliable.
     GROUP BY A.TSBId
            , A.UserId
            --, A.UserReceiveDate