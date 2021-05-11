#region Using

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#endregion

namespace DMT.Models
{
    public class MCurrency
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MCurrency() 
        {
            this.Active = true;
        }

        #endregion

        #region Public Properties

        public bool Active { get; set; }

        public int currencyDenomId { get; set; }
        public int currencyId { get; set; }
        public int denomTypeId { get; set; }
        public decimal denomValue { get; set; }
        public string abbreviation { get; set; }
        public string description { get; set; }

        #endregion

        #region Static Methods
        /// <summary>
        /// Gets Currencies.
        /// </summary>
        /// <returns>Returns List of MCurrency Master.</returns>
        public static List<MCurrency> GetCurrencies()
        {
            List<MCurrency> results = new List<MCurrency>();

            MCurrency item;
            item = new MCurrency()
            {
                Active = false,
                currencyDenomId = 1,
                abbreviation = "Satang25",
                description = "25 Satang",
                denomValue = (decimal)0.25,
                currencyId = 1,
                denomTypeId = 2 // coin
            };
            results.Add(item);
            item = new MCurrency()
            {
                Active = false,
                currencyDenomId = 2,
                abbreviation = "Satang50",
                description = "50 Satang",
                denomValue = (decimal)0.5,
                currencyId = 1,
                denomTypeId = 2 // coin
            };
            results.Add(item);
            item = new MCurrency()
            {
                currencyDenomId = 3,
                abbreviation = "Baht1",
                description = "1 Baht",
                denomValue = 1,
                currencyId = 1,
                denomTypeId = 2 // coin
            };
            results.Add(item);
            item = new MCurrency()
            {
                currencyDenomId = 4,
                abbreviation = "Baht2",
                description = "2 Baht",
                denomValue = 2,
                currencyId = 1,
                denomTypeId = 2 // coin
            };
            results.Add(item);
            item = new MCurrency()
            {
                currencyDenomId = 5,
                abbreviation = "Baht5",
                description = "5 Baht",
                denomValue = 5,
                currencyId = 1,
                denomTypeId = 2 // coin
            };
            results.Add(item);
            item = new MCurrency()
            {
                currencyDenomId = 6,
                abbreviation = "CBaht10",
                description = "10 Baht",
                denomValue = 10,
                currencyId = 1,
                denomTypeId = 2 // coin
            };
            results.Add(item);
            item = new MCurrency()
            {
                currencyDenomId = 7,
                abbreviation = "NBaht10",
                description = "10 Baht",
                denomValue = 10,
                currencyId = 1,
                denomTypeId = 1 // Note
            };
            results.Add(item);
            item = new MCurrency()
            {
                currencyDenomId = 8,
                abbreviation = "NBaht20",
                description = "20 Baht",
                denomValue = 20,
                currencyId = 1,
                denomTypeId = 1 // Note
            };
            results.Add(item);
            item = new MCurrency()
            {
                currencyDenomId = 9,
                abbreviation = "NBaht50",
                description = "50 Baht",
                denomValue = 50,
                currencyId = 1,
                denomTypeId = 1 // Note
            };
            results.Add(item);
            item = new MCurrency()
            {
                currencyDenomId = 10,
                abbreviation = "NBaht100",
                description = "100 Baht",
                denomValue = 100,
                currencyId = 1,
                denomTypeId = 1 // Note
            };
            results.Add(item);
            item = new MCurrency()
            {
                currencyDenomId = 11,
                abbreviation = "NBaht500",
                description = "500 Baht",
                denomValue = 500,
                currencyId = 1,
                denomTypeId = 1 // Note
            };
            results.Add(item);
            item = new MCurrency()
            {
                currencyDenomId = 12,
                abbreviation = "NBaht1000",
                description = "1000 Baht",
                denomValue = 1000,
                currencyId = 1,
                denomTypeId = 1 // Note
            };
            results.Add(item);

            return results;
        }

        #endregion
    }

    public class Detail
    {
        #region Public Properties

        public int CurrencyDenomId { get; set; }
        public int DenomTypeId { get; set; }
        public decimal DenomValue { get; set; }
        public string Description 
        { 
            get { return DenomValue.ToString("n0") + " บาท"; } 
        }

        public decimal Amount { get; set; }

        public string Unit { get { return "บาท"; } }

        #endregion

        public static List<Detail> GetDetails() 
        {
            List<Detail> results = new List<Detail>();

            MCurrency.GetCurrencies().ForEach(currency => 
            {
                if (!currency.Active) return; // if inactive ignore.
                var item = new Detail();
                item.CurrencyDenomId = currency.currencyDenomId;
                item.DenomValue = currency.denomValue;
                item.DenomTypeId = currency.denomTypeId;
                item.Amount = decimal.Zero;
                results.Add(item);
            });

            return results;
        }
    }

    public static class DetailExtensionMethods
    {
        public static List<Detail> Compact(this List<Detail> values)
        {
            List<Detail> results = new List<Detail>();
            if (null != values && values.Count > 0)
            {
                results = values.FindAll(value => 
                { 
                    return (value.Amount > decimal.Zero); 
                });
            }
            return results;

        }
    }
}
