﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjMusicBetter.Models;

public partial class TCoupon
{
    public int FCouponId { get; set; }

    public string FCouponContent { get; set; }

    public string FCouponCode { get; set; }

    public string FDescription { get; set; }

    public DateTime? FStartdate { get; set; }

    public DateTime? FEnddate { get; set; }

    public string FPicture { get; set; }

    public virtual ICollection<TDealClass> TDealClasses { get; set; } = new List<TDealClass>();

    public virtual ICollection<TDealProject> TDealProjects { get; set; } = new List<TDealProject>();

    public virtual ICollection<TDealSiteLoan> TDealSiteLoans { get; set; } = new List<TDealSiteLoan>();

    public virtual ICollection<TMemberCoupon> TMemberCoupons { get; set; } = new List<TMemberCoupon>();
}