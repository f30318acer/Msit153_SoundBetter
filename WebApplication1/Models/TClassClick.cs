﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjMusicBetter.Models;

public partial class TClassClick
{
    public int FClassClickId { get; set; }

    public int? FClassId { get; set; }

    public int? FClick { get; set; }

    public virtual TClass FClass { get; set; }
}