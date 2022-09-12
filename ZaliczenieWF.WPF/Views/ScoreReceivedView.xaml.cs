using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using ZaliczenieWF.Core.ViewModels.Main;

namespace ZaliczenieWF.WPF.Views
{
    /// <summary>
    /// Interaction logic for ScoreReceivedView.xaml
    /// </summary>
    ///
    [MvxViewFor(typeof(ScoreReceivedViewModel))]
    [MvxWindowPresentation(Identifier = nameof(ScoreReceivedView), Modal = true)]
    public partial class ScoreReceivedView : MvxWindow
    {
        public ScoreReceivedView()
        {
            InitializeComponent();
        }
    }
}
