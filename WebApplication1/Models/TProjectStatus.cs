﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjMusicBetter.Models;

public partial class TProjectStatus
{
    public int FProjectStatusId { get; set; }

    public string FDescription { get; set; }

    public virtual ICollection<TProject> TProjects { get; set; } = new List<TProject>();
}