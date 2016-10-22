using AspNetIdentityWithAngularjs.Data;
using AspNetIdentityWithAngularjs.Data.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(AspNetIdentityWithAngularjs.Startup))]
namespace AspNetIdentityWithAngularjs
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<ApplicationDbContext>(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);            

            //app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions {
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            //});

            app.UseOAuthBearerTokens(new OAuthAuthorizationServerOptions
            {
                Provider = new ApplicationAuthProvider(),
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/Authenticate")
            });
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext<ApplicationDbContext>(() => new ApplicationDbContext());
            app.CreatePerOwinContext<UserManager<IdentityUser>>(CreateManager);
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/oauth/token"),
                Provider = new AuthorizationServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                AllowInsecureHttp = true,

            });
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
        {
            public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
            {
                context.Validated();
                //string clientId;
                //string clientSecret;

                //if (context.TryGetBasicCredentials(out clientId, out clientSecret))
                //{
                //    // validate the client Id and secret 
                //    context.Validated();
                //}
                //else
                //{
                //    context.SetError("invalid_client", "Client credentials could not be retrieved from the Authorization header");
                //    context.Rejected();
                //}
            }
            public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
            {
                UserManager<IdentityUser> userManager = context.OwinContext.GetUserManager<UserManager<IdentityUser>>();
                IdentityUser user;
                try
                {
                    user = await userManager.FindAsync(context.UserName, context.Password);
                }
                catch
                {
                    // Could not retrieve the user due to error.
                    context.SetError("server_error");
                    context.Rejected();
                    return;
                }
                if (user != null)
                {
                    ClaimsIdentity identity = await userManager.CreateIdentityAsync(
                                                            user,
                                                            DefaultAuthenticationTypes.ExternalBearer);
                    context.Validated(identity);
                }
                else
                {
                    context.SetError("invalid_grant", "Invalid UserId or password'");
                    context.Rejected();
                }
            }
        }



        private static UserManager<IdentityUser> CreateManager(IdentityFactoryOptions<UserManager<IdentityUser>> options, IOwinContext context)
        {
            var userStore = new UserStore<IdentityUser>(context.Get<ApplicationDbContext>());
            var owinManager = new UserManager<IdentityUser>(userStore);
            return owinManager;
        }

    }
}