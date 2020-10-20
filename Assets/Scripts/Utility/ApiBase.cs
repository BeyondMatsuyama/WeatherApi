using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Web
{
    /// <summary>
    /// HTTP 通信基底クラス
    /// </summary>
    public class ApiBase
    {
        /// <summary>
        /// メソッドプロパティ
        /// </summary>
        public enum Method
        {
            POST = 0,
            GET
        }
        private readonly string[] strMethod = { "POST", "GET" };

        /// <summary>
        /// HTTP 通信の結果
        /// </summary>
        public struct Result
        {
            public bool     isSucceeded;    // true で成功
            public string   error;          // 失敗時のエラー内容

            /// <summary>
            /// 成功
            /// </summary>
            public void Suceeded()
            {
                isSucceeded = true;
                error = "";
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="error"></param>
            public void Failed(string error)
            {
                isSucceeded = false;
                this.error = error;
            }
        }

        // ベースURL
        // private const string BaseUrl = "https://localhost:5001/api/Test/";  // 適切なパスを設定してください
        private const string BaseUrl = "https://community-open-weather-map.p.rapidapi.com/";

        // レスポンス（JSON）
        private string resJson;
        // Api 名
        public string ApiName { get; protected set; }
        // HTTP メソッド
        public Method HttpMethod { get; protected set; }

        /// <summary>
        /// リクエスト（オブジェクト）を JSON に変換して HTTP（POST or GET）通信を行う
        /// </summary>
        /// <typeparam name="T">リクエスト型</typeparam>
        /// <param name="request">リクエストのオブジェクト</param>
        /// <param name="cb">コールバック</param>    
        public void Send<T>(ref T request, Action<Result> cb)
        {
            // リクエストオブジェクトを JSON に変換（byte配列）
            string reqJson = JsonUtility.ToJson(request);
            Debug.Log("Send : " + reqJson);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(reqJson);

            // url 生成
            var url = BaseUrl + ApiName;
            // GET のリクエストパラメータ設定
            if (HttpMethod == Method.GET)
            {
                string param = convGetParam(reqJson);
                url += param;
            }
            Debug.Log("url:" + url);

            // Http リクエスト
            CoroutineHandler.StartStaticCoroutine(onSend(url, data, cb));
        }

        /// <summary>
        /// HTTP（POST）通信の実行
        /// </summary>
        /// <param name="url">接続する URL</param>
        /// <param name="data">POST するデータ</param>
        /// <param name="cb">コールバック</param>
        /// <returns>コルーチン</returns>
        private IEnumerator onSend(string url, byte[] data, Action<Result> cb)
        {
            // HTTP（POST）の情報を設定
            var req = new UnityWebRequest(url, strMethod[(int)HttpMethod]);
            if (HttpMethod == Method.POST)
            {
                req.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
            }
            req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            req.SetRequestHeader("x-rapidapi-host", "community-open-weather-map.p.rapidapi.com");
            req.SetRequestHeader("x-rapidapi-key", "xxxxxxxxxxxxxxxx");

            // API 通信（完了待ち）
            yield return req.SendWebRequest();

            // 通信結果
            Result result = new Result();
            if (req.isNetworkError ||
                 req.isHttpError)  // 失敗
            {
                Debug.Log("Network error: " + req.error);
                result.Failed(req.error);
            }
            else                    // 成功
            {
                var res = req.downloadHandler.text;
                Debug.Log("Succeeded: " + res);
                resJson = res;
                result.Suceeded();
            }
            cb(result);
        }

        /// <summary>
        /// レスポンス（JSON）からオブジェクトを生成して返す
        /// </summary>
        /// <typeparam name="T">レスポンス型</typeparam>
        /// <returns>レスポンスのオブジェクト</returns>
        public T Response<T>()
        {
            return JsonUtility.FromJson<T>(resJson);
        }

        /// <summary>
        /// Json 形式のパラメータを GET 通信用に文字列に変換する
        /// </summary>
        /// <param name="json">Json 形式のパラメータ</param>
        /// <returns>パラメータ文字列</returns>
        private string convGetParam(string json)
        {
            string jsonTrim = json.Trim('{', '}');
            jsonTrim = jsonTrim.Replace("\"", "");
            string[] jsonArry = jsonTrim.Split(',');
            string retStr = "?";
            bool isFirst = true;
            foreach (var data in jsonArry)
            {
                if (!data.Contains(":")) break;

                if (!isFirst)
                {
                    retStr += "&";
                }
                else
                {
                    isFirst = false;
                }
                string[] paramAry = data.Split(':');
                retStr += paramAry[0] + "=" + paramAry[1];
            }
            return retStr;
        }
    }
}