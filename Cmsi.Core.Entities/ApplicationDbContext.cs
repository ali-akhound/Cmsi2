namespace AVA.Core.Entities
{
    using System.Data.Entity;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext() : base("Cmsi", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }
        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Migrations.Configuration>());
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<AdvertisePane> AdvertisePanes { get; set; }
        public virtual DbSet<ContactUs> ContactUses { get; set; }
        public virtual DbSet<DynamicMenu> DynamicMenus { get; set; }
        public virtual DbSet<EmailNotifyTable> EmailNotifyTables { get; set; }
        public virtual DbSet<MailTemplate> MailTemplates { get; set; }
        public virtual DbSet<News> Newses { get; set; }
        public virtual DbSet<Slider> Sliders { get; set; }
        public virtual DbSet<WebSiteConfig> WebSiteConfigs { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<FAQ> FAQs { get; set; }
        #region City & Province & Zone
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<CityZone> CityZones { get; set; }
        #endregion
        #region DynamicPage
        public virtual DbSet<DynamicPage> DynamicPages { get; set; }
        public virtual DbSet<DynamicPageContent> DynamicPageContents { get; set; }
        #endregion
        #region  Poll
        public virtual DbSet<Poll> Polls { get; set; }
        public virtual DbSet<PollQuestion> PollQuestions { get; set; }
        public virtual DbSet<PollAnswer> PollAnswers { get; set; }
        public virtual DbSet<PollQuestionAnswer> PollQuestionAnswers { get; set; }
        #endregion
        #region SysModule
        public virtual DbSet<SysModule> SysModules { get; set; }
        public virtual DbSet<SysRoleModule> SysRoleModules { get; set; }
        #endregion
        #region Invoices
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceType> InvoiceTypes { get; set; }
        #endregion
        #region  Gallery
        public virtual DbSet<GalleryAlbum> GalleryAlbums { get; set; }
        public virtual DbSet<GalleryImage> GalleryImages { get; set; }
        #endregion
        #region Category
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryName> CategoryNames { get; set; }
        #endregion
        #region Csmi
        public virtual DbSet<Conference> Conferences { get; set; }
        public virtual DbSet<ConferenceCompanion> ConferenceCompanions { get; set; }
        public virtual DbSet<PackageName> PackageNames { get; set; }
        public virtual DbSet<PackageNameTranslation> PackageNameTranslations { get; set; }
        public virtual DbSet<PackageType> PackageTypes { get; set; }
        public virtual DbSet<ConferencePackage> ConferencePackages { get; set; }
        public virtual DbSet<ConferenceCategory> ConferenceCategories { get; set; }
        public virtual DbSet<ConferenceArticle> ConferenceArticles { get; set; }
        public virtual DbSet<ConferenceExecutor> ConferenceExecutors { get; set; }
        public virtual DbSet<ConferenceScientificSecretary> ConferenceScientificSecretaries { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
        public virtual DbSet<SocietyVipOrder> SocietyVipOrders { get; set; }

        //public virtual DbSet<CompanyPackageUser> CompanyPackageUsers { get; set; }
        //public virtual DbSet<CompanyPackageName> CompanyPackageNames { get; set; }
        //public virtual DbSet<CompanyPackageNameTranslation> CompanyPackageNameTranslations { get; set; }


        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<ArticleWriter> ArticleWriters { get; set; }

        public virtual DbSet<ArticleCategory> ArticleCategories { get; set; }
        public virtual DbSet<ArticleStatus> ArticleStatuses { get; set; }
        public virtual DbSet<ArticlePresentType> ArticlePresentTypes { get; set; }
        public virtual DbSet<Referee> Referees { get; set; }
        public virtual DbSet<Executor> Executors { get; set; }
        public virtual DbSet<ScientificSecretary> ScientificSecretaries { get; set; }
        public virtual DbSet<ConferenceReferee> ConferenceReferees { get; set; }
        public virtual DbSet<RefereeQuestion> RefereeQuestions { get; set; }
        public virtual DbSet<RefereeAnswer> RefereeAnswers { get; set; }
        public virtual DbSet<RefereeQuestionAnswer> RefereeQuestionAnswers { get; set; }
        public virtual DbSet<RefereeArticle> RefereeArticles { get; set; }
        public virtual DbSet<RefereeStatus> RefereeStatuses { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<SocietyMember> SocietyMembers { get; set; }
        public virtual DbSet<SocietyMemberPeriod> SocietyMemberPeriods { get; set; }
        public virtual DbSet<SocietyExecutor> SocietyExecutors { get; set; }
        public virtual DbSet<Election> Elections { get; set; }
        public virtual DbSet<Candidate> Candidates { get; set; }
        public virtual DbSet<CandidateType> CandidateTypes { get; set; }
        public virtual DbSet<UserVoteToken> UserVoteTokens { get; set; }
        
        public virtual DbSet<UserVote> UserVotes { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<ContactUs>()
                .Property(e => e.Family)
                .IsFixedLength();
            modelBuilder.Entity<DynamicPage>()
            .HasMany(l => l.DynamicPageContents);

            base.OnModelCreating(modelBuilder);
        }
    }
}
