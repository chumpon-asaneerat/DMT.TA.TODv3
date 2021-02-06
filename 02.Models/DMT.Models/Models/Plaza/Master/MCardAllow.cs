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
	// Sample date from SCW
	/*

	{
		"list": [
			{
				"cardAllowId": 1,
				"abbreviation": "Card DMT P1",
				"description": "บัตร DMT (ป 1)"
			},
			{
				"cardAllowId": 2,
				"abbreviation": "Card DMT P2",
				"description": "บัตร DMT (ป 2)"
			}
		],
		"status": {
			"code": "S200",
			"message": "Success"
		}
	}

	*/

	/// <summary>
	/// The MCardAllow Data Model class.
	/// </summary>
	[TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
	[Serializable]
	[JsonObject(MemberSerialization.OptOut)]
	//[Table("MCardAllow")]
	public class MCardAllow : NTable<MCardAllow>
	{
		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public MCardAllow() : base()
		{
			ActiveStatus = MActiveStatus.Active;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets cardAllowId.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets cardAllowId.")]
		[PrimaryKey]
		[PropertyMapName("cardAllowId")]
		public int cardAllowId { get; set; }
		/// <summary>
		/// Gets or sets abbreviation.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets abbreviation.")]
		[PropertyMapName("abbreviation")]
		public string abbreviation { get; set; }
		/// <summary>
		/// Gets or sets description.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets description.")]
		[PropertyMapName("description")]
		public string description { get; set; }

		/// <summary>
		/// Gets or sets active status.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets active status.")]
		[PropertyMapName("ActiveStatus")]
		public MActiveStatus ActiveStatus { get; set; }

		#endregion

		#region Static Methods

		/// <summary>
		/// Gets Card Allows.
		/// </summary>
		/// <param name="db">The database connection.</param>
		/// <returns>Returns List of Card Allow Master.</returns>
		public static NDbResult<List<MCardAllow>> GetCardAllows(SQLiteConnection db)
		{
			var result = new NDbResult<List<MCardAllow>>();
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
					cmd += "SELECT * FROM MCardAllow ";
					result.Success();
					var data = NQuery.Query<MCardAllow>(cmd);
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
		/// Gets Card Allows.
		/// </summary>
		/// <returns>Returns List of Card Allow Master.</returns>
		public static NDbResult<List<MCardAllow>> GetCardAllows()
		{
			lock (sync)
			{
				SQLiteConnection db = Default;
				return GetCardAllows(db);
			}
		}
		/// <summary>
		/// Save all card alllows to database.
		/// </summary>
		/// <param name="values">The List of MCardAllow object.</param>
		/// <returns>Returns NDbResult instance.</returns>
		public static NDbResult SaveMCardAllows(List<MCardAllow> values)
		{
			lock (sync)
			{
				SQLiteConnection db = Default;
				MethodBase med = MethodBase.GetCurrentMethod();
				var result = new NDbResult();
				if (null == db)
				{
					result.DbConenctFailed();
					return result;
				}
				var originals = GetCardAllows().Value();
				try
				{
					db.BeginTransaction();
					values.ForEach(value =>
					{
						var match = originals.Find(item => { return item.cardAllowId == value.cardAllowId; });
						if (null != match) value.ActiveStatus = match.ActiveStatus; // Keep original status.
						MCardAllow.Save(value);
					});
					db.Commit();
					result.Success();
				}
				catch (Exception ex)
				{
					db.Rollback();
					med.Err(ex);
					result.Error(ex);
				}
				return result;
			}
		}

		#endregion
	}
}
