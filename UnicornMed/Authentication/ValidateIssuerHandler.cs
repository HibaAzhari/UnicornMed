// <copyright file="MustBeValidUpnHandler.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace UnicornMed.Authentication
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;

    using Microsoft.Extensions.Configuration;
    /// <summary>
    /// This class is an authorization handler, which handles the authorization requirement.
    /// </summary>
    public class ValidateIssuerHandler : AuthorizationHandler<ValidateIssuerRequirement>
    {
        /// <summary>
        /// This method handles the authorization requirement.
        /// </summary>
        /// <param name="context">AuthorizationHandlerContext instance.</param>
        /// <param name="requirement">IAuthorizationRequirement instance.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ValidateIssuerRequirement requirement)
        {

            if (context.User.FindFirst("iss") != null)
            {
                string issuer = context.User.FindFirst("iss").Issuer;
                if (issuer.StartsWith("https://sts.windows.net/"))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }

    }
}