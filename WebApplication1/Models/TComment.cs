﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjMusicBetter.Models;

public partial class TComment
{
    public int FCommentId { get; set; }

    public int FMemberId { get; set; }

    public string FCommentContent { get; set; }

    public int FArticleId { get; set; }

    public DateTime? FCommentTime { get; set; }

    public virtual TArticle FArticle { get; set; }

    public virtual TMember FMember { get; set; }
}

public partial class TArticle
{
	public List<TComment> Comments { get; set; }
}