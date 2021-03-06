using System.Threading;
using System.Threading.Tasks;
using Nancy;
using Nancy.Authentication.Token;
using Nancy.ModelBinding;
using UrlShortener.Infrastructure.Data.Queries.Account;
using UrlShortener.WebApi.Lib.Authentication;
using UrlShortener.WebApi.Models.Auth;

namespace UrlShortener.WebApi.Modules
{
    public class AuthModule : BaseModule
    {
        private readonly ITokenizer _tokenizer;
        private readonly GetByEmail _getByEmail;

        public AuthModule(ITokenizer tokenizer, GetByEmail getByEmail)
            : base("/auth")
        {
            _tokenizer = tokenizer;
            _getByEmail = getByEmail;

            Post["/", true] = Auth;
        }

        private async Task<dynamic> Auth(dynamic _, CancellationToken ct)
        {
            var model = this.Bind<Account>();
            var entity = await _getByEmail.GetResult(model.Email);

            if (entity == null)
            {
                return HttpStatusCode.Unauthorized;
            }

            if (!entity.ValidatePassword(model.Password))
            {
                return HttpStatusCode.Unauthorized;
            }

            var user = new UserIdentity
            {
                UserName = entity.Email,
                Claims = new[] { "admin" }
            };

            var token = _tokenizer.Tokenize(user, Context);

            var response = new
            {
                accessToken = "Token" + token
            };

            return Negotiate.WithModel(response);
        }
    }
}