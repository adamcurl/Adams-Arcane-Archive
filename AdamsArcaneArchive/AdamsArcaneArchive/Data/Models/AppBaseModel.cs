using System;
using System.ComponentModel.DataAnnotations;

namespace AdamsArcaneArchive.Data.Models
{
    public abstract class IdBaseModel : AppBaseModel
    {
        [Key]
        public Guid Id { get; set; }
    }

    public abstract class AppBaseModel
    {
        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
