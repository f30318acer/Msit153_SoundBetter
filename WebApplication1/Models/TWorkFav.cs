﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjMusicBetter.Models;

public partial class TWorkFav
{
    public int FWorkFav { get; set; }

    public int? FWorkId { get; set; }

    public int? FMemberId { get; set; }

    public virtual TMember FMember { get; set; }

    public virtual TWork FWork { get; set; }
}