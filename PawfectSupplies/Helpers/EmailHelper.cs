using System;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace PawfectSupplies.Helpers
{
    public class EmailHelper
    {
        public static void SendVerificationEmail(string userEmail, string token)
        {
            string verificationLink = $"https://localhost:44351/Pages/User/VerifyAccount.aspx?token={token}";
            string subject = "Verify Your Pawfect Supplies Account";

            string body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; border: 1px solid #ddd; padding: 20px; border-radius: 8px; background-color: #f9f9f9;'>
                    <div style='text-align: center;'>
                        <img src='https://i.imgur.com/NIgK6pS.png' alt='Pawfect Supplies' style='max-width: 120px; margin-bottom: 20px;' />
                    </div>
                    <h2 style='color: #333; text-align: center; font-weight: 600;'>Welcome to Pawfect Supplies!</h2>
                    <p style='color: #555; font-size: 16px; text-align: center;'>
                        Thank you for signing up! To activate your account, please verify your email by clicking the button below.
                    </p>
                    <div style='text-align: center; margin: 20px 0;'>
                        <a href='{verificationLink}' style='background-color: #17a2b8; color: white; text-decoration: none; padding: 12px 24px; border-radius: 6px; font-weight: 600; font-size: 16px; display: inline-block;'>Verify My Account</a>
                    </div>
                    <p style='color: #777; font-size: 14px; text-align: center;'>If you didn't create this account, please ignore this email.</p>
                    <hr style='border: none; border-top: 1px solid #ddd; margin: 20px 0;' />
                    <p style='color: #999; font-size: 12px; text-align: center;'>Pawfect Supplies • 123 Pet Street • Singapore</p>
                </div>";

            SendEmail(userEmail, subject, body);
        }

        public static void SendEmailOTP(string userEmail, string otpCode)
        {
            string subject = "Your One-Time Password (OTP) for Pawfect Supplies";

            string body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; border: 1px solid #ddd; padding: 20px; border-radius: 8px; background-color: #f9f9f9;'>
                    <div style='text-align: center;'>
                        <img src='https://i.imgur.com/NIgK6pS.png' alt='Pawfect Supplies' style='max-width: 120px; margin-bottom: 20px;' />
                    </div>
                    <h2 style='color: #333; text-align: center; font-weight: 600;'>Secure Login Verification</h2>
                    <p style='color: #555; font-size: 16px; text-align: center;'>
                        You requested to log in to Pawfect Supplies. Use the OTP below to complete your login.
                    </p>
                    <div style='text-align: center; font-size: 24px; font-weight: bold; color: #17a2b8; margin: 20px 0;'>
                        {otpCode}
                    </div>
                    <p style='color: #777; font-size: 14px; text-align: center;'>This OTP expires in 3 minutes.</p>
                    <p style='color: #777; font-size: 14px; text-align: center;'>If you didn’t request this, please ignore this email.</p>
                    <hr style='border: none; border-top: 1px solid #ddd; margin: 20px 0;' />
                    <p style='color: #999; font-size: 12px; text-align: center;'>Pawfect Supplies • 123 Pet Street • Singapore</p>
                </div>";

            SendEmail(userEmail, subject, body);
        }

        private static void SendEmail(string userEmail, string subject, string body)
        {
            try
            {
                string smtpHost = ConfigurationManager.AppSettings["SMTP_Host"];
                int smtpPort = int.Parse(ConfigurationManager.AppSettings["SMTP_Port"]);
                string smtpUser = ConfigurationManager.AppSettings["SMTP_User"];
                string smtpPass = ConfigurationManager.AppSettings["SMTP_Password"];
                bool enableSSL = bool.Parse(ConfigurationManager.AppSettings["SMTP_EnableSSL"]);

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(smtpUser, "Pawfect Supplies"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mail.To.Add(userEmail);

                SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = enableSSL
                };

                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email Error: " + ex.Message);
            }
        }
    }
}
