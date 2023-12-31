﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models;

[Table("tb_m_accounts")]
public class Account : BaseEntity
{

    [Column("password", TypeName = "nvarchar(255)")]
    public string Password { get; set; }

    [Column("is_deleted")]
    public Boolean IsDeleted { get; set; }

    [Column("otp")]
    public int Otp { get; set; }

    [Column("is_used")]
    public Boolean IsUsed { get; set; }

    [Column("expired_date")]
    public DateTime ExpiredDate { get; set; }
    //Cardinality
    public ICollection<AccountRole>? AccountRoles { get; set; }
    public Employee? Employee { get; set; }
    
}
