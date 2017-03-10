using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class SceneData
{
    public int ID;
    public string Name;
    public string LevelName;
    public string GameState;
}

public class SceneDataManager
{
    private Dictionary<int, SceneData> m_SceneDataDic;

    public SceneData GetData(int key)
    {
        if (m_SceneDataDic == null)
        {
            LoadSceneData();
        }
        return m_SceneDataDic.ContainsKey(key) ? m_SceneDataDic[key] : null;
    }

    public void LoadSceneData()
    {
        m_SceneDataDic = new Dictionary<int, SceneData>();
        string textAsset = ResourcesManager.Instance.LoadConfigXML("SceneData").text;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset);
        XmlNode xmlNode = xmlDoc.SelectSingleNode("SceneDatas");

        XmlNodeList list = xmlNode.ChildNodes;
        if (list != null && list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                XmlNode childNode = list[i];
                XmlElement element = childNode as XmlElement;
                if (element.Name.Equals("SceneData"))
                {
                    SceneData info = new SceneData();

                    info.ID = Int32.Parse(element.GetAttribute("ID"));
                    info.Name = element.GetAttribute("Name");
                    info.LevelName = element.GetAttribute("LevelName");
                    info.GameState = element.GetAttribute("GameState");

                    if (!m_SceneDataDic.ContainsKey(info.ID))
                    {
                        m_SceneDataDic.Add(info.ID, info);
                    }
                }
            }
        }
    }
}
