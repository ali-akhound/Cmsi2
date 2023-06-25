using AVA.UI.Helpers.Base;
using AVA.UI.Helpers.CustomAttribute;
using AVA.Web.Mvc.Areas.Admin.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace AVA.Web.Mvc.Models.Admin
{

    #region Vote
    public class VoteViewModel
    {
        [Key]
        [ShowInGridview]
        [GridColumnWidth(70)]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [StringLength(150)]
        public string ElectionName { get; set; }
        public string ElectionStartDate { get; set; }
        public string ElectionEndDate { get; set; }
        public List<UserVoteViewModel> UserVotes { get; set; } = new List<UserVoteViewModel>();
        [ShowInGridview]
        [Display(Name = "تاریخ ثبت")]
        public string CreateDateConverted { get; set; }
        public List<DropDownVm> CandidateTypes { get; set; } = new List<DropDownVm>();
        public int candidCnt { get; set; } = 0;
        public int inspectorCnt { get; set; } = 0;
        [Display(Name = "کد فعال سازی")]
        public string ActivationCode { get; set; }
        public ObjectState ObjectState { get; set; }
    }
    public class UserVoteViewModel
    {
        public CandidateViewModel Candidate { get; set; } = new CandidateViewModel();
        public bool IsChecked { get; set; }
    }
    #endregion
}
