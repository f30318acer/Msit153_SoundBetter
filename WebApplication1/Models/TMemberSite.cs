﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjMusicBetter.Models;

public partial class TMemberSite
{
    public int FMemberSiteId { get; set; }

    public int FMemberId { get; set; }

    public int FSiteId { get; set; }

    public virtual TMember FMember { get; set; }

    public virtual TSite FSite { get; set; }
}