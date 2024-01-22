using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DataAccessLayer.Entities
{
    public enum Gender
    {
        Male,
        Female
    }

    public class UserRegisterModel
    {
        public required string FullName { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public string? Address { get; set; }
        public DateTime? BirthDate { get; set; }

        [EnumDataType(typeof(Gender))]
        [JsonConverter(typeof(StringEnumConverter))]
        public required Gender Gender { get; set; } = Gender.Male;
        public string? PhoneNumber { get; set; }
    }

    public class UserEditModel
    {
        public required string FullName { get; set; }
        public DateTime? BirthDate { get; set; }

        [EnumDataType(typeof(Gender))]
        [JsonConverter(typeof(StringEnumConverter))]
        public required Gender Gender { get; set; } = Gender.Male;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class LoginCredentials
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class UserDto
    {
        public Guid Id { get; set; }
        public required string FullName { get; set; }
        public DateTime? BirthDate { get; set; }

        [EnumDataType(typeof(Gender))]
        [JsonConverter(typeof(StringEnumConverter))]
        public required Gender Gender { get; set; } = Gender.Male;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public required string Email { get; set; }
        public string Password { get; set; } = "";
    }

    public class LoggedOutToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = "";
    }
}
