using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Klinkby.Payments.Properties;

namespace Klinkby.Payments
{
    public class PaymentRequestParameters
    {
        private static readonly Regex Md5Check = new Regex(@"^[a-z0-9]{32}$",
                                                            RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                            RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

// ReSharper disable InconsistentNaming
        private static readonly Regex _Protocol = new Regex(@"^3|4$",
                                                            RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                            RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Msgtype = new Regex(@"^(authorize|subscribe)$",
                                                           RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                           RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Merchant = new Regex(@"^[0-9]{8}$",
                                                            RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                            RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Language = new Regex(@"^[a-z]{2}$",
                                                            RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                            RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Ordernumber = new Regex(@"^[a-zA-Z0-9]{4,20}$",
                                                               RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                               RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Amount = new Regex(@"^[0-9]{1,9}$",
                                                          RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                          RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Currency = new Regex(@"^[A-Z]{3}$",
                                                            RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                            RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Continueurl = new Regex(@"^https?://",
                                                               RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                               RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Cancelurl = new Regex(@"^https?://",
                                                             RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                             RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Callbackurl = new Regex(@"^https?://",
                                                               RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                               RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Autocapture = new Regex(@"^[0-1]?$",
                                                               RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                               RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Autofee = new Regex(@"^[0-1]?$",
                                                           RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                           RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Cardtypelock = new Regex(@"^[a-zA-Z,-]{0,}$",
                                                                RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                                RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Group = new Regex(@"^[0-9]{0,9}$",
                                                         RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                         RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Splitpayment = new Regex(@"^[0-1]{0,1}$",
                                                                RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                                RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
        // ReSharper restore InconsistentNaming

        private string _amount;
        private string _autocapture = Settings.Default.AutoCapture;
        private string _autofee = Settings.Default.AutoFee;
        private string _callbackurl;
        private string _cancelurl;
        private string _cardtypelock = Settings.Default.Cardtypelock;
        private string _continueurl;
        private string _currency = Settings.Default.Currency;

        private string _description;

//        static Regex _Description = new Regex(@"^[\w _\-.]{,20}$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture| RegexOptions.IgnoreCase);
        private string _group;
        private string _language = Settings.Default.Language;
        private string _merchant;
        private string _msgtype = Settings.Default.MsgType;
        private string _ordernumber;
        private string _protocol = Settings.Default.Protocol;
        private string _splitpayment = Settings.Default.SplitPayment;

        private string _testmode = Settings.Default.TestMode;

        /// <summary>
        ///     Defines the version of the protocol
        /// </summary>
        public string Protocol
        {
            get { return _protocol; }
            set
            {
                if (!_Protocol.IsMatch(value)) throw new ArgumentException();
                _protocol = value;
            }
        }

        /// <summary>
        ///     Defines wether the transaction should be a standard payment or a subscription payment.
        /// </summary>
        public string Msgtype
        {
            get { return _msgtype; }
            set
            {
                if (!_Msgtype.IsMatch(value)) throw new ArgumentException();
                _msgtype = value;
            }
        }

        /// <summary>
        ///     The QuickPayId
        /// </summary>
        public string Merchant
        {
            get { return _merchant; }
            set
            {
                if (!_Merchant.IsMatch(value)) throw new ArgumentException();
                _merchant = value;
            }
        }

        /// <summary>
        ///     The language to use in the HTML pages as 2-letter ISO 639-1 alphabetical code. See http://quickpay.net/features/languages/ for supported languages.
        /// </summary>
        public string Language
        {
            get { return _language; }
            set
            {
                if (!_Language.IsMatch(value)) throw new ArgumentException();
                _language = value;
            }
        }

        /// <summary>
        ///     A value by merchant's own choise. Must be unique for each transaction. Usually an incrementing sequence. The value may be reflected in the your bank account list.
        /// </summary>
        [Key]
        [DisplayName("Order №")]
        public string Ordernumber
        {
            get { return _ordernumber; }
            set
            {
                if (!_Ordernumber.IsMatch(value)) throw new ArgumentException();
                _ordernumber = value;
            }
        }

        /// <summary>
        ///     The transaction amount in its smallest unit. In example, 1 EUR is written 100.
        /// </summary>
        public string Amount
        {
            get { return _amount; }
            set
            {
                if (!_Amount.IsMatch(value)) throw new ArgumentException();
                _amount = value;
            }
        }

        /// <summary>
        ///     The transaction currency as the 3-letter ISO 4217 alphabetical code. See http://quickpay.net/features/multi-currency/ for more info
        /// </summary>
        public string Currency
        {
            get { return _currency; }
            set
            {
                if (!_Currency.IsMatch(value)) throw new ArgumentException();
                _currency = value;
            }
        }

        /// <summary>
        ///     QuickPay will redirect to this URL upon a succesful transaction.
        /// </summary>
        public string Continueurl
        {
            get { return _continueurl; }
            set
            {
                if (!_Continueurl.IsMatch(value)) throw new ArgumentException();
                _continueurl = value;
            }
        }

        /// <summary>
        ///     QuickPay will redirect to this URL if transaction is cancelled.
        /// </summary>
        public string Cancelurl
        {
            get { return _cancelurl; }
            set
            {
                if (!_Cancelurl.IsMatch(value)) throw new ArgumentException();
                _cancelurl = value;
            }
        }

        /// <summary>
        ///     QuickPay will make a call back to this URL with the result of the transaction. See http://quickpay.net/faq/callbackurl/ for more information.
        /// </summary>
        public string Callbackurl
        {
            get { return _callbackurl; }
            set
            {
                if (!_Callbackurl.IsMatch(value)) throw new ArgumentException();
                _callbackurl = value;
            }
        }

        /// <summary>
        ///     If set to 1, the transaction will be captured automatically. See http://quickpay.net/features/autocapture/ for more information. Note: autocapture is only valid for message type 'authorize'
        /// </summary>
        public string Autocapture
        {
            get { return _autocapture; }
            set
            {
                if (!_Autocapture.IsMatch(value)) throw new ArgumentException();
                _autocapture = value;
            }
        }

        /// <summary>
        ///     If set to 1, the fee charged by the acquirer will be calculated and added to the transaction amount. See http://quickpay.net/features/transaction-fees/ for more information.
        /// </summary>
        public string Autofee
        {
            get { return _autofee; }
            set
            {
                if (!_Autofee.IsMatch(value)) throw new ArgumentException();
                _autofee = value;
            }
        }

        /// <summary>
        ///     Lock to card type. Multiple card types allowed by comma separation. See http://quickpay.net/features/cardtypelock/ for available values.
        /// </summary>
        public string Cardtypelock
        {
            get { return _cardtypelock; }
            set
            {
                if (!_Cardtypelock.IsMatch(value)) throw new ArgumentException();
                _cardtypelock = value;
            }
        }

        /// <summary>
        ///     A value by the merchant's own choise. Used for identifying a subscription payment. Note: Required for message type 'subscribe'.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set
            {
//                if (!_Description.IsMatch(value)) throw new ArgumentException();
                _description = value;
            }
        }

        /// <summary>
        ///     Add subscription to this subscription group - (API v4 only)
        /// </summary>
        public string Group
        {
            get { return _group; }
            set
            {
                if (!_Group.IsMatch(value)) throw new ArgumentException();
                _group = value;
            }
        }

        /// <summary>
        ///     Enables inline testing. If set to '1', QuickPay will handle this and only this transaction in test-mode, while QuickPay is in production-mode.
        /// </summary>
        public string Testmode
        {
            get { return _testmode; }
            set
            {
                //              if (!_Testmode.IsMatch(value)) throw new ArgumentException();
                _testmode = value;
            }
        }

//        static Regex _Testmode = new Regex(@"^[0-1]?$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture| RegexOptions.IgnoreCase);

        /// <summary>
        ///     Enable split payment on transaction - (API v4 only)
        /// </summary>
        public string Splitpayment
        {
            get { return _splitpayment; }
            set
            {
                if (!_Splitpayment.IsMatch(value)) throw new ArgumentException();
                _splitpayment = value;
            }
        }

        /// <summary>
        ///     Quickpay merchant secret
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        ///     A MD5 checksum to ensure data integrity. See http://quickpay.net/faq/md5check/ for more information.}
        /// </summary>
        public string Md5check
        {
            get
            {
                string md5String = string.Concat(
                    _protocol,
                    _msgtype,
                    _merchant,
                    _language,
                    _ordernumber,
                    _amount,
                    _currency,
                    _continueurl,
                    _cancelurl,
                    _callbackurl,
                    _autocapture,
                    _autofee,
                    _cardtypelock,
                    _description,
                    _group,
                    _testmode,
                    _splitpayment,
                    Secret);

                using (var md5 = new MD5CryptoServiceProvider())
                {
                    byte[] md5Hash = md5.ComputeHash(Encoding.UTF8.GetBytes(md5String));
                    string md5HashStr = ToHexString(md5Hash);
                    if (!Md5Check.IsMatch(md5HashStr)) throw new ArgumentException();
                    return md5HashStr;
                }
            }
        }

        internal static string ToHexString(byte[] buffer)
        {
            return BitConverter.ToString(buffer).Replace("-", "").ToLowerInvariant();
        }

        internal NameValueCollection ToNameValueCollection()
        {
            Type t = GetType();
            PropertyInfo[] props =
                t.GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance |
                                BindingFlags.IgnoreCase);
            var coll = new NameValueCollection();
            foreach (PropertyInfo prop in props)
            {
                var value = (string) prop.GetValue(this, null);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    coll.Add(prop.Name.ToLowerInvariant(), value.Trim());
                }
            }
            return coll;
        }
    }
}