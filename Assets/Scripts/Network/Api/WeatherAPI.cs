using System;
using System.Collections.Generic;

namespace Api
{
    /// <summary>
    /// Weather API
    /// </summary>
    public class WeatherAPI : Web.ApiBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WeatherAPI()
        {
            ApiName = "weather";
            HttpMethod = Method.GET;
        }

        /// <summary>
        /// リクエストパラメータ
        /// </summary>
        [Serializable]
        public struct Request
        {
            //public string units;
            //public string mode;
            public string q;
        }
        public Request request;

        /// <summary>
        /// レスポンス詳細
        /// </summary>
        [Serializable]
        public struct Coord
        {
            public float lon;
            public float lat;
        }
        [Serializable]
        public struct Weather
        {
            public int    id;
            public string main;
            public string description;
            public string icon;
        }
        [Serializable]
        public struct Main
        {
            public float temp;
            public float fells_like;
            public float temp_min;
            public float temp_max;
            public int   pressure;
            public int   humidity;
        }
        [Serializable]
        public struct Wind
        {
            public float speed;
            public int   deg;
        }
        [Serializable]
        public struct Rain
        {
            // public float 1h;     // 1h を要素名にできない
        }
        [Serializable]
        public struct Clouds
        {
            public int all;
        }
        [Serializable]
        public struct Sys
        {
            public int    type;
            public int    id;
            public string country;
            public int    sunrise;
            public int    sunset;
        }

        /// <summary>
        /// レスポンスパラメータ
        /// </summary>
        [Serializable]
        public struct Response
        {
            public Coord         coord;
            public List<Weather> weather;
            // public string  base;     // base を要素名にできない
            public Main          main;
            public int           visibility;
            public Wind          wind;
            public Rain          rain;
            public Clouds        clouds;
            public int           dt;
            public Sys           sys;
            public int           timezone;
            public int           id;
            public string        name;
            public int           cod;
        }
        public Response response;

    }
}