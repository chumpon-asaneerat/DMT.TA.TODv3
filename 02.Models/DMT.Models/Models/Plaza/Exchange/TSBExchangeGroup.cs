#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

using NLib;
using NLib.Design;
using NLib.Reflection;

using SQLite;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;
// required for JsonIgnore attribute.
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

#endregion

namespace DMT.Models
{
	#region TSBExchangeGroup

	/// <summary>
	/// The TSBExchangeGroup Data Model class.
	/// </summary>
	[TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
	[Serializable]
	[JsonObject(MemberSerialization.OptOut)]
	//[Table("TSBExchangeGroup")]
	public class TSBExchangeGroup : NTable<TSBExchangeGroup>
	{
		#region Enum

		/// <summary>
		/// The Request Types.
		/// </summary>
		public enum RequestTypes : int
		{
			/// <summary>
			/// Exchange with Account.
			/// </summary>
			Account = 1,
			/// <summary>
			/// Exchange internal.
			/// </summary>
			Internal = 2
		}
		/// <summary>
		/// The Request state enum.
		/// </summary>
		public enum StateTypes : int
		{
			/// <summary>
			/// None. Used for ignore search in query.
			/// </summary>
			None = -1,
			/// <summary>
			/// Request by plaza.
			/// </summary>
			Request = 1,
			/// <summary>
			/// Canceled.
			/// </summary>
			Canceled = 2,
			/// <summary>
			/// Approve by account.
			/// </summary>
			Approve = 3,
			/// <summary>
			/// Reject by account.
			/// </summary>
			Reject = 4,
			/// <summary>
			/// Received by plaza.
			/// </summary>
			Received = 5,
			/// <summary>
			/// Exchange (send back to account dept.)
			/// </summary>
			Exchange = 6,
			/// <summary>
			/// Return from plaza but account not update status.
			/// </summary>
			Return = 7,
			/// <summary>
			/// Completed when account received returns credits and update status.
			/// </summary>
			Completed = 9
		}
		/// <summary>
		/// The Finished Flags
		/// </summary>
		public enum FinishedFlags : int
		{
			/// <summary>
			/// None. Used for ignore search in query.
			/// </summary>
			None = -1,
			/// <summary>
			/// Avaliable.
			/// </summary>
			Avaliable = 0,
			/// <summary>
			/// Completed.
			/// </summary>
			Completed = 1,
		}

		#endregion

		#region Internal Variables

		private int _PkId = 0;
		private Guid _GroupId = Guid.Empty;

		private RequestTypes _RequestType = RequestTypes.Account;
		private StateTypes _State = StateTypes.Request;
		private FinishedFlags _FinishFlag = FinishedFlags.Avaliable;
		private DateTime _RequestDate = DateTime.MinValue;
		// TSB
		private string _TSBId = string.Empty;
		private string _TSBNameEN = string.Empty;
		private string _TSBNameTH = string.Empty;
		// วงเงินอนุมัติ เป็นวงเงินที่ บ/ช กำหนดให้แต่ละด่าน เป็นค่าสูงสุดที่แต่ละด่านจะมีได้ โดยยอดนี้จะต้อง มากกว่าหรือเท่ากับ ยอดรวม + เงินยืมเพิ่ม
		private decimal _MaxCredit = decimal.Zero;

		// Request Transaction (runtime)
		/*
		private int _TransactionId = 0;
		private DateTime _TransactionDate = DateTime.MinValue;
		private TSBExchangeTransaction.TransactionTypes _TransactionType = TSBExchangeTransaction.TransactionTypes.Request;
		*/
		// Request User (runtime)
		private string _UserId = string.Empty;
		private string _FullNameEN = string.Empty;
		private string _FullNameTH = string.Empty;
		// Request Amounts (runtime)
		// วงเงินขอเพิ่ม เป็นเงินที่ ขอเพิ่มไปยัง บ/ช โดย เมื่อรวมกับยอดรวม ต้องไม่เกิน ยอดวงเงินอนุมัติ
		private decimal _RequestAdditionalBHT = decimal.Zero;
		// เงินยืมเพิ่ม ไม่จำกัด เพราะต้องคืน เท่ากับที่ยืมมา
		private decimal _RequestBorrowBHT = decimal.Zero;
		// เงินขอแลกเปลี่ยน 
		private decimal _RequestExchangeBHT = decimal.Zero;

		// Request Period (runtime)
		private DateTime? _RequestPeriodBegin = new DateTime?();
		private DateTime? _RequestPeriodEnd = new DateTime?();

		private bool _hasRemark = false;
		// Request Remark (runtime)
		private string _RequestRemark = string.Empty;

		// Approve Amounts (runtime)
		// วงเงินขอเพิ่ม เป็นเงินที่ ขอเพิ่มไปยัง บ/ช โดย เมื่อรวมกับยอดรวม ต้องไม่เกิน ยอดวงเงินอนุมัติ
		private decimal _ApproveAdditionalBHT = decimal.Zero;
		// เงินยืมเพิ่ม ไม่จำกัด เพราะต้องคืน เท่ากับที่ยืมมา
		private decimal _ApproveBorrowBHT = decimal.Zero;
		// เงินขอแลกเปลี่ยน 
		private decimal _ApproveExchangeBHT = decimal.Zero;
		// Approve Period (runtime)
		private DateTime? _ApprovePeriodBegin = new DateTime?();
		private DateTime? _ApprovePeriodEnd = new DateTime?();
		// Approve Remark (runtime)
		private string _ApproveRemark = string.Empty;


		private int _Status = 0;
		private DateTime _LastUpdate = DateTime.MinValue;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public TSBExchangeGroup() : base() { }

		#endregion

		#region Private Methods

		#endregion

		#region Public Properties

		#region Common

		/// <summary>
		/// Gets or sets PkId
		/// </summary>
		[Category("Common")]
		[Description(" Gets or sets PkId")]
		[ReadOnly(true)]
		[PrimaryKey, AutoIncrement]
		[PropertyMapName("PkId")]
		public int PkId
		{
			get
			{
				return _PkId;
			}
			set
			{
				if (_PkId != value)
				{
					_PkId = value;
					this.RaiseChanged("PkId");
				}
			}
		}
		/// <summary>
		/// Gets or sets Exchange GroupId
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Exchange GroupId")]
		[ReadOnly(true)]
		[PropertyMapName("GroupId")]
		public Guid GroupId
		{
			get
			{
				return _GroupId;
			}
			set
			{
				if (_GroupId != value)
				{
					_GroupId = value;
					this.RaiseChanged("GroupId");
				}
			}
		}
		/// <summary>
		/// Gets or sets Request Type.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Request Type.")]
		[ReadOnly(true)]
		[PropertyMapName("RequestType")]
		public RequestTypes RequestType
		{
			get { return _RequestType; }
			set
			{
				if (_RequestType != value)
				{
					_RequestType = value;
					this.RaiseChanged("RequestType");
				}
			}
		}
		/// <summary>
		/// Gets or sets State.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets State.")]
		[ReadOnly(true)]
		[PropertyMapName("State")]
		public StateTypes State
		{
			get { return _State; }
			set
			{
				if (_State != value)
				{
					_State = value;
					this.RaiseChanged("State");
					this.RaiseChanged("CanEdit");
					this.RaiseChanged("CanExchange");
				}
			}
		}
		/// <summary>
		/// Gets or sets Finish Flag (0: Completed, 1: Avaliable).
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Is Finished (0: Completed, 1: Avaliable).")]
		[ReadOnly(true)]
		[PropertyMapName("FinishFlag")]
		public virtual FinishedFlags FinishFlag
		{
			get
			{
				return _FinishFlag;
			}
			set
			{
				if (_FinishFlag != value)
				{
					_FinishFlag = value;
					this.RaiseChanged("FinishFlag");
				}
			}
		}
		/// <summary>
		/// Gets or sets Request Date.
		/// </summary>
		[Category("Common")]
		[Description(" Gets or sets Request Date")]
		[ReadOnly(true)]
		[PropertyMapName("RequestDate")]
		public DateTime RequestDate
		{
			get
			{
				return _RequestDate;
			}
			set
			{
				if (_RequestDate != value)
				{
					_RequestDate = value;
					this.RaiseChanged("RequestDate");
					this.RaiseChanged("RequestDateString");
					this.RaiseChanged("RequestTimeString");
					this.RaiseChanged("RequestDateTimeString");
				}
			}
		}
		/// <summary>
		/// Gets Request Date String.
		/// </summary>
		[Category("Common")]
		[Description("Gets Request Date String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string RequestDateString
		{
			get
			{
				var ret = (this.RequestDate == DateTime.MinValue) ? "" : this.RequestDate.ToThaiDateTimeString("dd/MM/yyyy");
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Request Time String.
		/// </summary>
		[Category("Common")]
		[Description("Gets Request Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string RequestTimeString
		{
			get
			{
				var ret = (this.RequestDate == DateTime.MinValue) ? "" : this.RequestDate.ToThaiTimeString();
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Request Date Time String.
		/// </summary>
		[Category("Common")]
		[Description("Gets Request Date Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string RequestDateTimeString
		{
			get
			{
				var ret = (this.RequestDate == DateTime.MinValue) ? "" : this.RequestDate.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
				return ret;
			}
			set { }
		}

		#endregion

		#region TSB

		/// <summary>
		/// Gets or sets TSBId.
		/// </summary>
		[Category("TSB")]
		[Description("Gets or sets TSBId.")]
		[ReadOnly(true)]
		[MaxLength(10)]
		[PropertyMapName("TSBId")]
		public string TSBId
		{
			get
			{
				return _TSBId;
			}
			set
			{
				if (_TSBId != value)
				{
					_TSBId = value;
					this.RaiseChanged("TSBId");
				}
			}
		}
		/// <summary>
		/// Gets or sets TSB Name EN.
		/// </summary>
		[Category("TSB")]
		[Description("Gets or sets TSB Name EN.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("TSBNameEN")]
		public virtual string TSBNameEN
		{
			get
			{
				return _TSBNameEN;
			}
			set
			{
				if (_TSBNameEN != value)
				{
					_TSBNameEN = value;
					this.RaiseChanged("TSBNameEN");
				}
			}
		}
		/// <summary>
		/// Gets or sets TSB Name TH.
		/// </summary>
		[Category("TSB")]
		[Description("Gets or sets TSB Name TH.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("TSBNameTH")]
		public virtual string TSBNameTH
		{
			get
			{
				return _TSBNameTH;
			}
			set
			{
				if (_TSBNameTH != value)
				{
					_TSBNameTH = value;
					this.RaiseChanged("TSBNameTH");
				}
			}
		}
		/// <summary>
		/// Gets or sets amount TSB Max BHT.
		/// </summary>
		[Category("TSB")]
		[Description("Gets or sets Max TSB Credit.")]
		[Ignore]
		[PropertyMapName("MaxCredit")]
		public virtual decimal MaxCredit
		{
			get { return _MaxCredit; }
			set
			{
				if (_MaxCredit != value)
				{
					_MaxCredit = value;
					// Raise event.
					this.RaiseChanged("MaxCredit");
				}
			}
		}

		#endregion

		#region Runtime (request transaction)

		#region UI Enable

		/// <summary>
		/// Gets Can Edit.
		/// </summary>
		[Category("Common")]
		[Description("Gets Can Edit.")]
		[Ignore]
		public bool CanEdit
		{
			get { return (_State == StateTypes.Request); }
			set { }
		}
		/// <summary>
		/// Gets Can Exchange.
		/// </summary>
		[Category("Common")]
		[Description("Gets Can Exchange.")]
		[Ignore]
		public bool CanExchange
		{
			get { return (_State == StateTypes.Approve); }
			set { }
		}
		/// <summary>
		/// Gets State String.
		/// </summary>
		[Category("Common")]
		[Description("Gets State String.")]
		[Ignore]
		public string StateString
		{
			get 
			{
				string ret = string.Empty;
				switch (_State)
				{
					case StateTypes.Request:
						ret = "รออนุมัติ";
						break;
					case StateTypes.Approve:
						ret = "อนุมัติ";
						break;
					case StateTypes.Received:
						ret = "รับเงินแล้ว";
						break;
					case StateTypes.Reject:
						ret = "ไม่อนุมัติ";
						break;
				}
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets or sets has remark.
		/// </summary>
		[Category("Runtime")]
		[Description("Gets or sets HasRemark.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("HasRemark")]
		public bool HasRemark
		{
			get { return _hasRemark; }
			set
			{
				if (_hasRemark != value)
				{
					_hasRemark = value;
					// Raise event.
					this.RaiseChanged("HasRemark");
					this.RaiseChanged("RemarkVisibility");
				}
			}
		}
		/// <summary>
		/// Gets or sets Remark Visibility.
		/// </summary>
		[Category("Runtime")]
		[Description("Gets or sets Remark Visibility.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		[PropertyMapName("RemarkVisibility")]
		public System.Windows.Visibility RemarkVisibility
		{
			get { return (_hasRemark) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed; }
			set { }
		}

		#endregion

		#region Common (comment out)

		/*
		/// <summary>
		/// Gets or sets TransactionId
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets TransactionId")]
		[Ignore]
		[PropertyMapName("TransactionId")]
		public virtual int TransactionId
		{
			get
			{
				return _TransactionId;
			}
			set
			{
				if (_TransactionId != value)
				{
					_TransactionId = value;
					this.RaiseChanged("TransactionId");
				}
			}
		}
		/// <summary>
		/// Gets or sets Transaction Date.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Transaction Date")]
		[Ignore]
		[PropertyMapName("TransactionDate")]
		public virtual DateTime TransactionDate
		{
			get
			{
				return _TransactionDate;
			}
			set
			{
				if (_TransactionDate != value)
				{
					_TransactionDate = value;
					this.RaiseChanged("TransactionDate");
					this.RaiseChanged("TransactionDateString");
					this.RaiseChanged("TransactionTimeString");
					this.RaiseChanged("TransactionDateTimeString");
				}
			}
		}
		/// <summary>
		/// Gets Transaction Date String.
		/// </summary>
		[Category("Common")]
		[Description("Gets Transaction Date String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string TransactionDateString
		{
			get
			{
				var ret = (this.TransactionDate == DateTime.MinValue) ? "" : this.TransactionDate.ToThaiDateTimeString("dd/MM/yyyy");
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Transaction Time String.
		/// </summary>
		[Category("Common")]
		[Description("Gets Transaction Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string TransactionTimeString
		{
			get
			{
				var ret = (this.TransactionDate == DateTime.MinValue) ? "" : this.TransactionDate.ToThaiTimeString();
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Transaction Date Time String.
		/// </summary>
		[Category("Common")]
		[Description("Gets Transaction Date Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string TransactionDateTimeString
		{
			get
			{
				var ret = (this.TransactionDate == DateTime.MinValue) ? "" : this.TransactionDate.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets or sets Transaction Type.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Transaction Type.")]
		[Ignore]
		[PropertyMapName("TransactionType")]
		public virtual TSBExchangeTransaction.TransactionTypes TransactionType
		{
			get { return _TransactionType; }
			set
			{
				if (_TransactionType != value)
				{
					_TransactionType = value;
					this.RaiseChanged("TransactionType");
				}
			}
		}
		*/

		#endregion

		#region User

		/// <summary>
		/// Gets or sets User Id
		/// </summary>
		[Category("User")]
		[Description("Gets or sets User Id.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("UserId")]
		public virtual string UserId
		{
			get
			{
				return _UserId;
			}
			set
			{
				if (_UserId != value)
				{
					_UserId = value;
					this.RaiseChanged("UserId");
				}
			}
		}
		/// <summary>
		/// Gets or sets User Full Name EN
		/// </summary>
		[Category("User")]
		[Description("Gets or sets User Full Name EN.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("FullNameEN")]
		public virtual string FullNameEN
		{
			get
			{
				return _FullNameEN;
			}
			set
			{
				if (_FullNameEN != value)
				{
					_FullNameEN = value;
					this.RaiseChanged("FullNameEN");
				}
			}
		}
		/// <summary>
		/// Gets or sets User Full Name TH
		/// </summary>
		[Category("User")]
		[Description("Gets or sets User Full Name TH.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("FullNameTH")]
		public virtual string FullNameTH
		{
			get
			{
				return _FullNameTH;
			}
			set
			{
				if (_FullNameTH != value)
				{
					_FullNameTH = value;
					this.RaiseChanged("FullNameTH");
				}
			}
		}

		#endregion

		#region Request Exchange/Borrow/Additional

		/// <summary>
		/// Gets or sets Request amount Exchange BHT.
		/// </summary>
		[Category("Request Summary (Amount)")]
		[Description("Gets or sets Request amount Exchange BHT.")]
		[Ignore]
		[PropertyMapName("RequestExchangeBHT")]
		public virtual decimal RequestExchangeBHT
		{
			get { return _RequestExchangeBHT; }
			set
			{
				if (_RequestExchangeBHT != value)
				{
					_RequestExchangeBHT = value;
					// Raise event.
					this.RaiseChanged("RequestExchangeBHT");
					this.RaiseChanged("RequestGrandTotalBHT");
				}
			}
		}
		/// <summary>
		/// Gets or sets Request amount Borrow BHT.
		/// </summary>
		[Category("Request Summary (Amount)")]
		[Description("Gets or sets Request amount Borrow BHT.")]
		[Ignore]
		[PropertyMapName("RequestBorrowBHT")]
		public virtual decimal RequestBorrowBHT
		{
			get { return _RequestBorrowBHT; }
			set
			{
				if (_RequestBorrowBHT != value)
				{
					_RequestBorrowBHT = value;
					// Raise event.
					this.RaiseChanged("RequestBorrowBHT");
					this.RaiseChanged("RequestGrandTotalBHT");
				}
			}
		}
		/// <summary>
		/// Gets or sets Request amount Additional BHT.
		/// </summary>
		[Category("Summary (Amount)")]
		[Description("Gets or sets Request amount Additional BHT.")]
		[Ignore]
		[PropertyMapName("RequestAdditionalBHT")]
		public virtual decimal RequestAdditionalBHT
		{
			get { return _RequestAdditionalBHT; }
			set
			{
				if (_RequestAdditionalBHT != value)
				{
					_RequestAdditionalBHT = value;
					// Raise event.
					this.RaiseChanged("RequestAdditionalBHT");
					this.RaiseChanged("RequestGrandTotalBHT");
				}
			}
		}
		/// <summary>
		/// Gets or sets Request Grand Total in baht.
		/// </summary>
		[Category("Summary (Amount)")]
		[Description("Gets or sets Request Grand Total in baht.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		//[PropertyMapName("GrandTotalBHT")]
		public decimal RequestGrandTotalBHT
		{
			get
			{
				return _RequestExchangeBHT + _RequestBorrowBHT + _RequestAdditionalBHT;
			}
			set { }
		}

		#endregion

		#region Request Periods

		/// <summary>
		/// Gets or sets Request Period Begin Date.
		/// </summary>
		[Category("Period")]
		[Description(" Gets or sets Request Period Begin Date")]
		[Ignore]
		[PropertyMapName("RequestPeriodBegin")]
		public virtual DateTime? RequestPeriodBegin
		{
			get
			{
				return _RequestPeriodBegin;
			}
			set
			{
				if (_RequestPeriodBegin != value)
				{
					_RequestPeriodBegin = value;
					this.RaiseChanged("RequestPeriodBegin");
					this.RaiseChanged("RequestPeriodBeginDateString");
					this.RaiseChanged("RequestPeriodBeginTimeString");
					this.RaiseChanged("RequestPeriodBeginDateTimeString");
				}
			}
		}
		/// <summary>
		/// Gets Request Period Begin Date String.
		/// </summary>
		[Category("Period")]
		[Description("Gets Request Period Begin Date String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string RequestPeriodBeginDateString
		{
			get
			{
				var ret = (!this.RequestPeriodBegin.HasValue) ? "" : this.RequestPeriodBegin.Value.ToThaiDateTimeString("dd/MM/yyyy");
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Request Period Begin Time String.
		/// </summary>
		[Category("Period")]
		[Description("Gets Request Period Begin Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string RequestPeriodBeginTimeString
		{
			get
			{
				var ret = (!this.RequestPeriodBegin.HasValue) ? "" : this.RequestPeriodBegin.Value.ToThaiTimeString();
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Request Period Begin Date Time String.
		/// </summary>
		[Category("Period")]
		[Description("Gets Request Period Begin Date Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string RequestPeriodBeginDateTimeString
		{
			get
			{
				var ret = (!this.RequestPeriodBegin.HasValue) ? "" : this.RequestPeriodBegin.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
				return ret;
			}
			set { }
		}

		/// <summary>
		/// Gets or sets Request Period End Date.
		/// </summary>
		[Category("Period")]
		[Description(" Gets or sets Request Period End Date")]
		[Ignore]
		[PropertyMapName("RequestPeriodEnd")]
		public virtual DateTime? RequestPeriodEnd
		{
			get
			{
				return _RequestPeriodEnd;
			}
			set
			{
				if (_RequestPeriodEnd != value)
				{
					_RequestPeriodEnd = value;
					this.RaiseChanged("RequestPeriodEnd");
					this.RaiseChanged("RequestPeriodEndDateString");
					this.RaiseChanged("RequestPeriodEndTimeString");
					this.RaiseChanged("RequestPeriodEndDateTimeString");
				}
			}
		}
		/// <summary>
		/// Gets Request Period End Date String.
		/// </summary>
		[Category("Period")]
		[Description("Gets Request Period End Date String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string RequestPeriodEndDateString
		{
			get
			{
				var ret = (!this.RequestPeriodEnd.HasValue) ? "" : this.RequestPeriodEnd.Value.ToThaiDateTimeString("dd/MM/yyyy");
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Request Period End Time String.
		/// </summary>
		[Category("Period")]
		[Description("Gets Request Period End Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string RequestPeriodEndTimeString
		{
			get
			{
				var ret = (!this.RequestPeriodEnd.HasValue) ? "" : this.RequestPeriodEnd.Value.ToThaiTimeString();
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Request Period End Date Time String.
		/// </summary>
		[Category("Common")]
		[Description("Gets Request Period End Date Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string RequestPeriodEndDateTimeString
		{
			get
			{
				var ret = (!this.RequestPeriodEnd.HasValue) ? "" : this.RequestPeriodEnd.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
				return ret;
			}
			set { }
		}

		#endregion

		#region Request Remark

		/// <summary>
		/// Gets or sets Request Remark.
		/// </summary>
		[Category("Remark")]
		[Description("Gets or sets Request Remark.")]
		[Ignore]
		[PropertyMapName("RequestRemark")]
		public virtual string RequestRemark
		{
			get { return _RequestRemark; }
			set
			{
				if (_RequestRemark != value)
				{
					_RequestRemark = value;
					// Raise event.
					this.RaiseChanged("Remark");
				}
			}
		}

		#endregion

		#region Approve Exchange/Borrow/Additional

		/// <summary>
		/// Gets or sets Approve amount Exchange BHT.
		/// </summary>
		[Category("Summary (Amount)")]
		[Description("Gets or sets Approve amount Exchange BHT.")]
		[Ignore]
		[PropertyMapName("ApproveExchangeBHT")]
		public virtual decimal ApproveExchangeBHT
		{
			get { return _ApproveExchangeBHT; }
			set
			{
				if (_ApproveExchangeBHT != value)
				{
					_ApproveExchangeBHT = value;
					// Raise event.
					this.RaiseChanged("ApproveExchangeBHT");
					this.RaiseChanged("ApproveGrandTotalBHT");
				}
			}
		}
		/// <summary>
		/// Gets or sets amount Borrow BHT.
		/// </summary>
		[Category("Summary (Amount)")]
		[Description("Gets or sets Approve amount Borrow BHT.")]
		[Ignore]
		[PropertyMapName("ApproveBorrowBHT")]
		public virtual decimal ApproveBorrowBHT
		{
			get { return _ApproveBorrowBHT; }
			set
			{
				if (_ApproveBorrowBHT != value)
				{
					_ApproveBorrowBHT = value;
					// Raise event.
					this.RaiseChanged("ApproveBorrowBHT");
					this.RaiseChanged("ApproveGrandTotalBHT");
				}
			}
		}
		/// <summary>
		/// Gets or sets Approve amount Additional BHT.
		/// </summary>
		[Category("Summary (Amount)")]
		[Description("Gets or sets Approve amount Additional BHT.")]
		[Ignore]
		[PropertyMapName("ApproveAdditionalBHT")]
		public virtual decimal ApproveAdditionalBHT
		{
			get { return _ApproveAdditionalBHT; }
			set
			{
				if (_ApproveAdditionalBHT != value)
				{
					_ApproveAdditionalBHT = value;
					// Raise event.
					this.RaiseChanged("ApproveAdditionalBHT");
					this.RaiseChanged("ApproveGrandTotalBHT");
				}
			}
		}
		/// <summary>
		/// Gets or sets Approve Grand Total in baht.
		/// </summary>
		[Category("Summary (Amount)")]
		[Description("Gets or sets Approve Grand Total in baht.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		//[PropertyMapName("GrandTotalBHT")]
		public decimal ApproveGrandTotalBHT
		{
			get
			{
				return _ApproveExchangeBHT + _ApproveBorrowBHT + _ApproveAdditionalBHT;
			}
			set { }
		}

		#endregion

		#region Approve Periods

		/// <summary>
		/// Gets or sets Approve Period Begin Date.
		/// </summary>
		[Category("Period")]
		[Description(" Gets or sets Approve Period Begin Date")]
		[Ignore]
		[PropertyMapName("ApprovePeriodBegin")]
		public virtual DateTime? ApprovePeriodBegin
		{
			get
			{
				return _ApprovePeriodBegin;
			}
			set
			{
				if (_ApprovePeriodBegin != value)
				{
					_ApprovePeriodBegin = value;
					this.RaiseChanged("ApprovePeriodBegin");
					this.RaiseChanged("ApprovePeriodBeginDateString");
					this.RaiseChanged("ApprovePeriodBeginTimeString");
					this.RaiseChanged("ApprovePeriodBeginDateTimeString");
				}
			}
		}
		/// <summary>
		/// Gets Approve Period Begin Date String.
		/// </summary>
		[Category("Period")]
		[Description("Gets Approve Period Begin Date String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string ApprovePeriodBeginDateString
		{
			get
			{
				var ret = (!this.ApprovePeriodBegin.HasValue) ? "" : this.ApprovePeriodBegin.Value.ToThaiDateTimeString("dd/MM/yyyy");
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Approve Period Begin Time String.
		/// </summary>
		[Category("Period")]
		[Description("Gets Approve Period Begin Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string ApprovePeriodBeginTimeString
		{
			get
			{
				var ret = (!this.ApprovePeriodBegin.HasValue) ? "" : this.ApprovePeriodBegin.Value.ToThaiTimeString();
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Approve Period Begin Date Time String.
		/// </summary>
		[Category("Period")]
		[Description("Gets Approve Period Begin Date Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string ApprovePeriodBeginDateTimeString
		{
			get
			{
				var ret = (!this.ApprovePeriodBegin.HasValue) ? "" : this.ApprovePeriodBegin.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
				return ret;
			}
			set { }
		}

		/// <summary>
		/// Gets or sets Approve Period End Date.
		/// </summary>
		[Category("Period")]
		[Description("Gets or sets Approve Period End Date")]
		[Ignore]
		[PropertyMapName("ApprovePeriodEnd")]
		public virtual DateTime? ApprovePeriodEnd
		{
			get
			{
				return _ApprovePeriodEnd;
			}
			set
			{
				if (_ApprovePeriodEnd != value)
				{
					_ApprovePeriodEnd = value;
					this.RaiseChanged("ApprovePeriodEnd");
					this.RaiseChanged("ApprovePeriodEndDateString");
					this.RaiseChanged("ApprovePeriodEndTimeString");
					this.RaiseChanged("ApprovePeriodEndDateTimeString");
				}
			}
		}
		/// <summary>
		/// Gets Approve Period End Date String.
		/// </summary>
		[Category("Period")]
		[Description("Gets Approve Period End Date String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string ApprovePeriodEndDateString
		{
			get
			{
				var ret = (!this.ApprovePeriodEnd.HasValue) ? "" : this.ApprovePeriodEnd.Value.ToThaiDateTimeString("dd/MM/yyyy");
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Approve Period End Time String.
		/// </summary>
		[Category("Period")]
		[Description("Gets Approve Period End Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string ApprovePeriodEndTimeString
		{
			get
			{
				var ret = (!this.ApprovePeriodEnd.HasValue) ? "" : this.ApprovePeriodEnd.Value.ToThaiTimeString();
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Approve Period End Date Time String.
		/// </summary>
		[Category("Common")]
		[Description("Gets Approve Period End Date Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string ApprovePeriodEndDateTimeString
		{
			get
			{
				var ret = (!this.ApprovePeriodEnd.HasValue) ? "" : this.ApprovePeriodEnd.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
				return ret;
			}
			set { }
		}

		#endregion

		#region Approve Remark

		/// <summary>
		/// Gets or sets Approve Remark.
		/// </summary>
		[Category("Remark")]
		[Description("Gets or sets Approve Remark.")]
		[Ignore]
		[PropertyMapName("Remark")]
		public virtual string ApproveRemark
		{
			get { return _ApproveRemark; }
			set
			{
				if (_ApproveRemark != value)
				{
					_ApproveRemark = value;
					// Raise event.
					this.RaiseChanged("ApproveRemark");
				}
			}
		}

		#endregion

		#endregion

		#region Transactions - Comment out
		/*
		/// <summary>
		/// Gets or sets Request transacton.
		/// </summary>
		[Category("Transaction")]
		[Description("Gets or sets Request transacton.")]
		[Ignore]
		public TSBExchangeTransaction Request { get; set; }
		/// <summary>
		/// Gets or sets Approve transacton.
		/// </summary>
		[Category("Transaction")]
		[Description("Gets or sets Approve transacton.")]
		[Ignore]
		public TSBExchangeTransaction Approve { get; set; }
		/// <summary>
		/// Gets or sets Received transacton.
		/// </summary>
		[Category("Transaction")]
		[Description("Gets or sets Received transacton.")]
		[Ignore]
		public TSBExchangeTransaction Received { get; set; }
		/// <summary>
		/// Gets or sets Exchange transacton.
		/// </summary>
		[Category("Transaction")]
		[Description("Gets or sets Exchange transacton.")]
		[Ignore]
		public TSBExchangeTransaction Exchange { get; set; }
		/// <summary>
		/// Gets or sets Returns transacton.
		/// </summary>
		[Category("Transaction")]
		[Description("Gets or sets Returns transacton.")]
		[Ignore]
		public TSBExchangeTransaction Return { get; set; }
		*/
		#endregion

		#region Status (DC)

		/// <summary>
		/// Gets or sets Status (1 = Sync, 0 = Unsync, etc..)
		/// </summary>
		[Category("DataCenter")]
		[Description("Gets or sets Status (1 = Sync, 0 = Unsync, etc..)")]
		[ReadOnly(true)]
		[PropertyMapName("Status", typeof(TSBExchangeGroup))]
		[PropertyOrder(10001)]
		public int Status
		{
			get
			{
				return _Status;
			}
			set
			{
				if (_Status != value)
				{
					_Status = value;
					this.RaiseChanged("Status");
				}
			}
		}
		/// <summary>
		/// Gets or sets LastUpdated (Sync to DC).
		/// </summary>
		[Category("DataCenter")]
		[Description("Gets or sets LastUpdated (Sync to DC).")]
		[ReadOnly(true)]
		[PropertyMapName("LastUpdate", typeof(TSBExchangeGroup))]
		[PropertyOrder(10002)]
		public DateTime LastUpdate
		{
			get { return _LastUpdate; }
			set
			{
				if (_LastUpdate != value)
				{
					_LastUpdate = value;
					this.RaiseChanged("LastUpdate");
				}
			}
		}

		#endregion

		#endregion

		#region Internal Class

		/// <summary>
		/// The internal FKs class for query data.
		/// </summary>
		public class FKs : TSBExchangeGroup, IFKs<TSBExchangeGroup>
		{
			#region TSB

			/// <summary>
			/// Gets or sets TSB Name EN.
			/// </summary>
			[MaxLength(100)]
			[PropertyMapName("TSBNameEN")]
			public override string TSBNameEN
			{
				get { return base.TSBNameEN; }
				set { base.TSBNameEN = value; }
			}
			/// <summary>
			/// Gets or sets TSB Name TH.
			/// </summary>
			[MaxLength(100)]
			[PropertyMapName("TSBNameTH")]
			public override string TSBNameTH
			{
				get { return base.TSBNameTH; }
				set { base.TSBNameTH = value; }
			}
			/// <summary>
			/// Gets or sets amount TSB Max BHT.
			/// </summary>
			[PropertyMapName("MaxCredit")]
			public override decimal MaxCredit
			{
				get { return base.MaxCredit; }
				set { base.MaxCredit = value; }
			}

			#endregion

			#region Runtime (request transaction)

			#region Common

			/*
			[PropertyMapName("TransactionId")]
			public override int TransactionId
			{
				get { return base.TransactionId; }
				set { base.TransactionId = value; }
			}
			[PropertyMapName("TransactionDate")]
			public override DateTime TransactionDate
			{
				get { return base.TransactionDate; }
				set { base.TransactionDate = value; }
			}
			[PropertyMapName("TransactionType")]
			public override TSBExchangeTransaction.TransactionTypes TransactionType
			{
				get { return base.TransactionType; }
				set { base.TransactionType = value; }
			}
			*/

			#endregion

			#region User

			[MaxLength(10)]
			[PropertyMapName("UserId")]
			public override string UserId
			{
				get { return base.UserId; }
				set { base.UserId = value; }
			}
			[MaxLength(150)]
			[PropertyMapName("FullNameEN")]
			public override string FullNameEN
			{
				get { return base.FullNameEN; }
				set { base.FullNameEN = value; }
			}
			[MaxLength(150)]
			[PropertyMapName("FullNameTH")]
			public override string FullNameTH
			{
				get { return base.FullNameTH; }
				set { base.FullNameTH = value; }
			}

			#endregion

			#region Request Exchange/Borrow/Additional

			[PropertyMapName("RequestExchangeBHT")]
			public override decimal RequestExchangeBHT
			{
				get { return base.RequestExchangeBHT; }
				set { base.RequestExchangeBHT = value; }
			}
			[PropertyMapName("RequestBorrowBHT")]
			public override decimal RequestBorrowBHT
			{
				get { return base.RequestBorrowBHT; }
				set { base.RequestBorrowBHT = value; }
			}
			[PropertyMapName("RequestAdditionalBHT")]
			public override decimal RequestAdditionalBHT
			{
				get { return base.RequestAdditionalBHT; }
				set { base.RequestAdditionalBHT = value; }
			}

			#endregion

			#region Request Period

			[PropertyMapName("RequestPeriodBegin")]
			public override DateTime? RequestPeriodBegin
			{
				get { return base.RequestPeriodBegin; }
				set { base.RequestPeriodBegin = value; }
			}
			[PropertyMapName("RequestPeriodEnd")]
			public override DateTime? RequestPeriodEnd
			{
				get { return base.RequestPeriodEnd; }
				set { base.RequestPeriodEnd = value; }
			}

			#endregion

			#region Request Remark

			[MaxLength(255)]
			[PropertyMapName("RequestRemark")]
			public override string RequestRemark
			{
				get { return base.RequestRemark; }
				set { base.RequestRemark = value; }
			}

			#endregion

			#region Approve Exchange/Borrow/Additional

			[PropertyMapName("ApproveExchangeBHT")]
			public override decimal ApproveExchangeBHT
			{
				get { return base.ApproveExchangeBHT; }
				set { base.ApproveExchangeBHT = value; }
			}
			[PropertyMapName("ApproveBorrowBHT")]
			public override decimal ApproveBorrowBHT
			{
				get { return base.ApproveBorrowBHT; }
				set { base.ApproveBorrowBHT = value; }
			}
			[PropertyMapName("ApproveAdditionalBHT")]
			public override decimal ApproveAdditionalBHT
			{
				get { return base.ApproveAdditionalBHT; }
				set { base.ApproveAdditionalBHT = value; }
			}

			#endregion

			#region Approve Period

			[PropertyMapName("ApprovePeriodBegin")]
			public override DateTime? ApprovePeriodBegin
			{
				get { return base.ApprovePeriodBegin; }
				set { base.ApprovePeriodBegin = value; }
			}
			[PropertyMapName("ApprovePeriodEnd")]
			public override DateTime? ApprovePeriodEnd
			{
				get { return base.ApprovePeriodEnd; }
				set { base.ApprovePeriodEnd = value; }
			}

			#endregion

			#region Approve Remark

			[MaxLength(255)]
			[PropertyMapName("ApproveRemark")]
			public override string ApproveRemark
			{
				get { return base.ApproveRemark; }
				set { base.ApproveRemark = value; }
			}

			#endregion

			#endregion
		}

		#endregion

		#region Static Methods

		public static NDbResult<List<TSBExchangeGroup>> GetRequestApproveTSBExchangeGroups(TSB tsb)
		{
			var result = new NDbResult<List<TSBExchangeGroup>>();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			if (null == tsb)
			{
				result.ParameterIsNull();
				return result;
			}

			lock (sync)
			{
				MethodBase med = MethodBase.GetCurrentMethod();
				try
				{
					string cmd = string.Empty;
					cmd += "SELECT * ";
					cmd += "  FROM TSBExchangeGroupView ";
					cmd += " WHERE TSBId = ? ";
					cmd += "   AND (State = ? OR State = ?)";
					cmd += "   AND FinishFlag = ? ";

					var rets = NQuery.Query<FKs>(cmd, tsb.TSBId,
						StateTypes.Request, StateTypes.Approve,
						FinishedFlags.Avaliable).ToList();
					var results = rets.ToModels();
					result.Success(results);
				}
				catch (Exception ex)
				{
					med.Err(ex);
					result.Error(ex);
				}
				return result;
			}
		}
		/// <summary>
		/// Gets TSB Exchange Group by PKId.
		/// </summary>
		/// <param name="tsb">The TSB instance.</param>
		/// <param name="requestId">The Request Id (PKId).</param>
		/// <returns></returns>
		public static NDbResult<TSBExchangeGroup> GetTSBExchangeGroup(TSB tsb, int requestId)
		{
			var result = new NDbResult<TSBExchangeGroup>();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			if (null == tsb)
			{
				result.ParameterIsNull();
				return result;
			}

			lock (sync)
			{
				MethodBase med = MethodBase.GetCurrentMethod();
				try
				{
					string cmd = string.Empty;
					cmd += "SELECT * ";
					cmd += "  FROM TSBExchangeGroupView ";
					cmd += " WHERE TSBId = ? ";
					cmd += "   AND PKId = ? ";

					var ret = NQuery.Query<FKs>(cmd, tsb.TSBId, requestId).FirstOrDefault();
					var val = (null != ret) ? ret.ToModel() : null;
					result.Success(val);
				}
				catch (Exception ex)
				{
					med.Err(ex);
					result.Error(ex);
				}
				return result;
			}
		}
		/// <summary>
		/// Gets TSB Request Exchange Groups.
		/// </summary>
		/// <param name="tsb">The TSB instance.</param>
		/// <param name="requestOnly">true for filter only request state.</param>
		/// <returns></returns>
		public static NDbResult<List<TSBExchangeGroup>> GetRequestExchangeGroups(TSB tsb, bool requestOnly = false)
		{
			var result = new NDbResult<List<TSBExchangeGroup>>();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			if (null == tsb)
			{
				result.ParameterIsNull();
				return result;
			}

			lock (sync)
			{
				MethodBase med = MethodBase.GetCurrentMethod();

				StateTypes state1 = StateTypes.Request;
				StateTypes state2 = StateTypes.Approve;
				FinishedFlags flag = FinishedFlags.Avaliable;

				try
				{
					string cmd = string.Empty;
					cmd += "SELECT * ";
					cmd += "  FROM TSBExchangeGroupView ";
					cmd += " WHERE TSBId = ? ";
					cmd += "   AND FinishFlag = ? ";
					if (requestOnly)
					{
						// request only
						cmd += "   AND State = ?";
						cmd += " ORDER BY RequestDate DESC";

						var rets = NQuery.Query<FKs>(cmd, tsb.TSBId, flag, state1).ToList();
						var results = rets.ToModels();
						result.Success(results);
					}
					else
					{
						// request and approve
						cmd += "   AND (State = ? OR State = ?)";
						cmd += " ORDER BY RequestDate DESC";

						var rets = NQuery.Query<FKs>(cmd, tsb.TSBId, flag, state1, state2).ToList();
						var results = rets.ToModels();
						result.Success(results);
					}

				}
				catch (Exception ex)
				{
					med.Err(ex);
					result.Error(ex);
				}
				return result;
			}
		}
		/*
		private static TSBCreditTransaction CloneTransaction(TSBExchangeTransaction value, bool isMinuis = false)
		{
			int sign = (isMinuis) ? -1 : 1;
			TSBCreditTransaction inst = new TSBCreditTransaction();

			// Common
			inst.TransactionDate = value.TransactionDate;
			inst.GroupId = value.GroupId;
			// TSB.
			inst.TSBId = value.TSBId;
			inst.TSBNameEN = value.TSBNameEN;
			inst.TSBNameTH = value.TSBNameTH;
			// Supervisor.
			inst.SupervisorId = value.UserId;
			inst.SupervisorNameEN = value.FullNameEN;
			inst.SupervisorNameTH = value.FullNameTH;
			// Amount
			inst.AmountST25 = sign * value.AmountST25;
			inst.AmountST50 = sign * value.AmountST50;
			inst.AmountBHT1 = sign * value.AmountBHT1;
			inst.AmountBHT2 = sign * value.AmountBHT2;
			inst.AmountBHT5 = sign * value.AmountBHT5;
			inst.AmountBHT10 = sign * value.AmountBHT10;
			inst.AmountBHT20 = sign * value.AmountBHT20;
			inst.AmountBHT50 = sign * value.AmountBHT50;
			inst.AmountBHT100 = sign * value.AmountBHT100;
			inst.AmountBHT500 = sign * value.AmountBHT500;
			inst.AmountBHT1000 = sign * value.AmountBHT1000;

			return inst;
		}
		*/
		/// <summary>
		/// Save TSB Exchange Group.
		/// </summary>
		/// <param name="value">The TSBExchangeGroup instance.</param>
		/// <returns>Returns save TSBExchangeGroup instance.</returns>
		public static NDbResult<TSBExchangeGroup> SaveTSBExchangeGroup(TSBExchangeGroup value)
		{
			var result = new NDbResult<TSBExchangeGroup>();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			if (null == value)
			{
				result.ParameterIsNull();
			}
			if (value.GroupId == Guid.Empty)
			{
				value.GroupId = Guid.NewGuid();
			}
			result = Save(value); // save group.

			return result;
		}

		#endregion
	}

	#endregion
}
