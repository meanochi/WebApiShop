using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text;
using DTOs;

namespace Services
{
    

    public class OrderConfirmationEmailService : IOrderConfirmationEmailService
    {
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
                // הציבי כאן Breakpoint כדי לראות מה כתוב בתוך ex.Message
                return new SendOrderConfirmationResult(false, ex.Message);
            }
        }

        private static string BuildHtmlBody(SendOrderConfirmationRequest request)
        {
            var he = CultureInfo.GetCultureInfo("he-IL");
            var sb = new StringBuilder();

            // הגדרות עיצוב כלליות המבוססות על ה-CSS של האתר
            sb.AppendLine(@"<div dir=""rtl"" style=""font-family: 'Assistant', Arial, sans-serif; background-color: #f8fafc; padding: 20px; color: #0f172a;"">");
            sb.AppendLine(@"  <div style=""max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 12px; border: 1px solid #e2e8f0; overflow: hidden; box-shadow: 0 4px 10px rgba(15, 23, 42, 0.08);"">");

            // כותרת עליונה כחולה (Primary Color)
            sb.AppendLine(@"    <div style=""background-color: #1e40af; padding: 30px; text-align: center; color: #ffffff;"">");
            sb.AppendLine($@"      <h1 style=""margin: 0; font-size: 24px;"">אישור הזמנה #{request.OrderCode}</h1>");
            sb.AppendLine(@"    </div>");

            // תוכן מרכזי
            sb.AppendLine(@"    <div style=""padding: 30px;"">");
            sb.AppendLine($@"      <p style=""font-size: 18px; font-weight: 600;"">שלום {(string.IsNullOrWhiteSpace(request.FirstName) ? "משתמש" : request.FirstName)},</p>");
            sb.AppendLine(@"      <p>תודה שרכשת אצלנו! ההזמנה שלך התקבלה בהצלחה.</p>");

            sb.AppendLine(@"      <div style=""background-color: #f1f5f9; border-radius: 8px; padding: 15px; margin: 20px 0;"">");
            sb.AppendLine($@"        <p style=""margin: 5px 0;""><strong>תאריך:</strong> {request.OrderDate.ToLocalTime().ToString("dd/MM/yyyy HH:mm", he)}</p>");
            sb.AppendLine(@"      </div>");

            sb.AppendLine(@"      <h3 style=""border-bottom: 2px solid #e2e8f0; padding-bottom: 10px; color: #1e40af;"">פרטי הכרטיסים</h3>");

            foreach (var item in request.Items)
            {
                var showDate = item.ShowDate.HasValue ? item.ShowDate.Value.ToLocalTime().ToString("dd/MM/yyyy", he) : "-";
                var showTime = string.IsNullOrWhiteSpace(item.ShowTime) ? "" : $" בשעה {item.ShowTime}";

                sb.AppendLine(@"      <div style=""border-bottom: 1px solid #f1f5f9; padding: 15px 0;"">");
                sb.AppendLine($@"        <p style=""margin: 0; font-weight: 600; font-size: 16px;"">{item.ShowTitle}</p>");
                sb.AppendLine($@"        <p style=""margin: 5px 0; color: #64748b; font-size: 14px;"">");
                sb.AppendLine($@"          {item.Section} • שורה {item.Row} • כיסא {item.Col}<br/>");
                sb.AppendLine($@"          מועד: {showDate}{showTime}");
                sb.AppendLine(@"        </p>");
                sb.AppendLine($@"        <p style=""margin: 5px 0; font-weight: 600;"">{item.Price:0.##}₪</p>");
                sb.AppendLine(@"      </div>");
            }

            // סיכום סופי
            sb.AppendLine(@"      <div style=""margin-top: 20px; text-align: left;"">");
            sb.AppendLine($@"        <p style=""font-size: 20px;""><strong>סה""כ שולם:</strong> <span style=""color: #1e40af;"">{request.TotalPaid:0.##}₪</span></p>");
            sb.AppendLine(@"      </div>");

            sb.AppendLine(@"    </div>"); // סגירת padding

            // כותרת תחתונה
            sb.AppendLine(@"    <div style=""background-color: #f8fafc; padding: 20px; text-align: center; border-top: 1px solid #e2e8f0; color: #64748b; font-size: 12px;"">");
            sb.AppendLine(@"      <p>© כל הזכויות שמורות למרכז המופעים</p>");
            sb.AppendLine(@"    </div>");

            sb.AppendLine(@"  </div>"); // סגירת container
            sb.AppendLine(@"</div>");

            return sb.ToString();
        }
    }

}
