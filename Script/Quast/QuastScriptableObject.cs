using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuastType { Kill, Claim }

[CreateAssetMenu(fileName = "Quast", menuName = "Quast/Pass/New Quast")]
public class QuastScriptableObject : ScriptableObject
{
    [Space]
    [Header("������")]
    public string QuastText;

    [Space]
    [Header("������ ������")]
    public QuastType quastType;

    [Space]
    [Header("���� ��� ���")]
    public MobScriptableObject QuastMob;

    [Space]
    [Header("����������")]
    public int Amount;

    [Space]
    [Header("�������")]
    public int PassMoneyReward;
}