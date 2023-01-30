using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Match
{
    public string ID;
    public int Map;
    public bool PublicMatch;
    public bool InMatch;
    public bool MatchFull;
    public List<GameObject> players = new List<GameObject>();

    public Match(string ID, GameObject player, bool publicMatch, int mapIndex)
    {
        Map = mapIndex;
        MatchFull = false;
        InMatch = false;
        this.ID = ID;
        PublicMatch = publicMatch;
        players.Add(player);
    }

    public Match()
    {

    }
}

public class MainMenu : NetworkBehaviour
{
    public static MainMenu instance;
    public readonly SyncList<Match> matches = new SyncList<Match>();
    public readonly SyncList<string> matchIDs = new SyncList<string>();
    public int MaxPlayers;
    private NetworkManager networkManager;

    [Header("MainMenu")]
    public TMP_InputField JoinInput;
    public Button[] Buttons;
    public Canvas LobbyCanvas;
    public Canvas SearchCanvas;
    private bool searching;

    [Header("Name")]
    public GameObject ChangeNamePanel;
    public GameObject CloseButton;
    public Button SetNameButton;
    public TMP_InputField NameInput;
    public int firstTime = 1;
    [SyncVar] public string DisplayName;

    [Header("Lobby")]
    public Transform UIPLayerParent;
    public GameObject UIPlayerPrefab;
    public TMP_Text IDText;
    public Button BeginGameButton;
    public GameObject localPlayerLobbyUI;
    public Image LobbyMapImage;
    public TMP_Text LobbyMapNameText;
    public bool inGame;

    [Header("Error")]
    public GameObject ErrorPanel;
    public TMP_Text ErrorText;

    [Header("Chat")]
    public TMP_Text ChatHistoryText;
    public TMP_InputField MessageInput;
    public Button SendButton;

    [Header("CustomLobby")]
    public Image MapImage;
    public TMP_Text MapNameText;
    public Toggle LobbyTypeToggle;
    public List<Map> Maps;
    private int mapIndex;

    private void Start()
    {
        instance = this;

        networkManager = FindObjectOfType<NetworkManager>();

        firstTime = PlayerPrefs.GetInt("firstTime", 1);

        SetSendButtonActive(MessageInput.text);

        MapImage.sprite = Maps[mapIndex].Image;
        MapNameText.text = Maps[mapIndex].Name;

        if (!PlayerPrefs.HasKey("Name"))
        {
            return;
        }

        string defaultName = PlayerPrefs.GetString("Name");
        NameInput.text = defaultName;
        DisplayName = defaultName;
        SetName(defaultName);
    }

    private void Update()
    {
        if (!inGame)
        {
            MultiPlayer[] players = FindObjectsOfType<MultiPlayer>();

            for (int i = 0; i < players.Length; i++)
            {
                players[i].gameObject.transform.localScale = Vector3.zero;
            }

            if (firstTime == 1)
            {
                ChangeNamePanel.SetActive(true);
                CloseButton.SetActive(false);

                JoinInput.interactable = false;
                for (int i = 0; i < Buttons.Length; i++)
                {
                    Buttons[i].interactable = false;
                }
            }
            else
            {
                CloseButton.SetActive(true);
            }
        }
    }

    public void SetName(string name)
    {
        if (name == DisplayName || string.IsNullOrEmpty(name))
        {
            SetNameButton.interactable = false;
        }
        else
        {
            SetNameButton.interactable = true;
        }
    }

