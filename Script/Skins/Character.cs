using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    SelectedCharacters.Data data = new SelectedCharacters.Data();
    public int i;
    public GameObject players;
    public GameObject[] AllCharacters;

    public void Start()
    {
        data = JsonUtility.FromJson<SelectedCharacters.Data>(PlayerPrefs.GetString("SaveGame"));
        if (PlayerPrefs.HasKey("SaveGame"))
        {
            foreach (GameObject character in AllCharacters)
            {
                if (character.name != data.currentCharacter)
                {
                    character.SetActive(false);
                }
                else
                {
                    character.SetActive(true);
                }
            }
        }
    }
}
