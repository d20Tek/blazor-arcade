//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;

namespace Blazor.Arcade.Service.UnitTests.Core
{
    [TestClass]
    public class EqualityTests
    {
        [TestMethod]
        public void UserProfile_Equals()
        {
            // arrange
            object m1 = new UserProfile
            {
                Id = "test-profile-1",
                Server = "s1",
                Avatar = "test-av",
                Name = "Test User",
                Gender = "M",
                Exp = 5,
                UserId = "test-user-1"
            };

            object m2 = m1;

            // act
            var eq = m1.Equals(m2);
            var hash = m1.GetHashCode() == m2.GetHashCode();

            // assert
            Assert.IsTrue(eq);
            Assert.IsTrue(hash);
        }

        [TestMethod]
        public void UserProfile_NotEquals()
        {
            // arrange
            var m1 = new UserProfile
            {
                Id = "test-profile-1",
                Server = "s1",
                Avatar = "test-av",
                Name = "Test User",
                Gender = "F",
                Exp = 5,
                UserId = "test-user-1"
            };

            var m2 = new UserProfile
            {
                Id = "test-profile-1",
                Server = "s1",
                Avatar = "test-av",
                Name = "Test User",
                Gender = "F",
                Exp = 10,
                UserId = "test-user-1"
            };

            // act
            var eq = m1.Equals(m2);
            var hash = m1.GetHashCode() == m2.GetHashCode();

            // assert
            Assert.IsFalse(eq);
            Assert.IsFalse(hash);
        }
    }
}
