#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

#endregion

namespace DMT.Models
{
    // Server data save(update)
    /*
    {
        "tsbId": "09",
        "userId": "00111",
        "userprefix": "นาย",
        "userfirstname": "หัสกร",
        "userlastname": "ทิพยไพศาล",
        "bagno": "1245",
        "credit": 10000,
        "flag": 0,
        "creditdate": "2021-02-20:13:03.112Z"
    }
    */
    /// <summary>The TAAccountUserCredit class.</summary>
    public class TAAccountUserCredit
    {
        /// <summary>Gets or sets TSBId.</summary>
        [PropertyMapName("TSBId")]
        public string TSBId { get; set; }

        /// <summary>Gets or sets UserId.</summary>
        [PropertyMapName("UserId")]
        public string UserId { get; set; }
        /// <summary>Gets or sets UserPrefix.</summary>
        [PropertyMapName("UserPrefix")]
        public string UserPrefix { get; set; }
        /// <summary>Gets or sets UserFirstName.</summary>
        [PropertyMapName("UserFirstName")]
        public string UserFirstName { get; set; }
        /// <summary>Gets or sets UserLastName.</summary>
        [PropertyMapName("UserLastName")]
        public string UserLastName { get; set; }

        /// <summary>Gets or sets BagNo.</summary>
        [PropertyMapName("BagNo")]
        public string BagNo { get; set; }
        /// <summary>Gets or sets Credit.</summary>
        [PropertyMapName("Credit")]
        public decimal? Credit { get; set; }
        /// <summary>Gets or sets CreditDate.</summary>
        [PropertyMapName("CreditDate")]
        public DateTime? CreditDate { get; set; }

        /// <summary>Gets or sets flag.</summary>
        [PropertyMapName("flag")]
        public int? flag { get; set; }
    }


    /// <summary>The TAAccountUserCreditResult class.</summary>
    public class TAAccountUserCreditResult
    {

    }

    static partial class Search
    {
        public static partial class TAxTOD
        {
            public static partial class Credit
            {
                public static partial class User
                {
                    #region Gets

                    /// <summary>
                    /// Gets.
                    /// </summary>
                    public class Gets : NSearch<Gets>
                    {
                        #region Public Properties

                        /// <summary>
                        /// Gets or sets TSBId.
                        /// </summary>
                        public string TSBId { get; set; }

                        #endregion

                        #region Static Method (Create)

                        /// <summary>
                        /// Create Search instance.
                        /// </summary>
                        /// <param name="tsbId">The TSB Id.</param>
                        /// <returns>Returns Search instance.</returns>
                        public static Gets Create(string tsbId)
                        {
                            var ret = new Gets();
                            ret.TSBId = tsbId;
                            return ret;
                        }

                        #endregion
                    }

                    #endregion
                }
            }
        }
    }
}
