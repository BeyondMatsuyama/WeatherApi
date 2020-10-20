using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApiSample : MonoBehaviour
{
    [SerializeField] private Button btnWeather;
    [SerializeField] private Text   txtInfo;
    [SerializeField] private Text   txtCity;
    [SerializeField] private Image  img;
    [SerializeField] private Text   txtGps;
    [SerializeField] private Text   txtLon;
    [SerializeField] private Text   txtLat;

    private void Start()
    {
        // GPS 開始
        Input.location.Start();
        Input.compass.enabled = true;
        StartCoroutine(gpsStatus());

        // ボタンが押されたら天気情報を取得する
        btnWeather.onClick.AddListener(() =>
        {
            if (isActiveLocation())
            {
                // 緯度経度から都市を検索
                City.Info info = City.NearbyCity(Input.location.lastData.longitude, Input.location.lastData.latitude);
                txtCity.text = info.city;
                txtLon.text = string.Format("{0}", Input.location.lastData.longitude);
                txtLat.text = string.Format("{0}", Input.location.lastData.latitude);

                // 天気API
                sendWeatherApi(info.name);
            }
            // sendWeatherApi("Osaka");
        });
    }

    // GPS 状態を監視
    private IEnumerator gpsStatus()
    {
        while (true)
        {
            // 状態を表示
            txtGps.text = string.Format("enable by user: {0}, service status: {1}", Input.location.isEnabledByUser, Input.location.status);
            yield return new WaitForSeconds(2f);
        }
    }

    /// <summary>
    /// アプリ終了時の処理
    /// </summary>
    void OnApplicationQuit()
    {
        // GPS 停止
        Input.compass.enabled = false;
        Input.location.Stop();
    }

    /// <summary>
    /// GPS が有効か判定
    /// </summary>
    /// <returns></returns>
    private bool isActiveLocation()
    {
        // GPS がアクティブでなければ処理しない
        if (Input.location.isEnabledByUser)
        {
            switch(Input.location.status)
            {
                case LocationServiceStatus.Initializing:
                    break;
                case LocationServiceStatus.Stopped:
                    Input.location.Start();
                    break;
                case LocationServiceStatus.Running:
                    return true;
                case LocationServiceStatus.Failed:
                    break;
            }
        }
        return false;
    }

    /// <summary>
    /// Weaher API 通信
    /// </summary>
    private void sendWeatherApi(string cityName)
    {
        Api.WeatherAPI api = Web.Network.Instance.GetInstance<Api.WeatherAPI>(Web.Network.ApiType.Weather);

        // リクエストパラメータを設定
        // api.request.units = "metric";
        // api.request.mode = "JSON";
        api.request.q = cityName;

        // 通信
        api.Send(ref api.request, result => {

            // リザルト
            if(result.isSucceeded)  // 成功
            {
                // レスポンスを展開
                api.response = api.Response<Api.WeatherAPI.Response>();

                // 内容確認
                Debug.Log("Weather API Succeed!!");
                log(api.response);

                // 天気アイコン
                Api.WeatherAPI.Weather info = api.response.weather[0];
                changeIcon(info.icon);

                // 天気情報
                txtInfo.text = string.Format("id:{0}, main:{1}, description:{2}", info.id, info.main, info.description);
            }
            else                    // 失敗
            {
                Debug.Log("Weather API Failed : " + result.error);
            }
        });
    }

    /// <summary>
    /// 天気スプライト更新
    /// </summary>
    /// <param name="icon">アイコン名</param>
    private void changeIcon(string icon)
    {
        img.sprite = Resources.Load<Sprite>(string.Format("weather_icons/{0}_2x", icon));
    }


    /// <summary>
    /// WEATHER API ログ出力
    /// </summary>
    /// <param name="res">weather api のレスポンス</param>
    private void log(Api.WeatherAPI.Response res)
    {
        Debug.Log(string.Format("coord:(lon:{0}, lat{1})", res.coord.lon, res.coord.lat));
        foreach (Api.WeatherAPI.Weather we in res.weather)
        {
            Debug.Log(string.Format("weather:(id:{0}, main:{1}, description:{2}, icon:{3})",
                    we.id, we.main, we.description, we.icon
                ));
        }
        Debug.Log(string.Format("main:(temp:{0}, feels_like:{1}, temp_min:{2}, temp_max:{3}, pressure:{4}, humidity:{5})",
                res.main.temp,
                res.main.fells_like,
                res.main.temp_min,
                res.main.temp_max,
                res.main.pressure,
                res.main.humidity
            ));
        Debug.Log("visibility:" + res.visibility);
        Debug.Log(string.Format("wind:(speed:{0}, deg:{1})", res.wind.speed, res.wind.deg));
        Debug.Log(string.Format("clouds:(all:{0})", res.clouds.all));
        Debug.Log("dt:" + res.dt);
        Debug.Log(string.Format("sys:(type:{0}, id:{1}, country:{2}, sunrise:{3}, sunset:{4})",
                res.sys.type,
                res.sys.id,
                res.sys.country,
                res.sys.sunrise,
                res.sys.sunset
            ));
        Debug.Log("timezone:" + res.timezone);
        Debug.Log("id:" + res.id);
        Debug.Log("name:" + res.name);
        Debug.Log("cod:" + res.cod);
    }

}
