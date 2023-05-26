-- Stock = 1, Lane = 2, SoldByLane = 3, SoldByTSB = 4
CREATE VIEW UserCouponOnHandTransactionView
AS
    SELECT *
      FROM TSBCouponTransactionView
     WHERE TSBCouponTransactionView.UserReceiveDate IS NOT NULL
       AND TSBCouponTransactionView.TransactionType = 2 -- Lane
