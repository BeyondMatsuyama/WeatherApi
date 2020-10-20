using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    private const double Lat2Km = 111.319491;   // 緯度（経度）１度の距離（km）

    public struct Info
    {
        public string city;
        public string name;
    }

    /// <summary>
    /// 都市データ
    /// </summary>
    private struct Data
    {
        public string prefecture;   // 県
        public string city;         // 市
        public string name;         // API に渡す都市名
        public double lon;          // 経度
        public double lat;          // 緯度
    }
    static private List<Data> datas = new List<Data>
    {
        new Data { prefecture = "北海道", city = "稚内市", name = "wakkanai", lon = 141.673889, lat = 45.409439 },
        new Data { prefecture = "北海道", city = "旭川市", name = "asahikawa", lon = 142.370285, lat = 43.76778 },
        new Data { prefecture = "北海道", city = "釧路市", name = "kushiro", lon = 144.374725, lat = 42.974998 },
        new Data { prefecture = "北海道", city = "札幌市", name = "sapporo", lon = 141.346939, lat = 43.064171 },
        new Data { prefecture = "北海道", city = "函館市", name = "hakodate", lon = 140.736664, lat = 41.775829 },
        new Data { prefecture = "青森県", city = "青森市", name = "aomori", lon = 140.740005, lat = 40.82444 },
        new Data { prefecture = "岩手県", city = "盛岡市", name = "morioka", lon = 141.152496, lat = 39.703609 },
        new Data { prefecture = "宮城県", city = "仙台市", name = "sendai", lon = 140.871933, lat = 38.26889 },
        new Data { prefecture = "秋田県", city = "秋田市", name = "akita", lon = 140.116669, lat = 39.716671 },
        new Data { prefecture = "山形県", city = "山形市", name = "yamagata", lon = 139.821671, lat = 38.721668 },
        new Data { prefecture = "福島県", city = "福島市", name = "fukushima", lon = 140.383331, lat = 37.400002 },
        new Data { prefecture = "茨城県", city = "水戸市", name = "mito", lon = 140.446671, lat = 36.341389 },
        new Data { prefecture = "栃木県", city = "宇都宮市", name = "utsunomiya", lon = 139.883606, lat = 36.56583 },
        new Data { prefecture = "群馬県", city = "前橋市", name = "maebashi", lon = 139.060837, lat = 36.391109 },
        new Data { prefecture = "埼玉県", city = "さいたま市", name = "saitama", lon = 139.485275, lat = 35.908611 },
        new Data { prefecture = "千葉県", city = "千葉市", name = "chiba", lon = 140.123337, lat = 35.604721 },
        new Data { prefecture = "東京都", city = "新宿区", name = "shinjuku", lon = 139.691711, lat = 35.689499 },
        new Data { prefecture = "東京都", city = "八丈町", name = "hachijo", lon = 139.783325, lat = 33.099998 },
        new Data { prefecture = "神奈川県", city = "横浜市", name = "yokohama", lon = 139.642502, lat = 35.447781 },
        new Data { prefecture = "新潟県", city = "新潟市", name = "niigata", lon = 139.023605, lat = 37.902222 },
        new Data { prefecture = "富山県", city = "富山市", name = "toyama", lon = 137.211395, lat = 36.695278 },
        new Data { prefecture = "石川県", city = "金沢市", name = "kanazawa", lon = 136.625565, lat = 36.59444 },
        new Data { prefecture = "福井県", city = "福井市", name = "fukui", lon = 136.222565, lat = 36.06443 },
        new Data { prefecture = "山梨県", city = "甲府市", name = "kofu", lon = 138.568329, lat = 35.663891 },
        new Data { prefecture = "長野県", city = "長野市", name = "nagano", lon = 138.181107, lat = 36.65139 },
        new Data { prefecture = "岐阜県", city = "岐阜市", name = "gifu", lon = 136.760391, lat = 35.422909 },
        new Data { prefecture = "静岡県", city = "静岡市", name = "shizuoka", lon = 138.383057, lat = 34.97694 },
        new Data { prefecture = "愛知県", city = "名古屋市", name = "nagoya", lon = 136.906403, lat = 35.181469 },
        new Data { prefecture = "三重県", city = "津市", name = "tsu", lon = 136.508606, lat = 34.730282 },
        new Data { prefecture = "滋賀県", city = "大津市", name = "otsu", lon = 135.868332, lat = 35.00444 },
        new Data { prefecture = "京都府", city = "京都市", name = "kyouto", lon = 135.753845, lat = 35.021069 },
        new Data { prefecture = "大阪府", city = "大阪市", name = "osaka", lon = 135.502182, lat = 34.693741 },
        new Data { prefecture = "兵庫県", city = "神戸市", name = "koube", lon = 135.182999, lat = 34.691299 },
        new Data { prefecture = "奈良県", city = "奈良市", name = "nara", lon = 135.804855, lat = 34.685051 },
        new Data { prefecture = "和歌山県", city = "和歌山市", name = "wakayama", lon = 135.167496, lat = 34.226109 },
        new Data { prefecture = "鳥取県", city = "鳥取市", name = "tottori", lon = 134.233337, lat = 35.5 },
        new Data { prefecture = "島根県", city = "松江市", name = "matsue", lon = 133.050568, lat = 35.472221 },
        new Data { prefecture = "岡山県", city = "岡山市", name = "okayama", lon = 133.934998, lat = 34.661671 },
        new Data { prefecture = "広島県", city = "広島市", name = "hiroshima", lon = 132.459366, lat = 34.396271 },
        new Data { prefecture = "山口県", city = "山口市", name = "yamaguchi", lon = 131.47139, lat = 34.185829 },
        new Data { prefecture = "徳島県", city = "徳島市", name = "tokushima", lon = 134.559433, lat = 34.06583 },
        new Data { prefecture = "香川県", city = "高松市", name = "takamatsu", lon = 134.043335, lat = 34.340279 },
        new Data { prefecture = "愛媛県", city = "松山市", name = "matsuyama", lon = 132.765747, lat = 33.839161 },
        new Data { prefecture = "高知県", city = "高知市", name = "kochi", lon = 133.531113, lat = 33.559719 },
        new Data { prefecture = "福岡県", city = "福岡市", name = "fukuoka", lon = 130.41806, lat = 33.606392 },
        new Data { prefecture = "佐賀県", city = "佐賀市", name = "saga", lon = 130.298798, lat = 33.249321 },
        new Data { prefecture = "長崎県", city = "長崎市", name = "nagasaki", lon = 129.873611, lat = 32.74472 },
        new Data { prefecture = "熊本県", city = "熊本市", name = "kumamoto", lon = 130.741669, lat = 32.789719 },
        new Data { prefecture = "大分県", city = "大分市", name = "oita", lon = 131.612503, lat = 33.23806 },
        new Data { prefecture = "宮崎県", city = "宮崎市", name = "miyazaki", lon = 131.423889, lat = 31.91111 },
        new Data { prefecture = "鹿児島県", city = "鹿児島市", name = "kagoshima", lon = 130.558136, lat = 31.560181 },
        new Data { prefecture = "沖縄県", city = "那覇市", name = "naha", lon = 127.681107, lat = 26.2125 },
        new Data { prefecture = "沖縄県", city = "石垣市", name = "isigaki", lon = 124.157173, lat = 24.34478 }
    };

    /// <summary>
    /// 緯度・経度から最も近い都市を検索
    /// </summary>
    /// <param name="lon">経度</param>
    /// <param name="lat">緯度</param>
    /// <returns></returns>
    static public Info NearbyCity(double lon, double lat)
    {
        Info info = new Info();
        float dist = 9999f;
        foreach(Data dat in datas)
        {
            double z = (dat.lat - lat) * Lat2Km;    // -z が南
            double x = (dat.lon - lon) * Lat2Km;    // +x が東
            Vector3 v = new Vector3((float)x, 0, (float)z);
            // Debug.Log(string.Format(">> Search City:{0}, dist:{1}km", dat.prefecture+dat.city, v.magnitude));
            if(v.magnitude < dist)
            {
                dist = v.magnitude;
                info.city = dat.prefecture + dat.city;
                info.name = dat.name;
                // Debug.Log(string.Format(">> Nearby City:{0}", dat.name));
            }
        }
        return info;
    }


}
