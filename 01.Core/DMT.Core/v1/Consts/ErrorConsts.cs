#region Using

using System;
using System.Collections.Generic;

#endregion

namespace DMT
{
    // TODO: Required to add more error code.
    public enum ErrNums : int
    {
        Success = 0,
        // Local Database Connection (100-149)
        DbConenctFailed = 100,
        // Web Service Connection (150-199)
        RestConenctFailed = 150,
        RestResponseError = 151,
        RestInvalidConfig = 152,
        // Models - Common (200-210)
        ParameterIsNull = 200,
        // Models - User (invalid password, invalid role, etc.) (211-230)

        // Models - TSB (Active TSB not found, etc.) (231-240)

        // Common Exception
        Exception = 900,
        // Unknown (999)
        UnknownError = 999
    }

    public class ErrConsts
    {
        private static Dictionary<ErrNums, string> _msgs;

        // TODO: Required to add more error message.
        static ErrConsts()
        {
            _msgs = new Dictionary<ErrNums, string>();
            _msgs.Add(ErrNums.Success, "Success.");
            // Local Database
            _msgs.Add(ErrNums.DbConenctFailed, "Database connection failed.");

            // Web Service Connection
            _msgs.Add(ErrNums.RestConenctFailed, "Web Service connection failed.");
            _msgs.Add(ErrNums.RestResponseError, "Web Service response error.");

            // Models - common
            _msgs.Add(ErrNums.ParameterIsNull, "Parameter is null.");

            // Common Exception
            _msgs.Add(ErrNums.Exception, "Exception detected.");

            // Unknown
            _msgs.Add(ErrNums.UnknownError, "Unknown error.");
        }

        public static string ErrMsg(ErrNums value)
        {
            if (_msgs.ContainsKey(value))
                return _msgs[value];
            else return _msgs[ErrNums.UnknownError];
        }
    }
}
