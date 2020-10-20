using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Web
{

    public class Network : SingletonMonoBehaviour<Network>
    {
        /// <summary>
        /// API タイプ
        /// </summary>
        public enum ApiType {
            Weather = 0,
        };

        /// <summary>
        /// API インスタンステーブル
        /// </summary>
        private Hashtable apiInstances = new Hashtable();

        /// <summary>
        /// API インスタンスの取得
        /// </summary>
        /// <param name="type">API タイプ</param>
        /// <returns>API インスタンス</returns>
        public T GetInstance<T>(ApiType type) where T : new()
        {
            if(!apiInstances.ContainsKey(type))
            {
                // Debug.Log("create instance : " + type);
                apiInstances[type] = new T();
            }
            return (T)apiInstances[type];
        }



    }
}