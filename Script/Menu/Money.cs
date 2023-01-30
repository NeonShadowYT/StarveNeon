using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Money : MonoBehaviour
{
    public int score = 0, priceScore = 0;
    [SerializeField] TMP_Text scoreText;
    public GameObject players, buyButton, enableButton;
    void Start()
    {
        if (PlayerPrefs.HasKey("Score")) score = PlayerPrefs.GetInt("Score");
        else
        {
            score = 0;
            buyButton.SetActive(true);
            enableButton.SetActive(false);
            PlayerPrefs.SetInt("Score", score);
        }
        ScoreUpdate();
    }
    public void ScoreAdd()
    {
        score += Random.Range(0, 5);
        PlayerPrefs.SetInt("Score", score);
    }
    public void ScoreUpdate()
    {
        scoreText.text = score.ToString();
        PlayerPrefs.SetInt("Score", score);
    }
    public void Buy()
    {
        if (score >= priceScore)
        {
            score -= priceScore;
            buyButton.SetActive(false);
            enableButton.SetActive(true);
            ScoreUpdate();
        }
    }
    public void DeleteAllSaves() => PlayerPrefs.DeleteAll();
}
