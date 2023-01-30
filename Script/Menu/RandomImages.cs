using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomImages : MonoBehaviour
{
    public Image BG;
    public List<Sprite> RandomImage = new List<Sprite>();
    public int RandomImagesInterval = 5;
    void Start()
    {
        BG.sprite = RandomImage[Random.Range(0, RandomImage.Count)];
        StartCoroutine(ImagesRandom());
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
}
