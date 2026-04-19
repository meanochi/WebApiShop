using System.Globalization;
using System.Net;
using System.Text;
using DTOs;

namespace Services
{
    public class OrderConfirmationEmailService : IOrderConfirmationEmailService
    {
        private const string BrandDisplayName = "Shows Center";
        private const string BrandLogoUrl = "https://localhost:44304/logo.png";
        private const string SupportEmail = "info@showscenter.com";
        private const string SupportPhone = "03-1234567";
        private const string SupportPhoneHref = "031234567";
        private const string SupportAddress = "רחוב הבידור 123, תל אביב";

        private readonly IEmailSender _emailSender;

        public OrderConfirmationEmailService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task<SendOrderConfirmationResult> SendAsync(
            SendOrderConfirmationRequest request,
            CancellationToken ct = default)
        {
            var email = request.Email?.Trim();
            if (string.IsNullOrWhiteSpace(email))
                return new SendOrderConfirmationResult(false, "Email is required.");

            if (request.Items == null || request.Items.Count == 0)
                return new SendOrderConfirmationResult(false, "Order items are required.");

            var subject = $"אישור הזמנה - {request.OrderCode}";
            var body = BuildHtmlBody(request);

            try
            {
                await _emailSender.SendAsync(email, subject, body, ct);
                return new SendOrderConfirmationResult(true);
            }
            catch (Exception ex)
            {
                return new SendOrderConfirmationResult(false, ex.Message);
            }
        }

