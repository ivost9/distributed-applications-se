using System.ComponentModel.DataAnnotations;

namespace App2.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Дата на поръчка")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Количество")]
        [Required]
        public int Quantity { get; set; }
        [Display(Name ="Крайна цена")]
        public double TotalPrice { get; set; }
        [Display(Name = "Продукт")]
        public int ProductId { get; set; }
        [Display(Name = "Продукт")]
        [Required]
        public Product Product { get; set; }
    }
}
