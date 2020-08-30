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

namespace MiniProject.ViewModels
{
    public class LocationViewModel : Conductor<object>
    {
        #region 커맨트 속성 변수
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

        #region 지역 API 검색 변수 영역
        // 검색할 단어 TextBox
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

        // 반환 결과 확인
        string txtResult;
        public string TxtResult
        {
            get => txtResult;
            set
            {
                txtResult = value;
                NotifyOfPropertyChange(() => TxtResult);
            }
        }
        #endregion

        #region 지역 변수 영역
        string locationWeb;
        public string LocationWeb
        {
            get => locationWeb;
            set
            {
                locationWeb = value;
                NotifyOfPropertyChange(() => LocationWeb);
            }
        }
        #endregion

        #region 즐겨찾기 변수 영역
        bool IsFavorite;

        string txtFavorite;
        public string TxtFavorite
        {
            get => txtFavorite;
            set
            {
                txtFavorite = value;
                NotifyOfPropertyChange(() => TxtFavorite);
            }
        }
        #endregion

        #region DataGrid 변수 영역
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

        LocationModel selectedLocation;
        public LocationModel SelectedLocation
        {
            get
            {
                return selectedLocation;
            }
            set
            {
                selectedLocation = value;
                if(selectedLocation != null)
                    LocationWeb = value.URL;

                NotifyOfPropertyChange(() => SelectedLocation);
                NotifyOfPropertyChange(() => LocationWeb);
                NotifyOfPropertyChange(() => CanShowPhone);
                NotifyOfPropertyChange(() => CanBtnFavorite);
            }
        }
        #endregion

        #region 생성자 영역
        public LocationViewModel()
        {
            IsFavorite = true;
            TxtFavorite = "삭제";

            FavoritesLocation();
        }
        #endregion

        #region 함수 영역
        private void FavoriteInsert()
        {
            try
            {
                if (selectedLocation == null)
                {
                    MessageBox.Show("장소를 선택하세요.");
                    return;
                }

                using (MySqlConnection conn = new MySqlConnection(Commons.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(FavoriteLocationSQL.INSERTQUERY, conn);

                    MySqlParameter paramLocation = new MySqlParameter("@Location", MySqlDbType.VarChar);
                    paramLocation.Value = SelectedLocation.Location;
                    cmd.Parameters.Add(paramLocation);

                    MySqlParameter paramCategory_name = new MySqlParameter("@Category_name", MySqlDbType.VarChar);
                    paramCategory_name.Value = SelectedLocation.Category_name;
                    cmd.Parameters.Add(paramCategory_name);

                    MySqlParameter paramURL = new MySqlParameter("@URL", MySqlDbType.VarChar);
                    paramURL.Value = SelectedLocation.URL;
                    cmd.Parameters.Add(paramURL);

                    MySqlParameter paramAddress_name = new MySqlParameter("@Address_name", MySqlDbType.VarChar);
                    paramAddress_name.Value = SelectedLocation.Address_name;
                    cmd.Parameters.Add(paramAddress_name);

                    MySqlParameter paramPhone = new MySqlParameter("@Phone", MySqlDbType.VarChar);
                    paramPhone.Value = SelectedLocation.Phone;
                    cmd.Parameters.Add(paramPhone);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("추가되었습니다.");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void FavoriteDelete()
        {
            try
            {
                if (selectedLocation == null)
                {
                    MessageBox.Show("장소를 선택하세요.");
                    return;
                }

                using (MySqlConnection conn = new MySqlConnection(Commons.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(FavoriteLocationSQL.DELETEQUERY, conn);

                    MySqlParameter paramId = new MySqlParameter("@Id", MySqlDbType.Int32);
                    paramId.Value = selectedLocation.Id;
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
        public void BtnSearch()
        {
            IsFavorite = false;
            TxtFavorite = "추가";

            try
            {
                String SearchText = TxtSearch;
                String SearchURL = "https://dapi.kakao.com/v2/local/search/keyword.json?query=" + SearchText;

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

                LocationList = new BindableCollection<LocationModel>();

                foreach (var item in items)
                {
                    LocationList.Add(new LocationModel
                    {
                        Location = item.SelectToken("place_name").ToString(),
                        Category_name = item.SelectToken("category_name").ToString(),
                        URL = item.SelectToken("place_url").ToString(),
                        Address_name = item.SelectToken("address_name").ToString(),
                        Phone = item.SelectToken("phone").ToString()
                    });
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void FavoritesLocation()
        {
            IsFavorite = true;
            TxtFavorite = "삭제";

            try
            {
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
                            Category_name = reader["category_name"].ToString(),
                            Location = reader["Location"].ToString(),
                            URL = reader["URL"].ToString(),
                            Address_name = reader["Address_name"].ToString(),
                            Phone = reader["Phone"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error {ex.Message}");
            }
        }

        public void BtnFavorite()
        {
            if (selectedLocation == null)
            {
                MessageBox.Show("장소를 선택해주세요.");
                return;
            }

            if (IsFavorite)
            {
                FavoriteDelete();
                FavoritesLocation();
                selectedLocation = null;
            }
            else
            {
                FavoriteInsert();
            }
        }

        public bool CanBtnFavorite
        {
            get => (selectedLocation != null);
        }

        public void ShowPhone()
        {
            if (SelectedLocation != null)
                MessageBox.Show(SelectedLocation.Phone);

            else
                MessageBox.Show("장소를 선택해주세요.");
        }

        public bool CanShowPhone
        {
            get => (SelectedLocation != null);
        }
        #endregion
    }
}
