//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.JSInterop;

namespace Blazor.Arcade.Client.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        private readonly IJSRuntime _jsRuntime;

        public MessageBoxService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public ValueTask Alert(string message)
        {
            return _jsRuntime.InvokeVoidAsync("alert", message);
        }

        public ValueTask<bool> Confirm(string message)
        {
            return _jsRuntime.InvokeAsync<bool>("confirm", message);
        }
    }
}
