using System.ComponentModel.DataAnnotations;

namespace Verim.Api.Entities;

/*
public enum AssetTypes 
{
    Tarla,
    Bag,
    Bahce
}
*/
public class Asset
{
    //Should they be init? (espacially ID)
    //Must create a additional table for this
    public int AssetID {get; set;}
    public int UserId {get; set;}
    public required User Owner {get; set;} // Navigation property
    [MaxLength(50)]public required string AssetType {get; set;}
    [MaxLength(50)]public required string ProvinceName {get; set;}
    [MaxLength(50)]public required string DistrictName {get; set;} 
    [MaxLength(50)]public required string NeighborhoodName {get; set;}
    public required string BlockNumber {get; set;}
    public required string ParcelNumber {get; set;}
}
