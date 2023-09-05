using System.ComponentModel.DataAnnotations;

namespace bulky_web.Models;

public class Category
{
  [Key]
  public int Id { get; set; }
  [Required]
  public string Name { get; set; }
  public int DisplayOrder { get; set; }
}
