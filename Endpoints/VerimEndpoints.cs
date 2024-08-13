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


namespace Verim.Api.Endpoints;

public static class VerimEndpoints
{
    public static WebApplication MapAuthenticationEndpoints(this WebApplication app)
    {
        var groupLogin = app.MapGroup("login");
        var groupRegister = app.MapGroup("register");

        //GET /login
        groupLogin.MapGet("/", () => "This is the login screen");

        //GET /register
        groupRegister.MapGet("/", () => "This is the Register screen");


        //POST /login
        groupLogin.MapPost("/", (CredentialsDto credentials, HttpContext httpContext, VerimContext dbcontext) =>
        {
            var AuthResult = AuthenticationHelper.AuthenticateUser(dbcontext, credentials);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(7), // expires in 7 days
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

        // POST /register
        groupRegister.MapPost("/", (RegisterDto RegisterDto, VerimContext dbContext) => 
        {   
            var existingUser = dbContext.Users.FirstOrDefault(user => user.Username == RegisterDto.Username);
            
            if(existingUser is not null)
            {
                return Results.BadRequest("User already exist, please choose another username...");
            }
            
            int id = dbContext.Users.Max(user => user.UserId) + 1;

            dbContext.Users.Add(RegisterDto.ToEntity(id));
            dbContext.SaveChanges();

            return Results.Created();
        });

        return app;
    }


    public static WebApplication MapMainEndpoints(this WebApplication app) 
    {
        var groupMain = app.MapGroup("main");

        //GET /main
        groupMain.MapGet("/", () => "This is the main screen");

        //POST /main
        //groupMain.MapPost("/", () => dummy);

        
        //GET /main/Assets
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

        //POST /main/assets
        groupMain.MapPost("/assets", (AssetsDto AssetsDto, VerimContext dbContext, string cookie = "") => 
        {
            /*
                Assets with unique AssetId's are being added to the database
                dummy cookie has been placed, remember to implement that later
                must move the assets adding page to main/addassets because this is only the main page.
            */
          
            //Verify
            if (cookie == "")
            {
                var owner = dbContext.Users //this is a dummy 
                                .SingleOrDefault(u => u.UserId == 1); // retrieve UserId from cookie and compare here

                if (owner == null) return Results.NotFound("Owner user not found.");

                var AssetId = dbContext.Assets.Max(u => u.AssetID) + 1; 

                var asset = AssetsDto.ToEntity(AssetId, owner);
                dbContext.Assets.Add(asset);
                dbContext.SaveChanges();
                
                return Results.Accepted();
            }
            return Results.BadRequest();

        
        });


        return app;
    }
}