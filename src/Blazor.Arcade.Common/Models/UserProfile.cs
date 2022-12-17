//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace Blazor.Arcade.Common.Models
{
    public class UserProfile : IEquatable<UserProfile>
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        public string Server { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = "U";

        public int Exp { get; set; } = 0;

        public string UserId { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            return Equals(obj as UserProfile);
        }

        public bool Equals(UserProfile? other)
        {
            return other != null &&
                   Id == other.Id &&
                   Server== other.Server &&
                   Name == other.Name &&
                   Avatar == other.Avatar &&
                   Gender== other.Gender &&
                   Exp == other.Exp &&
                   UserId == other.UserId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Server, Name, Avatar, Gender, Exp, UserId);
        }

        public void ValidateModel()
        {
            Id.ThrowIfEmpty(new FormatException("User profile must have a valid id"));

            UserId.ThrowIfEmpty(new FormatException("User profile must have a valid user id"));

            Server.ThrowIfEmpty(new FormatException("User profile must have a valid server id"));

            Name.ThrowIfEmpty(new FormatException("User profile must have a valid account name"));
        }
    }
}
