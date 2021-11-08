using System.ComponentModel.DataAnnotations;

namespace AdamsArcaneArchive.Data.Models
{
    public class ExampleObject : IdBaseModel
    {
        [StringLength(100)]
        public string Description { get; set; }
    }
}
