using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
//using movieAPI.Validations;

namespace movieAPI.Entities
{
    public class Genre
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage ="The field with name {0} is required")]
        [StringLength(50)]
      //  [FirstLetterUpperCase]
         public string Name { get; set; }

    }
}
