﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjMusicBetter.Models;

public partial class TSkill
{
    public int FSkillId { get; set; }

    public string FName { get; set; }

    public int FSkillCategoryId { get; set; }

    public string FDescription { get; set; }

    public string FThumbnailPath { get; set; }

    public virtual TSkillCategory FSkillCategory { get; set; }

    public virtual ICollection<TClass> TClasses { get; set; } = new List<TClass>();

    public virtual ICollection<TMemberSkill> TMemberSkills { get; set; } = new List<TMemberSkill>();

    public virtual ICollection<TProjectSkillRequire> TProjectSkillRequires { get; set; } = new List<TProjectSkillRequire>();

    public virtual ICollection<TProject> TProjects { get; set; } = new List<TProject>();
}