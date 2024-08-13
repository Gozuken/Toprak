using System.ComponentModel.DataAnnotations;

namespace Verim.Api.Dtos;

public record class AssetsDto(

    [Required][StringLength(30)]string AssetType,
    [Required][StringLength(30)]string ProvinceName,
    [Required][StringLength(30)]string DistrictName,
    [Required][StringLength(30)]string NeighborhoodName,
    [Required][StringLength(30)]string BlockNumber,
    [Required][StringLength(30)]string ParcelNumber

);
