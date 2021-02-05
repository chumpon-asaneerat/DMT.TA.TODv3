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

#endregion

namespace DMT.Models
{
	#region ViewHistory

	/// <summary>
	/// The ViewHistory Data Model Class.
	/// </summary>
	[TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
	[Serializable]
	[JsonObject(MemberSerialization.OptOut)]
	//[Table("ViewHistory")]
	public class ViewHistory : NTable<ViewHistory>
	{
		#region Intenral Variables

		private string _ViewName = string.Empty;
		private int _VersionId = 0;

		#endregion

		#region Constructor

		/// <summary>
		/// ViewHistory.
		/// </summary>
		public ViewHistory() : base() { }

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets ViewName
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets ViewName")]
		[PrimaryKey, MaxLength(100)]
		[PropertyMapName("ViewName")]
		public string ViewName
		{
			get
			{
				return _ViewName;
			}
			set
			{
				if (_ViewName != value)
				{
					_ViewName = value;
					this.RaiseChanged("ViewName");
				}
			}
		}
		/// <summary>
		/// Gets or sets VersionId
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets VersionId")]
		[NotNull]
		[PropertyMapName("VersionId")]
		public int VersionId
		{
			get
			{
				return _VersionId;
			}
			set
			{
				if (_VersionId != value)
				{
					_VersionId = value;
					this.RaiseChanged("VersionId");
				}
			}
		}

		#endregion
	}

	#endregion
}
