CREATE VIEW TSBCouponBalanceView
AS
	SELECT TSB.TSBId, TSB.TSBNameEN, TSB.TSBNameTH
		 , (
		     -- Count No of Coupon 35
			 SELECT (stock.CouponBHT35 + 
			         lane.CouponBHT35 - 
					 tsbsold.CouponBHT35 - 
					 usrsold.CouponBHT35) AS CouponBHT35
			   FROM TSBCouponStockBalanceView stock
			      , TSBCouponLaneBalanceView lane 
				  , TSBCouponSoldByTSBBalanceView tsbsold
				  , TSBCouponSoldByLaneBalanceView usrsold
			  WHERE stock.TSBId = TSB.TSBId
			    AND lane.TSBId = TSB.TSBId
			    AND tsbsold.TSBId = TSB.TSBId
			    AND usrsold.TSBId = TSB.TSBId
		   ) AS CouponBHT35
		 , (
		     -- Count No of Coupon 80
			 SELECT (stock.CouponBHT80 + 
			         lane.CouponBHT80 - 
					 tsbsold.CouponBHT80 - 
					 usrsold.CouponBHT80) AS CouponBHT80
			   FROM TSBCouponStockBalanceView stock
			      , TSBCouponLaneBalanceView lane 
				  , TSBCouponSoldByTSBBalanceView tsbsold
				  , TSBCouponSoldByLaneBalanceView usrsold
			  WHERE stock.TSBId = TSB.TSBId
			    AND lane.TSBId = TSB.TSBId
			    AND tsbsold.TSBId = TSB.TSBId
			    AND usrsold.TSBId = TSB.TSBId
		   ) AS CouponBHT80
		 , (
		     -- Calc Price of Coupon 35
			 SELECT (stock.PriceBHT35 + 
			         lane.PriceBHT35 - 
					 tsbsold.PriceBHT35 - 
					 usrsold.PriceBHT35) AS PriceBHT35
			   FROM TSBCouponStockBalanceView stock
			      , TSBCouponLaneBalanceView lane 
				  , TSBCouponSoldByTSBBalanceView tsbsold
				  , TSBCouponSoldByLaneBalanceView usrsold
			  WHERE stock.TSBId = TSB.TSBId
			    AND lane.TSBId = TSB.TSBId
			    AND tsbsold.TSBId = TSB.TSBId
			    AND usrsold.TSBId = TSB.TSBId
		   ) AS PriceBHT35
		 , (
		     -- Calc Price of Coupon 80
			 SELECT (stock.PriceBHT80 + 
			         lane.PriceBHT80 - 
					 tsbsold.PriceBHT80 - 
					 usrsold.PriceBHT80) AS PriceBHT80
			   FROM TSBCouponStockBalanceView stock
			      , TSBCouponLaneBalanceView lane 
				  , TSBCouponSoldByTSBBalanceView tsbsold
				  , TSBCouponSoldByLaneBalanceView usrsold
			  WHERE stock.TSBId = TSB.TSBId
			    AND lane.TSBId = TSB.TSBId
			    AND tsbsold.TSBId = TSB.TSBId
			    AND usrsold.TSBId = TSB.TSBId
		   ) AS PriceBHT80
		 , (
		     -- Calc Price of Coupon all types
			 SELECT (stock.CouponBHTTotal + 
			         lane.CouponBHTTotal - 
					 tsbsold.CouponBHTTotal - 
					 usrsold.CouponBHTTotal) AS CouponBHTTotal
			   FROM TSBCouponStockBalanceView stock
			      , TSBCouponLaneBalanceView lane 
				  , TSBCouponSoldByTSBBalanceView tsbsold
				  , TSBCouponSoldByLaneBalanceView usrsold
			  WHERE stock.TSBId = TSB.TSBId
			    AND lane.TSBId = TSB.TSBId
			    AND tsbsold.TSBId = TSB.TSBId
			    AND usrsold.TSBId = TSB.TSBId
		   ) AS CouponBHTTotal
	  FROM TSB