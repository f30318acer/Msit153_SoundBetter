﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace prjMusicBetter.Models;

public partial class TMember
{
    public int FMemberId { get; set; }

    public string FUsername { get; set; }

    public string FName { get; set; }

    public string FPassword { get; set; }

    public string FPhone { get; set; }

    public string FEmail { get; set; }

    public bool FGender { get; set; }

    public DateTime? FBirthday { get; set; }

    public DateTime? FCreationTime { get; set; }

    public string FIntroduction { get; set; }

    public int? FPermissionId { get; set; }

    public string FPhotoPath { get; set; }

    public virtual TMemberPromission FPermission { get; set; }

    public virtual ICollection<TApplicationRecord> TApplicationRecords { get; set; } = new List<TApplicationRecord>();

    public virtual ICollection<TArticleFav> TArticleFavs { get; set; } = new List<TArticleFav>();

    public virtual ICollection<TArticle> TArticles { get; set; } = new List<TArticle>();

    public virtual ICollection<TClassFav> TClassFavs { get; set; } = new List<TClassFav>();

    public virtual ICollection<TClass> TClasses { get; set; } = new List<TClass>();

    public virtual ICollection<TDealClass> TDealClasses { get; set; } = new List<TDealClass>();

    public virtual ICollection<TDealProject> TDealProjects { get; set; } = new List<TDealProject>();

    public virtual ICollection<TDealSiteLoan> TDealSiteLoans { get; set; } = new List<TDealSiteLoan>();

    public virtual ICollection<TEvaluate> TEvaluates { get; set; } = new List<TEvaluate>();

    public virtual ICollection<TMemberCoupon> TMemberCoupons { get; set; } = new List<TMemberCoupon>();

    public virtual ICollection<TMemberRelation> TMemberRelations { get; set; } = new List<TMemberRelation>();

    public virtual ICollection<TMemberSite> TMemberSites { get; set; } = new List<TMemberSite>();

    public virtual ICollection<TMemberSkill> TMemberSkills { get; set; } = new List<TMemberSkill>();

    public virtual ICollection<TNotification> TNotifications { get; set; } = new List<TNotification>();

    public virtual ICollection<TProjectFav> TProjectFavs { get; set; } = new List<TProjectFav>();

    public virtual ICollection<TProject> TProjects { get; set; } = new List<TProject>();

    public virtual ICollection<TSite> TSites { get; set; } = new List<TSite>();

    public virtual ICollection<TWorkFav> TWorkFavs { get; set; } = new List<TWorkFav>();

    public virtual ICollection<TWork> TWorks { get; set; } = new List<TWork>();
}