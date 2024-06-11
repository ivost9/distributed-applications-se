using System.ComponentModel.DataAnnotations;

namespace App2.Models
{
    public class Category
    {
        [Key]
        public int Id {  get; set; }
        [Display(Name = "Име")]
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Display(Name = "Дата на създаване")]
        public DateTime dateOfCreate { get; set; }
    }
}
