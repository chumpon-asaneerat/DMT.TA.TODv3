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
	#region PlazaGroup

	/// <summary>
	/// The PlazaGroup Data Model class.
	/// </summary>
	[TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
	[Serializable]
	[JsonObject(MemberSerialization.OptOut)]
	//[Table("Plaza")]
	public class PlazaGroup : NTable<PlazaGroup>
	{
		#region Intenral Variables

		private string _PlazaGroupId = string.Empty;
		private string _PlazaGroupNameEN = string.Empty;
		private string _PlazaGroupNameTH = string.Empty;
		private string _Direction = string.Empty;

		private string _TSBId = string.Empty;
		private string _TSBNameEN = string.Empty;
		private string _TSBNameTH = string.Empty;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public PlazaGroup() : base() { }

		#endregion

		#region Public Proprties

		#region Common

		/// <summary>
		/// Gets or sets PlazaId.
		/// </summary>
		[Category("Plaza Group")]
		[Description("Gets or sets PlazaId.")]
		[PrimaryKey, MaxLength(10)]
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
		/// Gets or sets PlazaGroupNameEN
		/// </summary>
		[Category("Plaza Group")]
		[Description("Gets or sets PlazaGroupNameEN")]
		[MaxLength(100)]
		[PropertyMapName("PlazaGroupNameEN")]
		public string PlazaGroupNameEN
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
		/// Gets or sets PlazaGroupNameTH
		/// </summary>
		[Category("Plaza Group")]
		[Description("Gets or sets PlazaGroupNameTH")]
		[MaxLength(100)]
		[PropertyMapName("PlazaGroupNameTH")]
		public string PlazaGroupNameTH
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
		/// Gets or sets Direction
		/// </summary>
		[Category("Plaza Group")]
		[Description("Gets or sets Direction")]
		[MaxLength(10)]
		[PropertyMapName("Direction")]
		public string Direction
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

		#endregion

		#region Internal Class

		/// <summary>
		/// The internal FKs class for query data.
		/// </summary>
		public class FKs : PlazaGroup, IFKs<PlazaGroup>
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
		}

		#endregion

		#region Static Methods

		#region Get PlazaGroups (all TSBs)

		/// <summary>
		/// Gets PlazaGroups.
		/// </summary>
		/// <param name="db">The database connection.</param>
		/// <returns>Returns List of PlazaGroup.</returns>
		public static NDbResult<List<PlazaGroup>> GetPlazaGroups(SQLiteConnection db)
		{
			var result = new NDbResult<List<PlazaGroup>>();
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
					cmd += "  FROM PlazaGroupView ";

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
		/// Gets PlazaGroups.
		/// </summary>
		/// <returns>Returns List of PlazaGroup.</returns>
		public static NDbResult<List<PlazaGroup>> GetPlazaGroups()
		{
			lock (sync)
			{
				SQLiteConnection db = Default;
				return GetPlazaGroups(db);
			}
		}

		#endregion

		#region Get PlazaGroup By PlazaGroupId

		/// <summary>
		/// Gets PlazaGroup (By PlazaGroupId).
		/// </summary>
		/// <param name="db">The database connection.</param>
		/// <param name="plazaGroupId">The Plaza Group Id.</param>
		/// <returns>Returns PlazaGroup instance.</returns>
		public static NDbResult<PlazaGroup> GetPlazaGroup(SQLiteConnection db, string plazaGroupId)
		{
			var result = new NDbResult<PlazaGroup>();
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
					cmd += "  FROM PlazaGroupView ";
					cmd += " WHERE PlazaGroupId = ? ";
					var ret = NQuery.Query<FKs>(cmd, plazaGroupId).FirstOrDefault();
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
		/// Gets PlazaGroup (By PlazaGroupId).
		/// </summary>
		/// <param name="plazaGroupId">The Plaza Group Id</param>
		/// <returns>Returns PlazaGroup instance.</returns>
		public static NDbResult<PlazaGroup> GetPlazaGroup(string plazaGroupId)
		{
			lock (sync)
			{
				SQLiteConnection db = Default;
				return GetPlazaGroup(db, plazaGroupId);
			}
		}

		#endregion

		#region Get PlazaGroups By TSB/TSBId

		/// <summary>
		/// Gets PlazaGroups (By TSB).
		/// </summary>
		/// <param name="value">The TSB instance.</param>
		/// <returns>Returns List of PlazaGroup.</returns>
		public static NDbResult<List<PlazaGroup>> GetTSBPlazaGroups(TSB value)
		{
			var result = new NDbResult<List<PlazaGroup>>();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			lock (sync)
			{
				return GetTSBPlazaGroups(value.TSBId);
			}
		}
		/// <summary>
		/// Gets PlazaGroups (By TSBId).
		/// </summary>
		/// <param name="tsbId">The TSB Id.</param>
		/// <returns>Returns List of PlazaGroup.</returns>
		public static NDbResult<List<PlazaGroup>> GetTSBPlazaGroups(string tsbId)
		{
			var result = new NDbResult<List<PlazaGroup>>();
			lock (sync)
			{
				MethodBase med = MethodBase.GetCurrentMethod();
				try
				{
					string cmd = string.Empty;
					cmd += "SELECT * ";
					cmd += "  FROM PlazaGroupView ";
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

		#region Save PlazaGroup

		/// <summary>
		/// Save PlazaGroup.
		/// </summary>
		/// <param name="value">The PlazaGroup instance.</param>
		/// <returns>Returns PlazaGroup instance.</returns>
		public static NDbResult<PlazaGroup> SavePlazaGroup(PlazaGroup value)
		{
			var result = new NDbResult<PlazaGroup>();
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
