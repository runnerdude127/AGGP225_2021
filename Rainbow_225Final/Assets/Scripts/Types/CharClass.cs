using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Class")]
public class CharClass : ScriptableObject
{
    public new string name;

    [Header("Stats")]
    public int health;
    public int speed;
    public int jumpHeight;
    public int unknownStat;

    [Header("Skill Data")]
    public int skillCost;
    public bool autoSkill;

    [Header("Icons")]
    public Sprite sprite;
    public RuntimeAnimatorController animations;
    public Sprite smallIcon;
    public Sprite bigIcon;
}
