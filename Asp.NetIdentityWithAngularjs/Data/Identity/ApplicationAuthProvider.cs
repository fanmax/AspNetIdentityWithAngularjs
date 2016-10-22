﻿using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetIdentityWithAngularjs.Data.Identity
{
    public class ApplicationAuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            ApplicationUserManager storeUserMgr = context.OwinContext.Get<ApplicationUserManager>("AspNet.Identity.Owin:" + typeof(ApplicationUserManager).AssemblyQualifiedName);

            IdentityUser user = await storeUserMgr.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The username or password is incorrect");
            }
            else
            {
                ClaimsIdentity ident = await storeUserMgr.CreateIdentityAsync(user, "Custom");
                AuthenticationTicket ticket = new AuthenticationTicket(ident, new AuthenticationProperties());
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(ident);
            }
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }
    }
}