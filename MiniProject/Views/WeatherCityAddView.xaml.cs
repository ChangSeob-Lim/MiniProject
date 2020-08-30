using MahApps.Metro.Controls;
using MiniProject.ViewModels;

namespace MiniProject.Views
{
    /// <summary>
    /// WeatherCityAddView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WeatherCityAddView : MetroWindow
    {
        public WeatherCityAddView()
        {
            InitializeComponent();
            DataContext = new WeatherCityAddViewModel();

            TxtSearch.Focus();
        }
    }
}
