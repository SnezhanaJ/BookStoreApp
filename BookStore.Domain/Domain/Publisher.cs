using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Domain
{
    public class Publisher : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Books>? Books { get; set; }
    }
}
