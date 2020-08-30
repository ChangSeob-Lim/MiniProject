using Caliburn.Micro;
using MiniProject.Helpers;
using MiniProject.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
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

namespace MiniProject.ViewModels
{
    public class WeatherCityAddViewModel : Conductor<object>
    {
        #region Command 변수 영역
        // Enter키 동작 커맨드
        RelayCommand enterCommand;
        public ICommand EnterCommand
        {
            get
            {
                if (enterCommand == null)
                    enterCommand = new RelayCommand(o => BtnSearch(), o => true);

                return enterCommand;
            }
        }
        #endregion

        #region 카카오 API 변수 영역
        // 검색할 단어 or 제목 TextBox
        string txtSearch;
        public string TxtSearch
        {
            get => txtSearch;
            set
            {
                txtSearch = value;
                NotifyOfPropertyChange(() => TxtSearch);
            }
        }
        #endregion

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

        #region 버튼 영역
        public void BtnSearch()
        {
            try
            {
                String SearchText = TxtSearch;
                String SearchURL = "https://dapi.kakao.com/v2/local/search/address.json?query=" + SearchText;

                HttpWebRequest web_request = (HttpWebRequest)WebRequest.Create(SearchURL);

                web_request.Method = "GET";

                web_request.Headers.Add("Authorization", MapAPI.RestAPIKey);

                HttpWebResponse web_response = (HttpWebResponse)web_request.GetResponse();
                Stream stream = web_response.GetResponseStream();
                byte[] buf = new byte[4096];

                string json = string.Empty;

                for (int len = 0; (len = stream.Read(buf, 0, buf.Length)) > 0;)
                {
                    json += Encoding.UTF8.GetString(buf, 0, len);
                }

                JObject obj = JObject.Parse(json);
                JArray items = JArray.Parse(obj.SelectToken("documents").ToString()); // item 안의 값을 배열로 가져오기

                LocationList = new BindableCollection<WeatherModel>();

                foreach (var item in items)
                {
                    LocationList.Add(new WeatherModel
                    {
                        Location = item.SelectToken("address_name").ToString(),
                        Latitude = float.Parse(item.SelectToken("y").ToString()),
                        Longitude = float.Parse(item.SelectToken("x").ToString())
                    });
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        public void AddCity()
        {
            try
            {
                if(SelectedLocation == null)
                {
                    MessageBox.Show("지역을 선택하세요.");
                    return;
                }

                using (MySqlConnection conn = new MySqlConnection(Commons.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(WeatherCitySQL.INSERTQUERY, conn);

                    MySqlParameter paramLocation = new MySqlParameter("@Location", MySqlDbType.VarChar);
                    paramLocation.Value = SelectedLocation.Location;
                    cmd.Parameters.Add(paramLocation);

                    MySqlParameter paramLatitude = new MySqlParameter("@Latitude", MySqlDbType.Float);
                    paramLatitude.Value = SelectedLocation.Latitude;
                    cmd.Parameters.Add(paramLatitude);

                    MySqlParameter paramLongitude = new MySqlParameter("@Longitude", MySqlDbType.Float);
                    paramLongitude.Value = SelectedLocation.Longitude;
                    cmd.Parameters.Add(paramLongitude);
                    
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("추가되었습니다.");

                    TryClose(true);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
