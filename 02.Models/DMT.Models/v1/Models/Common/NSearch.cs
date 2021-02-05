#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace DMT.Models
{
    /// <summary>
    /// The NSearch class.
    /// </summary>
    public abstract class NSearch
    {
        protected static object sync = new object();
    }
    /// <summary>
    /// The NSearch (Generic) class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class NSearch<T> : NSearch
        where T: NSearch, new()
    {
    }
}
