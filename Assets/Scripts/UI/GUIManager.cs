using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class GUIManager { 
    private static Dictionary<string, KeyValuePair<GameObject, IView>> m_UIViewDic
        = new Dictionary<string, KeyValuePair<GameObject, IView>>();

    private static GameObject InstantiatePanel(string prefabId)
    {
        GameObject prefab = ResourcesManager.Instance.GetUIPrefab(prefabId);
        if (prefab == null)
        {
            Debug.LogError("prefab is null, prefabName : " + prefabId);
            return null;
        }
        GameObject UIPrefab = GameObject.Instantiate(prefab) as GameObject;
        UIPrefab.name = prefabId;

        Camera uiCamera = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
        if (uiCamera == null)
        {
            Debug.LogError("UICamera is null");
            return null;
        }
        UIPrefab.transform.parent = uiCamera.transform;
        UIPrefab.transform.localScale = Vector3.one;
        UIPrefab.transform.localPosition = new Vector3(prefab.transform.localPosition.x,
            prefab.transform.localPosition.y, Mathf.Clamp(prefab.transform.localPosition.z, -2f, 2f));
        return UIPrefab;
    }
    public static void ShowView(string name)
    {
        IView view = null;
        GameObject panel = null;

        KeyValuePair<GameObject, IView> found;
        if (!m_UIViewDic.TryGetValue(name, out found))
        {
            view = Assembly.GetExecutingAssembly().CreateInstance(name) as IView;
            panel = InstantiatePanel(name);

            if (view == null || panel == null)
            {
                Debug.LogError("view or panel is null, " + name);
                return;
            }
            UIPanel[] childsPanel = panel.GetComponentsInChildren<UIPanel>(true);
            for (int i = 0; i < childsPanel.Length; i++)
            {
                UIPanel childPanel = childsPanel[i];
                childPanel.depth += (int) view.UILayer;
            }
            m_UIViewDic.Add(name, new KeyValuePair<GameObject, IView>(panel, view));

            view.Start();
        }
        else
        {
            view = found.Value;
            panel = found.Key;
        }

        if (view == null || panel == null)
        {
            Debug.LogError("view or panel is null, " + name);
            return;
        }

        foreach (KeyValuePair<string, KeyValuePair<GameObject, IView>> pair in m_UIViewDic)
        {
            if (view.UILayer != pair.Value.Value.UILayer)
            {
                continue;
            }
            if (!pair.Value.Key.activeSelf)
            {
                continue;
            }
            HideView(pair.Key);
        }
        UIPanel uiPanel = panel.GetComponent<UIPanel>();
        uiPanel.alpha = 1;

        panel.SetActive(true);
        view.Show();
    }

    public static void HideView(string name)
    {
        KeyValuePair<GameObject, IView> pair;
        if (!m_UIViewDic.TryGetValue(name, out pair))
        {
            return;
        }
        pair.Key.SetActive(false);
        pair.Value.Hide();
    }

    public static void DestoryAllView()
    {
        foreach (KeyValuePair<GameObject, IView> item in m_UIViewDic.Values)
        {
            item.Value.Destory();
            GameObject.Destroy(item.Key);
        }
        m_UIViewDic.Clear();
        Resources.UnloadUnusedAssets();
    }

    public static IView FindView(GameObject go)
    {
        GameObject panel = GetRoolPanel(go);
        if (panel == null)
        {
            return null;
        }
        KeyValuePair<GameObject, IView> pair;
        if (!m_UIViewDic.TryGetValue(panel.name, out pair))
        {
            return null;
        }
        return pair.Value;
    }

    /// <summary>
    /// 获取父级最高层的Panel
    /// </summary>
    /// <param name="gameobject"></param>
    /// <returns></returns>
    public static GameObject GetRoolPanel(GameObject gameobject)
    {
        if (gameobject == null)
        {
            return null;
        }

        Transform parent = gameobject.transform.parent;
        if (parent == null)
        {
            UIPanel tempPanel = gameobject.GetComponent<UIPanel>();
            return tempPanel == null ? null : tempPanel.gameObject;
        }
        UIPanel parentPanel = null;
        while (parent != null)
        {
            UIPanel tempPanel = parent.GetComponent<UIPanel>();
            if (tempPanel != null)
            {
                parentPanel = tempPanel;
            }
            parent = parent.parent;
        }
        return parentPanel.gameObject;
    }

    public static void Update()
    {
        foreach (KeyValuePair<GameObject, IView> item in m_UIViewDic.Values)
        {
            if (item.Key.activeInHierarchy)
            {
                item.Value.Update();
            }
        }
    }
}
