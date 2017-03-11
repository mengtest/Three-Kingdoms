using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDot : MonoBehaviour
{
    private Dictionary<string, Transform> DotDic = new Dictionary<string, Transform>();
    public List<Transform> DotList = new List<Transform>();

    void Awake()
    {
        for (int i = 0; i < DotList.Count; i++)
        {
            if (DotList[i] != null)
            {
                if (!DotDic.ContainsKey(DotList[i].name))
                {
                    DotDic.Add(DotList[i].name, DotList[i]);
                }
            }
        }
    }

    public Transform GetEffectDot(string name)
    {
        return DotDic.ContainsKey(name) ? DotDic[name] : null;
    }
}
