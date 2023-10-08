using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;

namespace Klinkby.Payments;

public class QuickPay : IPaymentGateway
{
    private const string PaymentWindowLink = "https://secure.quickpay.dk/form/";
    private readonly PaymentOptions _options;

    public QuickPay(PaymentOptions options = default)
    {
        _options = options ?? new PaymentOptions();
    }

    public void Authorize(string merchant, string invoiceNo, double amount, Uri callbackUrl, Uri cancelUrl,
        Uri continueUrl)
    {
        Authorize(
            new PaymentRequestParameters
            {
                Amount = ((int)Math.Round(amount * 100)).ToString(CultureInfo.InvariantCulture),
                //Autocapture
                //Autofee
                Callbackurl = callbackUrl.ToString(),
                Cancelurl = cancelUrl.ToString(),
                Continueurl = continueUrl.ToString(),
                Cardtypelock = _options.Cardtypelock,
                Currency = _options.Currency,
                //Description
                //Group                    
                Language = _options.Language,
                Merchant = merchant,
                Msgtype = "authorize",
                Ordernumber = invoiceNo,
                Protocol = _options.Protocol,
                Splitpayment = "0",
                Testmode = _options.TestMode
            });
    }

    private void Authorize(PaymentRequestParameters request)
    {
        var data = request.ToNameValueCollection();
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
        var response = PaymentResponseParameters.FromNameValueCollection(data);
        Callback(response);
    }

    private void Callback(PaymentResponseParameters response)
    {
    }
}