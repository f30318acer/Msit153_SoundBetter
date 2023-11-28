﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace prjMusicBetter.Models;

public partial class TCoupon
{
    public int FCouponId { get; set; }

    [DisplayName("優惠內容")] // 這個就是顯示在表格標題上的文字
    public string FCouponContent { get; set; }
    [DisplayName("優惠代碼")]
    public string FCouponCode { get; set; }
    [DisplayName("優惠描述")]
    public string FDescription { get; set; }
    [DisplayName("優惠開始日期")] // 這個就是顯示在表格標題上的文字
    public DateTime? FStartdate { get; set; }
    [DisplayName("優惠結束日期")]
    public DateTime? FEnddate { get; set; }
    [DisplayName("圖片")]
    public string FPicture { get; set; }

    public virtual ICollection<TDealClass> TDealClasses { get; set; } = new List<TDealClass>();

    public virtual ICollection<TDealProject> TDealProjects { get; set; } = new List<TDealProject>();

    public virtual ICollection<TDealSiteLoan> TDealSiteLoans { get; set; } = new List<TDealSiteLoan>();

    public virtual ICollection<TMemberCoupon> TMemberCoupons { get; set; } = new List<TMemberCoupon>();
}