//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;

namespace Blazor.Arcade.Client.Services
{
    public interface IArcadeMetadataService
    {
        public Task<IList<GameMetadata>?> GetGamesMetadataAsync();
    }
}
