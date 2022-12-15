//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Client.Services
{
    public interface IMessageBoxService
    {
        public ValueTask Alert(string message);

        public ValueTask<bool> Confirm(string message);
    }
}
