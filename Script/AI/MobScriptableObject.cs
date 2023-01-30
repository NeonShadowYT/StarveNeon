using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mob", menuName = "Mob/New Mob")]
public class MobScriptableObject : ScriptableObject
{
    [Space]
    [Header("’арактеристики моба")]
    public int Health = 100;
    public int FireArmor = 0;
    public int Damage = 20;

    [Space]
    public float movementSpeed = 7f;
    public float changePositionTime = 5f;
    public float moveDistance = 15f;

    [Space]
    [Header("Ћут")]
    public ItemScriptableObject resourceType;
    public GameObject[] Lut;

    [Space]
    [Header("«вук")]
    public AudioClip din; // звук

    [Space]
    [Header("Ёфект")]
    public GameObject hitFIX;

    [Space]
    [Header("ƒругое")]
    public GameObject сartridge;
}
