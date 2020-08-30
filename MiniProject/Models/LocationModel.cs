using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject.Models
{
    public class LocationModel
    {
        /// <summary>
        /// 순번
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 지역명
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 카테고리
        /// </summary>
        public string Category_name { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// 주소
        /// </summary>
        public string Address_name { get; set; }
        /// <summary>
        /// 전화번호
        /// </summary>
        public string Phone { get; set; }
    }
}
