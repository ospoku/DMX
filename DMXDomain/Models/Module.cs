using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXDomain.Models;

public class Module
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
    public Guid PublicId { get; set; } = Guid.NewGuid();
}
