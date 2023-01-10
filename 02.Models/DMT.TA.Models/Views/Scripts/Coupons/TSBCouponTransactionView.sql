CREATE VIEW TSBCouponTransactionView
AS
    SELECT TSBCouponTransaction.* 
         , TSB.TSBNameEN
         , TSB.TSBNameTH
      FROM TSBCouponTransaction
         , TSB
     WHERE TSBCouponTransaction.TSBId = TSB.TSBId
    /*
    SELECT TSBCouponTransaction.* 
         , TSB.TSBNameEN
         , TSB.TSBNameTH
         , LaneView.PlazaGroupId, LaneView.PlazaGroupNameEN, LaneView.PlazaGroupNameTH    
      FROM TSBCouponTransaction
      LEFT JOIN LaneView ON (LaneView.TSBId = TSBCouponTransaction.TSBId AND LaneView.LaneId = TSBCouponTransaction.LaneId)  
         , TSB
     WHERE TSBCouponTransaction.TSBId = TSB.TSBId
    */

