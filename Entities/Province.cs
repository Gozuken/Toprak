using System.ComponentModel.DataAnnotations;

namespace Verim.Api.Entities;


public class Province
{
    public int ProvinceId { get; set; }
    public string ProvinceName { get; set; }

    // Navigation property
    public ICollection<District> Districts { get; set; }
}
