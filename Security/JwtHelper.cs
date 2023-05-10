﻿using ApiService;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LogisticsApiServices.Security
{
    public class JwtHelper
    {
        /// <summary>
		/// Returns a Jwt Token from basic input parameters
		/// </summary>
		/// <param name="user_id"></param>
		/// <param name="uniqueKey"></param>
		/// <param name="issuer"></param>
		/// <param name="audience"></param>
		/// <param name="expiration"></param>
		/// <param name="additionalClaims"></param>
		/// <returns></returns>
		public static JwtSecurityToken GetJwtToken(
            string user_id,
            string uniqueKey,
            string issuer,
            string audience,
            TimeSpan expiration,
            Claim[] additionalClaims = null)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user_id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (additionalClaims is object)
            {
                var claimList = new List<Claim>(claims);
                claimList.AddRange(additionalClaims);
                claims = claimList.ToArray();
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(uniqueKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.UtcNow.Add(expiration),
                claims: claims,
                signingCredentials: creds
            );
        }

        /// <summary>
        /// Returns a token string from base claims
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="uniqueKey"></param>
        /// <param name="issuer"></param>
        /// <param name="audience"></param>
        /// <param name="expiration"></param>
        /// <param name="additionalClaims"></param>
        /// <returns></returns>
        public static string GetJwtTokenString(
            LoginObject user_data,
            string uniqueKey,
            string issuer,
            string audience,
            TimeSpan expiration,
            Claim[] additionalClaims = null)
        {
            DBContext db = new DBContext();
            var data = db.Users.Where(item => item.Login == user_data.Login).ToList();
            if (data.Count > 0)
            
                if (data[0].Password == user_data.Password)
                {
                    var token = GetJwtToken(user_data.Login, uniqueKey, issuer, audience, expiration, additionalClaims);
                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
                else
                    goto DataFailed;            
            else 
                goto DataFailed;

            DataFailed:
            {
                return "Invalid data";
            }
        }

        /// <summary>
        /// Converts an existing Jwt Token to a string
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetJwtTokenString(JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Returns an issuer key
        /// </summary>
        /// <param name="issuerKey"></param>
        /// <returns></returns>
        public static SymmetricSecurityKey GetSymetricSecurityKey(string issuerKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerKey));
        }
    }
}
