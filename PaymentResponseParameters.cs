using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Klinkby.Payments
{
    [Serializable]
    public class PaymentResponseParameters
    {
        private static readonly Regex Md5Check = new Regex(@"^[a-z0-9]{32}$",
                                                            RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                            RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

// ReSharper disable InconsistentNaming
        private static readonly Regex _Ordernumber = new Regex(@"^[a-zA-Z0-9]{4,20}$",
                                                               RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                               RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Amount = new Regex(@"^[0-9]{1,10}$",
                                                          RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                          RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Currency = new Regex(@"^[A-Z]{3}$",
                                                            RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                            RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Time = new Regex(@"^[0-9]{12}$",
                                                        RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                        RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _State = new Regex(@"^[1-9]{1,2}$",
                                                         RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                         RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Qpstat = new Regex(@"^[0-9]{3}$",
                                                          RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                          RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Qpstatmsg = new Regex(@"^[\w .-]{1,}$",
                                                             RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                             RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Chstat = new Regex(@"^[0-9]{3}$",
                                                          RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                          RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Chstatmsg = new Regex(@"^[\w .-]{1,}$",
                                                             RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                             RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Merchant = new Regex(@"^[\w .-]{1,100}$",
                                                            RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                            RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Merchantemail = new Regex(@"^[\w_.\@-]{6,}$",
                                                                 RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                                 RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Transaction = new Regex(@"^[0-9]{1,32}$",
                                                               RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                               RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Cardtype = new Regex(@"^[\w-]{1,32}$",
                                                            RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                            RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Regex _Cardexpire = new Regex(@"^[\w\s]{,4}$",
                                                              RegexOptions.Compiled | RegexOptions.CultureInvariant |
                                                              RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
        // ReSharper restore InconsistentNaming

        private string _amount;
        private string _cardexpire;

        private string _cardtype;
        private string _chstat;
        private string _chstatmsg;
        private string _currency;
        private string _md5check;
        private string _merchant;
        private string _merchantemail;
        private string _ordernumber;
        private string _qpstat;
        private string _qpstatmsg;
        private string _state;
        private string _time;
        private string _transaction;
        public string MsgType { get; set; }

        /// <summary>A value specified by merchant in the initial request.</summary>
        public string Ordernumber
        {
            get { return _ordernumber; }
            set
            {
                if (!_Ordernumber.IsMatch(value)) throw new ArgumentException();
                _ordernumber = value;
            }
        }

        /// <summary>The amount defined in the request in its smallest unit. In example, 1 EUR is written 100.</summary>
        public string Amount
        {
            get { return _amount; }
            set
            {
                if (!_Amount.IsMatch(value)) throw new ArgumentException();
                _amount = value;
            }
        }

        /// <summary>The transaction currency as the 3-letter ISO 4217 alphabetical code.</summary>
        public string Currency
        {
            get { return _currency; }
            set
            {
                if (!_Currency.IsMatch(value)) throw new ArgumentException();
                _currency = value;
            }
        }

        /// <summary>The time of which the message was handled. Format is YYMMDDHHIISS.</summary>
        public string Time
        {
            get { return _time; }
            set
            {
                if (!_Time.IsMatch(value)) throw new ArgumentException();
                _time = value;
            }
        }

        /// <summary>The current state of the transaction. See http://quickpay.net/faq/transaction-states/</summary>
        public string State
        {
            get { return _state; }
            set
            {
                if (!_State.IsMatch(value)) throw new ArgumentException();
                _state = value;
            }
        }

        /// <summary>return _code from QuickPay. See http://quickpay.net/faq/status-codes/</summary>
        public string Qpstat
        {
            get { return _qpstat; }
            set
            {
                if (!_Qpstat.IsMatch(value)) throw new ArgumentException();
                _qpstat = value;
            }
        }

        /// <summary>A message detailing errors and warnings if any.</summary>
        public string Qpstatmsg
        {
            get { return _qpstatmsg; }
            set
            {
                if (!_Qpstatmsg.IsMatch(value)) throw new ArgumentException();
                _qpstatmsg = value;
            }
        }

        /// <summary>return _code from the clearing house. Please refer to the clearing house documentation.</summary>
        public string Chstat
        {
            get { return _chstat; }
            set
            {
                if (!_Chstat.IsMatch(value)) throw new ArgumentException();
                _chstat = value;
            }
        }

        /// <summary>A message from the clearing house detailing errors and warnings if any.</summary>
        public string Chstatmsg
        {
            get { return _chstatmsg; }
            set
            {
                if (!_Chstatmsg.IsMatch(value)) throw new ArgumentException();
                _chstatmsg = value;
            }
        }

        /// <summary>The QuickPay merchant name</summary>
        public string Merchant
        {
            get { return _merchant; }
            set
            {
                if (!_Merchant.IsMatch(value)) throw new ArgumentException();
                _merchant = value;
            }
        }

        /// <summary>The QuickPay merchant email/username</summary>
        public string Merchantemail
        {
            get { return _merchantemail; }
            set
            {
                if (!_Merchantemail.IsMatch(value)) throw new ArgumentException();
                _merchantemail = value;
            }
        }

        /// <summary>The id assigned to the current transaction.</summary>
        public string Transaction
        {
            get { return _transaction; }
            set
            {
                if (!_Transaction.IsMatch(value)) throw new ArgumentException();
                _transaction = value;
            }
        }

        /// <summary>The card type used to authorize the transaction.</summary>
        public string Cardtype
        {
            get { return _cardtype; }
            set
            {
                if (!_Cardtype.IsMatch(value)) throw new ArgumentException();
                _cardtype = value;
            }
        }

        //string _cardnumber;
        ///// <summary>A truncated version of the card number - eg. 'XXXX XXXX XXXX 1234'. Note: This field will be empty for other message types than 'authorize' and 'subscribe'.</summary>
        //public string Cardnumber { get { return _cardnumber; } set { if (!_Cardnumber	 .IsMatch(value)) throw new ArgumentException(); _cardnumber	 = value;}}
        //static Regex _Cardnumber = new Regex(@"^[\w\s]{,32}$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture| RegexOptions.IgnoreCase);
        public string CardNumber { get; set; }

        /// <summary>Expire date on the card used in a 'subscribe'. Notation is 'yymm'. Note: This field will be empty for other message types than 'subscribe'.</summary>
        public string Cardexpire
        {
            get { return _cardexpire; }
            set
            {
                if (!_Cardexpire.IsMatch(value)) throw new ArgumentException();
                _cardexpire = value;
            }
        }

        //string _fee;
        ///// <summary>Will contain the calculated fee, if autofee was activated in request. See http://quickpay.net/features/transaction-fees/ for more information.</summary>
        //public string Fee { get { return _fee; } set { if (!_Fee	 .IsMatch(value)) throw new ArgumentException(); _fee	 = value;}}
        //static Regex _Fee = new Regex(@"^[0-9]{,10}$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture| RegexOptions.IgnoreCase);
        public string Fee { get; set; }

        public string Md5check
        {
            get { return _md5check; }
            set { _md5check = value; }
        }

        /// <summary>
        ///     Quickpay merchant secret
        /// </summary>
        public string Secret { get; set; }

        public string SplitPayment { get; set; }

        public string FraudProbability { get; set; }
        public string FraudRemarks { get; set; }
        public string FraudReport { get; set; }

        /// <summary>A MD5 checksum to ensure data integrity. See http://quickpay.net/faq/md5check/ for more information.</summary>
        public string CalculateMD5()
        {
            string md5String =
                MsgType + _ordernumber + _amount + _currency + _time + _state + _qpstat + _qpstatmsg + _chstat +
                _chstatmsg
                + _merchant + _merchantemail + _transaction + _cardtype + CardNumber + _cardexpire +
                SplitPayment + FraudProbability + FraudRemarks + FraudReport + Fee + Secret;

            using (var md5 = new MD5CryptoServiceProvider())
            {
                byte[] md5Hash = md5.ComputeHash(Encoding.UTF8.GetBytes(md5String));
                string md5HashStr = PaymentRequestParameters.ToHexString(md5Hash);
                if (!Md5Check.IsMatch(md5HashStr)) throw new ArgumentException();
                return md5HashStr;
            }
        }

        internal static PaymentResponseParameters FromNameValueCollection(NameValueCollection coll)
        {
            var res = new PaymentResponseParameters();
            Type t = res.GetType();
            foreach (string name in coll.AllKeys)
            {
                PropertyInfo prop = t.GetProperty(name,
                                                  BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance |
                                                  BindingFlags.IgnoreCase);
                if (prop != null)
                {
                    prop.SetValue(res, coll.Get(name), null);
                }
            }
            return res;
        }
    }
}