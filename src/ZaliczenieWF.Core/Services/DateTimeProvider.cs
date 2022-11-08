using System;
using System.Collections.Generic;
using System.Text;

namespace ZaliczenieWF.Core.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
