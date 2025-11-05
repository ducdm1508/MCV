using System;
using System.Collections.Generic;

namespace databasefirt.Models;

public partial class Lecturer
{
    public int LecturerId { get; set; }

    public string FullName { get; set; } = null!;

    public string Degree { get; set; } = null!;

    public int? DepartmentId { get; set; }

    public virtual Department? Department { get; set; }
}
