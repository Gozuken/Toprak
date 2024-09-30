using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Verim.Api.Entities;


public class Survey
{
    public int SurveyId { get; set; } 
    [ForeignKey(nameof(ownerAsset))]public int AssetId { get; set; } 
    public int AmountStar { get; set; } 
    public int FutureStar { get; set; } 
    public bool FertilizerUse { get; set; }
    public string Comment { get; set; }
    public required Asset ownerAsset {get; set;}
}
