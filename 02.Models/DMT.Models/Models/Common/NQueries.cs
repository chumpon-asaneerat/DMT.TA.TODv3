#region Using

using System;
using System.Collections.Generic;
using System.Linq;

using SQLite;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;
using System.ComponentModel;
//using DMT.Services;
// required for JsonIgnore.
using Newtonsoft.Json;
using NLib;
using NLib.Reflection;

#endregion

namespace DMT.Models
{
    #region NQuery

    /// <summary>
    /// The NQuery class.
    /// </summary>
    public class NQuery : DMTModelBase
    {
        #region Static Variables and Properties

        /// <summary>
        /// Gets empty object array.
        /// </summary>
        public static readonly object[] Empty = new object[] { };

        /// <summary>
        /// sync object used for lock concurrent access.
        /// </summary>
        protected static object sync = new object();
        /// <summary>
        /// Gets default Connection.
        /// </summary>
        public static SQLiteConnection Default { get; set; }
        /// <summary>
        /// Query.
        /// </summary>
        /// <typeparam name="T">The Target Class.</typeparam>
        /// <param name="db">The connection.</param>
        /// <param name="query">The query string.</param>
        /// <param name="args">The query arguments.</param>
        /// <returns>Returns query result in List.</returns>
        public static List<T> Query<T>(SQLiteConnection db, string query, params object[] args)
            where T: new()
        {
            lock (sync)
            {
                List<T> results = null;
                if (null == db || string.IsNullOrEmpty(query)) return results;
                // execute query.
                results = db.Query<T>(query, args).ToList();
                return results;
            }
        }
        /// <summary>
        /// Query.
        /// </summary>
        /// <typeparam name="T">The Target Class.</typeparam>
        /// <param name="query">The query string.</param>
        /// <param name="args">The query arguments.</param>
        /// <returns>Returns query result in List.</returns>
        public static List<T> Query<T>(string query, params object[] args)
            where T : new()
        {
            lock (sync)
            {
                SQLiteConnection db = Default;
                return Query<T>(db, query, args);
            }
        }
        /// <summary>
        /// Execute Non Query.
        /// </summary>
        /// <param name="db">The connection.</param>
        /// <param name="query">The query string.</param>
        /// <param name="args">The query arguments.</param>
        /// <returns>Returns effected row(s) count.</returns>
        public static int Execute(SQLiteConnection db, string query, params object[] args)
        {
            lock (sync)
            {
                int result = 0;
                if (null == db || string.IsNullOrEmpty(query)) return result;
                // execute query.
                result = db.Execute(query, args);
                return result;
            }
        }
        /// <summary>
        /// Execute Non Query.
        /// </summary>
        /// <param name="query">The query string.</param>
        /// <param name="args">The query arguments.</param>
        /// <returns>Returns effected row(s) count.</returns>
        public static int Execute(string query, params object[] args)
        {
            lock (sync)
            {
                SQLiteConnection db = Default;
                return Execute(db, query, args);
            }
        }

        #endregion
    }

    #endregion
}
