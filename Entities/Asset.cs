using System.ComponentModel.DataAnnotations;

namespace Verim.Api.Entities;

/*
public enum PlantProducts 
{
    wheat,
    rice,
    corn,
    potato,
    onion
}
*/
public class Asset
{
    public int AssetId {get; init;}
    public int UserId {get; set;}
    public required User Owner {get; set;} // Navigation property for owner user
    public ICollection<Survey>? Surveys { get; set; } // Navigation property for the SurveyResults
    [MaxLength(50)]public required string AssetType {get; set;}
    [MaxLength(50)]public required string ProvinceName {get; set;}
    [MaxLength(50)]public required string DistrictName {get; set;} 
    [MaxLength(50)]public required string NeighborhoodName {get; set;}
    public string? BlockNumber {get; set;}
    public required string ParcelNumber {get; set;}
    public string? PlantedProduct {get; set;}
    public DateOnly? PlantDate {get; set;}
}
