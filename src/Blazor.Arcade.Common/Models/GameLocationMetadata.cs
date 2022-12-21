//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Common.Models
{
    public class GameLocationMetadata
    {
        private const string _defaultLobbyPath = "/lobby";

        public string GameUrl { get; set; } = string.Empty;

        public string IconUrl { get; set; } = string.Empty;

        public string LargeLogoUrl { get; set; } = string.Empty;

        public string BannerImageUrl { get; set; } = string.Empty;

        public virtual string GameLobbyUrl => GameUrl + _defaultLobbyPath;
    }
}
