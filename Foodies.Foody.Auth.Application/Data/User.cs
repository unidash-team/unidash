using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foodies.Foody.Auth.Application.Data
{
    public class User : Entity
    {
        public string DisplayName { get; set; }

        public string EmailAddress { get; set; }
    }
}
