using Verim.Api.Data;
using Verim.Api.Dtos;
using Verim.Api.Mapping;
using Verim.Api.Entities;
using Verim.Api.Authentication;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Verim.Api.OptionsData;

namespace Verim.Api.Endpoints;

public static class VerimEndpoints
{
    public static WebApplication MapAuthenticationEndpoints(this WebApplication app)
    {
        var groupLogin = app.MapGroup("login");
        var groupRegister = app.MapGroup("register");

        //POST /login //HOOKED
        groupLogin.MapPost("/", (LoginCredentialsDto credentials, HttpContext httpContext, VerimContext dbcontext) =>
        {
            var AuthResult = AuthenticationHelper.AuthenticateUser(dbcontext, credentials);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(7), // expires in 7 days
                Path = "/", // Cookie available for all paths
                HttpOnly = true, // Prevent client-side access to the cookie
                Secure = httpContext.Request.IsHttps, // Use Secure only if HTTPS is used
                SameSite = SameSiteMode.Lax // Allows cookies to be sent with same-site requests
            };

            if (AuthResult.IsValid)
            {
                httpContext.Response.Cookies.Append("AuthToken", AuthResult.Token, cookieOptions);
                return Results.Ok();
            }
            else
            {
                return Results.BadRequest(AuthResult.ErrorMessage);
            }
        });

        // POST /register //HOOKED
        groupRegister.MapPost("/", (RegisterCredentialsDto credentialsDto, VerimContext dbContext) => 
        {   
            var existingUser = dbContext.Users.FirstOrDefault(user => user.Username == credentialsDto.Username);
            
            if(existingUser is not null)
            {
                return Results.BadRequest("User already exist, please choose another username...");
            }

            int id = dbContext.Users.Max(user => user.UserId) + 1;

            dbContext.Users.Add(credentialsDto.ToEntity(id));
            dbContext.SaveChanges();

            return Results.Created();
        });

