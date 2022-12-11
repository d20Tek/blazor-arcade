//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;

namespace Blazor.Arcade.Service.UnitTests.Helpers
{
    internal class ObjectResultValidation
    {
        internal static void AssertStatusCode<T>(int expectedStatusCode, ActionResult<T> actionResult)
        {
            Assert.IsNotNull(actionResult.Result);
            if (actionResult.Result.GetType() == typeof(StatusCodeResult))
            {
                var objRes = (StatusCodeResult)actionResult.Result;
                Assert.AreEqual(expectedStatusCode, objRes.StatusCode);
            }
            else
            {
                Assert.IsInstanceOfType(actionResult.Result, typeof(ObjectResult));

                var objRes = (ObjectResult)actionResult.Result;
                Assert.AreEqual(expectedStatusCode, objRes.StatusCode);
            }
        }

        internal static void AssertSuccess<T>(ActionResult<T> actionResult)
        {
            Assert.IsNull(actionResult.Result);
        }
    }
}
