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
	#region Role

	/// <summary>
	/// The Role Data Model Class.
	/// </summary>
	[TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
	[Serializable]
	[JsonObject(MemberSerialization.OptOut)]
	//[Table("Role")]
	public class Role : NTable<Role>
	{
		#region Intenral Variables

		private string _RoleId = string.Empty;
		private int _GroupId = 0;
		private string _RoleNameEN = string.Empty;
		private string _RoleNameTH = string.Empty;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public Role() : base() { }

		#endregion

		#region Public Proprties

		#region Common

		/// <summary>
		/// Gets or sets RoleId
		/// </summary>
		[Category("Role")]
		[Description("Gets or sets RoleId")]
		[ReadOnly(true)]
		[PrimaryKey, MaxLength(20)]
		[PropertyMapName("RoleId")]
		public string RoleId
		{
			get
			{
				return _RoleId;
			}
			set
			{
				if (_RoleId != value)
				{
					_RoleId = value;
					this.RaiseChanged("RoleId");
				}
			}
		}
		/// <summary>
		/// Gets or sets Group Id.
		/// </summary>
		[Category("Role")]
		[Description("Gets or sets Group Id.")]
		[NotNull]
		[Indexed]
		[PropertyMapName("GroupId")]
		public int GroupId
		{
			get
			{
				return _GroupId;
			}
			set
			{
				if (_GroupId != value)
				{
					_GroupId = value;
					this.RaiseChanged("GroupId");
				}
			}
		}
		/// <summary>
		/// Gets or sets RoleNameEN
		/// </summary>
		[Category("Role")]
		[Description("Gets or sets RoleNameEN")]
		[MaxLength(50)]
		[PropertyMapName("RoleNameEN")]
		public string RoleNameEN
		{
			get
			{
				return _RoleNameEN;
			}
			set
			{
				if (_RoleNameEN != value)
				{
					_RoleNameEN = value;
					this.RaiseChanged("RoleNameEN");
				}
			}
		}
		/// <summary>
		/// Gets or sets RoleNameTH
		/// </summary>
		[Category("Role")]
		[Description("Gets or sets RoleNameTH")]
		[MaxLength(50)]
		[PropertyMapName("RoleNameTH")]
		public string RoleNameTH
		{
			get
			{
				return _RoleNameTH;
			}
			set
			{
				if (_RoleNameTH != value)
				{
					_RoleNameTH = value;
					this.RaiseChanged("RoleNameTH");
				}
			}
		}

		#endregion

		#endregion

		#region Static Methods

		#region Get Rules

		/// <summary>
		/// Gets Roles.
		/// </summary>
		/// <param name="db">The database connection.</param>
		/// <returns>Returns List of Role.</returns>
		public static NDbResult<List<Role>> GetRoles(SQLiteConnection db)
		{
			var result = new NDbResult<List<Role>>();
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
					cmd += "SELECT * FROM Role ";
					var results = NQuery.Query<Role>(cmd);
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
		/// Gets Roles.
		/// </summary>
		/// <returns>Returns List of Role.</returns>
		public static NDbResult<List<Role>> GetRoles()
		{
			lock (sync)
			{
				SQLiteConnection db = Default;
				return GetRoles(db);
			}
		}

		#endregion

		#region Get Role by RoleId

		/// <summary>
		/// Gets Role.
		/// </summary>
		/// <param name="db">The database connection.</param>
		/// <param name="roleId">The Role Id.</param>
		/// <returns>Returns Role instance.</returns>
		public static NDbResult<Role> GetRole(SQLiteConnection db, string roleId)
		{
			var result = new NDbResult<Role>();
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
					cmd += "SELECT * FROM Role ";
					cmd += " WHERE RoleId = ? ";
					var results = NQuery.Query<Role>(cmd, roleId).FirstOrDefault();
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
		/// Gets Role.
		/// </summary>
		/// <param name="roleId">The Role Id.</param>
		/// <returns>Returns Role instance.</returns>
		public static NDbResult<Role> GetRole(string roleId)
		{
			lock (sync)
			{
				SQLiteConnection db = Default;
				return GetRole(db, roleId);
			}
		}

		#endregion

		#region Save Role

		/// <summary>
		/// Save Role.
		/// </summary>
		/// <param name="value">The Role instance.</param>
		/// <returns>Returns Role instance.</returns>
		public static NDbResult<Role> SaveRole(Role value)
		{
			var result = new NDbResult<Role>();
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

	#region Search (Role)
	/*
	partial class Search
	{
		/// <summary>
		/// Search (Role).
		/// </summary>
		public static class Roles
		{
			#region ById

			/// <summary>
			/// Seach Role By Role Id.
			/// </summary>
			public class ById : NSearch<ById>
			{
				#region Public Properties

				/// <summary>
				/// Gets or sets Role Id.
				/// </summary>
				public string RoleId { get; set; }

				#endregion

				#region Static Method (Create)

				/// <summary>
				/// Create Search instance.
				/// </summary>
				/// <param name="roleId">The Role Id.</param>
				/// <returns>Returns Search instance.</returns>
				public static ById Create(string roleId)
				{
					var ret = new ById();
					ret.RoleId = roleId;
					return ret;
				}

				#endregion
			}

			#endregion
		}
	}
	*/
	#endregion
}
