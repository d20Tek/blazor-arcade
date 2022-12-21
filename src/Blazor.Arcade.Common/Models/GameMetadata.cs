//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core;

namespace Blazor.Arcade.Common.Models
{
    public class GameMetadata
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public ValueRange NumPlayers { get; set; }

        public ValueRange Duration { get; set; }

        public int Complexity { get; set; } = 1;

        public IList<string> Tags { get; set; } = new List<string>();

        public int SortOrder { get; set; } = 9999;

        public GameLocationMetadata Locations { get; set; } = new GameLocationMetadata();
    }
}
