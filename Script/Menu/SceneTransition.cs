using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneTransition : MonoBehaviour //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    public TMP_Text LoadingPercentage;
    public Image LoadingProgressBar;
    public bool RandomImages = false;
    public int RandomImagesInterval = 5;
    public List<Sprite> RandomImage = new List<Sprite>();
    public Image BG;
    private static SceneTransition instance;
    private static bool shouldPlayOpeningAnimation = false; 
    private Animator componentAnimator;
    private AsyncOperation loadingSceneOperation;

    public void SwitchToScene(string sceneName)
    {
        instance.componentAnimator.SetTrigger("Loading");
        instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        instance.loadingSceneOperation.allowSceneActivation = false; // Чтобы сцена не начала переключаться пока играет анимация closing:
        instance.LoadingProgressBar.fillAmount = 0;
    }

    public void DeathToScene()
    {
        string menu = "Menu";
        SwitchToScene(menu);
    }

    private void Start()
    {
        instance = this;
        componentAnimator = GetComponent<Animator>();
        if (shouldPlayOpeningAnimation) 
        {
            componentAnimator.SetTrigger("LoadingEnd");
            instance.LoadingProgressBar.fillAmount = 1;
            shouldPlayOpeningAnimation = false;  // Чтобы если следующий переход будет обычным SceneManager.LoadScene, не проигрывать анимацию opening:
        }
        if (RandomImages == true)
        {
            BG.sprite = RandomImage[Random.Range(0, RandomImage.Count)];
            StartCoroutine(ImagesRandom());
        }
    }

    private void Update()
    {
        if (loadingSceneOperation != null)
        {
            LoadingPercentage.text = Mathf.RoundToInt(loadingSceneOperation.progress * 100) + "%"; // Просто присвоить прогресс: // LoadingProgressBar.fillAmount = loadingSceneOperation.progress;
            LoadingProgressBar.fillAmount = Mathf.Lerp(LoadingProgressBar.fillAmount, loadingSceneOperation.progress, Time.deltaTime * 5); // Присвоить прогресс с быстрой анимацией, чтобы ощущалось плавнее:
        }
    }

    IEnumerator ImagesRandom()
    {
        int i = 0;
        while (i == 0)
        {
            yield return new WaitForSeconds(RandomImagesInterval);
            BG.sprite = RandomImage[Random.Range(0, RandomImage.Count)];
        }
    }

    public void OnAnimationOver()
    {
        shouldPlayOpeningAnimation = true; // Чтобы при открытии сцены, куда мы переключаемся, проигралась анимация opening:
        loadingSceneOperation.allowSceneActivation = true;
    }
}
