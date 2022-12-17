//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------

using Blazor.Arcade.Common;
using Blazor.Arcade.Common.Models;

namespace Blazor.Arcade.Client.UnitTests
{
    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void String_ValueOrDefault()
        {
            // arrange
            string s = "test";

            // act
            var result = s.ValueOrDefault();

            // assert
            Assert.AreEqual("test", result);
        }

        [TestMethod]
        public void String_ValueOrDefault_Null()
        {
            // arrange
            string? s = null;

            // act
            var result = s.ValueOrDefault();

            // assert
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void List_ListOrDefault()
        {
            // arrange
            var s = new List<string> { "foo", "bar" };

            // act
            var result = s.ListOrDefault();

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void List_ListOrDefault_Null()
        {
            // arrange
            List<UserProfile>? s = null;

            // act
            var result = s.ListOrDefault();

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Entity_ObjectOrDefault()
        {
            // arrange
            var s = new UserProfile { Id = "account1", Name = "Tester", UserId = "user1" };

            // act
            var result = s.ObjectOrDefault();

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("account1", result.Id);
        }

        [TestMethod]
        public void Entity_ObjectOrDefault_Null()
        {
            // arrange
            UserProfile? s = null;

            // act
            var result = s.ObjectOrDefault();

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("", result.Id);
        }
    }
}
