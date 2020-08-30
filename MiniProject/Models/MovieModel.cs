using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject.Models
{
    public class MovieModel
    {
        /// <summary>
        /// 순번
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 제목
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 링크
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// 이미지
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// 영문 제목
        /// </summary>
        public string Subtitle { get; set; }
        /// <summary>
        /// 개봉일
        /// </summary>
        public string PubDate { get; set; }
        /// <summary>
        /// 감독명
        /// </summary>
        public string Director { get; set; }
        /// <summary>
        /// 배우
        /// </summary>
        public string Actor { get; set; }
        /// <summary>
        /// 평점
        /// </summary>
        public string UserRating { get; set; }
    }
}
