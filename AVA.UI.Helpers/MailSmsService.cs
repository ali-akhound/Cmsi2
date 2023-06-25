using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Net.Mail;
using System.Configuration;
using System.Threading.Tasks;
using AVA.Core.Entities;
using System.Data.Entity;
using System.Linq;

/// <summary>
/// Summary description for SendEmail
/// </summary>
/// 
namespace AVA.UI.Helpers.MailSmsService
{
    public partial class EmailService
    {
        public enum EmailEnum
        {
            Success = 1,
            NotExistsUserID = 0,
            SmtpProblem = -1
        }
        public class EmailResponse
        {
            public int Code;
            public string Text;
        }
        static public EmailResponse SendWelcome(string userId, string ActivationLink)
        {
            var user = new ApplicationDbContext().Users
                .Include(item => item.Person)
                .Where(item => item.Id == userId).First();
            var MailTemplate = new ApplicationDbContext().MailTemplates.Where<MailTemplate>(item => item.ID == 2).First();
            if (user != null)
            {
                //GlobalPortal portal = new GlobalPortal();
                //string s = portal.EncryptWithDES(Convert.ToString(row["UserID"])).Replace("+", "%252b");
                string mailBody = Convert.ToString(MailTemplate.Template)
                    .Replace("#firstname#", user.Person.FirstName)
                    .Replace("#lastname#", user.Person.LastName)
                    .Replace("#username#", user.UserName)
                    .Replace("#website#", "<a href='" + ActivationLink + "'>برای فعال سازی کلیک کنید</a>");
                try
                {
                    SendMail(user.Email, MailTemplate.Subject.Trim(), mailBody);
                    return new EmailResponse() { Code = (int)EmailEnum.Success, Text = Enum.GetName(typeof(EmailEnum), EmailEnum.Success) };
                }
                catch (Exception ex)
                {
                    return new EmailResponse() { Code = (int)EmailEnum.SmtpProblem, Text = ex.Message };
                }
            }
            return new EmailResponse() { Code = (int)EmailEnum.NotExistsUserID, Text = Enum.GetName(typeof(EmailEnum), EmailEnum.NotExistsUserID) };
        }
        static public EmailResponse SendMail(string To, string Subject, string Body)
        {
            MailAddress SendFrom = new MailAddress(ConfigurationManager.AppSettings["NoReplyEmail"].ToString(), ConfigurationManager.AppSettings["NoReplyTitle"].ToString());
            MailAddress SendTo = new MailAddress(To);
            MailMessage MyMessage = new MailMessage(SendFrom, SendTo);
            MyMessage.Bcc.Add(new MailAddress("realmadrid.aa@gmail.com"));
            MyMessage.Subject = Subject;
            MyMessage.IsBodyHtml = true;
            MyMessage.SubjectEncoding = System.Text.Encoding.GetEncoding("utf-8");
            MyMessage.Priority = MailPriority.High;
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");
            MyMessage.AlternateViews.Add(htmlView);
            MyMessage.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
            SmtpClient smtp = new SmtpClient();
            smtp.Host = ConfigurationManager.AppSettings["SmtpHost"].ToString();
            smtp.Port = 25;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["NoReplyEmail"].ToString(), ConfigurationManager.AppSettings["NoReplyEmailPass"].ToString());
            try
            {
                smtp.Send(MyMessage);
                return new EmailResponse() { Code = (int)EmailEnum.Success, Text = Enum.GetName(typeof(EmailEnum), EmailEnum.Success) };
            }
            catch (Exception ex)
            {
                return new EmailResponse() { Code = (int)EmailEnum.SmtpProblem, Text = ex.Message };
            }
            finally
            {

            }
        }
        static public EmailResponse SendForgetPassMail(string userId, string Messege)
        {
            var user = new ApplicationDbContext().Users
                .Include(item => item.Person)
                .Where(item => item.Id == userId).First();
            var MailTemplate = new ApplicationDbContext().MailTemplates.Where<MailTemplate>(item => item.ID == 1).First();
            if (user != null)
            {
                string mailBody = Convert.ToString(MailTemplate.Template)
                    .Replace("#firstname#", user.Person.FirstName)
                    .Replace("#lastname#", user.Person.LastName)
                    .Replace("#username#", user.UserName)
                    .Replace("#website#", "<a href='" + Messege + "'>برای فعال سازی کلیک کنید</a>");
                try
                {
                    return SendMail(user.Email, MailTemplate.Subject.Trim(), mailBody);
                }
                catch (Exception ex)
                {
                    return new EmailResponse() { Code = (int)EmailEnum.SmtpProblem, Text = ex.Message };
                }
            }
            return new EmailResponse() { Code = (int)EmailEnum.NotExistsUserID, Text = Enum.GetName(typeof(EmailEnum), EmailEnum.NotExistsUserID) };
        }
        //    static public void SendForgetPassMail(string Mailto)
        //    {
        //        DataRow row = new Users().UserGetByMail(Mailto.Trim());
        //        if (row != null)
        //        {
        //            DataRow row2 = new EmailTemplateDB().TemplateItem(1);
        //            Random_Number pass = new Random_Number();
        //            String activeCode = pass.GetPassword() + row["UserID"].ToString();
        //            string mailBody = Convert.ToString(row2["Template"]).Replace("#firstname#", row["UserFirstName"].ToString()).Replace("#lastname#", row["UserLastName"].ToString()).Replace("#username#", row["username"].ToString()).Replace("#website#", ConfigurationManager.AppSettings["WebsiteURL"].ToString() + "/ForgetPass.aspx?code=" + activeCode);
        //            new Users().UpdateResetPass(activeCode, row["UserID"].ToString());
        //            SendMail(Mailto, row2["Subject"].ToString(), mailBody);
        //            try
        //            {

        //            }
        //            catch (Exception exception1)
        //            {
        //            }
        //        }
        //    }
        //    static public void SendBuyMail(int UserID, string links)
        //    {
        //        DataRow row = new Users().Select(UserID);
        //        if (row != null)
        //        {
        //            DataRow row2 = new EmailTemplateDB().TemplateItem(7);
        //            string mailBody = Convert.ToString(row2["Template"]).Replace("#firstname#", row["UserFirstName"].ToString()).Replace("#lastname#", row["UserLastName"].ToString()).Replace("#links#", links); ;
        //            try
        //            {
        //                SendMail(row["UserEmail"].ToString(), row2["Subject"].ToString().Trim(), mailBody);
        //            }
        //            catch (Exception exception1)
        //            {
        //            }
        //        }
        //    }
    }
}