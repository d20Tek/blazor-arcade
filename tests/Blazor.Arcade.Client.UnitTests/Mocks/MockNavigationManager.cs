//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;

namespace Blazor.Arcade.Client.UnitTests.Mocks
{
    internal class MockNavigationManager : NavigationManager
    {
        public MockNavigationManager()
        {
            Initialize("https://test.com/", "https://test.com/api");
        }
    }
}
