using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T:MonoSingleton<T>
{
    private static string m_parentName="Singleton";
    private static T m_instance;
    public static T Instance
    {
        get {
            if (m_instance == null) {
                GameObject game = GameObject.Find(m_parentName);
                if (game==null) {
                    game = new GameObject(m_parentName);
                }
                m_instance= game.AddComponent<T>();
            }
            return m_instance;
        }
    }

}
