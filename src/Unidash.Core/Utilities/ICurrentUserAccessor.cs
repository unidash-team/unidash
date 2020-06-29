using System;
using System.Collections.Generic;
using System.Text;

namespace Unidash.Core.Utilities
{
    public interface ICurrentUserAccessor
    {
        string GetUserId();
    }
}
