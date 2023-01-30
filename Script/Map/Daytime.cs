using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Daytime : MonoBehaviour //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    SelectedCharacters.Data data = new SelectedCharacters.Data();

    [Space]
    [Header("�����")]
    public List<Fraction> fraction;

    public List<GameObject> Nobj;

    [Space]
    public MoneySlot TexnoSlot;
    public Indicators indicators;
    private Animator _animator; // �������� ��� ��������� ����������� � �������

    [Space]
    [Header("���")]
    public GameObject RouSpawner;

    [Space]
    [Header("���")]
    [Space]
    private int _Day;
    public int Day
    {
        get { return _Day; }
        set
        {
            _Day = value;
            if (_Day >= 3)
            {
                TexnoSlot.amount += Random.Range(125, 400);
            }
            else
            {
                TexnoSlot.amount += Random.Range(100, 250);
            }

            data = JsonUtility.FromJson<SelectedCharacters.Data>(PlayerPrefs.GetString("SaveGame"));
            data.money += Random.Range(100, 500);
            PlayerPrefs.SetString("SaveGame", JsonUtility.ToJson(data));

            foreach (Fraction fract in fraction) foreach (FractionUpdate fractup in fract.fractionUpdate) if (fractup.DayUpdate == _Day) fract.UpdateLvlFract();
        }
    }
    public GameObject DayTextObject;
    public TMP_Text DayText;

    [Space]
    public bool once = false;
    public bool once2 = false, _sleepsAI = false;

    [Space]
    [SerializeField] Gradient directionalLightGradient, ambientLightGradient;
    [SerializeField, Range(1, 3600)] float timeDayInSeconds = 600;
    [SerializeField, Range(0, 1f)] float timeProgress = 0.25f;

    [SerializeField] public bool day = true, newDay = false;
    [SerializeField] Light dirLight;
    public Transform dirLightTransform;
    Vector3 defaultAngles;

    void Start()
    {
        if (Application.isPlaying)
        {
            _animator = GetComponent<Animator>();

            defaultAngles = dirLightTransform.localEulerAngles;

            once2 = true;

            foreach (GameObject _Nobj in Nobj) _Nobj.SetActive(false);
        }
    }

    void LateUpdate()
    {
        if (Application.isPlaying) timeProgress += Time.deltaTime / timeDayInSeconds;

        dirLight.color = directionalLightGradient.Evaluate(timeProgress);
        RenderSettings.ambientLight = ambientLightGradient.Evaluate(timeProgress);
        dirLightTransform.localEulerAngles = new Vector3(360f * timeProgress - 90, defaultAngles.x, defaultAngles.z);

        if (timeProgress > 1f) // ���������� �����
        {
            timeProgress = 0f;
            newDay = true;
        }

        if (day == true && newDay == false)// ���� ������ ����
        {
            if (timeProgress > 0.25f && once == true)// � ��������� ����
            {
                _animator.SetBool("Day", true);

                Day++;

                DayText.text = Day.ToString();
                DayTextObject.SetActive(true);

                indicators.Ivent = Random.Range(0, 3);
            }

            once = false;
            once2 = true;
        }

        if (timeProgress > 0.265f && once2 == true) //����� �������� �� Invoke
        {
            once2 = false;
            DayTextObject.SetActive(false); // � ��������� ����
        }

        if (timeProgress > 0.725f) // � ��������� ����
        {
            _animator.SetBool("Day", false);

            indicators._sleepAI = 1;// ��������
            _sleepsAI = true;

            foreach (GameObject _Nobj in Nobj) _Nobj.SetActive(true);

            day = false;// ��������� ����
        }

        if (day == false && newDay == true)// ���� ������ ����
        {
            if (timeProgress > 0.25f)// � ��������� ����
            {
                indicators._sleepAI = 0;// �����������
                _sleepsAI = false;

                foreach (GameObject _Nobj in Nobj) _Nobj.SetActive(false);

                once = true;
                day = true;// �������� ����
                newDay = false;
            }
        }
    }
}