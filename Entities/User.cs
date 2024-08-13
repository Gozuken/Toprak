using System;
using Verim.Api.Dtos;

namespace Verim.Api.Entities;

    public class User
    {
        public int UserId {get; set;}
        public ICollection<Asset>? Assets { get; set; } // Navigation property
        public required string Username {get; set;}
        public required string Password {get; set;}
        public string? AuthToken {get; set;}

    }
