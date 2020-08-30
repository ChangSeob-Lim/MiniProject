using MiniProject.ViewModels;
using System.Windows.Controls;

namespace MiniProject.Views
{
    /// <summary>
    /// MovieView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MovieView : UserControl
    {
        public MovieView()
        {
            InitializeComponent();
            DataContext = new MovieViewModel();
        }
    }
}
