//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Common.Models
{
    public class GameSession : IEquatable<GameSession>
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string ServerId { get; set; } = string.Empty;

        public string MetadataId { get; set; } = string.Empty;

        public string HostId { get; set; } = string.Empty;

        public int Phase { get; set; }

        public Dictionary<string, object> GameState { get; set; } = new Dictionary<string, object>();

        public override bool Equals(object? obj)
        {
            return Equals(obj as GameSession);
        }

        public bool Equals(GameSession? other)

        {
            return other != null &&
                   Id == other.Id &&
                   Name== other.Name &&
                   ServerId == other.ServerId &&
                   MetadataId == other.MetadataId &&
                   HostId == other.HostId &&
                   Phase == other.Phase &&
                   EqualityComparer<Dictionary<string, object>>.Default.Equals(GameState, other.GameState);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, ServerId, Name, MetadataId, HostId, Phase);
        }

        public void ValidateModel()
        {
            Id.ThrowIfEmpty(new FormatException("Game session must have a valid id"));

            HostId.ThrowIfEmpty(new FormatException("Game session must have a valid host id"));

            MetadataId.ThrowIfEmpty(new FormatException("Game session must have a valid game metadata id"));

            ServerId.ThrowIfEmpty(new FormatException("Game session must have a valid server id"));

            Name.ThrowIfEmpty(new FormatException("Game session must have a valid name"));
        }
    }
}