        return app;
    }

    public static WebApplication MapMainEndpoints(this WebApplication app) 
    {
        var groupMain = app.MapGroup("main");
        
        //GET /main/assets //HOOKED
        groupMain.MapGet("/assets", (HttpContext httpContext, VerimContext dbContext) => 
        {
            var cookie = httpContext.Request.Cookies["AuthToken"];
            if (cookie is null) return Results.Redirect("/login");

            //finds related userId from cookie
            var userID = dbContext.Users
                                .Where(u => u.AuthToken == cookie)
                                .Select(u => u.UserId)
                                .SingleOrDefault();

            if (userID is 0) return Results.Unauthorized();

            //finds related assets from userId
            var assets = dbContext.Assets
                                .Where(a => a.UserId == userID)
                                .ToList();

            if (!assets.Any()) return Results.Ok($"No assets found for user with UserID: {userID}.");
            
            var assetDtos = assets.Select(a => a.ToDto());

            return Results.Ok(assetDtos);
        });

        //POST /main/assets //HOOKED
        groupMain.MapPost("/assets", (AssetsIncomingDto AssetsDto, VerimContext dbContext, HttpContext httpContext) => 
        {
            var token = httpContext.Request.Cookies["AuthToken"];
            if (token is null) return Results.Redirect("/login");

            var owner = dbContext.Users //access (User)owner from token (need better authentication than cookies)
                                 .SingleOrDefault(u => u.AuthToken == token); 

            if (owner == null) return Results.Unauthorized(); //not sure if this handling is correct (enumeration attacks?)

            //Add Asset to database
            var AssetId = dbContext.Assets.Max(u => u.AssetId) + 1; 
            var asset = AssetsDto.ToEntity(AssetId, owner);
            
            dbContext.Assets.Add(asset);
            dbContext.SaveChanges();

            return Results.Accepted();
        });

        //GET /main/assets/{AssetId} //HOOKED
        groupMain.MapGet("/assets/{AssetId}", (VerimContext dbContext, HttpContext httpContext, int AssetId) =>
        {
            var token = httpContext.Request.Cookies["AuthToken"];

            if (token is null) return Results.Redirect("/login");
            
            var ownerId = dbContext.Users // find user with matching token and userid
                    .Where(u => u.AuthToken == token)
                    .Select(u => u.UserId)
                    .FirstOrDefault();

            var asset = dbContext.Assets
                .Where(a => a.AssetId == AssetId && a.UserId == ownerId)
                .Include(a => a.Owner)  // Ensure related user data is loaded
                .FirstOrDefault();

            if (asset == null) return Results.NotFound(ownerId);

            //might need another dto to get the PlantedProduct
            //need to add a function on authhelper that checks if the cookie is related to the asset
            return Results.Ok(asset.ToDto());

        });
            //PUT /main/assets/{AssetId} //HOOKED
            groupMain.MapPut("/assets/{AssetId}", (VerimContext dbContext, HttpContext httpContext, AssetsIncomingDto assetsIncomingDto, int AssetId) =>
            {
                var token = httpContext.Request.Cookies["AuthToken"];
                if (token is null) return Results.Redirect("/login");

                //Does this user with this Token own this asset?
                var owner = dbContext.Users
                            .SingleOrDefault(u => u.AuthToken == token);

                var oldAsset = dbContext.Assets
                                    .Include(a => a.Owner) //eager load Owner
                                    .SingleOrDefault(a => a.AssetId == AssetId);//this might not be right

                if (oldAsset == null || oldAsset.Owner != owner) return Results.NotFound();
                //After authentication: 

                oldAsset.UpdateAsset(assetsIncomingDto);
                dbContext.SaveChanges();
                
                return Results.Ok(oldAsset.ToDto());
            });

            groupMain.MapDelete("/assets/{assetId}", (VerimContext dbContext, HttpContext httpContext, int AssetId) =>
            {
                var token = httpContext.Request.Cookies["AuthToken"];
                /*
                var token= dbContext.Users
                                .Where(u => u.UserId == 1)
                                .Select(t => t.AuthToken )
                                .FirstOrDefault();
                */
                if (token is null) return Results.Redirect("/login");

                //Does this user with this Token own this asset?
                var owner = dbContext.Users
                            .SingleOrDefault(u => u.AuthToken == token);

                //find asset from id 
                var asset = dbContext.Assets
                                    .Include(a => a.Owner) //eager load Owner
                                    .SingleOrDefault(a => a.AssetId == AssetId);//this might not be right
                if (asset == null || asset.Owner != owner) return Results.NotFound();
                //After authentication: 
                
                dbContext.Assets.Remove(asset);
                dbContext.SaveChanges();
                
                return Results.NoContent();
            });

            groupMain.MapPost("/survey/{assetId}", (SurveyDto surveyDto, VerimContext dbContext, HttpContext httpContext) => 
            {
                var token = httpContext.Request.Cookies["AuthToken"];
                if (token is null) return Results.Redirect("/login");

                var ownerUser = dbContext.Users //access (User)owner from token (need better authentication than cookies)
                                    .SingleOrDefault(u => u.AuthToken == token); 

                if (ownerUser == null) return Results.Unauthorized(); //not sure if this handling is correct (enumeration attacks?)

                var ownerAsset = dbContext.Assets
                                .Include(a => a.Owner) //eager load Owner
                                .SingleOrDefault(a => a.AssetId == surveyDto.AssetId);//this might not be right
                //Add survey to database
                var SurveyId = dbContext.SurveyResults.Max(u => u.SurveyId) + 1;  
                var survey = surveyDto.ToEntity(SurveyId, ownerAsset);
                
                dbContext.SurveyResults.Add(survey);
                dbContext.SaveChanges();

                return Results.Accepted();
            });

        return app;
    }

    public static WebApplication MapDataEndpoints(this WebApplication app) 
    {
        var groupAssets = app.MapGroup("main/assets");

        //GET /main/assets/options //HOOKED
        groupAssets.MapGet("/options", (HttpContext httpContext, VerimContext dbContext) =>
        {
            return Results.Ok(
                CombosData.GetComboOptionsDto(dbContext)
                );
        });

        groupAssets.MapGet("/districtOptions/{provinceId}", (HttpContext httpContext, VerimContext dbContext, int provinceId) =>
        {
            return Results.Ok(
                CombosData.GetDistrictOptions(dbContext, provinceId)
                );
        });

        groupAssets.MapGet("/neighborhoodOptions", (HttpContext httpContext, VerimContext dbContext) =>
        {
            return Results.Ok(
                CombosData.GetNeighborhoodOptions()
                );
        });

        groupAssets.MapGet("/top_products/{targetAssetId}", (HttpContext httpContext, VerimContext dbContext, int targetAssetId) =>
        {
            var assetDistrictId = (from asset in dbContext.Assets
                                    where asset.AssetId == targetAssetId
                                    join district in dbContext.District on asset.DistrictName equals district.DistrictName
                                    select district.DistrictId).FirstOrDefault();

            var top_products = ExcelService.GetTopProductsData(assetDistrictId);//var top_products = ExcelService.GetTopProductsData(districtId);

            if (top_products.Count == 0)
            {
                return Results.BadRequest();
            }
            else
            {
                return Results.Ok(top_products);
            }
            
        });

        return app;
    }
}
