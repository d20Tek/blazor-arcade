//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blazor.Arcade.Service.Controllers
{
    [ApiController]
    [Route("api/v1/game-metadata")]
    [Authorize]
    public class GameMetadataController : ArcadeControllerBase
    {
        private readonly IReadRepository<GameMetadata> _repo;

        public GameMetadataController(
            IReadRepository<GameMetadata> repository,
            ILogger<GameMetadataController> logger)
            : base(logger)
        {
            _repo = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<GameMetadata>>> GetGameMetadataAll()
        {
            return await EndpointOperationAsync<List<GameMetadata>>(nameof(GetGameMetadataAll), async () =>
            {
                var results = await _repo.GetAll();
                return results.ToList();
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameMetadata?>> GetGameMetadataById(string id)
        {
            return await EndpointOperationAsync<GameMetadata?>(nameof(GetGameMetadataById), async () =>
            {
                return await _repo.GetById(id);
            });
        }
    }
}
