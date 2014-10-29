using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    public interface IOAuth2
    {
        Int32 Otype{get;}

        String State { get; }

        String Access_token { get; }

        String Refresh_token { get; }

        DateTime Expire { get; }

        String OUID { get; }
    }
}
