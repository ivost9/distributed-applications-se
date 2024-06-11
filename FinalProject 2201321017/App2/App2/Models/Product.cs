using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App2.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Описанието не може да бъде по-дълго от 20 символа.")]
        [Display(Name = "Име")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        [StringLength(200, ErrorMessage = "Описанието не може да бъде по-дълго от 200 символа.")]
        public string Description { get; set; }
        [Display(Name = "Цена")]
        [Required]
        public double Price { get; set; }
        [Display(Name = "Категория")]
        public int CategoryId {  get; set; }
        [Display(Name = "Категория")]
        [Required]
        public Category Category { get; set; }
    }
}
