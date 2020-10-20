using UnityEngine;

/// <summary>
/// シングルトン
/// </summary>
/// <typeparam name="T">ジェネリック</typeparam>
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool shutdown = false;
    private static T m_instance  = null;
    public  static T Instance {
        get {
            if(shutdown)
            {
                return null;
            }
            else {
                if (m_instance == null)
                {
                    var singletonObject = new GameObject();
                    m_instance = singletonObject.AddComponent<T>();
                    m_instance.name = typeof(T).ToString() + " (Singleton)";
                    DontDestroyOnLoad(m_instance);
                }
            }
            return m_instance;
        }
    }

    void OnDestroy() {
        shutdown = true;
    }

    void OnApplicationQuit() {
        shutdown = true;
    }
}
