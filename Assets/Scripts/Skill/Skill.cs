using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillTypes
{
    BornSkill = 0,      //降临技能
    NormalSkill = 1,    // 普通攻击技能
    AttackSkill = 2,    // 攻击技能
    SuperSkill = 3, 	// 大招
}

public enum SkillHitTypes
{   //HitType: 击中类型(0:对敌方目标 1:敌方范围 2:友方目标 3:友方范围 4:自己);
    SelectedEnemy = 0,
    ScaleEnemy = 1,
    SelectedFriend = 2,
    ScaleFriend = 3,
    Self = 4,
};

public enum SkillHitSharpTypes
{//HitSharpType: 攻击范围类型(0：对目标 1：圆形 2：扇形);
    None = 0,
    Circle = 1,
    Fan = 2,
};

public enum SkillEffectTypes
{
    ImmedDamage = 0,                // 立即伤害
    ImmedHeal = 1,                  // 治疗
    AddBuff = 2,                // 添加buff
    Sneer = 3,                      // 嘲讽
    HitDown = 4,                // 击倒
    HitOut = 5,				// 击飞
};

public class Skill
{
    //public float AttackDist;

    public int SkillId;
    private SkillData m_SkillBaseData;
    public SkillData BaseData
    {
        get
        {
            return m_SkillBaseData;
        }
    }

    public string SkillName
    {
        get { return m_SkillBaseData.Name; }
    }

    public SkillTypes SkillType
    {
        get { return (SkillTypes)m_SkillBaseData.SkillType; }

    }
    public string Icon
    {
        get { return m_SkillBaseData.Icon; }
    }
    public bool CoolDown = false;
    public float CDTime
    {
        get { return m_SkillBaseData.CDTime; }
    }
    public float AttackDist
    {
        get { return m_SkillBaseData.AttackDist; }
    }
    public int HitNum
    {
        get { return m_SkillBaseData.HitNum; }
    }
    public float AttackRadius
    {
        get { return m_SkillBaseData.AttackRadius; }
    }
    public float AttackAngel
    {
        get { return m_SkillBaseData.AttackAngle; }
    }
    public SkillHitTypes HitType
    {
        get { return (SkillHitTypes)m_SkillBaseData.HitType; }
    }
    public SkillHitSharpTypes HitSharpType
    {
        get { return (SkillHitSharpTypes)m_SkillBaseData.HitSharpType; }
    }
    public int BpNeed
    {
        get { return m_SkillBaseData.BpNeed; }
    }
    public List<SkillEffectData> EffectList
    {
        get { return m_SkillBaseData.EffectList; }
    }


    public Skill(int dataId)
    {
        SkillId = dataId;
        m_SkillBaseData = DataManager.s_SkillDataManager.GetData(dataId);
        if (m_SkillBaseData == null)
        {
            Debug.LogError("SkillData is null, dataID=" + dataId);
        }
    }
}