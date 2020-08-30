using Caliburn.Micro;
using MiniProject.Helpers;
using MiniProject.Models;
using MySql.Data.MySqlClient;
using System;
using System.Windows;

namespace MiniProject.ViewModels
{
    public class WeatherCityViewModel : Conductor<object>
    {
        #region DataGrid 변수 영역
        BindableCollection<WeatherModel> locationList;
        public BindableCollection<WeatherModel> LocationList
        {
            get => locationList;
            set
            {
                locationList = value;
                NotifyOfPropertyChange(() => LocationList);
            }
        }

        WeatherModel selectedLocation;
        public WeatherModel SelectedLocation
        {
            get
            {
                return selectedLocation;
            }
            set
            {
                selectedLocation = value;
                NotifyOfPropertyChange(() => SelectedLocation);
            }
        }
        #endregion

        public WeatherCityViewModel()
        {
            InitList();
        }

        #region 함수 영역
        private void InitList()
        {
            using(MySqlConnection conn = new MySqlConnection(Commons.CONNSTR))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(WeatherCitySQL.SELECTQUERY, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                LocationList = new BindableCollection<WeatherModel>();

                while (reader.Read())
                {
                    LocationList.Add(new WeatherModel
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        Location = reader["Location"].ToString()
                    });
                }
            }
        }

        private void DeleteDB()
        {
            try
            {
                if (SelectedLocation == null)
                {
                    MessageBox.Show("지역을 선택하세요.");
                    return;
                }

                using (MySqlConnection conn = new MySqlConnection(Commons.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(WeatherCitySQL.DELETEQUERY, conn);

                    MySqlParameter paramId = new MySqlParameter("@Id", MySqlDbType.Int32);
                    paramId.Value = SelectedLocation.Id;
                    cmd.Parameters.Add(paramId);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("삭제되었습니다.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error {ex.Message}");
            }
        }
        #endregion

        #region 버튼 영역
        public void AddCity()
        {
            var wManager = new WindowManager();
            var result = wManager.ShowDialog(new WeatherCityAddViewModel());

            if (result == true)
                InitList();
        }

        public void DeleteCity()
        {
            if (SelectedLocation == null)
            {
                MessageBox.Show("지역을 선택하세요.");
                return;
            }

            if (MessageBoxResult.OK == MessageBox.Show("삭제하시겠습니까?", "삭제", MessageBoxButton.OKCancel))
            {
                DeleteDB();
                InitList();
            }
        }
        #endregion
    }
}
