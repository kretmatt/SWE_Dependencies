using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities;

public class FinancialProduct
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int FinancialProductId { get; set; }
    [Required]
    public double Balance { get; set; }
    [Required]
    public string ProductCode { get; set; }
    [Required]
    public double InterestRate { get; set; }
}