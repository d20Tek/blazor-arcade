//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Common.Models
{
    public enum GameSessionPhase
    {
        Created = 0,
        Available = 1,
        Initializing = 10,
        Playing = 20,
        BetweenRounds = 90,
        Completed = 100
    }
}
