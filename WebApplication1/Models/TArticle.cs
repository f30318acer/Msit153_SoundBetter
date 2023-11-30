﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjMusicBetter.Models;

public partial class TArticle
{
    public int FArticleId { get; set; }

    public int FMemberId { get; set; }

    public string FContent { get; set; }

    public int FStyleId { get; set; }

    public DateTime? FUpdateTime { get; set; }

    public string FTitle { get; set; }

    public string FPhotoPath { get; set; }

    public virtual TMember FMember { get; set; }

    public virtual TStyle FStyle { get; set; }

    public virtual ICollection<TArticlePicture> TArticlePictures { get; set; } = new List<TArticlePicture>();
}