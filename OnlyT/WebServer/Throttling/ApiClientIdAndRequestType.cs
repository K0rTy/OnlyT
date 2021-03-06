﻿namespace OnlyT.WebServer.Throttling
{
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    internal struct ApiClientIdAndRequestType
    {
        public string ClientId { get; }

        public ApiRequestType RequestType { get; }

        public ApiClientIdAndRequestType(string clientId, ApiRequestType requestType)
        {
            ClientId = clientId;
            RequestType = requestType;
        }
    }
}
