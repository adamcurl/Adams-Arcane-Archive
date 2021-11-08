using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdamsArcaneArchive.Data.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        [StringLength(30)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string LastName { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }

    public class AppUserRole : IdentityUserRole<Guid>
    {
        // Identity Navigation Properties
        public AppUser User { get; set; }

        public AppRole Role { get; set; }
    }

    public class AppRole : IdentityRole<Guid>
    {
        public const string Administrator = "Administrator";
        public const string User = "User";

        // Identity Navigation Properties
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
