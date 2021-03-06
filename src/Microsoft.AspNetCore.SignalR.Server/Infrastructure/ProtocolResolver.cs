// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


using System;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.SignalR.Infrastructure
{
    public class ProtocolResolver
    {
        private const string ProtocolQueryParameter = "clientProtocol";
        private readonly Version _minSupportedProtocol;
        private readonly Version _maxSupportedProtocol;
        private readonly Version _minimumDelayedStartVersion = new Version(1, 4);

        public ProtocolResolver() :
            this(new Version(1, 2), new Version(1, 5))
        {
        }

        public ProtocolResolver(Version min, Version max)
        {
            _minSupportedProtocol = min;
            _maxSupportedProtocol = max;
        }

        public Version Resolve(HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            Version clientProtocol;

            if (Version.TryParse(request.Query[ProtocolQueryParameter], out clientProtocol))
            {
                if (clientProtocol > _maxSupportedProtocol)
                {
                    clientProtocol = _maxSupportedProtocol;
                }
                else if (clientProtocol < _minSupportedProtocol)
                {
                    clientProtocol = _minSupportedProtocol;
                }
            }

            return clientProtocol ?? _minSupportedProtocol;
        }

        public bool SupportsDelayedStart(HttpRequest request)
        {
            return Resolve(request) >= _minimumDelayedStartVersion;
        }
    }
}
