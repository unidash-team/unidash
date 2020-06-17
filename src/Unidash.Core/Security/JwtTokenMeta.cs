using System;

namespace Unidash.Core.Security
{
    public class JwtTokenMeta
    {
        public DateTime ExpiresAt { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }

        public JwtTokenMeta()
        {
        }

        public JwtTokenMeta(DateTime expiresAt, string audience, string issuer)
        {
            ExpiresAt = expiresAt;
            Audience = audience;
            Issuer = issuer;
        }
    }
}