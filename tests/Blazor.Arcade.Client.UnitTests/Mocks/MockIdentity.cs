//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;

namespace Blazor.Arcade.Client.UnitTests.Mocks
{
    internal class MockIdentity : IIdentity
    {
        private const string _defaultUserName = "Test User";
        private const string _defaultAuthType = "TestAuth";
        private readonly string _userName;

        public MockIdentity()
            : this(_defaultUserName)
        {
        }

        public MockIdentity(string name)
        {
            _userName = name;
        }

        public string? AuthenticationType => _defaultAuthType;

        [ExcludeFromCodeCoverage]
        public bool IsAuthenticated => true;

        public string? Name => _userName;
    }
}
