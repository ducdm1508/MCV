using System;
using System.Collections.Generic;

namespace databasefirt.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public string? Dean { get; set; }

    public virtual ICollection<Lecturer>? Lecturers { get; set; }
}
