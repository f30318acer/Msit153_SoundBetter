﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjMusicBetter.Models;

public partial class TCity
{
    public int FCityId { get; set; }

    public string FCity { get; set; }

    public virtual ICollection<TSite> TSites { get; set; } = new List<TSite>();
}