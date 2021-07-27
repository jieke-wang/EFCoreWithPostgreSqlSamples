using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DALEntity.Entities
{
    public partial class Account
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime? RegisterDate { get; set; }
        [Required]
        public bool? RegisteredUser { get; set; }
        public decimal Balance { get; set; }
    }
}
