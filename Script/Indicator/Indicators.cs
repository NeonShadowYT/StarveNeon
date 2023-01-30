using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Indicators : MonoBehaviour //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("—ÍËÔÚ˚")]
    public CustomCharacterController customCharacterController;
    public MenuManagerm menuManagerm;
    public SceneTransition sceneTransition;

    public Daytime daytime;

    private Camera mainCamera;

    [Space]
    [Header("Œ·˙ÂÍÚ˚")]
    public GameObject players;
    private GameObject RouSpawner;

    [Space]
    public GameObject DayTextObject;
    public TMP_Text DayText;

    [Space]
    [Header("—Ú‡ÚËÒÚËÍ‡")]
    public GameObject healthStats;
    public GameObject waterStats, foodStats, coldStats, hillStats, infectionStats, webStats, fireStats;

    [Space]
    [Header("›ÙÂÍÚ˚")]
    public GameObject healEffect;
    public GameObject damageEffect, infectionEffect;

    [Space]
    [Header("œÓÍ‡Á‡ÚÂÎË")]
    private bool Death;
    private float _healthAmount = 200;
    public float healthAmount
    {
        get { return _healthAmount; }
        set
        {
            _healthAmount = value;
            if (_healthAmount <= 40) healthStats.SetActive(true); else healthStats.SetActive(false);
            if (_healthAmount <= 0)
            {
                if(Death == false)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    Death = true;

                    customCharacterController.walkingSpeed = 0f;
                    customCharacterController.runningSpeed = 0f;
                    customCharacterController.swimmingSpeed = 0f;
                    customCharacterController.currentSpeed = 0f;
                    customCharacterController.jumpForce = 0f;

                    sceneTransition.DeathToScene();
                }
            }
            uiHealthAmount = Mathf.Lerp(uiHealthAmount, healthAmount, Time.deltaTime * changeFactor);
            healthBar.fillAmount = healthAmount / 200;
        }
    }
    private float _foodAmount = 200;
    public float foodAmount
    {
        get { return _foodAmount; }
        set
        {
            _foodAmount = value;
            if (_foodAmount <= 40)
            {
                foodStats.SetActive(true);
                isHeall2 = 0;
            }
            else foodStats.SetActive(false);
            if (_foodAmount > 140) isHeall2 = 1; else isHeall2 = 0;
            if (_foodAmount > 0)
            {
                uiFoodAmount = Mathf.Lerp(uiFoodAmount, foodAmount, Time.deltaTime * changeFactor);
                foodBar.fillAmount = uiFoodAmount / 200;
            }
            else
            {
                uiFoodAmount = 0;
                foodBar.fillAmount = uiFoodAmount / 200;
            }
        }
    }
    private float _waterAmount = 200;
    public float waterAmount
    {
        get { return _waterAmount; }
        set
        {
            _waterAmount = value;
            if (_waterAmount <= 40)
            {
                waterStats.SetActive(true);
                isHeall1 = 0;
            }
            else waterStats.SetActive(false);
            if (_waterAmount > 140) isHeall1 = 1; else isHeall1 = 0;
            if (_waterAmount > 0)
            {
                uiWaterAmount = Mathf.Lerp(uiWaterAmount, waterAmount, Time.deltaTime * changeFactor);
                waterBar.fillAmount = uiWaterAmount / 200;
            }
            else
            {
                uiWaterAmount = 0;
                waterBar.fillAmount = uiWaterAmount / 200;
            }
        }
    }
    private float _ÒoldAmount = 200;
    public float ÒoldAmount
    {
        get { return _ÒoldAmount; }
        set
        {

            _ÒoldAmount = value;
            if (_ÒoldAmount <= 40)
            {
                coldStats.SetActive(true);
                isHeall3 = 0;
            }
            else coldStats.SetActive(false);
            if (_ÒoldAmount > 140) isHeall3 = 1; else isHeall3 = 0;
            if (_ÒoldAmount > 0)
            {
                ui—oldAmount = Mathf.Lerp(ui—oldAmount, ÒoldAmount, Time.deltaTime * changeFactor);
                coldBar.fillAmount = ui—oldAmount / 200;
            }
            else
            {
                ui—oldAmount = 0;
                coldBar.fillAmount = ui—oldAmount / 200;
            }
        }
    }

    public Image healthBar, foodBar, waterBar, coldBar;

    private float uiHealthAmount = 200, uiFoodAmount = 200, uiWaterAmount = 200, ui—oldAmount = 200, changeFactor = 6f;
    public float secondsToEmptyFood, secondsToEmptyWater, secondsToEmptyHealth, secondsToEmpty—old;

    [Space]
    [Header("—ÎÓÊÌÓÒÚ¸")]
    public int Complicated = 1;
    public int IndexStartLuts = 0;

    public TMP_Text complicatedText;

    public bool izi, normal, hard, ultraHard, rou, neonStand;

    [Space]
    [Header("—Ú‡ÚÓ‚˚È Ì‡·Ó")]
    public bool NoStartLuts;
    public GameObject startLuts1, startLuts2, startLuts3, startLuts4;

    [Space]
    [Header("¡ÓÌˇ")]
    public int Armor;
    public int Water, Infected, StormArmor;
    private int _ColdArmor;
    public int ColdArmor
    {
        get { return _ColdArmor; }
        set
        {
            _ColdArmor = value;
            if (_sleepAIs == 1)
            {
                if (_ColdArmor > 0)
                {
                    if (_ColdArmor == 4) secondsToEmpty—old = 1500f;
                    if (_ColdArmor == 3) secondsToEmpty—old = 1200f;
                    if (_ColdArmor == 2) secondsToEmpty—old = 850f;
                    if (_ColdArmor == 1) secondsToEmpty—old = 600f;
                }
                else secondsToEmpty—old = 300f;
            }
            else
            {
                if (_ColdArmor > 0)
                {
                    if (_ColdArmor == 4) secondsToEmpty—old = 2000f;
                    if (_ColdArmor == 3) secondsToEmpty—old = 1500f;
                    if (_ColdArmor == 2) secondsToEmpty—old = 1200f;
                    if (_ColdArmor == 1) secondsToEmpty—old = 850f;
                }
                else secondsToEmpty—old = 600f;
            }
        }
    }
    private int _JumpPower;
    public int JumpPower
    {
        get { return _JumpPower; }
        set
        {
            _JumpPower = value;
            if (_JumpPower == 1) customCharacterController.jumpForce = 7; else customCharacterController.jumpForce = 5;
        }
    }
    private int _Speed;
    private int oldSpeed;
    public int Speed
    {
        get { return _Speed; }
        set
        {
            _Speed = value;
            SpeedCheck();
        }
    }

    public TMP_Text ArmorPointText, ColdArmorPointText, InfectedPointText, SpeedPointText;

    [Space]
    [Header("¬ÂÏˇ")]
    public float startTimeInfection, startTimeWeb; // ‚ÂÏˇ ‚ Ì‡˜‡ÎÂ
    private float _timeInfection, _timeWeb;
    public float timeInfection // ÚÂÍÛ˘ÂÂ ‚ÂÏˇ
    {
        get { return _timeInfection; }
        set
        {
            _timeInfection = value;
            if (isInfection == 1 && _timeInfection <= 0) // ÂÒÎË ÚÂÍÛ˘ÂÂ ‚ÂÏˇ ÏÂÌ¸¯Â ËÎË ‡‚ÌÓ 0 ÚÓ...
            {
                isInfection = 0;
                timeInfection = startTimeInfection; // Ì‡˜ËÌ‡ÂÏ ÓÚ˘∏Ú ‚ÂÏÂÌË ÒÌ‡˜‡Î‡
            }
        }
    }
    public float timeWeb // ÚÂÍÛ˘ÂÂ ‚ÂÏˇ
    {
        get { return _timeWeb; }
        set
        {
            _timeWeb = value;
            if (_timeWeb <= 0) // ÂÒÎË ÚÂÍÛ˘ÂÂ ‚ÂÏˇ ÏÂÌ¸¯Â ËÎË ‡‚ÌÓ 0 ÚÓ...
            {
                isWeb = 0;
                timeWeb = startTimeWeb; // Ì‡˜ËÌ‡ÂÏ ÓÚ˘∏Ú ‚ÂÏÂÌË ÒÌ‡˜‡Î‡
                Speed = oldSpeed;
                onceOldSpeed = false;
            }
        }
    }

    [Space]
    [Header("»‚ÂÌÚ˚")]
    public bool StormDamage;
    public GameObject SnowStorm, SandStorm;
    private int _Ivent;
    public int Ivent
    {
        get { return _Ivent; }
        set
        {
            _Ivent = value;
            if (_Ivent >= 3)
            {
                if (SnowBiom == 1)
                {
                    SnowStorm.SetActive(true);
                    if (ColdArmor < 3) StormDamage = true;
                }
                else
                {
                    SnowStorm.SetActive(false);
                    StormDamage = false;
                }
                if (SandBiom == 1)
                {
                    SandStorm.SetActive(true);
                    if (ColdArmor < 3) StormDamage = true;
                }
                else
                {
                    SandStorm.SetActive(false);
                    StormDamage = false;
                }
            }
            else
            {
                SandStorm.SetActive(false);
                SnowStorm.SetActive(false);
                StormDamage = false;
            }
        }
    }
    private int _SnowBiom;
    public int SnowBiom
    {
        get { return _SnowBiom; }
        set
        {
            _SnowBiom = value;
            if (_SnowBiom == 1 && Ivent >= 3)
            {
                SnowStorm.SetActive(true);
                if (ColdArmor < 3) StormDamage = true;
            }
            else
            {
                SnowStorm.SetActive(false);
                StormDamage = false;
            }
        }
    }
    private int _SandBiom;
    public int SandBiom
    {
        get { return _SandBiom; }
        set
        {
            _SandBiom = value;
            if (_SandBiom == 1 && Ivent >= 3)
            {
                SandStorm.SetActive(true);
                if (ColdArmor < 3) StormDamage = true;
            }
            else
            {
                SandStorm.SetActive(false);
                StormDamage = false;
            }
        }
    }

    [Space]
    [Header("¡ÛÎÎ˚")]
    public bool onceRegen;
    public bool isIn—old2, isIn—old3, onceOldSpeed; // isIn—old2 +2, isIn—old3 -1

    [Space]
    [Header("œÂÂÏÂÌÌ˚Â")]
    public int MapEnd;
    private int _isHeall1;
    public int isHeall1
    {
        get { return _isHeall1; }
        set
        {
            _isHeall1 = value;
            RegenereitCheck();
        }
    }
    private int _isHeall2;
    public int isHeall2
    {
        get { return _isHeall2; }
        set
        {
            _isHeall2 = value;
            RegenereitCheck();
        }
    }
    private int _isHeall3;
    public int isHeall3
    {
        get { return _isHeall3; }
        set
        {
            _isHeall3 = value;
            RegenereitCheck();
        }
    }
    private int _isInfection;
    public int isInfection
    {
        get { return _isInfection; }
        set
        {
            _isInfection = value;
            if (_isInfection == 1)
            {
                infectionEffect.SetActive(true);
                infectionStats.SetActive(true);
            }
            else
            {
                infectionEffect.SetActive(false);
                infectionStats.SetActive(false);
            }
            RegenereitCheck();
        }
    }
    private int _isWeb;
    public int isWeb
    {
        get { return _isWeb; }
        set
        {
            _isWeb = value;
            if (_isWeb == 1)
            {
                if (onceOldSpeed == false)
                {
                    oldSpeed = Speed;
                    onceOldSpeed = true;
                }
                Speed = -3;
                webStats.SetActive(true);
                if (Infected == 1) Speed = -2;
            }
            else webStats.SetActive(false);
        }
    }
    private int _isCheckWeb;
    public int isCheckWeb
    {
        get { return _isCheckWeb; }
        set
        {
            _isCheckWeb = value;
            if (_isCheckWeb == 1) timeWeb = startTimeWeb; // Ì‡˜ËÌ‡ÂÏ ÓÚ˘∏Ú ‚ÂÏÂÌË ÒÌ‡˜‡Î‡
        }
    }
    private int _isCheckInfection;
    public int isCheckInfection
    {
        get { return _isCheckInfection; }
        set
        {
            _isCheckInfection = value;
            if (_isCheckInfection == 1) timeInfection = startTimeInfection; // Ì‡˜ËÌ‡ÂÏ ÓÚ˘∏Ú ‚ÂÏÂÌË ÒÌ‡˜‡Î‡
        }
    }
    private int _sleepAIs;
    public int _sleepAI
    {
        get { return _sleepAIs; }
        set
        {
            _sleepAIs = value;
            if (_sleepAIs == 1)
            {
                if (ColdArmor > 0)
                {
                    if (ColdArmor == 4) secondsToEmpty—old = 1500f;
                    if (ColdArmor == 3) secondsToEmpty—old = 1200f;
                    if (ColdArmor == 2) secondsToEmpty—old = 850f;
                    if (ColdArmor == 1) secondsToEmpty—old = 600f;
                }
                else secondsToEmpty—old = 300f;
            }
            else
            {
                if (ColdArmor > 0)
                {
                    if (ColdArmor == 4) secondsToEmpty—old = 2000f;
                    if (ColdArmor == 3) secondsToEmpty—old = 1500f;
                    if (ColdArmor == 2) secondsToEmpty—old = 1200f;
                    if (ColdArmor == 1) secondsToEmpty—old = 850f;
                }
                else secondsToEmpty—old = 600f;
            }
        }
    }
    private int _isDamage;
    public int isDamage
    {
        get { return _isDamage; }
        set
        {
            _isDamage = value;
            if (_isDamage == 1) damageEffect.SetActive(true);
            RegenereitCheck();
        }
    }
    private int _isFire;
    public int isFire
    {
        get { return _isFire; }
        set
        {
            _isFire = value;
            if (_isFire == 1) fireStats.SetActive(true); else fireStats.SetActive(false);
            RegenereitCheck();
        }
    }
    private int _isWater;
    public int isWater
    {
        get { return _isWater; }
        set
        {
            _isWater = value;
            SpeedCheck();
        }
    }

    public bool NoActiveScript;

    void Awake()
    {
        if(NoActiveScript == false)
        {
            mainCamera = Camera.main;

            daytime = FindObjectOfType<Daytime>().GetComponent<Daytime>();

            daytime.indicators = this;
            daytime.TexnoSlot = customCharacterController.inventoryManager.verstAddItem.texnoSlot;

            daytime.DayTextObject = DayTextObject;
            daytime.DayText = DayText;
            daytime.DayTextObject = DayText.gameObject;

            healthBar.fillAmount = healthAmount / 200;
            foodBar.fillAmount = foodAmount / 200;
            waterBar.fillAmount = waterAmount / 200;
            coldBar.fillAmount = ÒoldAmount / 200;

            if (PlayerPrefs.HasKey("Complicated")) Complicated = PlayerPrefs.GetInt("Complicated"); else Complicated = 1;
            if (PlayerPrefs.HasKey("IndexStartLut")) IndexStartLuts = PlayerPrefs.GetInt("IndexStartLut"); else IndexStartLuts = 0;

            StartIndicators();
            ComplexityEdit();
            EguipComplexity();
            LutIndex();

            complicatedText.text = Complicated.ToString();
            timeInfection = startTimeInfection; // Ò‡ÁÛ ÔËÒ‚‡Ë‚‡ÂÏ ÚÂÍÛ˘ÂÂ ‚ÂÏˇ Í Ì‡˜‡Î¸ÌÓÏÛ ÚÓÂÒÚ¸ Ì‡˜ËÌ‡ÂÏ ÓÚ˘∏Ú Ò Ì‡˜‡Î‡
            timeWeb = startTimeWeb; // Ò‡ÁÛ ÔËÒ‚‡Ë‚‡ÂÏ ÚÂÍÛ˘ÂÂ ‚ÂÏˇ Í Ì‡˜‡Î¸ÌÓÏÛ ÚÓÂÒÚ¸ Ì‡˜ËÌ‡ÂÏ ÓÚ˘∏Ú Ò Ì‡˜‡Î‡
        }
    }

    void LateUpdate()
    {
        if (MapEnd == 1) healthAmount -= 20 * Time.deltaTime;
        if (_isInfection == 1)
        {
            healthAmount -= 10 * Time.deltaTime;
            if (timeInfection > 0) // ÂÒÎË ÚÂÍÛ˘ÂÂ ‚ÂÏˇ ÏÂÌ¸¯Â ËÎË ‡‚ÌÓ 0 ÚÓ...
            {
                timeInfection -= Time.deltaTime; // Ò˜ËÚ‡ÂÏ Â‡Î¸ÌÓÂ ‚ÂÏˇ
                if (Infected == 1) timeInfection -= Time.deltaTime; // Ò˜ËÚ‡ÂÏ Â‡Î¸ÌÓÂ ‚ÂÏˇ
            }
        }
        if (isWeb == 1 && timeWeb > 0)
        {
            timeWeb -= Time.deltaTime; // Ò˜ËÚ‡ÂÏ Â‡Î¸ÌÓÂ ‚ÂÏˇ
            if (Infected == 1) timeWeb -= Time.deltaTime; // Ò˜ËÚ‡ÂÏ Â‡Î¸ÌÓÂ ‚ÂÏˇ
        }
        if (foodAmount > 0) foodAmount -= 200 / secondsToEmptyFood * Time.deltaTime;
        if (ÒoldAmount > 0) ÒoldAmount -= 200 / secondsToEmpty—old * Time.deltaTime;
        if (waterAmount > 0) waterAmount -= 200 / secondsToEmptyWater * Time.deltaTime;
        if (foodAmount <= 0 || waterAmount <= 0 || ÒoldAmount <= 0) healthAmount -= 200 / secondsToEmptyHealth * Time.deltaTime;
    }

    public void SpeedCheck()
    {
        if (_isWater == 1)
        {
            ChangeWaterAmount(200);
            if (Water == 1)
            {
                customCharacterController.walkingSpeed = 8f;
                customCharacterController.runningSpeed = 12f;
                customCharacterController.anim.speed = 1.1f;
            }
            else
            {
                customCharacterController.walkingSpeed = 6f;
                customCharacterController.runningSpeed = 9f;
                customCharacterController.anim.speed = 0.9f;
            }
        }
        else
        {
            if (_Speed == 0)
            {
                customCharacterController.walkingSpeed = 7f;
                customCharacterController.runningSpeed = 11f;
                customCharacterController.anim.speed = 1f;
            }
            else
            {
                if (_Speed == 1)
                {
                    customCharacterController.walkingSpeed = 8f;
                    customCharacterController.runningSpeed = 12f;
                    customCharacterController.anim.speed = 1.1f;
                }
                if (_Speed == -1)
                {
                    customCharacterController.walkingSpeed = 6f;
                    customCharacterController.runningSpeed = 10f;
                    customCharacterController.anim.speed = 0.9f;
                }
                if (_Speed == -2)
                {
                    customCharacterController.walkingSpeed = 1f;
                    customCharacterController.runningSpeed = 3f;
                    customCharacterController.anim.speed = 0.85f;
                }
                if (_Speed == -3)
                {
                    customCharacterController.walkingSpeed = 0f;
                    customCharacterController.runningSpeed = 2f;
                    customCharacterController.anim.speed = 0.75f;
                }
            }
        }
    }

    public void RegenereitCheck()
    {
        if (_isDamage == 0 && _isFire == 0 && _isInfection == 0 && _isHeall1 == 1 && _isHeall2 == 1 && _isHeall3 == 1)
        {
            if (healthAmount < 200)
            {
                hillStats.SetActive(true);
                healthAmount += 2 * Time.deltaTime;
                healEffect.SetActive(true);
                onceRegen = true;
            }
            else if (healthAmount >= 200)
            {
                hillStats.SetActive(false);
                healEffect.SetActive(false);
                onceRegen = false;
            }
        }
        else if (onceRegen == true)
        {
            hillStats.SetActive(false);
            healEffect.SetActive(false);
            onceRegen = false;
        }
    }

    public void ChangeFoodAmount(float changeValue)
    {
        if (foodAmount + changeValue > 200) foodAmount = 200; else foodAmount += changeValue;
    }
    public void ChangeWaterAmount(float changeValue)
    {
        if (waterAmount + changeValue > 200) waterAmount = 200; else waterAmount += changeValue;
    }
    public void ChangeHealthAmount(float changeValue)
    {
        if (healthAmount + changeValue > 200) healthAmount = 200; else healthAmount += changeValue;
    }
    public void ChangeColdAmount(float changeValue)
    {
        if (ÒoldAmount + changeValue > 200) ÒoldAmount = 200; else ÒoldAmount += changeValue;
    }

    public void ComplexityEdit()
    {
        if (Complicated == 0) izi = true;
        if (Complicated == 1) normal = true;
        if (Complicated == 2) hard = true;
        if (Complicated == 3) ultraHard = true;
        if (Complicated == 4) rou = true;
        if (Complicated == 5) neonStand = true;
    }

    public void EguipComplexity()
    {
        if (izi == true)
        {
            secondsToEmptyFood = 420f;
            secondsToEmptyWater = 400f;
            secondsToEmptyHealth = 80f;
            secondsToEmpty—old = 600f;
            startTimeInfection = 6f;
            startTimeWeb = 2f;
        }
        if (normal == true)
        {
            secondsToEmptyFood = 340f;
            secondsToEmptyWater = 320f;
            secondsToEmptyHealth = 60f;
            secondsToEmpty—old = 600f;
            startTimeInfection = 8f;
            startTimeWeb = 3f;
        }
        if (hard == true)
        {
            secondsToEmptyFood = 280f;
            secondsToEmptyWater = 260f;
            secondsToEmptyHealth = 45f;
            secondsToEmpty—old = 600f;
            startTimeInfection = 8f;
            startTimeWeb = 3f;
        }
        if (ultraHard == true)
        {
            secondsToEmptyFood = 220f;
            secondsToEmptyWater = 200f;
            secondsToEmptyHealth = 30f;
            secondsToEmpty—old = 600f;
            startTimeInfection = 10f;
            startTimeWeb = 4f;
        }
        if (rou == true)
        {
            secondsToEmptyFood = 340f;
            secondsToEmptyWater = 320f;
            secondsToEmptyHealth = 60f;
            secondsToEmpty—old = 600f;
            startTimeInfection = 8f;
            startTimeWeb = 3f;
            RouSpawner = daytime.RouSpawner;
            RouSpawner.SetActive(true);
        }
        if (neonStand == true)
        {
            secondsToEmptyFood = 460f;
            secondsToEmptyWater = 420f;
            secondsToEmptyHealth = 80f;
            secondsToEmpty—old = 99000f;
            startTimeInfection = 4f;
            startTimeWeb = 1f;
        }
    }

    public void LutIndex()
    {
        if (IndexStartLuts == 0) NoStartLuts = true;
        if (NoStartLuts == false)
        {
            if (IndexStartLuts == 1) startLuts1.SetActive(true);
            if (IndexStartLuts == 2) startLuts2.SetActive(true);
            if (IndexStartLuts == 3) startLuts3.SetActive(true);
            if (IndexStartLuts == 4) startLuts4.SetActive(true);
        }
        else
        {
            startLuts1.SetActive(false);
            startLuts2.SetActive(false);
            startLuts3.SetActive(false);
            startLuts4.SetActive(false);
        }
    }

    public void StartIndicators()
    {
        customCharacterController.jumpForce = 5f;
        customCharacterController.walkingSpeed = 7f;
        customCharacterController.currentSpeed = 0f;
        customCharacterController.runningSpeed = 11f;
        customCharacterController.swimmingSpeed = 5f;

        healthBar.fillAmount = healthAmount / 200;
        foodBar.fillAmount = foodAmount / 200;
        waterBar.fillAmount = waterAmount / 200;
        coldBar.fillAmount = ÒoldAmount / 200;

        healthAmount = 200;
        foodAmount = 200;
        waterAmount = 200;
        ÒoldAmount = 200;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}