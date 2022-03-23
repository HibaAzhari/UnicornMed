// <copyright file="MustBeValidUpnRequirement.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace UnicornMed.Authentication
{
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    /// This class is an authorization policy requirement.
    /// It specifies that the issuer is a valid one
    /// </summary>
    public class ValidateIssuerRequirement : IAuthorizationRequirement
    {
    }
}
