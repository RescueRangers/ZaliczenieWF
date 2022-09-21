using System;
using ZaliczenieWF.Models;

namespace ZaliczenieWF.Core.Events
{
    public class ScoreReceivedEventArgs : EventArgs
    {
        public string Score { get; set; }
        public Competition Competition { get; set; }
    }
}
