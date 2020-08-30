using Caliburn.Micro;
using MiniProject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MiniProject.ViewModels
{
    public class MainViewModel : Conductor<object>
    {
        #region Command 변수 영역
        Visibility openMenuVisibility;
        public Visibility OpenMenuVisibility
        {
            get => openMenuVisibility;
            set
            {
                openMenuVisibility = value;
                NotifyOfPropertyChange(() => OpenMenuVisibility);
            }
        }

        Visibility closeMenuVisibility;
        public Visibility CloseMenuVisibility
        {
            get => closeMenuVisibility;
            set
            {
                closeMenuVisibility = value;
                NotifyOfPropertyChange(() => CloseMenuVisibility);
            }
        }

        public RelayCommand ExitCommand { get; set; }
        #endregion

        #region 네비게이션 변수 영역
        BindableCollection<ListViewItem> listViewMenu;
        public BindableCollection<ListViewItem> ListViewMenu
        {
            get => listViewMenu;
            set
            {
                listViewMenu = value;
                NotifyOfPropertyChange(() => ListViewMenu);
            }
        }

        ListViewItem selectedListView;
        public ListViewItem SelectedListView
        {
            get
            {
                return selectedListView;
            }
            set
            {
                selectedListView = value;

                switch (selectedListView.Name)
                {
                    case "HomeTab":
                        ActivateItem(new HomeViewModel());
                        break;
                    case "MovieTab":
                        ActivateItem(new MovieViewModel());
                        break;
                    case "LocationTab":
                        ActivateItem(new LocationViewModel());
                        break;
                    default:
                        break;
                }
                NotifyOfPropertyChange(() => SelectedListView);
            }
        }
        #endregion

        #region 시계 영역
        DispatcherTimer clockTimer;

        string clock;
        public string Clock
        {
            get => clock;
            set
            {
                clock = value;
                NotifyOfPropertyChange(() => Clock);
            }
        }
        #endregion

        public MainViewModel()
        {
            InitCommand();
            InitTimer();
            Clock = DateTime.Now.ToString();
            ActivateItem(new HomeViewModel());

            OpenMenuVisibility = Visibility.Visible;
            CloseMenuVisibility = Visibility.Collapsed;
        }

        private void InitCommand()
        {
            ExitCommand = new RelayCommand(o => EndThis());
        }

        #region 초기화 함수
        private void InitTimer()
        {
            // 시계 타이머
            clockTimer = new DispatcherTimer();
            clockTimer.Interval = TimeSpan.FromSeconds(1);
            clockTimer.Tick += clockTimer_Tick;
            clockTimer.Start();
        }
        #endregion

        #region 타이머 함수
        private void clockTimer_Tick(object sender, EventArgs e)
        {
            Clock = DateTime.Now.ToString();
        }
        #endregion

        #region 버튼 이벤트 영역
        public void OpenMenu(object sender, RoutedEventArgs e)
        {
            CloseMenuVisibility = Visibility.Visible;
            OpenMenuVisibility = Visibility.Collapsed;
        }

        public void CloseMenu(object sender, RoutedEventArgs e)
        {
            CloseMenuVisibility = Visibility.Collapsed;
            OpenMenuVisibility = Visibility.Visible;
        }

        public void EndThis()
        {
            Environment.Exit(0);
        }
        #endregion
    }
}
