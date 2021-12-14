using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterClass
{
    public string name;
    public int hp;
    public int speed;
    public int jumpHeight;
    public int mystery;
    public Sprite sprite;
    public RuntimeAnimatorController animations;

    public CharacterClass(string newName, int newHP, int newSpeed, int newJump, int newMystery, Sprite newSprite, RuntimeAnimatorController newAnims)
    {
        name = newName;
        hp = newHP;
        speed = newSpeed;
        jumpHeight = newJump;
        sprite = newSprite;
        animations = newAnims;
    }
}

