﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjMusicBetter.Models;

public partial class TProjectSkillRequire
{
    public int FProjectSkillRequireId { get; set; }

    public int FProjectId { get; set; }

    public int FSkillId { get; set; }

    public virtual TProject FProject { get; set; }

    public virtual TSkill FSkill { get; set; }
}