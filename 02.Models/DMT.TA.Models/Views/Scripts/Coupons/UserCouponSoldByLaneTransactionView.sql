-- Stock = 1, Lane = 2, SoldByLane = 3, SoldByTSB = 4
CREATE VIEW UserCouponSoldByLaneTransactionView
AS
    SELECT *
      FROM TSBCouponTransactionView
     WHERE (    TSBCouponTransactionView.SoldBy IS NOT NULL
            AND TSBCouponTransactionView.SoldBy <> '')
       AND TSBCouponTransactionView.TransactionType = 3 -- SoldByLane
       AND TSBCouponTransactionView.FinishFlag = 0 -- Completed.
