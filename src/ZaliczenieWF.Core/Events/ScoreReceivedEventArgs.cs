using System;
using System.Collections.Generic;
using System.Text;
using ZaliczenieWF.Core.Services;

namespace ZaliczenieWF.Core.Events
{
    public class ScoreReceivedEventArgs : EventArgs
    {
        public string Score { get; set; }
        public Competition Competition { get; set; }
    }
}
