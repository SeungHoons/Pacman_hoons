using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singletone<T> : MonoBehaviour where T : MonoBehaviour
{
    private static GameObject m_Obj;
    private static T m_Inst = null;

    public static T Inst
    {
        get
        {
            if (null != m_Inst)
            {
                return m_Inst;
            }

            m_Inst = (T)FindObjectOfType(typeof(T));

            if (m_Inst == null)
            {
                m_Obj = new GameObject(typeof(T).Name);
                m_Inst = m_Obj.AddComponent<T>();
            }

            return m_Inst;
        }
    }
}