        private static string BuildHtmlBody(SendOrderConfirmationRequest request)
        {
            var he = CultureInfo.GetCultureInfo("he-IL");
            var firstNameText = string.IsNullOrWhiteSpace(request.FirstName)
                ? "לקוח/ה יקר/ה"
                : request.FirstName.Trim();
            var orderCodeText = string.IsNullOrWhiteSpace(request.OrderCode)
                ? "ללא קוד"
                : request.OrderCode.Trim();

            var firstName = HtmlEncode(firstNameText);
            var orderCode = HtmlEncode(orderCodeText);
            var orderDate = request.OrderDate.ToLocalTime().ToString("dd/MM/yyyy HH:mm", he);
            var totalPaid = FormatCurrency(request.TotalPaid, he);
            var ticketCount = request.Items.Count.ToString("N0", he);
            var preheader = HtmlEncode($"הזמנה {orderCodeText} התקבלה בהצלחה. כל הפרטים מחכים לך כאן.");
            var logoUrl = NormalizeImageUrl(BrandLogoUrl);

            var sb = new StringBuilder();

            sb.Append($$"""
<!DOCTYPE html>
<html lang="he" dir="rtl">
<head>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <meta name="x-apple-disable-message-reformatting" />
  <title>אישור הזמנה</title>
</head>
<body style="margin:0; padding:0; background-color:#f3f7ff;" dir="rtl>
  <div style="display:none; max-height:0; overflow:hidden; opacity:0; mso-hide:all; font-size:1px; line-height:1px; color:#f3f7ff;">
    {{preheader}}
  </div>
  <table role="presentation" cellpadding="0" cellspacing="0" width="100%" style="width:100%; margin:0; padding:18px 10px; background-color:#f3f7ff;">
    <tr>
      <td align="center">
        <table role="presentation" cellpadding="0" cellspacing="0" width="100%" style="max-width:640px; width:100%;">
          <tr>
            <td style="background-color:#ffffff; border:1px solid #d9e6fb; border-radius:22px; padding:20px 20px 18px 20px;">
              <table role="presentation" cellpadding="0" cellspacing="0" width="100%" style="width:100%;">
                <tr>
                  <td style="vertical-align:middle;">
                    <table role="presentation" cellpadding="0" cellspacing="0">
                      <tr>
                        <td style="vertical-align:middle; padding-left:12px;">
""");

            if (!string.IsNullOrWhiteSpace(logoUrl))
            {
                sb.Append($$"""
                          <img src="{{HtmlEncode(logoUrl)}}" alt="{{HtmlEncode(BrandDisplayName)}}" width="50" height="50" style="display:block; width:50px; height:50px; border-radius:14px;" />
""");
            }
            else
            {
                sb.Append($$"""
                          <div style="width:50px; height:50px; border-radius:14px; background-color:#1e90ff; font-family:'Segoe UI', Arial, sans-serif; font-size:18px; line-height:50px; font-weight:700; text-align:center; color:#ffffff;">
                            SC
                          </div>
""");
            }

            sb.Append($$"""
                        </td>
                        <td style="vertical-align:middle; font-family:'Segoe UI', Arial, sans-serif;">
                          <div style="font-size:18px; line-height:24px; font-weight:700; color:#0f172a;">{{HtmlEncode(BrandDisplayName)}}</div>
                          <div style="font-size:13px; line-height:20px; color:#64748b;">אישור הזמנה</div>
                        </td>
                      </tr>
                    </table>
                  </td>
                  <td align="left" style="vertical-align:middle;">
                    <span style="display:inline-block; padding:8px 14px; border-radius:999px; background-color:#e8f2ff; font-family:'Segoe UI', Arial, sans-serif; font-size:12px; line-height:18px; font-weight:700; color:#1454c4;">
                      ההזמנה אושרה
                    </span>
                  </td>
                </tr>
              </table>

              <table role="presentation" cellpadding="0" cellspacing="0" width="100%" style="width:100%; margin-top:18px;">
                <tr>
                  <td style="font-family:'Segoe UI', Arial, sans-serif;">
                    <div style="font-size:24px; line-height:32px; font-weight:700; color:#0f172a;">שלום {{firstName}},</div>
                    <div style="margin-top:6px; font-size:15px; line-height:24px; color:#475569;">
                      תודה על הרכישה. ריכזנו עבורך את פרטי ההזמנה במבנה קליל ונוח לצפייה.
                    </div>
                  </td>
                </tr>
              </table>

              <table role="presentation" cellpadding="0" cellspacing="0" width="100%" style="width:100%; margin-top:18px;">
                <tr>
                  <td width="50%" style="width:50%; padding:0 0 10px 5px; vertical-align:top;">
""");

            AppendSummaryCard(sb, "קוד הזמנה", WrapLtr($"#{orderCode}"));

            sb.Append($$"""
                  </td>
                  <td width="50%" style="width:50%; padding:0 0 10px 5px; vertical-align:top;">
""");

            AppendSummaryCard(sb, "מספר כרטיסים", WrapLtr(ticketCount));

            sb.Append($$"""
                  </td>
                </tr>
                <tr>
                  <td width="50%" style="width:50%; padding:0 0 0 5px; vertical-align:top;">
""");

            AppendSummaryCard(sb, "תאריך", WrapLtr(orderDate));

            sb.Append($$"""
                  </td>
                  <td width="50%" style="width:50%; padding:0 0 0 5px; vertical-align:top;">
""");

            AppendSummaryCard(sb, "סה\"כ", WrapLtr(totalPaid));

            sb.Append($$"""
                  </td>
                </tr>
              </table>

              <div style="margin-top:18px; font-family:'Segoe UI', Arial, sans-serif; font-size:18px; line-height:26px; font-weight:700; color:#0f172a;">
                המופעים שלך
              </div>

              <table role="presentation" cellpadding="0" cellspacing="0" width="100%" style="width:100%; margin-top:12px;">
""");

            foreach (var item in request.Items)
            {
                AppendTicketCard(sb, item, he);
            }

            sb.Append($$"""
              </table>

              <table role="presentation" cellpadding="0" cellspacing="0" width="100%" style="width:100%; margin-top:18px; background-color:#0f172a; border-radius:18px;">
                <tr>
                  <td style="padding:16px 18px;">
                    <table role="presentation" cellpadding="0" cellspacing="0" width="100%" style="width:100%;">
                      <tr>
                        <td style="font-family:'Segoe UI', Arial, sans-serif; font-size:13px; line-height:20px; color:#cbd5e1;">
                          סה"כ לתשלום
                        </td>
                        <td align="left" style="font-family:'Segoe UI', Arial, sans-serif; font-size:24px; line-height:30px; font-weight:700; color:#ffffff;">
                          {{WrapLtr(totalPaid)}}
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>

              <table role="presentation" cellpadding="0" cellspacing="0" width="100%" style="width:100%; margin-top:18px; background-color:#f8fbff; border:1px solid #dce8fa; border-radius:18px;">
                <tr>
                  <td style="padding:16px 18px 8px 18px; font-family:'Segoe UI', Arial, sans-serif; font-size:16px; line-height:24px; font-weight:700; color:#0f172a;">
                    צריכים עזרה?
                  </td>
                </tr>
                <tr>
                  <td style="padding:0 18px 18px 18px;">
                    <table role="presentation" cellpadding="0" cellspacing="0" width="100%" style="width:100%;">
""");

            AppendContactCell(
                sb,
                "אימייל",
                $@"<a href=""mailto:{HtmlEncode(SupportEmail)}"" style=""color:#1454c4; text-decoration:none;"">{HtmlEncode(SupportEmail)}</a>");
            AppendContactCell(
                sb,
                "טלפון",
                $@"<a href=""tel:{HtmlEncode(SupportPhoneHref)}"" style=""color:#1454c4; text-decoration:none;"">{WrapLtr(HtmlEncode(SupportPhone))}</a>");
            AppendContactCell(sb, "כתובת", HtmlEncode(SupportAddress));

            sb.Append($$"""
                    </table>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <tr>
            <td style="padding:12px 6px 0 6px; text-align:center; font-family:'Segoe UI', Arial, sans-serif; font-size:12px; line-height:20px; color:#7c8aa5;">
              הודעה זו נשלחה אוטומטית בעקבות רכישה שבוצעה באתר.
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</body>
</html>
""");

            return sb.ToString();
        }

