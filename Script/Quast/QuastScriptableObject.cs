using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuastType { Kill, Claim }

[CreateAssetMenu(fileName = "Quast", menuName = "Quast/Pass/New Quast")]
public class QuastScriptableObject : ScriptableObject
{
    [Space]
    [Header("Визуал")]
    public string QuastText;

    [Space]
    [Header("Задача игрока")]
    public QuastType quastType;

    [Space]
    [Header("Кого или Что")]
    public MobScriptableObject QuastMob;

    [Space]
    [Header("Количество")]
    public int Amount;

    [Space]
    [Header("Награда")]
    public int PassMoneyReward;
}