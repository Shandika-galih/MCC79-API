﻿using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs.AccountRole;

public class GetAccountRoleDto
{
    public Guid Guid { get; set; }
    public Guid AccountGuid { get; set; }
    public Guid RoleGuid { get; set; }
}
