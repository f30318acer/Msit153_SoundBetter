﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjMusicBetter.Models;

public partial class TSite
{
    public int FSiteId { get; set; }

    public string FSiteName { get; set; }

    public int FMemberId { get; set; }

    public string FPhone { get; set; }

    public int FSiteType { get; set; }

    public int FCityId { get; set; }

    public string FAddress { get; set; }

    public virtual TCity FCity { get; set; }

    public virtual TMember FMember { get; set; }

    public virtual TSitePicture FSitePicture { get; set; }

    public virtual ICollection<TClass> TClasses { get; set; } = new List<TClass>();

    public virtual ICollection<TDealSiteLoan> TDealSiteLoans { get; set; } = new List<TDealSiteLoan>();

    public virtual ICollection<TMemberSite> TMemberSites { get; set; } = new List<TMemberSite>();

    public virtual ICollection<TProject> TProjects { get; set; } = new List<TProject>();

    public virtual ICollection<TSitePeriod> TSitePeriods { get; set; } = new List<TSitePeriod>();

    //public virtual ICollection<TSitePicture> TSitePictures { get; set; } = new List<TSitePicture>();
    public string SiteTypeText
    {
        get
        {
            var siteTypeMapping = new Dictionary<int, string>
                {
                    { 1, "音樂學校" },
                    { 2, "錄音室" },
                };
            return siteTypeMapping.GetValueOrDefault(FSiteType, "未知");
        }
    }
}