﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.UnitTests.Mocks;
using Blazor.Arcade.Misc;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Blazor.Arcade.Client.UnitTests.Misc
{
    [TestClass]
    public class AuthenticationHelperTests
    {
        [TestMethod]
        public async Task IsUserAuthenticated_Authenticated()
        {
            // arrange 
            var authTask = CreateAuthenticatedUser();

            // act
            var result = await AuthenticationHelper.IsUserAuthenticated(authTask);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsUserAuthenticated_Unauthenticated()
        {
            // arrange 
            var authTask = CreateUnauthenticatedUser();

            // act
            var result = await AuthenticationHelper.IsUserAuthenticated(authTask);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task IsUserAuthenticated_NullAuthStateTask()
        {
            // arrange 

            // act
            var result = await AuthenticationHelper.IsUserAuthenticated(null);

            // assert
            Assert.IsFalse(result);
        }

        private Task<AuthenticationState> CreateAuthenticatedUser()
        {
            var principal = new ClaimsPrincipal(new MockIdentity());
            var authState = new AuthenticationState(principal);

            return Task.FromResult(authState);
        }

        private Task<AuthenticationState> CreateUnauthenticatedUser()
        {

            var principal = new ClaimsPrincipal();
            var authState = new AuthenticationState(principal);

            return Task.FromResult(authState);
        }
    }
}