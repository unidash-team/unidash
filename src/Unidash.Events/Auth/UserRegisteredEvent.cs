using System;
using System.Collections.Generic;
using System.Text;

namespace Unidash.Events.Auth
{
    public class UserRegisteredEvent
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }
    }
}
