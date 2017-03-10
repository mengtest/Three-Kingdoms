using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    private static ResourcesManager _Instance;
    public static ResourcesManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new ResourcesManager();
            }
            return _Instance;
        }
    }

    private string uiPanelPath = "UI/Panel";
    public GameObject GetUIPrefab(string name)
    {
        return LoadPrefab(name, uiPanelPath);
    }

    public GameObject LoadPrefab(string name, string path)
    {
        string loadPath = path + "/" + name;
        GameObject prefab = Resources.Load(loadPath, typeof(GameObject)) as GameObject;
        if (prefab == null)
        {
            Debug.LogError("prefab is null, loadPath : " + loadPath);
        }
        return prefab;
    }

    private string xmlPath = "Config";
    public TextAsset LoadConfigXML(string name)
    {
        return LoadXMLAsset(name, xmlPath);
    }

    public TextAsset LoadXMLAsset(string name, string path)
    {
        string loadPath = path + "/" + name;
        TextAsset textAsset = Resources.Load(loadPath, typeof(TextAsset)) as TextAsset;

        if (textAsset == null)
        {
            Debug.LogError(loadPath + " is null");
            return null;
        }

        return textAsset;
    }
}
