using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using Klinkby.Payments.Properties;

namespace Klinkby.Payments
{
    public class QuickPay : IPaymentGateway
    {
        private const string PaymentWindowLink = "https://secure.quickpay.dk/form/";

        public void Authorize(string merchant, string invoiceNo, double amount, Uri callbackUrl, Uri cancelUrl,
                              Uri continueUrl)
        {
            Authorize(
                new PaymentRequestParameters
                    {
                        Amount = ((int) Math.Round((amount*100))).ToString(CultureInfo.InvariantCulture),
                        //Autocapture
                        //Autofee
                        Callbackurl = callbackUrl.ToString(),
                        Cancelurl = cancelUrl.ToString(),
                        Continueurl = continueUrl.ToString(),
                        Cardtypelock = Settings.Default.Cardtypelock,
                        Currency = Settings.Default.Currency,
                        //Description
                        //Group                    
                        Language = Settings.Default.Language,
                        Merchant = merchant,
                        Msgtype = "authorize",
                        Ordernumber = invoiceNo,
                        Protocol = Settings.Default.Protocol,
                        Splitpayment = "0",
                        Testmode = Settings.Default.TestMode
                    });
        }

        private void Authorize(PaymentRequestParameters request)
        {
            NameValueCollection data = request.ToNameValueCollection();
            Authorize(data);
        }

        private void Authorize(NameValueCollection data)
        {
            using (var wc = new WebClient())
            {
                wc.UploadValues(PaymentWindowLink, "POST", data);
            }
        }

        private void Callback(NameValueCollection data)
        {
            PaymentResponseParameters response = PaymentResponseParameters.FromNameValueCollection(data);
            Callback(response);
        }

        private void Callback(PaymentResponseParameters response)
        {
        }
    }
}