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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using ZaliczenieWF.Core.ViewModels.Main;

namespace ZaliczenieWF.WPF.Views
{
    /// <summary>
    /// Interaction logic for _10x10View.xaml
    /// </summary>

    [MvxContentPresentation]
    [MvxViewFor(typeof(MainViewModel))]
    public partial class ParticipantsWindow : MvxWpfView
    {
        public ParticipantsWindow()
        {
            InitializeComponent();
        }

        private void SfDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Dupa");
        }
    }
}
