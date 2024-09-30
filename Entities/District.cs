using System.ComponentModel.DataAnnotations;

namespace Verim.Api.Entities;


public class District
{
    public int DistrictId { get; set; }
    public string DistrictName { get; set; }

    // Foreign key
    public int ProvinceId { get; set; }
    
    // Navigation property
    public Province Province { get; set; }
}