    public void SaveName()
    {
        ChangeNamePanel.SetActive(false);
        JoinInput.interactable = false;
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }
        firstTime = 0;
        DisplayName = NameInput.text;
        PlayerPrefs.SetInt("firstTime", firstTime);
        PlayerPrefs.SetString("Name", DisplayName);
        Invoke(nameof(Disconnect), 1f);
    }

    public void ChangeMapIndex(bool previous)
    {
        if (!previous)
        {
            mapIndex = (mapIndex + 1) % Maps.Count;
        }
        else
        {
            mapIndex--;
            if (mapIndex < 0)
            {
                mapIndex += Maps.Count;
            }
        }

        MapImage.sprite = Maps[mapIndex].Image;
        MapNameText.text = Maps[mapIndex].Name;
    }

    public void SetSendButtonActive(string message)
    {
        SendButton.interactable = !string.IsNullOrWhiteSpace(message);
    }

    public void HandleMessage()
    {
        if (!string.IsNullOrWhiteSpace(MessageInput.text))
        {
            MultiPlayer.localPlayer.CmdHandleMessage(MessageInput.text);
        }
    }

    public void SendMessageToServer(string message)
    {
        ChatHistoryText.text += message + "\n";
        MessageInput.text = string.Empty;
    }

    public void Disconnect()
    {
        if (networkManager.mode == NetworkManagerMode.Host)
        {
            networkManager.StopHost();
        }
        else if (networkManager.mode == NetworkManagerMode.ClientOnly)
        {
            networkManager.StopClient();
        }
    }

    public void SetBeginButtonActive(bool active)
    {
        BeginGameButton.interactable = active;
    }

    public void Host()
    {
        JoinInput.interactable = false;
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }

        MultiPlayer.localPlayer.HostGame(LobbyTypeToggle.isOn, mapIndex);
    }

    public void HostSuccess(bool success, string matchID, int mapIndex)
    {
        if (success)
        {
            LobbyCanvas.enabled = true;

            if (localPlayerLobbyUI != null)
            {
                Destroy(localPlayerLobbyUI);
            }

            localPlayerLobbyUI = SpawnPlayerUIPrefab(MultiPlayer.localPlayer);
            IDText.text = matchID;
            SetLobbyMap(mapIndex);
            BeginGameButton.interactable = true;
        }
        else
        {
            ErrorPanel.SetActive(true);
            ErrorText.text = "Не удалось создать лобби";
        }
    }

    public void SetLobbyMap(int mapIndex)
    {
        LobbyMapImage.sprite = Maps[mapIndex].Image;
        LobbyMapNameText.text = Maps[mapIndex].Name;
    }

    public void Join()
    {
        JoinInput.interactable = false;
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }

        MultiPlayer.localPlayer.JoinGame(JoinInput.text.ToUpper());
    }

    public void JoinSuccess(bool success, string matchID)
    {
        if (success)
        {
            LobbyCanvas.enabled = true;

            if (localPlayerLobbyUI != null)
            {
                Destroy(localPlayerLobbyUI);
            }

            localPlayerLobbyUI = SpawnPlayerUIPrefab(MultiPlayer.localPlayer);
            IDText.text = matchID;
            BeginGameButton.interactable = false;
        }
        else
        {
            ErrorPanel.SetActive(true);
            ErrorText.text = "ID не найден";
        }
    }

    public void Enable()
    {
        ErrorPanel.SetActive(false);
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = true;
        }
        JoinInput.interactable = true;
    }

    public void DisconnectGame()
    {
        if (localPlayerLobbyUI != null)
        {
            Destroy(localPlayerLobbyUI);
        }

        MultiPlayer.localPlayer.DisconnectGame();
        LobbyCanvas.enabled = false;
        JoinInput.interactable = true;
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = true;
        }
    }

    public bool HostGame(string matchID, GameObject player, bool publicMatch, int mapIndex)
    {
        if (!matchIDs.Contains(matchID))
        {
            matchIDs.Add(matchID);
            Match match = new Match(matchID, player, publicMatch, mapIndex);
            matches.Add(match);
            player.GetComponent<MultiPlayer>().CurrentMatch = match;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool JoinGame(string matchID, GameObject player)
    {
        if (matchIDs.Contains(matchID))
        {
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].ID == matchID)
                {
                    if (!matches[i].InMatch && !matches[i].MatchFull)
                    {
                        matches[i].players.Add(player);
                        player.GetComponent<MultiPlayer>().CurrentMatch = matches[i];
                        matches[i].players[0].GetComponent<MultiPlayer>().PlayerCountUpdated(matches[i].players.Count);
                        if (matches[i].players.Count == MaxPlayers)
                        {
                            matches[i].MatchFull = true;
                        }
                        break;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool SearchGame(GameObject player, out string ID)
    {
        ID = "";

        for (int i = 0; i < matches.Count; i++)
        {
            Debug.Log("Проверка ID " + matches[i].ID + " | в игре " + matches[i].InMatch + " | лобби полный " + matches[i].MatchFull + " | публичное лобби " + matches[i].PublicMatch);
            if (!matches[i].InMatch && !matches[i].MatchFull && matches[i].PublicMatch)
            {
                if (JoinGame(matches[i].ID, player))
                {
                    ID = matches[i].ID;
                    return true;
                }
            }
        }

        return false;
    }

    public static string GetRandomID()
    {
        string ID = string.Empty;
        for (int i = 0; i < 5; i++)
        {
            int rand = UnityEngine.Random.Range(0, 36);
            if (rand < 26)
            {
                ID += (char)(rand + 65);
            }
            else
            {
                ID += (rand - 26).ToString();
            }
        }
        return ID;
    }

    public GameObject SpawnPlayerUIPrefab(MultiPlayer player)
    {
        GameObject newUIPlayer = Instantiate(UIPlayerPrefab, UIPLayerParent);
        newUIPlayer.GetComponent<PlayerUI>().SetPlayer(player.PlayerDisplayName);

        return newUIPlayer;
    }

    public void StartGame()
    {
        MultiPlayer.localPlayer.BeginGame();
    }

    public void SearchGame()
    {
        StartCoroutine(Searching());
    }

    public void CancelSearchGame()
    {
        JoinInput.interactable = true;
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = true;
        }

        searching = false;
    }

    public void SearchGameSuccess(bool success, string ID)
    {
        if (success)
        {
            SearchCanvas.enabled = false;
            searching = false;
            JoinSuccess(success, ID);
        }
    }

    public void BeginGame(string matchID)
    {
        for (int i = 0; i < matches.Count; i++)
        {
            if (matches[i].ID == matchID)
            {
                matches[i].InMatch = true;
                foreach (var player in matches[i].players)
                {
                    player.GetComponent<MultiPlayer>().StartGame();
                }
                break;
            }
        }
    }

    public void PlayerDisconnected(GameObject player, string ID)
    {
        for (int i = 0; i < matches.Count; i++)
        {
            if (matches[i].ID == ID)
            {
                int playerIndex = matches[i].players.IndexOf(player);
                if (matches[i].players.Count > playerIndex)
                {
                    matches[i].players.RemoveAt(playerIndex);
                }

                if (matches[i].players.Count == 0)
                {
                    matches.RemoveAt(i);
                    matchIDs.Remove(ID);
                }
                else
                {
                    matches[i].players[0].GetComponent<MultiPlayer>().PlayerCountUpdated(matches[i].players.Count);
                }
                break;
            }
        }
    }

    public void Exit()
    {
        Disconnect();
        Application.Quit();
    }

    IEnumerator Searching()
    {
        JoinInput.interactable = false;
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }
        SearchCanvas.enabled = true;
        searching = true;

        float searchInterval = 1;
        float currentTime = 1;

        while (searching)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                currentTime = searchInterval;
                MultiPlayer.localPlayer.SearchGame();
            }
            yield return null;
        }
        SearchCanvas.enabled = false;
    }
}

public static class MatchExtension
{
    public static Guid ToGuid(this string id)
    {
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        byte[] inputBytes = Encoding.Default.GetBytes(id);
        byte[] hasBytes = provider.ComputeHash(inputBytes);

        return new Guid(hasBytes);
    }
}

[System.Serializable]
public class Map
{
    public string Name;
    public Sprite Image;
    [Scene] public string MapScene;
}