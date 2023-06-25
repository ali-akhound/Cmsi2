using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;

namespace AVA.Core.Entities
{
    //// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    //public class ApplicationUser : IdentityUser
    //{
    //    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    //    {
    //        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
    //        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
    //        // Add custom user claims here
    //        return userIdentity;
    //    }
    //}

    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public bool? SocialPageConfirm { get; set; }
        public bool? IsSocialPageAdmin { get; set; }
        public int? FavorateResource { get; set; }
        public bool? AllowDelete { get; set; }
        public DateTime? VipStartDate { get; set; }
        public DateTime? VipEndDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public bool Enable { get; set; }

        public virtual Person Person { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual ICollection<AspNetRole> UserRoles { get; set; }
        //public virtual ApplicationUser LastModifyUser { get; set; }
        //public virtual ApplicationUser CreatorUser { get; set; }
        //public virtual ICollection<ApplicationUser> CreatorUsers { get; set; }
        //public virtual ICollection<CompanyUserLike> CompanyUserLikes { get; set; }





    }
}

