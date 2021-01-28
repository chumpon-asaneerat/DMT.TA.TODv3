CREATE VIEW TSBCouponSummarryView
AS
    -- 1 = Stock
    -- 2 = Lane
    -- 3 = SoldByLane
    -- 4 = SoldByTSB
    SELECT A.UserId, A.FullNameEN, A.FullNameTH
    	 --, A.UserReceiveDate
    	 , (SELECT COUNT(C35.CouponType) 
    	      FROM TSBCouponTransactionView C35 
    		 WHERE C35.CouponType = 35 
    		   AND C35.TSBId = A.TSBId
    		   AND C35.UserId = A.UserId
    		   AND C35.FinishFlag = A.FinishFlag
               AND (C35.TransactionType = 2 OR C35.TransactionType = 3) -- Lane or SoldByLane
             GROUP BY C35.TSBId
                    , C35.UserId
    	   ) AS CountCouponBHT35
    	 , (SELECT SUM(C35.Price) 
    	      FROM TSBCouponTransactionView C35 
    		 WHERE C35.CouponType = 35 
    		   AND C35.TSBId = A.TSBId
    		   AND C35.UserId = A.UserId
    		   AND C35.FinishFlag = A.FinishFlag
               AND (C35.TransactionType = 2 OR C35.TransactionType = 3) -- Lane or SoldByLane
             GROUP BY C35.TSBId
                    , C35.UserId
    	   ) AS AmountCouponBHT35
    	 , (SELECT COUNT(C80.CouponType) 
    	      FROM TSBCouponTransactionView C80
    		 WHERE C80.CouponType = 80
    		   AND C80.TSBId = A.TSBId
    		   AND C80.UserId = A.UserId
    		   AND C80.FinishFlag = A.FinishFlag
               AND (C80.TransactionType = 2 OR C80.TransactionType = 3) -- Lane or SoldByLane
             GROUP BY C80.TSBId
                    , C80.UserId
    	   ) AS CountCouponBHT80
    	 , (SELECT SUM(C80.Price) 
    	      FROM TSBCouponTransactionView C80
    		 WHERE C80.CouponType = 80
    		   AND C80.TSBId = A.TSBId
    		   AND C80.UserId = A.UserId
    		   AND C80.FinishFlag = A.FinishFlag
               AND (C80.TransactionType = 2 OR C80.TransactionType = 3) -- Lane or SoldByLane
             GROUP BY C80.TSBId
                    , C80.UserId
    	   ) AS AmountCouponBHT80
         , A.TSBId, A.TSBNameEN, A.TSBNameTH
    	 --, A.SoldBy, A.SoldByFullNameEN, A.SoldByFullNameTH
      FROM TSBCouponTransactionView A
     WHERE (    A.UserId IS NOT NULL
            AND A.UserId <> '')
       AND A.FinishFlag = 1 -- Avaliable.
     GROUP BY A.TSBId
            , A.UserId
            --, A.UserReceiveDate
