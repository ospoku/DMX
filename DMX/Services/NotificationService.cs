namespace DMX.Services
{
    public class NotificationService
    {
        public void SendEmail(string email, string subject, string message)
        {
            // Code to send email
            Console.WriteLine($"Email sent to {email}: {subject}");
        }

        public void SendSMS(string phoneNumber, string message)
        {
            // Code to send SMS
            Console.WriteLine($"SMS sent to {phoneNumber}: {message}");
        }
    }
}

