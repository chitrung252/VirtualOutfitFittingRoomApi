using DataService.APIViewModels;
using DataService.Domain;
using DataService.Models.Identities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Models.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
                var array = context.Scope.ToArray();
                var fbId = array.GetValue(0).ToString();
                var brandID = Convert.ToInt32(array.GetValue(1));
                var phone = array.GetValue(2).ToString();
                var customerId = Convert.ToInt32(array.GetValue(3));
                //var a = array.GetValue(1);
                //= (int)array.GetValue(1);

                ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);


                if (user == null)
                {

                    if (fbId.Length > 0)
                    {
                        var customerDomain = new CustomerDomain();
                        CustomerAPIViewModel customer = customerDomain.GetCustomerByBrandIdAndFbId(brandID, fbId);
                        if (customer != null)
                        {
                            ApplicationDbContext db = new ApplicationDbContext();
                            ApplicationUser userVM = db.Users.FirstOrDefault(x => x.Id == customer.AspUserVM.Id);
                            ClaimsIdentity oAuthIdentityVM = await userVM.GenerateUserIdentityAsync(userManager,
                                OAuthDefaults.AuthenticationType);
                            ClaimsIdentity cookiesIdentityVM = await userVM.GenerateUserIdentityAsync(userManager,
                                CookieAuthenticationDefaults.AuthenticationType);

                            AuthenticationProperties propertiesVM = CreateProperties(userVM.UserName);
                            AuthenticationTicket ticketVM = new AuthenticationTicket(oAuthIdentityVM, propertiesVM);
                            context.Validated(ticketVM);
                            context.Request.Context.Authentication.SignIn(cookiesIdentityVM);
                        }
                    }
                    else if (phone.Length > 0)
                    {
                        var customerDomain = new CustomerDomain();
                        CustomerAPIViewModel customerByPhone = customerDomain.GetCustomersByPhonenumber(phone, brandID);
                        if (customerByPhone != null)
                        {
                            ApplicationDbContext db = new ApplicationDbContext();
                            ApplicationUser userVM = db.Users.FirstOrDefault(x => x.Id == customerByPhone.AspUserVM.Id);
                            ClaimsIdentity oAuthIdentityVM = await userVM.GenerateUserIdentityAsync(userManager,
                                OAuthDefaults.AuthenticationType);
                            ClaimsIdentity cookiesIdentityVM = await userVM.GenerateUserIdentityAsync(userManager,
                                CookieAuthenticationDefaults.AuthenticationType);

                            AuthenticationProperties propertiesVM = CreateProperties(userVM.UserName);
                            AuthenticationTicket ticketVM = new AuthenticationTicket(oAuthIdentityVM, propertiesVM);
                            context.Validated(ticketVM);
                            context.Request.Context.Authentication.SignIn(cookiesIdentityVM);
                        }
                    }
                    else if (customerId > 0)
                    {
                        var customerDomain = new CustomerDomain();
                        CustomerAPIViewModel customerById = customerDomain.GetCustomerById(customerId);
                        if (customerById != null)
                        {
                            ApplicationDbContext db = new ApplicationDbContext();
                            ApplicationUser userVM = db.Users.FirstOrDefault(x => x.Id == customerById.AspUserVM.Id);
                            ClaimsIdentity oAuthIdentityVM = await userVM.GenerateUserIdentityAsync(userManager,
                                OAuthDefaults.AuthenticationType);
                            ClaimsIdentity cookiesIdentityVM = await userVM.GenerateUserIdentityAsync(userManager,
                                CookieAuthenticationDefaults.AuthenticationType);

                            AuthenticationProperties propertiesVM = CreateProperties(userVM.UserName);
                            AuthenticationTicket ticketVM = new AuthenticationTicket(oAuthIdentityVM, propertiesVM);
                            context.Validated(ticketVM);
                            context.Request.Context.Authentication.SignIn(cookiesIdentityVM);
                        }
                    }
                    else
                    {
                        context.SetError("invalid_grant", "The user name or password is incorrect.");
                        return;
                    }

                }
                else
                {
                    ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
                   OAuthDefaults.AuthenticationType);
                    ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                        CookieAuthenticationDefaults.AuthenticationType);

                    AuthenticationProperties properties = CreateProperties(user.UserName);
                    AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                    context.Validated(ticket);
                    context.Request.Context.Authentication.SignIn(cookiesIdentity);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}
