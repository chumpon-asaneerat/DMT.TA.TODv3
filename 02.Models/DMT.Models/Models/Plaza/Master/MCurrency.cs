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
		public MCurrency() : base() { }

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

		#endregion

		#region Static Methods

		/// <summary>
		/// Gets Currencies.
		/// </summary>
		/// <param name="db">The database connection.</param>
		/// <returns>Returns List of Coupon Master.</returns>
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
		/// <returns>Returns List of Coupon Master.</returns>
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
				try
				{
					db.BeginTransaction();
					values.ForEach(value =>
					{
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
