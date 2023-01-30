using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class StarveNeonUpdateManager : MonoBehaviour
{
    public string versionUrl = "";
    public string currentVersion;

    private string latestVersion;

    public GameObject newVersionAvailable;
    public TMP_Text NewVersionText;

    public void Start() => StartCoroutine(LoadTxtData(versionUrl));
    public void Check() => StartCoroutine(LoadTxtData(versionUrl));

    private IEnumerator LoadTxtData(string url)
    {
        UnityWebRequest loaded = new UnityWebRequest(url);
        loaded.downloadHandler = new DownloadHandlerBuffer();

        yield return loaded.SendWebRequest();
        latestVersion = loaded.downloadHandler.text;

        CheckVersion();
    }
    private void CheckVersion()
    {
        Debug.Log("Текущая версия = " + currentVersion);
        Debug.Log("Последняя версия = " + latestVersion);

        NewVersionText.text = latestVersion;
        if ((latestVersion != "") && (currentVersion != latestVersion)) newVersionAvailable.SetActive(true); //Мальчик обнови версию ;D
    }
}