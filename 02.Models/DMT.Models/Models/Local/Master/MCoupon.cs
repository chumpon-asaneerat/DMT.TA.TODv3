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
	/// The MCoupon Data Model class.
	/// </summary>
	[TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
	[Serializable]
	[JsonObject(MemberSerialization.OptOut)]
	//[Table("MCoupon")]
	public class MCoupon : NTable<MCoupon>
	{
		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public MCoupon() : base() { }

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets couponId.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets couponId.")]
		[PrimaryKey]
		[PropertyMapName("couponId")]
		public int couponId { get; set; }
		/// <summary>
		/// Gets or sets couponValue.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets couponValue.")]
		[PropertyMapName("couponValue")]
		public decimal couponValue { get; set; }

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
		/// Gets Coupons.
		/// </summary>
		/// <param name="db">The database connection.</param>
		/// <returns>Returns List of Coupon Master.</returns>
		public static NDbResult<List<MCoupon>> GetCoupons(SQLiteConnection db)
		{
			var result = new NDbResult<List<MCoupon>>();
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
					var data = NQuery.Query<MCoupon>(cmd);
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
		/// Gets Coupons.
		/// </summary>
		/// <returns>Returns List of Coupon Master.</returns>
		public static NDbResult<List<MCoupon>> GetCoupons()
		{
			lock (sync)
			{
				SQLiteConnection db = Default;
				return GetCoupons(db);
			}
		}
		/// <summary>
		/// Save all coupons to database.
		/// </summary>
		/// <param name="values">The List of MCoupon object.</param>
		/// <returns>Returns NDbResult instance.</returns>
		public static NDbResult SaveMCoupons(List<MCoupon> values)
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
						MCoupon.Save(value);
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
