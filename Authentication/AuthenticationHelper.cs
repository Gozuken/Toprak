using Verim.Api.Data;
using Verim.Api.Dtos;
using Verim.Api.Mapping;
using Verim.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
namespace Verim.Api.Authentication;

public static class AuthenticationHelper
{
    public static string GenerateToken()
    {
        //might need to check if the token is already in the database for edge cases
        using var rng = RandomNumberGenerator.Create();
        var tokenBytes = new byte[32]; // 32 bytes = 256 bits
        rng.GetBytes(tokenBytes);
        string token = Convert.ToBase64String(tokenBytes);

        return token;
    }

    //This function finds the related user from the database and assigns a random token for future login.
    //returns AuthenticationResult with IsValid true/false, Token empty_string/random_token, ErrorMessage null/error_message
    public static AuthenticationResult AuthenticateUser(VerimContext dbcontext, LoginCredentialsDto credentials)
    {
        var user = dbcontext.Users.FirstOrDefault(u => u.Username == credentials.Username 
                                                        && u.Password == credentials.Password);

        if (user is null)
        {
            return new AuthenticationResult 
                { IsValid = false,Token = string.Empty, ErrorMessage = "Invalid Credentials" };
        }

        var token = GenerateToken();
        user.AuthToken = token;
        dbcontext.SaveChanges();

        return new AuthenticationResult { IsValid = true, Token = token };
    }
    
}

public class AuthenticationResult
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
    public required string Token { get; set; }
}