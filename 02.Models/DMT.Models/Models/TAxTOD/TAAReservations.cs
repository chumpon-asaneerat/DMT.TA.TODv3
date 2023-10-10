#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

using Newtonsoft.Json;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Diagnostics.Eventing.Reader;

#endregion

namespace DMT.Models
{
    /*
    {
      "MATERIAL_NUM": "310000030",
      "Description": "คูปอง 30 บาท",
      "UOM": "BOK",
      "Price": 600,
      "CouponType": "30"
    }    
    */
    public class CouponMaster
    {
        public string MATERIAL_NUM { get; set; }
        public string Description { get; set; }
        public string UOM { get; set; }
        public decimal Price { get; set; }
        public string CouponType { get; set; }
    }

    public class Storagelocation
    {
        public string Storage_location { get; set; }
    }

    public class ReserveRunningNo
    {
        public string PrefixTxt { get; set; }
        public int RunningNo { get; set; }
    }

    /*
    {
        "basedate": "20230826",
        "movementtype": "311",
        "goodsrecipient": "TADD20230001",
        "receivingstor": "CCDD",
        "userid": "20001",
        "postingdate": null,
        "mat_slip": null,
        "headertext": null,
        "items": [
            { 
                "itemnumber": "1", 
                "goodsrecipient": "TADD20230001", 
                "materialnum": "310000080",
                "quantity": 100,
                "unit": "BOK",
                "plant": "1010",
                "fromstor": "CDMT"
            },
            { 
                "itemnumber": "2", 
                "goodsrecipient": "TADD20230001", 
                "materialnum": "310000035",
                "quantity": 50,
                "unit": "BOK",
                "plant": "1010",
                "fromstor": "CDMT"
            }
        ]
    }
    */

    public class ReserveRequest
    {
        public string basedate { get; set; }
        public string movementtype { get; set; } = "311"; // Fixed
        public string goodsrecipient { get; set; }
        public string receivingstor { get; set; }
        public string userid { get; set; }
        public string postingdate { get; set; }
        public string mat_slip { get; set; }
        public string headertext { get; set; }

        public List<ReserveRequestItem> items { get; set; } = new List<ReserveRequestItem>();
    }

    public class ReserveRequestItem
    {
        public string itemnumber { get; set; }
        public string goodsrecipient { get; set; }
        public string materialnum { get; set;}
        public string description { get; set; } // for UI binding only
        public int quantity { get; set; }
        public string unit { get; set; } = "BOK"; // Fixed
        public string plant { get; set; } = "1010"; // Fixed
        public string fromstor { get; set; } = "CDMT"; // Fixed
    }

