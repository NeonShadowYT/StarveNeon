using UnityEngine;
using UnityEngine.UI;
public class DynamicResolution : MonoBehaviour //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    private Vector2 MainRes;
    public float CurScale = 1, MinScale = 0.5f, ScaleStep = 0.05f;
    public int MinFPS = 40, MaxFPS = 55;
    private float MinFPSS, MaxFPSS;
    public float Delay;
    private float DelayTime;
    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 60;
        MainRes = new Vector2(Screen.width, Screen.height);
        DelayTime = Delay;
        MinFPSS = 1f / (float)MinFPS;
        MaxFPSS = 1f / (float)MaxFPS;
    }
    void Update()
    {
        if (Time.time > DelayTime)
        {
            if (Time.deltaTime > MinFPSS)
            {
                if (CurScale > MinScale)
                {
                    CurScale -= ScaleStep;
                    Screen.SetResolution((int)(MainRes.x * CurScale), (int)(MainRes.y * CurScale), true);
                    DelayTime = Time.time + Delay;
                }
            }
            else if (CurScale < 1 && Time.deltaTime < MaxFPSS)
            {
                CurScale += ScaleStep;
                Screen.SetResolution((int)(MainRes.x * CurScale), (int)(MainRes.y * CurScale), true);
                DelayTime = Time.time + Delay;
            }
            DelayTime = Time.time + 0.5f;
        }
    }
}