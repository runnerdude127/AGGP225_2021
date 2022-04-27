using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon")]
public class Weapon : ScriptableObject
{
    public int id;
    public new string name;
    public int bulletID;

    [Header("Stats")]
    public bool auto;
    public int burst;
    public float delay;
    public float recoil;
    public float accuracy;

    [Header("Graphics")]
    public Sprite sprite;
    public Sprite pickupSprite;

    [Header("Sounds")]
    public AudioClip callout;
    public AudioClip loadSound;
    public AudioClip shootSound;
    public Vector2 barrelOffset;
}
