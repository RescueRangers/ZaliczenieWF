using System;
using System.Collections.Generic;
using System.Text;
using ZaliczenieWF.Core.Services;
using ZaliczenieWF.Models;

namespace ZaliczenieWF.Core.Events
{
    public class ScoreReceivedEventArgs : EventArgs
    {
        public string Score { get; set; }
        public Competition Competition { get; set; }
    }
}
