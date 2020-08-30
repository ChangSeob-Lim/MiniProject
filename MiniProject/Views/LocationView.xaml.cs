using MiniProject.ViewModels;
using System.Windows.Controls;

namespace MiniProject.Views
{
    /// <summary>
    /// KeywordSearchView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LocationView : UserControl
    {
        public LocationView()
        {
            InitializeComponent();
            DataContext = new LocationViewModel();
        }
    }
}
