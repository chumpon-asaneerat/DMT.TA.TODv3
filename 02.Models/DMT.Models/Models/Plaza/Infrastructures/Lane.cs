#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using NLib;
using NLib.Design;
using NLib.Reflection;

using SQLite;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;
// required for JsonIgnore attribute.
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Reflection;

#endregion

namespace DMT.Models
{
	#region Lane

	/// <summary>
	/// The Lane Data Model class.
	/// </summary>
	[TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
	[Serializable]
	[JsonObject(MemberSerialization.OptOut)]
	//[Table("Lane")]
	public class Lane : NTable<Lane>
	{
		#region Intenral Variables

		private int _PkId = 0;
		private int _LaneNo = 0;
		private string _LaneId = string.Empty;
		private string _LaneType = string.Empty;
		private string _LaneAbbr = string.Empty;

		private string _TSBId = string.Empty;
		private string _TSBNameEN = string.Empty;
		private string _TSBNameTH = string.Empty;

		private string _PlazaGroupId = string.Empty;
		private string _PlazaGroupNameEN = string.Empty;
		private string _PlazaGroupNameTH = string.Empty;
		private string _Direction = string.Empty;

		private string _PlazaId = string.Empty;
		private int _SCWPlazaId = 0;
		private string _PlazaNameEN = string.Empty;
		private string _PlazaNameTH = string.Empty;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public Lane() : base() { }

		#endregion

		#region Public Proprties

		#region Common

		/// <summary>
		/// Gets or sets LanePkId
		/// </summary>
		[Category("Lane")]
		[Description("Gets or sets LanePkId")]
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
		/// Gets or sets Lane No.
		/// </summary>
		[Category("Lane")]
		[Description("Gets or sets Lane No.")]
		[PropertyMapName("LaneNo")]
		public int LaneNo
		{
			get
			{
				return _LaneNo;
			}
			set
			{
				if (_LaneNo != value)
				{
					_LaneNo = value;
					this.RaiseChanged("LaneNo");
				}
			}
		}
		/// <summary>
		/// Gets or sets LaneId
		/// </summary>
		[Category("Lane")]
		[Description("Gets or sets LaneId")]
		[MaxLength(10)]
		[PropertyMapName("LaneId")]
		public string LaneId
		{
			get
			{
				return _LaneId;
			}
			set
			{
				if (_LaneId != value)
				{
					_LaneId = value;
					this.RaiseChanged("LaneId");
				}
			}
		}
		/// <summary>
		/// Gets or sets LaneType
		/// </summary>
		[Category("Lane")]
		[Description("Gets or sets LaneType")]
		[MaxLength(10)]
		[PropertyMapName("LaneType")]
		public string LaneType
		{
			get
			{
				return _LaneType;
			}
			set
			{
				if (_LaneType != value)
				{
					_LaneType = value;
					this.RaiseChanged("LaneType");
				}
			}
		}
		/// <summary>
		/// Gets or sets LaneAbbr
		/// </summary>
		[Category("Lane")]
		[Description("Gets or sets LaneAbbr")]
		[MaxLength(10)]
		[PropertyMapName("LaneAbbr")]
		public string LaneAbbr
		{
			get
			{
				return _LaneAbbr;
			}
			set
			{
				if (_LaneAbbr != value)
				{
					_LaneAbbr = value;
					this.RaiseChanged("LaneAbbr");
				}
			}
		}

		#endregion

		#region TSB

		/// <summary>
		/// Gets or sets TSBId.
		/// </summary>
		[Category("TSB")]
		[Description("Gets or sets TSBId.")]
		[ReadOnly(true)]
		[NotNull]
		[Indexed]
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

		#endregion

		#region PlazaGroup

		/// <summary>
		/// Gets or sets Plaza Group Id.
		/// </summary>
		[Category("Plaza Group")]
		[Description("Gets or sets Plaza Group Id.")]
		[ReadOnly(true)]
		[NotNull]
		[Indexed]
		[MaxLength(10)]
		[PropertyMapName("PlazaGroupId")]
		public string PlazaGroupId
		{
			get
			{
				return _PlazaGroupId;
			}
			set
			{
				if (_PlazaGroupId != value)
				{
					_PlazaGroupId = value;
					this.RaiseChanged("PlazaGroupId");
				}
			}
		}
		/// <summary>
		/// Gets or sets Plaza Group Name EN.
		/// </summary>
		[Category("Plaza Group")]
		[Description("Gets or sets Plaza Group Name EN.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("PlazaGroupNameEN")]
		public virtual string PlazaGroupNameEN
		{
			get
			{
				return _PlazaGroupNameEN;
			}
			set
			{
				if (_PlazaGroupNameEN != value)
				{
					_PlazaGroupNameEN = value;
					this.RaiseChanged("PlazaGroupNameEN");
				}
			}
		}
		/// <summary>
		/// Gets or sets Plaza Group Name TH.
		/// </summary>
		[Category("Plaza Group")]
		[Description("Gets or sets Plaza Group Name TH.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("PlazaGroupNameTH")]
		public virtual string PlazaGroupNameTH
		{
			get
			{
				return _PlazaGroupNameTH;
			}
			set
			{
				if (_PlazaGroupNameTH != value)
				{
					_PlazaGroupNameTH = value;
					this.RaiseChanged("PlazaGroupNameTH");
				}
			}
		}
		/// <summary>
		/// Gets or sets Direction.
		/// </summary>
		[Category("Plaza Group")]
		[Description("Gets or sets Direction.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("Direction")]
		public virtual string Direction
		{
			get
			{
				return _Direction;
			}
			set
			{
				if (_Direction != value)
				{
					_Direction = value;
					this.RaiseChanged("Direction");
				}
			}
		}

		#endregion

		#region Plaza

		/// <summary>
		/// Gets or sets Plaza Id.
		/// </summary>
		[Category("Plaza")]
		[Description("Gets or sets Plaza Id.")]
		[ReadOnly(true)]
		[NotNull]
		[Indexed]
		[MaxLength(10)]
		[PropertyMapName("PlazaId")]
		public string PlazaId
		{
			get
			{
				return _PlazaId;
			}
			set
			{
				if (_PlazaId != value)
				{
					_PlazaId = value;
					this.RaiseChanged("PlazaId");
				}
			}
		}
		/// <summary>
		/// Gets or sets SCW Plaza Id
		/// </summary>
		[Category("Plaza")]
		[Description("Gets or sets SCW Plaza Id")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("SCWPlazaId")]
		public virtual int SCWPlazaId
		{
			get
			{
				return _SCWPlazaId;
			}
			set
			{
				if (_SCWPlazaId != value)
				{
					_SCWPlazaId = value;
					this.RaiseChanged("SCWPlazaId");
				}
			}
		}
		/// <summary>
		/// Gets or sets Plaza Name EN
		/// </summary>
		[Category("Plaza")]
		[Description("Gets or sets Plaza Name EN")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("PlazaNameEN")]
		public virtual string PlazaNameEN
		{
			get
			{
				return _PlazaNameEN;
			}
			set
			{
				if (_PlazaNameEN != value)
				{
					_PlazaNameEN = value;
					this.RaiseChanged("PlazaNameEN");
				}
			}
		}
		/// <summary>
		/// Gets or sets Plaza Name TH
		/// </summary>
		[Category("Plaza")]
		[Description("Gets or sets Plaza Name TH")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("PlazaNameTH")]
		public virtual string PlazaNameTH
		{
			get
			{
				return _PlazaNameTH;
			}
			set
			{
				if (_PlazaNameTH != value)
				{
					_PlazaNameTH = value;
					this.RaiseChanged("PlazaNameTH");
				}
			}
		}

		#endregion

		#endregion

		#region Internal Class

		/// <summary>
		/// The internal FKs class for query data.
		/// </summary>
		public class FKs : Lane, IFKs<Lane>
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

			#endregion

			#region PlazaGroup

			/// <summary>
			/// Gets or sets Plaza Group Name EN.
			/// </summary>
			[MaxLength(100)]
			[PropertyMapName("PlazaGroupNameEN")]
			public override string PlazaGroupNameEN
			{
				get { return base.PlazaGroupNameEN; }
				set { base.PlazaGroupNameEN = value; }
			}
			/// <summary>
			/// Gets or sets Plaza Group Name TH.
			/// </summary>
			[MaxLength(100)]
			[PropertyMapName("PlazaGroupNameTH")]
			public override string PlazaGroupNameTH
			{
				get { return base.PlazaGroupNameTH; }
				set { base.PlazaGroupNameTH = value; }
			}
			/// <summary>
			/// Gets or sets Direction.
			/// </summary>
			[MaxLength(10)]
			[PropertyMapName("Direction")]
			public override string Direction
			{
				get { return base.Direction; }
				set { base.Direction = value; }
			}

			#endregion

			#region Plaza

			/// <summary>
			/// Gets or sets SCW Plaza Id.
			/// </summary>
			[PropertyMapName("SCWPlazaId")]
			public override int SCWPlazaId
			{
				get { return base.SCWPlazaId; }
				set { base.SCWPlazaId = value; }
			}
			/// <summary>
			/// Gets or sets Plaza Name EN.
			/// </summary>
			[MaxLength(100)]
			[PropertyMapName("PlazaNameEN")]
			public override string PlazaNameEN
			{
				get { return base.PlazaNameEN; }
				set { base.PlazaNameEN = value; }
			}
			/// <summary>
			/// Gets or sets Plaza Name TH.
			/// </summary>
			[MaxLength(100)]
			[PropertyMapName("PlazaNameTH")]
			public override string PlazaNameTH
			{
				get { return base.PlazaNameTH; }
				set { base.PlazaNameTH = value; }
			}

			#endregion
		}

		#endregion

		#region Static Methods

		#region Get Lanes (all TSBs)

		/// <summary>
		/// Gets Lanes (all TSBs).
		/// </summary>
		/// <param name="db">The database connection.</param>
		/// <returns>Returns List fo Lanes.</returns>
		public static NDbResult<List<Lane>> GetLanes(SQLiteConnection db)
		{
			var result = new NDbResult<List<Lane>>();
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			lock (sync)
			{
				MethodBase med = MethodBase.GetCurrentMethod();
				try
				{
					string cmd = string.Empty;
					cmd += "SELECT * ";
					cmd += "  FROM LaneView ";

					var rets = NQuery.Query<FKs>(cmd).ToList();
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
		/// Gets Lanes (all TSBs).
		/// </summary>
		/// <returns>Returns List fo Lanes.</returns>
		public static NDbResult<List<Lane>> GetLanes()
		{
			lock (sync)
			{
				SQLiteConnection db = Default;
				return GetLanes(db);
			}
		}

		#endregion

		#region Get Lane By Land Id

		/// <summary>
		/// Get Lane.
		/// </summary>
		/// <param name="db">The database connection.</param>
		/// <param name="laneId">The lane Id.</param>
		/// <returns>Returns instance of Lane.</returns>
		public static NDbResult<Lane> GetLane(SQLiteConnection db, string laneId)
		{
			var result = new NDbResult<Lane>();
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			lock (sync)
			{
				MethodBase med = MethodBase.GetCurrentMethod();
				try
				{
					string cmd = string.Empty;
					cmd += "SELECT * ";
					cmd += "  FROM LaneView ";
					cmd += " WHERE LaneId = ? ";

					var ret = NQuery.Query<FKs>(cmd, laneId).FirstOrDefault();
					var data = (null != ret) ? ret.ToModel() : null;
					result.Success(data);
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
		/// Get Lane.
		/// </summary>
		/// <param name="laneId">The lane Id.</param>
		/// <returns>Returns instance of Lane.</returns>
		public static NDbResult<Lane> GetLane(string laneId)
		{
			lock (sync)
			{
				SQLiteConnection db = Default;
				return GetLane(db, laneId);
			}
		}

		#endregion

		#region Get Lane By Land No

		/// <summary>
		/// Get Lane.
		/// </summary>
		/// <param name="db">The database connection.</param>
		/// <param name="laneNo">The lane No.</param>
		/// <returns>Returns instance of Lane.</returns>
		public static NDbResult<Lane> GetLane(SQLiteConnection db, int laneNo)
		{
			var result = new NDbResult<Lane>();
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			lock (sync)
			{
				MethodBase med = MethodBase.GetCurrentMethod();
				try
				{
					string cmd = string.Empty;
					cmd += "SELECT * ";
					cmd += "  FROM LaneView ";
					cmd += " WHERE LaneNo = ? ";

					var ret = NQuery.Query<FKs>(cmd, laneNo).FirstOrDefault();
					var data = (null != ret) ? ret.ToModel() : null;
					result.Success(data);
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
		/// Get Lane.
		/// </summary>
		/// <param name="laneNo">The lane No.</param>
		/// <returns>Returns instance of Lane.</returns>
		public static NDbResult<Lane> GetLane(int laneNo)
		{
			lock (sync)
			{
				SQLiteConnection db = Default;
				return GetLane(db, laneNo);
			}
		}

		#endregion

		#region Get Lanes by TSB/TSBId

		/// <summary>
		/// Get Lanes (By TSB).
		/// </summary>
		/// <param name="value">The TSB instance.</param>
		/// <returns>Returns List fo Lanes.</returns>
		public static NDbResult<List<Lane>> GetTSBLanes(TSB value)
		{
			var result = new NDbResult<List<Lane>>();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			lock (sync)
			{
				return GetTSBLanes(value.TSBId);
			}
		}
		/// <summary>
		/// Gets Lanes (By TSBId).
		/// </summary>
		/// <param name="tsbId">The TSB Id.</param>
		/// <returns>Returns List fo Lanes.</returns>
		public static NDbResult<List<Lane>> GetTSBLanes(string tsbId)
		{
			var result = new NDbResult<List<Lane>>();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			lock (sync)
			{
				MethodBase med = MethodBase.GetCurrentMethod();
				try
				{
					string cmd = string.Empty;
					cmd += "SELECT * ";
					cmd += "  FROM LaneView ";
					cmd += " WHERE TSBId = ? ";
					var rets = NQuery.Query<FKs>(cmd, tsbId).ToList();
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

		#endregion

		#region Get Lanes by PlazaGroup and by TSBId/PlazaGroupId

		/// <summary>
		/// Gets Lanes (By PlazaGroup).
		/// </summary>
		/// <param name="value">The PlazaGroup instance.</param>
		/// <returns>Returns List fo Lanes.</returns>
		public static NDbResult<List<Lane>> GetPlazaGroupLanes(PlazaGroup value)
		{
			var result = new NDbResult<List<Lane>>();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			lock (sync)
			{
				return GetPlazaGroupLanes(value.TSBId, value.PlazaGroupId);
			}
		}
		/// <summary>
		/// Gets Lanes (By TSBId, PlazaGroupId)
		/// </summary>
		/// <param name="tsbId">The TSB Id.</param>
		/// <param name="plazaGroupId">The Plaza Group Id.</param>
		/// <returns>Returns List fo Lanes.</returns>
		public static NDbResult<List<Lane>> GetPlazaGroupLanes(string tsbId, string plazaGroupId)
		{
			var result = new NDbResult<List<Lane>>();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			lock (sync)
			{
				MethodBase med = MethodBase.GetCurrentMethod();
				try
				{
					string cmd = string.Empty;
					cmd += "SELECT * ";
					cmd += "  FROM LaneView ";
					cmd += " WHERE TSBId = ? ";
					cmd += "   AND PlazaGroupId = ? ";

					var rets = NQuery.Query<FKs>(cmd, tsbId, plazaGroupId).ToList();
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

		#endregion

		#region Get Lanes by Plaza and by TSBId/PlazaGroupId/PlazaId

		/// <summary>
		/// Gets Lanes (By Plaza).
		/// </summary>
		/// <param name="value">The Plaza instance.</param>
		/// <returns>Returns List fo Lanes.</returns>
		public static NDbResult<List<Lane>> GetPlazaLanes(Plaza value)
		{
			var result = new NDbResult<List<Lane>>();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			lock (sync)
			{
				return GetPlazaLanes(value.TSBId, value.PlazaGroupId, value.PlazaId);
			}
		}
		/// <summary>
		/// Gets Lanes (By TSBId, PlazaGroupId. PlazaId).
		/// </summary>
		/// <param name="tsbId">The TSB Id.</param>
		/// <param name="plazaGroupId">The Plaza Group Id.</param>
		/// <param name="plazaId">The Plaza Id.</param>
		/// <returns>Returns List fo Lanes.</returns>
		public static NDbResult<List<Lane>> GetPlazaLanes(string tsbId, string plazaGroupId, string plazaId)
		{
			var result = new NDbResult<List<Lane>>();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			lock (sync)
			{
				MethodBase med = MethodBase.GetCurrentMethod();
				try
				{
					string cmd = string.Empty;
					cmd += "SELECT * ";
					cmd += "  FROM LaneView ";
					cmd += " WHERE TSBId = ? ";
					cmd += "   AND PlazaGroupId = ? ";
					cmd += "   AND PlazaId = ? ";

					var rets = NQuery.Query<FKs>(cmd, tsbId, plazaGroupId, plazaId).ToList();
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

		#endregion

		#region Get Lane by PlazaId/LaneNo

		/// <summary>
		/// Gets Plaza Lane.
		/// </summary>
		/// <param name="plazaId">The plaza Id.</param>
		/// <param name="laneNo">The lane number.</param>
		/// <returns>Returns match lane.</returns>
		public static NDbResult<Lane> GetPlazaLane(string plazaId, int laneNo)
		{
			var result = new NDbResult<Lane>();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			lock (sync)
			{
				MethodBase med = MethodBase.GetCurrentMethod();
				try
				{
					string cmd = string.Empty;
					cmd += "SELECT * ";
					cmd += "  FROM LaneView ";
					cmd += " WHERE PlazaId = ? ";
					cmd += "   AND LaneNo = ? ";

					var ret = NQuery.Query<FKs>(cmd, plazaId, laneNo).FirstOrDefault();
					var data = (null != ret) ? ret.ToModel() : null;
					result.Success(data);
				}
				catch (Exception ex)
				{
					med.Err(ex);
					result.Error(ex);
				}
				return result;
			}
		}

		#endregion

		#region Save Lane

		/// <summary>
		/// Save Lane.
		/// </summary>
		/// <param name="value">The Lane instance.</param>
		/// <returns>Returns Lane instance.</returns>
		public static NDbResult<Lane> SaveLane(Lane value)
		{
			var result = new NDbResult<Lane>();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			lock (sync)
			{
				MethodBase med = MethodBase.GetCurrentMethod();
				try
				{
					//TODO: Required check exists before save see SaveUser for implementations.
					result = Save(value);

				}
				catch (Exception ex)
				{
					med.Err(ex);
					result.Error(ex);
				}
				return result;
			}
		}

		#endregion

		#endregion
	}

	#endregion
}
