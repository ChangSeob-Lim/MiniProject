using MahApps.Metro.Controls;
using MiniProject.ViewModels;

namespace MiniProject.Views
{
    /// <summary>
    /// WeatherCityView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WeatherCityView : MetroWindow
    {
        public WeatherCityView()
        {
            InitializeComponent();
            DataContext = new WeatherCityViewModel();
        }
    }
}