    public class ReservationRequestStatus
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public static List<ReservationRequestStatus> Gets()
        {
            var results = new List<ReservationRequestStatus>();

            results.Add(new ReservationRequestStatus() { Code = "A", Description = "ไม่ระบุ" });
            results.Add(new ReservationRequestStatus() { Code = null, Description = "รอส่ง" });
            results.Add(new ReservationRequestStatus() { Code = "S", Description = "สำเร็จ" });
            results.Add(new ReservationRequestStatus() { Code = "E", Description = "ไม่สำเร็จ" });

            return results;
        }
    }

    public class ReservationTransferStatus
    {
        public string Description { get; set; }
        public string Code { get; set; }

        public static List<ReservationTransferStatus> Gets()
        {
            var results = new List<ReservationTransferStatus>();

            results.Add(new ReservationTransferStatus() { Code = null, Description = "ไม่ระบุ" });
            results.Add(new ReservationTransferStatus() { Code = "N", Description = "รอใบเบิก" });
            results.Add(new ReservationTransferStatus() { Code = "Y", Description = "รับโอน" });

            return results;
        }
    }

    /*
    {
        "BASE_DATE": "20231010",
        "MOVEMENT_TYPE": "311",
        "GOODS_RECIPIENT": "TAAN20230002",
        "RECEIVING_STOR": "CCAN",
        "CREATE_BY": "00444",
        "CREATE_DATE": "2023-10-10T11:20:39.997Z",
        "POSTING_DATE": null,
        "MAT_SLIP": null,
        "HEADER_TXT": null,
        "TRANSFER_DATE": null,
        "SENDSAP": "0",
        "SENDRESULT": null,
        "RESULTMSG": null
    }
    */
    public class ReserveDocument
    {
        public string BASE_DATE { get; set; }

        public string BaseDateText
        {
            get { return BASE_DATE; }
            set { }
        }

        public string MOVEMENT_TYPE { get; set; }

        public string GOODS_RECIPIENT { get; set; }
        public string RECEIVING_STOR { get; set; }

        public string CREATE_BY { get; set; }

        private string _userName = null;
        public string CreateUserName
        {
            get 
            { 
                if (null ==  _userName)
                {
                    if (string.IsNullOrWhiteSpace(CREATE_BY))
                    {
                        _userName = string.Empty;
                    }
                    else
                    {
                        var user = User.GetByUserId(CREATE_BY.Trim()).Value();
                        _userName = (null != user) ? user.FullNameTH : CREATE_BY.Trim();
                    }
                }
                return _userName; 
            }
            set { }
        }

        public DateTime? CREATE_DATE { get; set; }
        public string CreateDateText
        {
            get 
            {
                if (CREATE_DATE.HasValue)
                {
                    return CREATE_DATE.Value.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                }
                else
                {
                    return string.Empty;
                }
            }
            set { }
        }

        public string POSTING_DATE { get; set; }

        private string _PostingDateText = null;
        public string PostingDateText
        {
            get
            {
                if (_PostingDateText == null)
                {
                    if (string.IsNullOrWhiteSpace(POSTING_DATE))
                    {
                        _PostingDateText = string.Empty;
                    }
                    else
                    {
                        try
                        {
                            DateTime dt = DateTime.ParseExact(POSTING_DATE.Trim(), "yyyyMMdd", CultureInfo.InvariantCulture);
                            _PostingDateText = dt.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                        }
                        catch 
                        {
                            _PostingDateText = string.Empty;
                        }
                    }                        
            }
                else
                {
                    _PostingDateText = string.Empty;
                }
                return _PostingDateText;
            }
            set { }
        }

        public string MAT_SLIP { get; set; }
        public string HEADER_TXT { get; set; }

        public DateTime? TRANSFER_DATE { get; set; }
        public string TransferDateText
        {
            get
            {
                if (TRANSFER_DATE.HasValue)
                {
                    return TRANSFER_DATE.Value.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                }
                else
                {
                    return string.Empty;
                }
            }
            set { }
        }

        public string SENDSAP { get; set; }
        public string SENDRESULT { get; set; }
        public string RESULTMSG { get; set; }

        public string ReserveStatus 
        { 
            get
            {
                string ret;
                if (!string.IsNullOrWhiteSpace(SENDRESULT))
                {
                    var r = SENDRESULT.Trim();
                    if (r == "S")
                        ret = "สำเร็จ";
                    else
                        ret = "ไม่สำเร็จ";
                }
                else ret = "รอส่ง";
                return ret;
            }
            set { }
        }
        public string ReserveStatusText 
        { 
            get
            {
                string ret;
                if (!string.IsNullOrWhiteSpace(RESULTMSG))
                {
                    ret = RESULTMSG.Trim();
                }
                else
                {
                    ret = string.Empty;
                }
                return ret;
            }
            set { }
        }

        public string TransferStatus 
        {
            get 
            { 
                string ret;
                if (!string.IsNullOrWhiteSpace(MAT_SLIP))
                {
                    ret = "รับโอน";
                }
                else
                {
                    ret = "รอใบเบิก";
                }
                return ret;
            }
            set { }
        }
    }

    /*
    {
      "GOODS_RECIPIENT": "TAAN20230002",
      "ITEM_NUMBER": "1",
      "MATERIAL_NUM": "310000030",
      "MATERIAL_DESCRIPTION": "คูปอง 30 บ",
      "QUANTITY": 10,
      "UNIT_OF_MEASURE": "BOK",
      "PLANT": "1010",
      "FROM_STOR": "CDMT",
      "MAT_SLIP": null,
      "TRANSFER_QTY": null
    }
    */
    public class ReserveDocumentItem
    {
        public string GOODS_RECIPIENT { get; set; }
        public string ITEM_NUMBER { get; set; }
        public string MATERIAL_NUM { get; set; }
        public string MATERIAL_DESCRIPTION { get; set; }
        public int? QUANTITY { get; set; }
        public string UNIT_OF_MEASURE { get; set; }
        public string PLANT { get; set; }
        public string FROM_STOR { get; set; }
        public string MAT_SLIP { get; set; }
        public int? TRANSFER_QTY { get; set; }
    }
}
