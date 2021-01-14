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
	#region Plaza

	/// <summary>
	/// The Plaza Data Model class.
	/// </summary>
	[TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
	[Serializable]
	[JsonObject(MemberSerialization.OptOut)]
	//[Table("Plaza")]
	public class Plaza : NTable<Plaza>
	{
		#region Intenral Variables

		private string _PlazaId = string.Empty;
		private int _SCWPlazaId = 0;
		private string _PlazaNameEN = string.Empty;
		private string _PlazaNameTH = string.Empty;

		private string _TSBId = string.Empty;
		private string _TSBNameEN = string.Empty;
		private string _TSBNameTH = string.Empty;

		private string _PlazaGroupId = string.Empty;
		private string _PlazaGroupNameEN = string.Empty;
		private string _PlazaGroupNameTH = string.Empty;
		private string _Direction = string.Empty;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public Plaza() : base() { }

		#endregion

		#region Public Proprties

		#region Common

		/// <summary>
		/// Gets or sets PlazaId.
		/// </summary>
		[Category("Plaza")]
		[Description("Gets or sets PlazaId.")]
		[PrimaryKey, MaxLength(10)]
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
		/// Gets or sets SCWPlazaId.
		/// </summary>
		[Category("Plaza")]
		[Description("Gets or sets SCWPlazaId.")]
		[PropertyMapName("SCWPlazaId")]
		public int SCWPlazaId
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
		/// Gets or sets PlazaNameEN
		/// </summary>
		[Category("Plaza")]
		[Description("Gets or sets PlazaNameEN")]
		[MaxLength(100)]
		[PropertyMapName("PlazaNameEN")]
		public string PlazaNameEN
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
		/// Gets or sets PlazaNameTH
		/// </summary>
		[Category("Plaza")]
		[Description("Gets or sets PlazaNameTH")]
		[MaxLength(100)]
		[PropertyMapName("PlazaNameTH")]
		public string PlazaNameTH
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

		#endregion

		#region Internal Class

		/// <summary>
		/// The internal FKs class for query data.
		/// </summary>
		public class FKs : Plaza, IFKs<Plaza>
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
		}

		#endregion

		#region Static Methods

		#region Get Plazas (all TSBs)

		/// <summary>
		/// Gets Plazas.
		/// </summary>
		/// <param name="db">The database connection.</param>
		/// <returns>Returns List of Plaza.</returns>
		public static NDbResult<List<Plaza>> GetPlazas(SQLiteConnection db)
		{
			var result = new NDbResult<List<Plaza>>();
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
					cmd += "  FROM PlazaView ";

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
		/// Gets Plazas.
		/// </summary>
		/// <returns>Returns List of Plaza.</returns>
		public static NDbResult<List<Plaza>> GetPlazas()
		{
			lock (sync)
			{
				SQLiteConnection db = Default;
				return GetPlazas(db);
			}
		}

		#endregion

		#region Get Plaza By PlazaId

		/// <summary>
		/// Gets Plaza.
		/// </summary>
		/// <param name="db">The database connection.</param>
		/// <param name="plazaId">The Plaza Id</param>
		/// <returns>Returns Plaza instance.</returns>
		public static NDbResult<Plaza> GetPlaza(SQLiteConnection db, string plazaId)
		{
			var result = new NDbResult<Plaza>();
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
					cmd += "  FROM PlazaView ";
					cmd += " WHERE PlazaId = ? ";
					var ret = NQuery.Query<FKs>(cmd, plazaId).FirstOrDefault();
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
		/// Gets Plaza.
		/// </summary>
		/// <param name="plazaId">The Plaza Id.</param>
		/// <returns>Returns Plaza instance.</returns>
		public static NDbResult<Plaza> GetPlaza(string plazaId)
		{
			lock (sync)
			{
				SQLiteConnection db = Default;
				return GetPlaza(db, plazaId);
			}
		}

		#endregion

		#region Get Plaza By SCWPlazaId

		/// <summary>
		/// Gets Plaza by SCW Id.
		/// </summary>
		/// <param name="db">The database connection.</param>
		/// <param name="scwPlazaId">The SCW Plaza Id</param>
		/// <returns>Returns Plaza instance.</returns>
		public static NDbResult<Plaza> GetPlazaBySCWPlazaId(SQLiteConnection db, int scwPlazaId)
		{
			var result = new NDbResult<Plaza>();
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
					cmd += "  FROM PlazaView ";
					cmd += " WHERE SCWPlazaId = ? ";
					var ret = NQuery.Query<FKs>(cmd, scwPlazaId).FirstOrDefault();
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
		/// Gets Plaza by SCW Id.
		/// </summary>
		/// <param name="scwPlazaId">The SCW Plaza Id.</param>
		/// <returns>Returns Plaza instance.</returns>
		public static NDbResult<Plaza> GetPlazaBySCWPlazaId(int scwPlazaId)
		{
			lock (sync)
			{
				SQLiteConnection db = Default;
				return GetPlazaBySCWPlazaId(db, scwPlazaId);
			}
		}

		#endregion

		#region Get Plazas By TSB and By TSBId

		/// <summary>
		/// Gets Plazas (By TSB).
		/// </summary>
		/// <param name="value">The TSB Instance.</param>
		/// <returns>Returns List of Plaza.</returns>
		public static NDbResult<List<Plaza>> GetTSBPlazas(TSB value)
		{
			var result = new NDbResult<List<Plaza>>();
			SQLiteConnection db = Default;
			if (null == value)
			{
				result.ParameterIsNull();
				return result;
			}

			lock (sync)
			{
				return GetTSBPlazas(value.TSBId);
			}
		}
		/// <summary>
		/// Gets Plazas (By TSB d).
		/// </summary>
		/// <param name="tsbId">The TSB Id.</param>
		/// <returns>Returns List of Plaza.</returns>
		public static NDbResult<List<Plaza>> GetTSBPlazas(string tsbId)
		{
			var result = new NDbResult<List<Plaza>>();
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
					cmd += "  FROM PlazaView ";
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

		#region Get Plazas By PlazaGroup and By TSBId/PlazaGroupId

		/// <summary>
		/// Gets Plazas (By PlazaGroup).
		/// </summary>
		/// <param name="value">The PlazaGroup Instance.</param>
		/// <returns>Returns List of Plaza.</returns>
		public static NDbResult<List<Plaza>> GetPlazaGroupPlazas(PlazaGroup value)
		{
			var result = new NDbResult<List<Plaza>>();
			SQLiteConnection db = Default;
			if (null == value)
			{
				result.ParameterIsNull();
				return result;
			}

			lock (sync)
			{
				return GetPlazaGroupPlazas(value.TSBId, value.PlazaGroupId);
			}
		}
		/// <summary>
		/// Gets Plazas (By PlazaGroup Id).
		/// </summary>
		/// <param name="tsbId">The TSB Id.</param>
		/// <param name="plazaGroupId">The PlazaGroup Id.</param>
		/// <returns>Returns List of Plaza.</returns>
		public static NDbResult<List<Plaza>> GetPlazaGroupPlazas(string tsbId, string plazaGroupId)
		{
			var result = new NDbResult<List<Plaza>>();
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
					cmd += "  FROM PlazaView ";
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

		#region Save Plaza

		/// <summary>
		/// Save Plaza.
		/// </summary>
		/// <param name="value">The Plaza instance.</param>
		/// <returns>Returns Plaza instance.</returns>
		public static NDbResult<Plaza> SavePlaza(Plaza value)
		{
			var result = new NDbResult<Plaza>();
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
