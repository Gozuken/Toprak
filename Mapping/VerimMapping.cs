using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Verim.Api.Dtos;
using Verim.Api.Entities;

namespace Verim.Api.Mapping;

public static class VerimMapping
{
    public static User ToEntity(this RegisterDto registerDto, int id)
    {
        return new User()
            {
                UserId = id,
                Username = registerDto.Username,
                Password = registerDto.Password
            };
    }

    public static Asset ToEntity(this AssetsDto Dto, int AssetId, User owner)
    {
        return new Asset()
                {
                    AssetID = AssetId,
                    UserId = owner.UserId,
                    Owner = owner,
                    AssetType = Dto.AssetType,
                    ProvinceName = Dto.ProvinceName,
                    DistrictName = Dto.DistrictName,
                    NeighborhoodName = Dto.NeighborhoodName,
                    BlockNumber = Dto.BlockNumber,
                    ParcelNumber = Dto.ParcelNumber
                };

    }

    public static AssetsDto ToDto(this Asset asset)
    {
        return new AssetsDto(
            asset.AssetType,
            asset.ProvinceName,
            asset.DistrictName,
            asset.NeighborhoodName,
            asset.BlockNumber,
            asset.ParcelNumber
        );
                    
    }
}
