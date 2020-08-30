using Caliburn.Micro;
using MiniProject.Helpers;
using MiniProject.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MiniProject.ViewModels
{
    public class HomeViewModel : Conductor<object>
    {
        #region 날씨 변수 영역
        DispatcherTimer weatherTimer;

        // 서울, 부산, 대전, 대구, 광주
        //public string[] CityList = { "Seoul", "Busan", "Daejeon", "Daegu", "Gwangju" };
        int CityCount = 0;

        BindableCollection<WeatherModel> weatherList;
        public BindableCollection<WeatherModel> WeatherList
        {
            get => weatherList;
            set
            {
                weatherList = value;
                NotifyOfPropertyChange(() => WeatherList);
            }
        }

        string weatherImage;
        public string WeatherImage
        {
            get => weatherImage;
            set
            {
                weatherImage = value;
                NotifyOfPropertyChange(() => WeatherImage);
            }
        }

        string weatherText;
        public string WeatherText
        {
            get => weatherText;
            set
            {
                weatherText = value;
                NotifyOfPropertyChange(() => WeatherText);
            }
        }

        string weatherCity;
        public string WeatherCity
        {
            get => weatherCity;
            set
            {
                weatherCity = value;
                NotifyOfPropertyChange(() => WeatherCity);
            }
        }

        string weatherTemp;
        public string WeatherTemp
        {
            get => weatherTemp;
            set
            {
                weatherTemp = value;
                NotifyOfPropertyChange(() => WeatherTemp);
            }
        }

        string weatherAddText;
        public string WeatherAddText
        {
            get => weatherAddText;
            set
            {
                weatherAddText = value;
                NotifyOfPropertyChange(() => WeatherAddText);
            }
        }
        #endregion

        #region 영화 즐겨찾기 변수 영역
        public BindableCollection<MovieModel> movieList;
        public BindableCollection<MovieModel> MovieList
        {
            get => movieList;
            set
            {
                movieList = value;
                NotifyOfPropertyChange(() => MovieList);
            }
        }
        #endregion

        #region 지역 즐겨찾기 변수 영역
        BindableCollection<LocationModel> locationList;
        public BindableCollection<LocationModel> LocationList
        {
            get => locationList;
            set
            {
                locationList = value;
                NotifyOfPropertyChange(() => LocationList);
            }
        }
        #endregion

        #region 생성자 영역
        public HomeViewModel()
        {
            InitCityList();
            InitFavoriteList();
            InitTimer();
            GetWeather();
        }
        #endregion

        #region 초기화 함수
        private void InitCityList()
        {
            using (MySqlConnection conn = new MySqlConnection(Commons.CONNSTR))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(WeatherCitySQL.SELECTQUERY, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                WeatherList = new BindableCollection<WeatherModel>();

                while (reader.Read())
                {
                    WeatherList.Add(new WeatherModel
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        Location = reader["Location"].ToString(),
                        Latitude = float.Parse(reader["Latitude"].ToString()),
                        Longitude = float.Parse(reader["Longitude"].ToString())
                    });
                }
            }
        }

        private void InitFavoriteList()
        {
            using (MySqlConnection conn = new MySqlConnection(Commons.CONNSTR))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(FavoriteMovieSQL.SELECTQUERY, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                MovieList = new BindableCollection<MovieModel>();

                while (reader.Read())
                {
                    MovieList.Add(new MovieModel
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        Title = reader["Title"].ToString(),
                        Link = reader["Link"].ToString(),
                        Image = reader["Image"].ToString(),
                        Subtitle = reader["Subtitle"].ToString(),
                        PubDate = reader["PubDate"].ToString(),
                        Director = reader["Director"].ToString(),
                        Actor = reader["Actor"].ToString(),
                        UserRating = reader["UserRating"].ToString()
                    });
                }
            }

            using (MySqlConnection conn = new MySqlConnection(Commons.CONNSTR))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(FavoriteLocationSQL.SELECTQUERY, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                LocationList = new BindableCollection<LocationModel>();

                while (reader.Read())
                {
                    LocationList.Add(new LocationModel
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        Location = reader["Location"].ToString(),
                        Category_name = reader["Category_name"].ToString(),
                        URL = reader["URL"].ToString(),
                        Address_name = reader["Address_name"].ToString(),
                        Phone = reader["Phone"].ToString()
                    });
                }
            }
        }

        private void InitTimer()
        {
            // 날씨 타이머
            weatherTimer = new DispatcherTimer();
            weatherTimer.Interval = TimeSpan.FromSeconds(5);
            weatherTimer.Tick += weatherTimer_Tick;
            weatherTimer.Start();
        }
        #endregion

        #region 날씨 함수
        private void GetWeather()
        {
            WebClient wc = null;
            wc = new WebClient() { Encoding = Encoding.UTF8 };

            StringBuilder str = new StringBuilder();
            str.Append("http://api.openweathermap.org/data/2.5/weather"); // 기본 URL
            //str.Append($"?q={CityList[CityCount++]}&appid={WeatherAPI.AppID}&units=metric"); // 인증키
            str.Append($"?lat={WeatherList[CityCount].Latitude}&lon={WeatherList[CityCount].Longitude}&appid={WeatherAPI.AppID}&units=metric"); // 인증키

            string json = wc.DownloadString(str.ToString());
            JObject obj = JObject.Parse(json);

            WeatherImage = "http://openweathermap.org/img/wn/" + obj.SelectToken("weather")[0].SelectToken("icon").ToString() + "@2x.png";
            WeatherText = obj.SelectToken("weather")[0].SelectToken("main").ToString();
            WeatherCity = WeatherList[CityCount].Location; // obj.SelectToken("name").ToString();
            WeatherTemp = obj.SelectToken("main.temp").ToString() + "\u00B0C";

            if (CityCount == WeatherList.Count - 1)
                CityCount = 0;
            else
                CityCount++;
        }

        public void WeatherCityAdd()
        {
            var wManager = new WindowManager();
            var result = wManager.ShowDialog(new WeatherCityViewModel());

            InitCityList();
            CityCount = 0;
        }
        #endregion

        #region 타이머 함수
        private void weatherTimer_Tick(object sender, EventArgs e)
        {
            GetWeather();
        }
        #endregion
    }
}
