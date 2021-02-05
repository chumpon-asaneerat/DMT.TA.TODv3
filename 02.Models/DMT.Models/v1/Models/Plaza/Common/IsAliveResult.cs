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
	#region IsAliveResult

	/// <summary>
	/// The IsAliveResult Model Class.
	/// </summary>
	[JsonObject(MemberSerialization.OptOut)]
	public class IsAliveResult
	{
		/// <summary>Gets or sets Time Stamp.</summary>
		//[PropertyMapName("TimeStamp")]
		public DateTime? TimeStamp { get; set; }
	}

	#endregion
}
