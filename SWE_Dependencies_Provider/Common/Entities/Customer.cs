using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Enums;

namespace Common.Entities;

public class Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CustomerId { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required]
    public string EmailAddress { get; set; }
    [Required]
    public ECustomerStatus Status { get; set; }
    
    public virtual ICollection<FinancialProduct> FinancialProducts { get; set; }
}