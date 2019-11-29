using System;

namespace Foodies.Foody.Core.Security
{
    public class JwtTokenMeta
    {
        public DateTime ExpiresAt { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
}