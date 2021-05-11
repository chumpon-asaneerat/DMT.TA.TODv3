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
				"currencyId": 1,
				"currencyDenomId": 1,
				"abbreviation": "Satang26",
				"description": "26 Satang",
				"denomValue": 0.26,
				"denomTypeId": 2
			},
			{
				"currencyId": 1,
				"currencyDenomId": 2,
				"abbreviation": "Satang50",
				"description": "50 Satang",
				"denomValue": 0.5,
				"denomTypeId": 2
			},
			{
				"currencyId": 1,
				"currencyDenomId": 3,
				"abbreviation": "Baht1",
				"description": "1 Baht",
				"denomValue": 1,
				"denomTypeId": 2
			},
			{
				"currencyId": 1,
				"currencyDenomId": 4,
				"abbreviation": "Baht2",
				"description": "2 Baht",
				"denomValue": 2,
				"denomTypeId": 2
			},
			{
				"currencyId": 1,
				"currencyDenomId": 5,
				"abbreviation": "Baht5",
				"description": "5 Baht",
				"denomValue": 5,
				"denomTypeId": 2
			},
			{
				"currencyId": 1,
				"currencyDenomId": 6,
				"abbreviation": "CBaht10",
				"description": "10Baht",
				"denomValue": 10,
				"denomTypeId": 2
			},
			{
				"currencyId": 1,
				"currencyDenomId": 7,
				"abbreviation": "NBaht10",
				"description": "10 Baht",
				"denomValue": 10,
				"denomTypeId": 1
			},
			{
				"currencyId": 1,
				"currencyDenomId": 8,
				"abbreviation": "NBaht20",
				"description": "20 Baht",
				"denomValue": 20,
				"denomTypeId": 1
			},
			{
				"currencyId": 1,
				"currencyDenomId": 9,
				"abbreviation": "NBaht50",
				"description": "50 Baht",
				"denomValue": 50,
				"denomTypeId": 1
			},
			{
				"currencyId": 1,
				"currencyDenomId": 10,
				"abbreviation": "NBaht100",
				"description": "100 Baht",
				"denomValue": 100,
				"denomTypeId": 1
			},
			{
				"currencyId": 1,
				"currencyDenomId": 11,
				"abbreviation": "NBaht500",
				"description": "500 Baht",
				"denomValue": 500,
				"denomTypeId": 1
			},
			{
				"currencyId": 1,
				"currencyDenomId": 12,
				"abbreviation": "NBaht1000",
				"description": "1000 Baht",
				"denomValue": 1000,
				"denomTypeId": 1
			}
		],
		"status": {
			"code": "S200",
			"message": "Success"
		}
	}

	*/

	/// <summary>
	/// The MCurrency Data Model class.
	/// </summary>
	[TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
	[Serializable]
	[JsonObject(MemberSerialization.OptOut)]
	//[Table("MCurrency")]
	public class MCurrency : NTable<MCurrency>
	{
		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public MCurrency() : base()
		{
			ActiveStatus = MActiveStatus.Active;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets currencyDenomId.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets currencyDenomId.")]
		[PrimaryKey]
		[PropertyMapName("currencyDenomId")]
		public int currencyDenomId { get; set; }
		/// <summary>
		/// Gets or sets currencyId.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets currencyId.")]
		[PropertyMapName("currencyId")]
		public int currencyId { get; set; }
		/// <summary>
		/// Gets or sets denomTypeId.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets denomTypeId.")]
		[PropertyMapName("denomTypeId")]
		public int denomTypeId { get; set; }
		/// <summary>
		/// Gets or sets denomValue.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets denomValue.")]
		[PropertyMapName("denomValue")]
		public decimal denomValue { get; set; }
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
		/// Gets Currencies.
		/// </summary>
		/// <param name="db">The database connection.</param>
		/// <returns>Returns List of MCurrency.</returns>
		public static NDbResult<List<MCurrency>> GetCurrencies(SQLiteConnection db)
		{
			var result = new NDbResult<List<MCurrency>>();
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
					cmd += "SELECT * FROM MCurrency ";
					result.Success();
					var data = NQuery.Query<MCurrency>(cmd);
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
		/// Gets Currencies.
		/// </summary>
		/// <returns>Returns List of MCurrency.</returns>
		public static NDbResult<List<MCurrency>> GetCurrencies()
		{
			lock (sync)
			{
				SQLiteConnection db = Default;
				return GetCurrencies(db);
			}
		}
		/// <summary>
		/// Save all currencies to database.
		/// </summary>
		/// <param name="values">The List of MCurrency object.</param>
		/// <returns>Returns NDbResult instance.</returns>
		public static NDbResult SaveMCurrencies(List<MCurrency> values)
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
				var originals = GetCurrencies().Value();
				try
				{
					db.BeginTransaction();
					values.ForEach(value =>
					{
						var match = originals.Find(item => { return item.currencyDenomId == value.currencyDenomId; });
						if (null != match) value.ActiveStatus = match.ActiveStatus; // Keep original status.
						MCurrency.Save(value);
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
