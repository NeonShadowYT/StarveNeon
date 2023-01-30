using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mob", menuName = "Mob/New Mob")]
public class MobScriptableObject : ScriptableObject
{
    [Space]
    [Header("�������������� ����")]
    public int Health = 100;
    public int FireArmor = 0;
    public int Damage = 20;

    [Space]
    public float movementSpeed = 7f;
    public float changePositionTime = 5f;
    public float moveDistance = 15f;

    [Space]
    [Header("���")]
    public ItemScriptableObject resourceType;
    public GameObject[] Lut;

    [Space]
    [Header("����")]
    public AudioClip din; // ����

    [Space]
    [Header("�����")]
    public GameObject hitFIX;

    [Space]
    [Header("������")]
    public GameObject �artridge;
}
