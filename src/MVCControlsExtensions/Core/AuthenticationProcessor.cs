using System;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using SODA.Domain;
using SODA.Domain.Entities;

namespace SODA.Core
{
    public static class AuthenticationProcessor
    {
        public static string FormsCookieName { get { return FormsAuthentication.FormsCookieName + "_Soda"; } }

        public static void SetAuthenticationTicket(this HttpContextBase httpContext, User user, string[] roles, bool isPersistent)
        {
            var ticket = new UserTicket
                {
                    Name = user.Username,
                    UserId = user.UserId,
                    Roles = roles != null && roles.Length > 0
                                ? roles.Aggregate((c, n) => c + "," + n)
                                : string.Empty,
                };

            var jsonSerializer = new JavaScriptSerializer();

            var userData = jsonSerializer.Serialize(ticket);

            var authTicket = new FormsAuthenticationTicket(1, user.Username,
                                                           DateTime.Now, DateTime.Now.AddHours(8),
                                                           isPersistent, userData);

            
            var encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsCookieName, encryptedTicket)
                {
                    Expires = isPersistent ? authTicket.Expiration : DateTime.MinValue
                };

            httpContext.Response.Cookies.Add(authCookie);
        }

         public static void DeleteAuthenticationTicket(this HttpContextBase httpContext)
         {
             var authCookie = new HttpCookie(FormsCookieName)
                 {
                     Expires = DateTime.Now.AddDays(-10)
                 };

             httpContext.Response.Cookies.Add(authCookie);
         }

        public static void AuthenticationTicket(this HttpContext httpContext)
        {
            var authCookie = httpContext.Request.Cookies[FormsCookieName];

            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value))
                return;

            try
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                var jsonSerializer = new JavaScriptSerializer();

                if (authTicket == null) return;
                var ticket = jsonSerializer.Deserialize<UserTicket>(authTicket.UserData);

                var sodaIdentity = new SodaIdentity(ticket.Name)
                    {
                        UserId = ticket.UserId,
                        Roles = ticket.Roles.Split(','),
                    };

                httpContext.User = new SodaPrincipal(sodaIdentity);
            }
            catch
            {

            }
        }

        public class UserTicket
        {
            public int Id { get; set; }
            public Guid UserId { get; set; }
            public string Name { get; set; }
            public string Token { get; set; }
            public string Roles { get; set; }
        }

    }
}