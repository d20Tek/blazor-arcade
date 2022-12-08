//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Common.Core.Services
{
    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException(string entityIdName, object entityIdValue, Exception? innerException = null)
            : base($"Entity with {entityIdName} = {entityIdValue} already exists in this repository.", innerException)
        {
            EntityIdName = entityIdName;
            EntityIdValue = entityIdValue;
        }

        public string EntityIdName { get; private set; }

        public object EntityIdValue { get; private set; }
    }
}
