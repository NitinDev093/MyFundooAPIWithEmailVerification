using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace UtilityLayer
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            //It is for AllowAnonymus We will correct thius letter
            // Fix: Ensure GetEndpoint extension method is available by adding the correct using directive
            //var endpoint = context.GetEndpoint();
            //if (endpoint?.Metadata?.GetMetadata<Microsoft.AspNetCore.Authorization.IAllowAnonymous>() != null)
            //{
            //    // Skip JWT validation for anonymous actions
            //    await _next(context);
            //    return;
            //}

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                AttachUserToContext(context, token);

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");

                var tokenHandler = new JwtSecurityTokenHandler();
                string SecretKey = jwtSettings["SecretKey"];
                var key = Encoding.ASCII.GetBytes(SecretKey);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["ValidIssuer"],// set to true if needed
                    ValidateAudience = true,
                    ValidAudience= jwtSettings["ValidAudience"],// set to true if needed
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                string userData = jwtToken.Claims.First(x => x.Type == "unique_name").Value;
                DataTable userDeserializeObject =  JsonConvert.DeserializeObject<DataTable>(userData);
                context.Items["UserId"] = userDeserializeObject.Rows[0]["userId"].ToString();
                //context.Items["Email"] = userDeserializeObject.Rows[0]["Email"].ToString();
                //we can add more items if needed
            }
            catch
            {
                throw new SecurityTokenExpiredException("Invalid Token");
            }
        }
    }
}
