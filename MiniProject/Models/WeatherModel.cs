using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject.Models
{
    public class WeatherModel
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
        /// 위도
        /// </summary>
        public float Latitude { get; set; }
        /// <summary>
        /// 경도
        /// </summary>
        public float Longitude { get; set; }
    }
}
