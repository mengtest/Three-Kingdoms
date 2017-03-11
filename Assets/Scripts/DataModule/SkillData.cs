using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class SkillData {
    public int ID
    {
        get;
        set;
    }

    public string Name
    {
        get;
        set;
    }

    public string Desc
    {
        get;
        set;
    }

    public string Icon
    {
        get;
        set;
    }

    public SkillTypes SkillType
    {
        get;
        set;
    }

    public float CDTime
    {
        get;
        set;
    }

    public SkillHitTypes HitType
    {
        get;
        set;
    }

    public int HitNum
    {
        get;
        set;
    }

    public float AttackDist
    {
        get;
        set;
    }

    public SkillHitSharpTypes HitSharpType
    {
        get;
        set;
    }

    public float AttackRadius
    {
        get;
        set;
    }

    public float AttackAngle
    {
        get;
        set;
    }

    public int BpNeed
    {
        get;
        set;
    }
    public List<SkillEffectData> EffectList = new List<SkillEffectData>();
}

public class SkillEffectData
{
    public int ID
    {
        get;
        set;
    }

    public SkillEffectTypes Type
    {
        get;
        set;
    }

    public List<int> HitEffect = new List<int>();

    public float Para1
    {
        get;
        set;
    }

    public float Para2
    {
        get;
        set;
    }

    public float Para3
    {
        get;
        set;
    }

    public float Para4
    {
        get;
        set;
    }
}

public class SkillDataManager
{
    private Dictionary<int, SkillData> m_SkillDataDic = null;

    /// <summary>
    /// 技能数据;
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public SkillData GetData(int key)
    {
        if (m_SkillDataDic == null)
            LoadSkillData();

        return m_SkillDataDic.ContainsKey(key) ? m_SkillDataDic[key] : null;
    }

    /// <summary>
    /// 技能数据;
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public List<SkillData> GetAll()
    {
        if (m_SkillDataDic == null)
            LoadSkillData();

        return new List<SkillData>(m_SkillDataDic.Values);
    }

    public void LoadSkillData()
    {
        m_SkillDataDic = new Dictionary<int, SkillData>();
        string textAsset = ResourcesManager.Instance.LoadConfigXML("SkillData").text;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset);
        XmlNode equipXN = xmlDoc.SelectSingleNode("SkillDatas");

        XmlNodeList list = equipXN.ChildNodes;
        if (list != null && list.Count > 0)
        {
            foreach (XmlNode node in list)
            {
                XmlElement element = node as XmlElement;
                if (element.Name.Equals("SkillData"))
                {
                    SkillData info = new SkillData();

                    info.ID = CommonHelper.Str2Int(element.GetAttribute("ID"));
                    info.Name = element.GetAttribute("Name");
                    info.Desc = element.GetAttribute("Desc");
                    info.Icon = element.GetAttribute("Icon");
                    info.SkillType = (SkillTypes)CommonHelper.Str2Int(element.GetAttribute("SkillType"));
                    info.CDTime = CommonHelper.Str2Float(element.GetAttribute("CDTime"));
                    info.HitType = (SkillHitTypes)CommonHelper.Str2Int(element.GetAttribute("HitType"));
                    info.AttackDist = CommonHelper.Str2Float(element.GetAttribute("AttackDist"));
                    info.HitNum = CommonHelper.Str2Int(element.GetAttribute("HitNum"));
                    info.HitSharpType = (SkillHitSharpTypes)CommonHelper.Str2Int(element.GetAttribute("HitSharpType"));
                    info.AttackRadius = CommonHelper.Str2Float(element.GetAttribute("AttackRadius"));
                    info.AttackAngle = CommonHelper.Str2Float(element.GetAttribute("AttackAngle"));
                    info.BpNeed = CommonHelper.Str2Int(element.GetAttribute("BpNeed"));

                    XmlNodeList data = element.ChildNodes;
                    if (data == null || data.Count < 1)
                    {
                        continue;
                    }

                    foreach (XmlNode subNode in data)
                    {
                        XmlElement subElement = subNode as XmlElement;
                        if (subElement.Name.Equals("SkillEffectData"))
                        {
                            SkillEffectData effectData = new SkillEffectData();
                            effectData.ID = CommonHelper.Str2Int(subElement.GetAttribute("ID"));
                            effectData.Type = (SkillEffectTypes)CommonHelper.Str2Int(subElement.GetAttribute("Type"));
                            effectData.HitEffect = CommonHelper.Str2IntList(subElement.GetAttribute("HitEffect"));
                            effectData.Para1 = CommonHelper.Str2Float(subElement.GetAttribute("Para1"));
                            effectData.Para2 = CommonHelper.Str2Float(subElement.GetAttribute("Para2"));
                            effectData.Para3 = CommonHelper.Str2Float(subElement.GetAttribute("Para3"));
                            effectData.Para4 = CommonHelper.Str2Float(subElement.GetAttribute("Para4"));
                            info.EffectList.Add(effectData);
                        }
                    }

                    if (!m_SkillDataDic.ContainsKey(info.ID))
                    {
                        m_SkillDataDic.Add(info.ID, info);
                    }
                }
            }
        }
    }
}