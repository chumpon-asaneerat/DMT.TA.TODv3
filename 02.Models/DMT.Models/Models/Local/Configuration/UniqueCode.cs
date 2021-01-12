#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

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
	#region UniqueCode

	/// <summary>
	/// The UniqueCode Data Model Class.
	/// </summary>
	[TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
	[Serializable]
	[JsonObject(MemberSerialization.OptOut)]
	//[Table("UniqueCode")]
	public class UniqueCode : NTable<UniqueCode>
	{
		#region Enum

		public enum ResetMode
		{
			Yearly = 1,
			Monthly = 2
		}

		#endregion

		#region Intenral Variables

		private string _Key = string.Empty;
		private ResetMode _Mode = ResetMode.Yearly;
		private string _Prefix = string.Empty;
		private int _LastNumber = 0;
		private DateTime? _LastUpdate = new DateTime?();

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public UniqueCode() : base() { }

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets Key
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Key")]
		[PrimaryKey, MaxLength(50)]
		[PropertyMapName("Key")]
		public string Key
		{
			get
			{
				return _Key;
			}
			set
			{
				if (_Key != value)
				{
					_Key = value;
					this.RaiseChanged("Key");
				}
			}
		}
		/// <summary>
		/// Gets or sets Mode
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Mode")]
		[NotNull]
		[PropertyMapName("Mode")]
		public ResetMode Mode
		{
			get
			{
				return _Mode;
			}
			set
			{
				if (_Mode != value)
				{
					_Mode = value;
					this.RaiseChanged("Mode");
				}
			}
		}
		/// <summary>
		/// Gets or sets Prefix
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Prefix")]
		[MaxLength(30)]
		[PropertyMapName("Prefix")]
		public string Prefix
		{
			get
			{
				return _Prefix;
			}
			set
			{
				if (_Prefix != value)
				{
					_Prefix = value;
					this.RaiseChanged("Prefix");
				}
			}
		}
		/// <summary>
		/// Gets or sets Last Number
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Last Number")]
		[NotNull]
		[PropertyMapName("LastNumber")]
		public int LastNumber
		{
			get
			{
				return _LastNumber;
			}
			set
			{
				if (_LastNumber != value)
				{
					_LastNumber = value;
					this.RaiseChanged("LastNumber");
				}
			}
		}
		/// <summary>
		/// Gets or sets Last Update
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Last Update")]
		[ReadOnly(true)]
		[NotNull]
		[PropertyMapName("LastUpdate")]
		public DateTime? LastUpdate
		{
			get
			{
				return _LastUpdate;
			}
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

		#region Static Methods

		/// <summary>
		/// Gets Unique Id By Key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static NDbResult<UniqueCode> GetUniqueId(string key)
		{
			var result = new NDbResult<UniqueCode>();
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
					if (string.IsNullOrWhiteSpace(key))
					{
						result.ParameterIsNull();
						return result;
					}

					string cmd = @"
					SELECT *
					  FROM UniqueCode
					 WHERE Key = ? ";

					var ret = NQuery.Query<UniqueCode>(cmd,
						key).FirstOrDefault();
					var inst = ret;
					if (null == ret)
					{
						// not found key
						inst = new UniqueCode();
						inst.Key = key;
						inst.Mode = ResetMode.Yearly;
						inst.LastUpdate = DateTime.Now;
						inst.LastNumber = 1;
						Save(inst);
					}

					if (null != inst)
					{
						int year = DateTime.Now.Year;
						if (!inst.LastUpdate.HasValue || inst.LastUpdate.Value.Year != year)
						{
							// not same year.
							inst.LastUpdate = DateTime.Now;
							inst.LastNumber = 1;
							Save(inst);
						}
					}

					result.Success(inst);
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
		/// Increase Unique Id by key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static NDbResult<UniqueCode> IncreaseUniqueId(string key)
		{
			var result = new NDbResult<UniqueCode>();
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
					var inst = GetUniqueId(key).Value();
					if (null != inst)
					{
						inst.LastNumber = inst.LastNumber + 1;
						inst.LastUpdate = DateTime.Now;
						Save(inst);

						result.Success(inst);
					}
					else
					{
						result.Error(new Exception("No instance found."));
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

		#endregion
	}

	#endregion
}
