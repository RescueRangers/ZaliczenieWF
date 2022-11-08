using System;
using System.Collections.Generic;
using System.Text;

namespace ZaliczenieWF.Core.Services
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
