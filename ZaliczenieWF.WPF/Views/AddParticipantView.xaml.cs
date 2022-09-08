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

namespace ZaliczenieWF.WPF.Views
{
    /// <summary>
    /// Interaction logic for AddParticipantView.xaml
    /// </summary>
    [MvxWindowPresentation(Identifier = nameof(AddParticipantView), Modal = true)]
    public partial class AddParticipantView
    {
        public AddParticipantView()
        {
            InitializeComponent();
        }
    }
}
