namespace AVA.Core.Entities
{
    public class Enums
    {
        public enum PackageType
        {
            SocietyPackage = 1,
            RegisterConferencePackage = 2,
            ConferenceFeaturePackage = 3,
            SocietyPackageFeature = 4,
        }
        public enum VipPackageType
        {
            Normal = 10,
            University_Student = 9,
        }
        public enum ConferencePackageName
        {
            RegisterUserPackage = 1,
            UniversityUserPackage = 2,
            RegularUserPackage = 3,
            SecondArticlePackage = 4,
        }
        public enum ArticleStatus
        {
            Checking = 1,
            SendToReferee = 2,
            RefereeConfirmed = 3,
            RefereeNotConfirmed = 4,
            Confirmed = 5,
        }
        public enum ArticlePresentType
        {
            NotConfirmed = 0,
            Checking = 1,
            Verbally = 2,
            Poster = 3,
            PrintInCd = 4,
        }
        public enum RefereeStatusType
        {
            Checking = 1,
            Confirmed = 2,
            NotConfirmed = 3,
        }
        public enum OrderStatus
        {
            PreInvoice = 1,
            Checking = 2,
            Confirmed = 3,
            NotConfirmed = 4
        }
        public enum InvoiceType
        {
            Online = 1,
            CashBankPay = 2,
        }
    }
}
