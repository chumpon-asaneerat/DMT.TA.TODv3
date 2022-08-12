CREATE VIEW UserCreditTransactionView
AS
	SELECT strftime('%Y-%m-%d', UserCreditTransaction.TransactionDate) AS TransDate
		 , UserCreditTransaction.*
	     , TSB.TSBNameEN, TSB.TSBNameTH
		 , PlazaGroup.PlazaGroupNameEN, PlazaGroup.PlazaGroupNameTH, PlazaGroup.Direction
		 , UserCreditBalance.ShiftId
		 , SH.ShiftNameEN, SH.ShiftNameTH
		 , UserCreditBalance.UserCreditDate
		 , UserCreditBalance.[State], UserCreditBalance.BagNo, UserCreditBalance.BeltNo
	  FROM UserCreditTransaction, TSB, PlazaGroup
		 , UserCreditBalance
		   LEFT JOIN Shift SH ON (UserCreditBalance.ShiftId = SH.ShiftId)
	 WHERE PlazaGroup.TSBId = TSB.TSBId
	   AND UserCreditBalance.TSBId = TSB.TSBId
	   AND UserCreditBalance.PlazaGroupId = PlazaGroup.PlazaGroupId
	   AND UserCreditTransaction.UserId = UserCreditBalance.UserId
	   AND UserCreditTransaction.UserCreditId = UserCreditBalance.UserCreditId
