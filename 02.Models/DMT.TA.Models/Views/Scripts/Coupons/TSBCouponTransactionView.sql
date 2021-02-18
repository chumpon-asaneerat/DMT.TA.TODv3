CREATE VIEW TSBCouponTransactionView
AS
    SELECT TSBCouponTransaction.* 
         , TSB.TSBNameEN
         , TSB.TSBNameTH
         --, usr.FullNameEN, usr.FullNameTH
         --, sup.FullNameEN AS SoldByFullNameEN, sup.FullNameTH AS SoldByFullNameTH
         , LaneView.PlazaGroupId, LaneView.PlazaGroupNameEN, LaneView.PlazaGroupNameTH    
      FROM TSBCouponTransaction
      LEFT JOIN LaneView ON (TSBCouponTransaction.TSBId = LaneView.TSBId AND TSBCouponTransaction.LaneId = LaneView.LaneId)  
         , TSB
    --LEFT JOIN [UserView] usr ON (TSBCouponTransaction.UserId = usr.UserId) 
    --LEFT JOIN [UserView] sup ON (TSBCouponTransaction.SoldBy = sup.UserId) 
     WHERE TSBCouponTransaction.TSBId = TSB.TSBId
