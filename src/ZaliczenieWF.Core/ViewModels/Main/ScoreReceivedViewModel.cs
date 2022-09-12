using System;
using System.Collections.Generic;
using System.Text;
using ZaliczenieWF.Core.Models;

namespace ZaliczenieWF.Core.ViewModels.Main
{
    public class ScoreReceivedViewModel : BaseViewModel<Score, Score>
    {
        public override void Prepare(Score parameter)
        {

        }

        public Score Score { get; set; }
    }
}
