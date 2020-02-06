using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Debug_UI : Singletone<Debug_UI>
{
	public int Count = 0;
	private Dictionary<string, string> m_arrText;
	private string str;
	private bool m_active = true;

	private void OnGUI()
	{
		if (m_active)
		{
			int temp = 3 + Count * 17;
			GUI.TextField(new Rect(24, 24, 200, temp), str);
		}
	}

	private void Start()
	{
		if (m_arrText == null)
			m_arrText = new Dictionary<string, string>();
	}
	private void Update()
	{
		str = "";
		foreach (var Dic in m_arrText)
		{
			str += Dic.Key;
			str += " : ";
			str += Dic.Value;
			str += "\n";
		}
	}

	public void SetDebugText(string _str, string _text)
	{
		if (m_arrText == null)
			m_arrText = new Dictionary<string, string>();

		if (!m_arrText.ContainsKey(_str))
		{
			Count++;
			m_arrText.Add(_str, _text);
		}
		else
			m_arrText[_str] = _text;

	}

	//public void AddTextObject(string _str, string _text)
	//{
	//}

	public void ChageActive()
	{
		if (m_active)
			m_active = false;
		else
			m_active = true;
	}
}
