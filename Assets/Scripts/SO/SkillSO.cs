using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/SkillSO")]
public class SkillSO : ScriptableObject
{
    public Sprite SkillIcon;
    public string SkillName;
    public string SkillDesc;
    public int Tier;
    public int Damage;
    public float CoolTime;

}
