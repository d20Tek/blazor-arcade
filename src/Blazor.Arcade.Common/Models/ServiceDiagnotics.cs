//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------

namespace Blazor.Arcade.Common.Models
{
    public class ServiceDiagnostics
    {
        public string Result { get; set; } = string.Empty;

        public string EndpointUrl { get; set; } = string.Empty;

        public long Timestamp { get; set; }

        public string? CallerId { get; set; }

        public string? CallerName { get; set; }
    }
}
