using System;
using System.Collections.Generic;
using System.Text;

namespace ZaliczenieWF.Core.Events
{
    public class ScoreReceivedEventArgs : EventArgs
    {
        public string Score { get; set; }
    }
}
