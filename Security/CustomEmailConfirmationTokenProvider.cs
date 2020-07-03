using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapp.Security
{
    public class CustomEmailConfirmationTokenProvider<Tuser>: DataProtectorTokenProvider<Tuser> where Tuser : class
    {
        public CustomEmailConfirmationTokenProvider(IDataProtectionProvider dataProtectionProvider,
                                                     IOptions<CustomEmailConfirmationTokenProviderOptions> options,
                                                      ILogger<CustomEmailConfirmationTokenProvider<Tuser>> logger)
            : base(dataProtectionProvider, options, logger)
        { }

    }
}
