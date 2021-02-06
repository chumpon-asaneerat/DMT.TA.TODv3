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
				"couponBookId": 1,
				"couponBookValue": 665,
				"abbreviation": "35",
				"description": "35 บาท"
			},
			{
				"couponBookId": 2,
				"couponBookValue": 1520,
				"abbreviation": "80",
				"description": "80 บาท"
			}
		],
		"status": {
			"code": "S200",
			"message": "Success"
		}
	}

	*/

	/// <summary>
	/// The MCoupon Data Model class.
	/// </summary>
	[TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
	[Serializable]
	[JsonObject(MemberSerialization.OptOut)]
	//[Table("MCouponBook")]
	public class MCouponBook : NTable<MCouponBook>
	{
		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public MCouponBook() : base()
		{
			ActiveStatus = MActiveStatus.Active;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets couponId.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets couponBookId.")]
		[PrimaryKey]
		[PropertyMapName("couponBookId")]
		public int couponBookId { get; set; }
		/// <summary>
		/// Gets or sets couponValue.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets couponBookValue.")]
		[PropertyMapName("couponBookValue")]
		public decimal couponBookValue { get; set; }

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
		/// Gets Coupon Books.
		/// </summary>
		/// <param name="db">The database connection.</param>
		/// <returns>Returns List of Coupon Book Master.</returns>
		public static NDbResult<List<MCouponBook>> GetCouponBooks(SQLiteConnection db)
		{
			var result = new NDbResult<List<MCouponBook>>();
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
					cmd += "SELECT * FROM MCoupon ";
					result.Success();
					var data = NQuery.Query<MCouponBook>(cmd);
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
		/// Gets Coupon Books.
		/// </summary>
		/// <returns>Returns List of Coupon Book Master.</returns>
		public static NDbResult<List<MCouponBook>> GetCouponBooks()
		{
			lock (sync)
			{
				SQLiteConnection db = Default;
				return GetCouponBooks(db);
			}
		}
		/// <summary>
		/// Save all coupon books to database.
		/// </summary>
		/// <param name="values">The List of MCouponBook object.</param>
		/// <returns>Returns NDbResult instance.</returns>
		public static NDbResult SaveMCouponBooks(List<MCouponBook> values)
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
				var originals = GetCouponBooks().Value();
				try
				{
					db.BeginTransaction();
					values.ForEach(value =>
					{
						var match = originals.Find(item => { return item.couponBookId == value.couponBookId; });
						if (null != match) value.ActiveStatus = match.ActiveStatus; // Keep original status.
						MCouponBook.Save(value);
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