        private static void AppendSummaryCard(StringBuilder sb, string label, string valueHtml)
        {
            sb.Append($$"""
                    <table role="presentation" cellpadding="0" cellspacing="0" width="100%" style="width:100%; background-color:#f8fbff; border:1px solid #dce8fa; border-radius:16px;">
                      <tr>
                        <td style="padding:12px 14px; font-family:'Segoe UI', Arial, sans-serif;">
                          <div style="font-size:12px; line-height:18px; color:#64748b;">{{HtmlEncode(label)}}</div>
                          <div style="margin-top:3px; font-size:16px; line-height:22px; font-weight:700; color:#0f172a;">{{valueHtml}}</div>
                        </td>
                      </tr>
                    </table>
""");
        }

        private static void AppendTicketCard(StringBuilder sb, SendOrderConfirmationItemDto item, CultureInfo he)
        {
            var title = string.IsNullOrWhiteSpace(item.ShowTitle)
                ? "מופע"
                : item.ShowTitle.Trim();
            var section = string.IsNullOrWhiteSpace(item.Section)
                ? "אזור לא צוין"
                : item.Section.Trim();
            var imageUrl = NormalizeImageUrl(item.ShowImageUrl);
            var showDate = item.ShowDate.HasValue
                ? HtmlEncode(item.ShowDate.Value.ToLocalTime().ToString("dddd, dd MMMM yyyy", he))
                : "תאריך יעודכן בהמשך";
            var showTime = string.IsNullOrWhiteSpace(item.ShowTime)
                ? string.Empty
                : $" | שעה {WrapLtr(HtmlEncode(item.ShowTime.Trim()))}";
            var seatText = $"אזור {HtmlEncode(section)} | שורה {item.Row} | כיסא {item.Col}";
            var price = WrapLtr(FormatCurrency(item.Price, he));

            sb.Append($$"""
                <tr>
                  <td style="padding:0 0 12px 0;">
                    <table role="presentation" cellpadding="0" cellspacing="0" width="100%" style="width:100%; background-color:#ffffff; border:1px solid #dce8fa; border-radius:18px;">
                      <tr>
                        <td style="padding:14px;">
                          <table role="presentation" cellpadding="0" cellspacing="0" width="100%" style="width:100%;">
                            <tr>
                              <td width="92" style="width:92px; vertical-align:top; padding-left:12px;">
""");

            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                sb.Append($$"""
                                <img src="{{HtmlEncode(imageUrl)}}" alt="{{HtmlEncode(title)}}" width="92" height="92" style="display:block; width:92px; height:92px; object-fit:cover; border-radius:14px; background-color:#e2e8f0;" />
""");
            }
            else
            {
                sb.Append($$"""
                                <div style="width:92px; height:92px; border-radius:14px; background-color:#e8f2ff; font-family:'Segoe UI', Arial, sans-serif; font-size:13px; line-height:18px; font-weight:700; text-align:center; color:#1454c4;">
                                  <div style="padding-top:28px;">אין<br />תמונה</div>
                                </div>
""");
            }

            sb.Append($$"""
                              </td>
                              <td style="vertical-align:top; font-family:'Segoe UI', Arial, sans-serif;">
                                <div style="font-size:18px; line-height:24px; font-weight:700; color:#0f172a;">{{HtmlEncode(title)}}</div>
                                <div style="margin-top:6px; font-size:13px; line-height:20px; color:#475569;">{{showDate}}{{showTime}}</div>
                                <div style="margin-top:4px; font-size:13px; line-height:20px; color:#475569;">{{seatText}}</div>
                                <div style="margin-top:10px;">
                                  <span style="display:inline-block; padding:7px 12px; border-radius:999px; background-color:#0f172a; font-size:13px; line-height:18px; font-weight:700; color:#ffffff;">
                                    {{price}}
                                  </span>
                                </div>
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
""");
        }

        private static void AppendContactCell(StringBuilder sb, string label, string valueHtml)
        {
            sb.Append($$"""
                      <tr>
                        <td style="padding:0 0 10px 0; font-family:'Segoe UI', Arial, sans-serif;">
                          <div style="font-size:12px; line-height:18px; color:#64748b;">{{HtmlEncode(label)}}</div>
                          <div style="margin-top:3px; font-size:14px; line-height:22px; color:#0f172a;">{{valueHtml}}</div>
                        </td>
                      </tr>
""");
        }

        private static string NormalizeImageUrl(string? value)
        {
            var trimmed = value?.Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
                return string.Empty;

            if (!Uri.TryCreate(trimmed, UriKind.Absolute, out var uri))
                return string.Empty;

            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
                return string.Empty;

            return uri.ToString();
        }

        private static string FormatCurrency(decimal amount, CultureInfo culture)
        {
            return $"{amount.ToString("N2", culture)} ₪";
        }

        private static string HtmlEncode(string value)
        {
            return WebUtility.HtmlEncode(value);
        }

        private static string WrapLtr(string value)
        {
            return $@"<span dir=""ltr"" style=""display:inline-block;"">{value}</span>";
        }
    }
}
