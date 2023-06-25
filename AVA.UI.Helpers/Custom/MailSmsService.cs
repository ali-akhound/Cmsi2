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
        static public EmailResponse SendSubmitArticle(string Firstname, string Lastname, string Username, string Email, string ArticleTitle)
        {
            var MailTemplate = new ApplicationDbContext().MailTemplates.Where<MailTemplate>(item => item.ID == 4).First();
            string mailBody = Convert.ToString(MailTemplate.Template)
                    .Replace("#firstname#", Firstname)
                    .Replace("#lastname#", Lastname)
                    .Replace("#username#", Username)
                    .Replace("#article#", ArticleTitle);
            try
            {
                return SendMail(Email, MailTemplate.Subject.Trim(), mailBody);
            }
            catch (Exception ex)
            {
                return new EmailResponse() { Code = (int)EmailEnum.SmtpProblem, Text = ex.Message };
            }
        }
        static public EmailResponse SendEditArticle(string Firstname, string Lastname, string Username, string Email, string ArticleTitle)
        {
            var MailTemplate = new ApplicationDbContext().MailTemplates.Where<MailTemplate>(item => item.ID == 5).First();
            string mailBody = Convert.ToString(MailTemplate.Template)
                    .Replace("#firstname#", Firstname)
                    .Replace("#lastname#", Lastname)
                    .Replace("#username#", Username)
                    .Replace("#article#", ArticleTitle);
            try
            {
                return SendMail(Email, MailTemplate.Subject.Trim(), mailBody);
            }
            catch (Exception ex)
            {
                return new EmailResponse() { Code = (int)EmailEnum.SmtpProblem, Text = ex.Message };
            }
        }
        static public EmailResponse SendRefereeResult(string Firstname, string Lastname, string Username, string Email, string ArticleTitle, string RefereeStatus)
        {
            var MailTemplate = new ApplicationDbContext().MailTemplates.Where<MailTemplate>(item => item.ID == 6).First();

            string mailBody = Convert.ToString(MailTemplate.Template)
                .Replace("#firstname#", Firstname)
                .Replace("#lastname#", Lastname)
                .Replace("#username#", Username)
                .Replace("#article#", ArticleTitle)
                .Replace("#status#", RefereeStatus);
            try
            {
                return SendMail(Email, MailTemplate.Subject.Trim(), mailBody);
            }
            catch (Exception ex)
            {
                return new EmailResponse() { Code = (int)EmailEnum.SmtpProblem, Text = ex.Message };
            }
        }
        static public EmailResponse SendRefereePeresentTypeResult(string Firstname, string Lastname, string Username, string Email, string ArticleTitle, string PresentType, string ArticleStatus)
        {
            var MailTemplate = new ApplicationDbContext().MailTemplates.Where<MailTemplate>(item => item.ID == 14).First();

            string mailBody = Convert.ToString(MailTemplate.Template)
                .Replace("#firstname#", Firstname)
                .Replace("#lastname#", Lastname)
                .Replace("#username#", Username)
                .Replace("#article#", ArticleTitle)
                .Replace("#PresentType#", PresentType)
                .Replace("#ArticleStatus#", ArticleStatus);
            try
            {
                return SendMail(Email, MailTemplate.Subject.Trim(), mailBody);
            }
            catch (Exception ex)
            {
                return new EmailResponse() { Code = (int)EmailEnum.SmtpProblem, Text = ex.Message };
            }
        }
        static public EmailResponse SendPayNotification(string Firstname, string Lastname, string Count, string Module, string Email)
        {
            var MailTemplate = new ApplicationDbContext().MailTemplates.Where<MailTemplate>(item => item.ID == 7).First();

            string mailBody = Convert.ToString(MailTemplate.Template)
                .Replace("#firstname#", Firstname)
                .Replace("#lastname#", Lastname)
                .Replace("#count#", Count)
                .Replace("#module#", Module);
            try
            {
                return SendMail(Email, MailTemplate.Subject.Trim(), mailBody);
            }
            catch (Exception ex)
            {
                return new EmailResponse() { Code = (int)EmailEnum.SmtpProblem, Text = ex.Message };
            }
        }
        static public EmailResponse SendRefereeNotification(string Firstname, string Lastname,string Username, string Count, string Email)
        {
            var MailTemplate = new ApplicationDbContext().MailTemplates.Where<MailTemplate>(item => item.ID == 8).First();

            string mailBody = Convert.ToString(MailTemplate.Template)
                .Replace("#firstname#", Firstname)
                .Replace("#lastname#", Lastname)
                .Replace("#count#", Count)
                .Replace("#username#", Username);
            try
            {
                return SendMail(Email, MailTemplate.Subject.Trim(), mailBody);
            }
            catch (Exception ex)
            {
                return new EmailResponse() { Code = (int)EmailEnum.SmtpProblem, Text = ex.Message };
            }
        }
       
        static public EmailResponse SendSocietyPaymentConfirmation(string Firstname, string Lastname, string CompanyID, string Email)
        {
            var MailTemplate = new ApplicationDbContext().MailTemplates.Where<MailTemplate>(item => item.ID == 9).First();

            string mailBody = Convert.ToString(MailTemplate.Template)
                .Replace("#firstname#", Firstname)
                .Replace("#lastname#", Lastname)
                .Replace("#companyid#", CompanyID);
            try
            {
                return SendMail(Email, MailTemplate.Subject.Trim(), mailBody);
            }
            catch (Exception ex)
            {
                return new EmailResponse() { Code = (int)EmailEnum.SmtpProblem, Text = ex.Message };
            }
        }
        static public EmailResponse SendSocietyPaymentNotConfirmation(string Firstname, string Lastname, string CompanyID, string Email)
        {
            var MailTemplate = new ApplicationDbContext().MailTemplates.Where<MailTemplate>(item => item.ID == 11).First();

            string mailBody = Convert.ToString(MailTemplate.Template)
                .Replace("#firstname#", Firstname)
                .Replace("#lastname#", Lastname)
                .Replace("#companyid#", CompanyID);
            try
            {
                return SendMail(Email, MailTemplate.Subject.Trim(), mailBody);
            }
            catch (Exception ex)
            {
                return new EmailResponse() { Code = (int)EmailEnum.SmtpProblem, Text = ex.Message };
            }
        }
        static public EmailResponse SendConferencePaymentConfirmation(string Firstname, string Lastname, string Email)
        {
            var MailTemplate = new ApplicationDbContext().MailTemplates.Where<MailTemplate>(item => item.ID == 12).First();

            string mailBody = Convert.ToString(MailTemplate.Template)
                .Replace("#firstname#", Firstname)
                .Replace("#lastname#", Lastname);
            try
            {
                return SendMail(Email, MailTemplate.Subject.Trim(), mailBody);
            }
            catch (Exception ex)
            {
                return new EmailResponse() { Code = (int)EmailEnum.SmtpProblem, Text = ex.Message };
            }
        }
        static public EmailResponse SendConferencePaymentNotConfirmation(string Firstname, string Lastname, string Email)
        {
            var MailTemplate = new ApplicationDbContext().MailTemplates.Where<MailTemplate>(item => item.ID == 13).First();

            string mailBody = Convert.ToString(MailTemplate.Template)
                .Replace("#firstname#", Firstname)
                .Replace("#lastname#", Lastname);
            try
            {
                return SendMail(Email, MailTemplate.Subject.Trim(), mailBody);
            }
            catch (Exception ex)
            {
                return new EmailResponse() { Code = (int)EmailEnum.SmtpProblem, Text = ex.Message };
            }
        }
        static public EmailResponse SendActivationCode(string userId, string ActivationCode)
        {
            var user = new ApplicationDbContext().Users
                .Include(item => item.Person)
                .Where(item => item.Id == userId).First();
            var MailTemplate = new ApplicationDbContext().MailTemplates.Where<MailTemplate>(item => item.ID == 17).First();
            if (user != null)
            {
                //GlobalPortal portal = new GlobalPortal();
                //string s = portal.EncryptWithDES(Convert.ToString(row["UserID"])).Replace("+", "%252b");
                string mailBody = Convert.ToString(MailTemplate.Template)
                    .Replace("#firstname#", user.Person.FirstName)
                    .Replace("#lastname#", user.Person.LastName)
                    .Replace("#username#", user.UserName)
                    .Replace("#code#", ActivationCode);
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
        static public EmailResponse SendElectionInvitation(string userId,string siteUrl)
        {
            var user = new ApplicationDbContext().Users
                .Include(item => item.Person)
                .Where(item => item.Id == userId).First();
            var MailTemplate = new ApplicationDbContext().MailTemplates.Where<MailTemplate>(item => item.ID == 18).First();
            if (user != null)
            {
                string mailBody = Convert.ToString(MailTemplate.Template)
                    .Replace("#firstname#", user.Person.FirstName)
                    .Replace("#lastname#", user.Person.LastName)
                    .Replace("#username#", user.UserName)
                    .Replace("#website#", "<a href='" + siteUrl + "'>برای ورود به پورتال کلیک کنید</a>");
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
        #region Send Welcome Email
        static public EmailResponse SendRefereeWelcome(string userId)
        {
            var user = new ApplicationDbContext().Users
                .Include(item => item.Person)
                .Where(item => item.Id == userId).First();
            var MailTemplate = new ApplicationDbContext().MailTemplates.Where<MailTemplate>(item => item.ID == 10).First();
            if (user != null)
            {
                //GlobalPortal portal = new GlobalPortal();
                //string s = portal.EncryptWithDES(Convert.ToString(row["UserID"])).Replace("+", "%252b");
                string mailBody = Convert.ToString(MailTemplate.Template)
                    .Replace("#firstname#", user.Person.FirstName)
                    .Replace("#lastname#", user.Person.LastName)
                    .Replace("#username#", user.UserName);
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
        static public EmailResponse SendExecutorWelcome(string userId)
        {
            var user = new ApplicationDbContext().Users
                .Include(item => item.Person)
                .Where(item => item.Id == userId).First();
            var MailTemplate = new ApplicationDbContext().MailTemplates.Where<MailTemplate>(item => item.ID == 15).First();
            if (user != null)
            {
                //GlobalPortal portal = new GlobalPortal();
                //string s = portal.EncryptWithDES(Convert.ToString(row["UserID"])).Replace("+", "%252b");
                string mailBody = Convert.ToString(MailTemplate.Template)
                    .Replace("#firstname#", user.Person.FirstName)
                    .Replace("#lastname#", user.Person.LastName)
                    .Replace("#username#", user.UserName);
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
        static public EmailResponse SendScientificSecretaryWelcome(string userId)
        {
            var user = new ApplicationDbContext().Users
                .Include(item => item.Person)
                .Where(item => item.Id == userId).First();
            var MailTemplate = new ApplicationDbContext().MailTemplates.Where<MailTemplate>(item => item.ID == 16).First();
            if (user != null)
            {
                //GlobalPortal portal = new GlobalPortal();
                //string s = portal.EncryptWithDES(Convert.ToString(row["UserID"])).Replace("+", "%252b");
                string mailBody = Convert.ToString(MailTemplate.Template)
                    .Replace("#firstname#", user.Person.FirstName)
                    .Replace("#lastname#", user.Person.LastName)
                    .Replace("#username#", user.UserName);
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
        #endregion
    }
}