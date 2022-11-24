//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;

namespace Blazor.Arcade.Service.Repositories
{
    public interface IGameMetadataRepository
    {
        public Task<IList<GameMetadata>> GetAll();

        public Task<GameMetadata?> GetById(string id);
    }
}
