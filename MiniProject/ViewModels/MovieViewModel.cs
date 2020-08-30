using Caliburn.Micro;
using MiniProject.Helpers;
using MiniProject.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MiniProject.ViewModels
{
    public class MovieViewModel : Conductor<object>
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

        #region 영화 API 검색 변수 영역
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

        #region 영화 변수 영역
        int movieId;
        public int MovieId
        {
            get => movieId;
            set
            {
                movieId = value;
                NotifyOfPropertyChange(() => MovieId);
            }
        }

        string movieTitle;
        public string MovieTitle
        {
            get => movieTitle;
            set
            {
                movieTitle = value;
                NotifyOfPropertyChange(() => MovieTitle);
            }
        }

        string movieLink;
        public string MovieLink
        {
            get => movieLink;
            set
            {
                movieLink = value;
                NotifyOfPropertyChange(() => MovieLink);
                NotifyOfPropertyChange(() => CanShowMore);
            }
        }

        string movieImage;
        public string MovieImage
        {
            get => movieImage;
            set
            {
                movieImage = value;
                NotifyOfPropertyChange(() => MovieImage);
            }
        }

        string movieSubtitle;
        public string MovieSubtitle
        {
            get => movieSubtitle;
            set
            {
                movieSubtitle = "(" + value + ")";
                NotifyOfPropertyChange(() => MovieSubtitle);
            }
        }

        string moviePubDate;
        public string MoviePubDate
        {
            get => moviePubDate;
            set
            {
                moviePubDate = value;
                NotifyOfPropertyChange(() => MoviePubDate);
            }
        }

        string movieDirector;
        public string MovieDirector
        {
            get => movieDirector;
            set
            {
                movieDirector = value;
                NotifyOfPropertyChange(() => MovieDirector);
            }
        }

        string movieActor;
        public string MovieActor
        {
            get => movieActor;
            set
            {
                movieActor = value;
                NotifyOfPropertyChange(() => MovieActor);
            }
        }

        string movieUserRating;
        public string MovieUserRating
        {
            get => movieUserRating;
            set
            {
                movieUserRating = value;
                RatingBar = float.Parse(value);
                NotifyOfPropertyChange(() => MovieUserRating);
                NotifyOfPropertyChange(() => RatingBar);
            }
        }

        float ratingBar;
        public float RatingBar
        {
            get => ratingBar;
            set
            {
                ratingBar = value;
                NotifyOfPropertyChange(() => RatingBar);
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
        // 전체 영화 리스트
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
        // 리스트 중 선택된 하나의 MovieModel
        MovieModel selectedMovie;
        public MovieModel SelectedMovie
        {
            get
            {
                return selectedMovie;
            }
            set
            {
                selectedMovie = value;

                if (value != null) // value가 null 값이라면 값이 없으므로 널 참조
                {
                    MovieTitle = value.Title;
                    MovieLink = value.Link;
                    MovieImage = value.Image;
                    MovieSubtitle = value.Subtitle;
                    MoviePubDate = value.PubDate;
                    MovieDirector = value.Director;
                    MovieActor = value.Actor;
                    MovieUserRating = value.UserRating;
                }
                else
                {
                    MovieTitle = null;
                    MovieLink = null;
                    MovieImage = null;
                    MovieSubtitle = null;
                    MoviePubDate = null;
                    MovieDirector = null;
                    MovieActor = null;
                    MovieUserRating = null;
                }

                NotifyOfPropertyChange(() => SelectedMovie);
                NotifyOfPropertyChange(() => CanBtnFavorite);
            }
        }
        #endregion

        #region 생성자 영역
        public MovieViewModel()
        {
            IsFavorite = true;
            TxtFavorite = "삭제";

            FavoritesMovie();
        }
        #endregion

        #region 함수 영역
        private void FavoriteInsert()
        {
            try
            {
                if (SelectedMovie == null)
                {
                    MessageBox.Show("영화를 선택하세요.");
                    return;
                }

                using (MySqlConnection conn = new MySqlConnection(Commons.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(FavoriteMovieSQL.INSERTQUERY, conn);

                    MySqlParameter paramTitle = new MySqlParameter("@Title", MySqlDbType.VarChar);
                    paramTitle.Value = SelectedMovie.Title;
                    cmd.Parameters.Add(paramTitle);

                    MySqlParameter paramLink = new MySqlParameter("@Link", MySqlDbType.VarChar);
                    paramLink.Value = SelectedMovie.Link;
                    cmd.Parameters.Add(paramLink);

                    MySqlParameter paramImage = new MySqlParameter("@Image", MySqlDbType.VarChar);
                    paramImage.Value = SelectedMovie.Image;
                    cmd.Parameters.Add(paramImage);

                    MySqlParameter paramSubtitle = new MySqlParameter("@Subtitle", MySqlDbType.VarChar);
                    paramSubtitle.Value = SelectedMovie.Subtitle;
                    cmd.Parameters.Add(paramSubtitle);

                    MySqlParameter paramPubDate = new MySqlParameter("@PubDate", MySqlDbType.VarChar);
                    paramPubDate.Value = SelectedMovie.PubDate;
                    cmd.Parameters.Add(paramPubDate);

                    MySqlParameter paramDirector = new MySqlParameter("@Director", MySqlDbType.VarChar);
                    paramDirector.Value = SelectedMovie.Director;
                    cmd.Parameters.Add(paramDirector);

                    MySqlParameter paramActor = new MySqlParameter("@Actor", MySqlDbType.VarChar);
                    paramActor.Value = SelectedMovie.Actor;
                    cmd.Parameters.Add(paramActor);

                    MySqlParameter paramUserRating = new MySqlParameter("@UserRating", MySqlDbType.VarChar);
                    paramUserRating.Value = SelectedMovie.UserRating;
                    cmd.Parameters.Add(paramUserRating);

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
                if (SelectedMovie == null)
                {
                    MessageBox.Show("영화를 선택하세요.");
                    return;
                }

                using (MySqlConnection conn = new MySqlConnection(Commons.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(FavoriteMovieSQL.DELETEQUERY, conn);

                    MySqlParameter paramId = new MySqlParameter("@Id", MySqlDbType.Int32);
                    paramId.Value = SelectedMovie.Id;
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
                String SearchURL = "https://openapi.naver.com/v1/search/movie.json?query=" + SearchText;

                HttpWebRequest web_request = (HttpWebRequest)WebRequest.Create(SearchURL);

                web_request.Method = "GET";

                web_request.Headers.Add("X-Naver-Client-Id", MovieAPI.ClientID);
                web_request.Headers.Add("X-Naver-Client-Secret", MovieAPI.ClientSecret);

                HttpWebResponse web_response = (HttpWebResponse)web_request.GetResponse();
                Stream stream = web_response.GetResponseStream();
                byte[] buf = new byte[4096];

                string json = string.Empty;

                for (int len = 0; (len = stream.Read(buf, 0, buf.Length)) > 0;)
                {
                    TxtResult += Encoding.UTF8.GetString(buf, 0, len) + Environment.NewLine;
                    json += Encoding.UTF8.GetString(buf, 0, len);
                }

                json = json.Replace("<b>", string.Empty);
                json = json.Replace("</b>", string.Empty);
                json = json.Replace("&nbsp;", string.Empty);

                JObject obj = JObject.Parse(json);
                JArray items = JArray.Parse(obj.SelectToken("items").ToString()); // item 안의 값을 배열로 가져오기

                try
                {
                    MovieList = new BindableCollection<MovieModel>();
                    foreach (var item in items)
                    {
                        MovieList.Add(new MovieModel
                        {
                            Title = item.SelectToken("title").ToString() == null ? string.Empty : item.SelectToken("title").ToString(),
                            Link = item.SelectToken("link").ToString() == null ? string.Empty : item.SelectToken("link").ToString(),
                            Image = item.SelectToken("image").ToString() == null ? string.Empty : item.SelectToken("image").ToString(),
                            Subtitle = item.SelectToken("subtitle").ToString() == null ? string.Empty : item.SelectToken("subtitle").ToString(),
                            PubDate = item.SelectToken("pubDate").ToString() == null ? string.Empty : item.SelectToken("pubDate").ToString(),
                            Director = item.SelectToken("director").ToString() == null ? string.Empty : item.SelectToken("director").ToString().Replace("|", " "),
                            Actor = item.SelectToken("actor").ToString() == null ? string.Empty : item.SelectToken("actor").ToString().Replace("|", " "),
                            UserRating = item.SelectToken("userRating").ToString() == null ? string.Empty : item.SelectToken("userRating").ToString()
                        });
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void FavoritesMovie()
        {
            IsFavorite = true;
            TxtFavorite = "삭제";

            try
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error {ex.Message}");
            }
        }

        public void BtnFavorite()
        {
            if (SelectedMovie == null)
            {
                MessageBox.Show("영화를 선택해주세요.");
                return;
            }

            if (IsFavorite)
            {
                FavoriteDelete();
                FavoritesMovie();
                SelectedMovie = null;
            }
            else
            {
                FavoriteInsert();
            }
        }

        public bool CanBtnFavorite
        {
            get => (SelectedMovie != null);
        }

        public void ShowMore()
        {
            if (MovieLink != null)
                System.Diagnostics.Process.Start(MovieLink);

            else
                MessageBox.Show("영화를 선택해주세요.");
        }

        public bool CanShowMore
        {
            get => (MovieLink != null);
        }
        #endregion
    }
}
