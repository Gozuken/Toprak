using Verim.Api.Dtos;
using Verim.Api.Entities;
using Verim.Api.OptionsData;
namespace Verim.Api.Mapping;

public static class VerimMapping
{
    public static User ToEntity(this RegisterCredentialsDto credentialsDto, int id)
    {
        return new User()
            {
                UserId = id,
                Mail = credentialsDto.Mail ?? "no mail",
                Username = credentialsDto.Username,
                Password = credentialsDto.Password
            };
    }

    public static Asset ToEntity(this AssetsIncomingDto Dto, int AssetId, User owner)
    {

        return new Asset()
                {
                    AssetId = AssetId,
                    UserId = owner.UserId,
                    Owner = owner,
                    AssetType = Dto.AssetType,
                    ProvinceName = Dto.ProvinceName,
                    DistrictName = Dto.DistrictName,
                    NeighborhoodName = Dto.NeighborhoodName,
                    BlockNumber = Dto.BlockNumber,
                    ParcelNumber = Dto.ParcelNumber,
                    PlantedProduct = Dto.PlantedProduct,
                    PlantDate = Dto.PlantDate
                };
        

    }

    public static AssetsOutgoingDto ToDto(this Asset asset)
    {
        return new AssetsOutgoingDto(
            asset.AssetId,
            asset.AssetType,
            asset.ProvinceName,
            asset.DistrictName,
            asset.NeighborhoodName,
            asset.BlockNumber,
            asset.ParcelNumber,
            asset.PlantedProduct,
            asset.PlantDate
        );
                    
    }

    //Doesn't update AssetId or UserId
    public static void UpdateAsset(this Asset asset, AssetsIncomingDto dto) 
    {  

        asset.AssetType = dto.AssetType;
        asset.ProvinceName = dto.ProvinceName;
        asset.DistrictName = dto.DistrictName;
        asset.NeighborhoodName = dto.NeighborhoodName;
        asset.BlockNumber = dto.BlockNumber ?? asset.BlockNumber;
        asset.ParcelNumber = dto.ParcelNumber ?? asset.ParcelNumber;
        asset.PlantedProduct = dto.PlantedProduct ?? asset.PlantedProduct;
        asset.PlantDate = dto.PlantDate ?? asset.PlantDate;
    }

    public static Survey ToEntity(this SurveyDto surveyDto, int id, Asset ownerAsset)
    {
        return new Survey()
            {
                AssetId = surveyDto.AssetId,
                SurveyId = id,
                AmountStar = surveyDto.AmountStar,
                FutureStar = surveyDto.FutureStar,
                FertilizerUse = surveyDto.FertilizerUse,
                Comment = surveyDto.Comment,
                ownerAsset = ownerAsset
            };
    }
}
