﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjMusicBetter.Models;

public partial class TVision
{
    public int FVisionId { get; set; }

    public int FMemberId { get; set; }

    public string FVisionName { get; set; }

    public string FVisionDescription { get; set; }

    public string FVisionPath { get; set; }

    public virtual TMember FMember { get; set; }
}