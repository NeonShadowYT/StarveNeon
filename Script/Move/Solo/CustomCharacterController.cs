using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class CustomCharacterController : NetworkBehaviour //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{
    [Space]
    [Header("Скрипты")]
    public InventoryManager inventoryManager;
    public CraftManager craftManager;
    public QuickslotInventory quickslotInventory;
    public Indicators indicators;

    public MyFloater myFloater;
    public TurnController turnController;

    public DynamicResolution dynamicResolution;
    public FixedJoystick fixedJoystick;

    private Transform currentGatherItem;
    private GatherResources currentGatherResources;

    public ClothAdder ClothAdder;

    public Animator anim;
    public Rigidbody rig;

    public NetworkMatch networkMatch;

    [Space]
    [Header("Переменные")]
    public float jumpForce = 5f;
    public float walkingSpeed = 7f, runningSpeed = 11f, swimmingSpeed = 5f, currentSpeed, _waterLevel = -5f, horizontal, vertical, lerpMultiplier = 7f, changeRunTime = 2f, animationInterpolation = 1f;

    [Space]
    [Header("Объекты")]
    public GameObject Canvas;
    public GameObject MobaillMenu, runGroundEffect, runSandEffect, runWaterEffect, runSnowEffect, runInfectionEffect, runADEffect, runRockEffect, fixedJoysticks, skinsManager, playerRenderModel, Cvc1, Cvc3, CanvasName, InpectionCamera, InspectionCanvas;

    [Space]
    [Header("Разное")]
    public Transform mainCamera;
    public Transform mTransform, _neckObj, _groundObj;

    public TMP_Text textNames;
    private int noRunEffectNumbers, TurnNumbers, DynamicRes = 1; // 0 работает 1 не работаит

    private bool CanJump;

    [Space]
    public GameObject LocalObj;
    public Transform CMvcam1, CMvcam2;

    [Space]
    [Header("Булы")]
    public bool PC;
    public bool isNeckUnderWater, ground, sand, snow, infection, ad, rock, run, once, noRunEffect;

    void Start()
    {
        if (networkMatch != null)
        {
            if (hasAuthority)
            {
                CanvasName.SetActive(false);
            }
            else
            {
                Destroy(LocalObj);
            }
        }
        else CanvasName.SetActive(false);

        mainCamera.parent = null;
        CMvcam1.parent = null;
        CMvcam2.parent = null;

        if (PC == true)
        {
            MobaillMenu.SetActive(false);
            fixedJoysticks.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked; // Прекрепляем курсор к середине экрана
            Cursor.visible = false; // и делаем его невидимым
        }
        if (PC == false)
        {
            MobaillMenu.SetActive(true);
            fixedJoysticks.SetActive(true);
        }
        if (PlayerPrefs.HasKey("RunEffectSettings")) noRunEffectNumbers = PlayerPrefs.GetInt("RunEffectSettings"); else noRunEffectNumbers = 0; // 1 всё не работает, 0 всё работает
        if (noRunEffectNumbers != 0) noRunEffect = true; else InvokeRepeating(nameof(RunEffectManager), changeRunTime, changeRunTime);
        if (PlayerPrefs.HasKey("TurnSettings")) TurnNumbers = PlayerPrefs.GetInt("TurnSettings"); else TurnNumbers = 0; // 1 всё не работает, 0 всё работает
        if (TurnNumbers != 0) turnController.enabled = false;
        if (PlayerPrefs.HasKey("DynamicResSave")) DynamicRes = PlayerPrefs.GetInt("DynamicResSave"); else DynamicRes = 1;
        if (DynamicRes != 0) dynamicResolution.enabled = false;
    }

    void Run()
    {
        run = true;

        animationInterpolation = Mathf.Lerp(animationInterpolation, 1.5f, Time.deltaTime * 3);
        anim.SetFloat("x", (horizontal + Input.GetAxis("Horizontal")) * animationInterpolation);
        anim.SetFloat("y", (vertical + Input.GetAxis("Vertical")) * animationInterpolation);
        currentSpeed = Mathf.Lerp(currentSpeed, runningSpeed, Time.deltaTime * 3);
    }

    public void Walk()
    {
        run = false;
        if (noRunEffect == false && once == true) NoRunEffect();

        animationInterpolation = Mathf.Lerp(animationInterpolation, 1f, Time.deltaTime * 3); // Mathf.Lerp - отвчает за то, чтобы каждый кадр число animationInterpolation(в данном случае) приближалось к числу 1 со скоростью Time.deltaTime * 3.
        anim.SetFloat("x", (horizontal + Input.GetAxis("Horizontal")) * animationInterpolation);
        anim.SetFloat("y", (vertical + Input.GetAxis("Vertical")) * animationInterpolation);
        currentSpeed = Mathf.Lerp(currentSpeed, walkingSpeed, Time.deltaTime * 3); // Time.deltaTime - это время между этим кадром и предыдущим кадром. Это позволяет плавно переходить с одного числа до второго НЕЗАВИСИМО ОТ КАДРОВ В СЕКУНДУ (FPS)!!!
    }

    private void Update()
    {
        if (PC == false)
        {
            horizontal = Mathf.Lerp(horizontal, fixedJoystick.Horizontal, Time.deltaTime * lerpMultiplier);
            vertical = Mathf.Lerp(vertical, fixedJoystick.Vertical, Time.deltaTime * lerpMultiplier);
            Run();
        }
        else
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)) if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) Walk(); else Run(); else Walk();// Зажаты ли еще кнопки A S D? Если да, то мы идем пешком. Если нет, то тогда бежим! // Если W & Shift не зажаты, то мы просто идем пешком if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) Walk(); else Run();// Зажаты ли еще кнопки A S D? Если да, то мы идем пешком. Если нет, то тогда бежим! // Если W & Shift не зажаты, то мы просто идем пешком// Зажаты ли кнопки W и Shift?

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (quickslotInventory.activeSlot != null && quickslotInventory.activeSlot.item != null && quickslotInventory.activeSlot.item.itemType == ItemType.Instrument && inventoryManager.isOpened == false && craftManager.isOpened == false && inventoryManager.isOpenedEsc == false && inventoryManager.isOpenedMap == false)
                {
                    if (quickslotInventory.activeSlot.item.speedAttacke == SpeedAttacke.Medium) anim.SetBool("SpeedMedium", true); else anim.SetBool("SpeedMedium", false);
                    if (quickslotInventory.activeSlot.item.speedAttacke == SpeedAttacke.Low) anim.SetBool("SpeedLow", true); else anim.SetBool("SpeedLow", false);
                    if (quickslotInventory.activeSlot.item.speedAttacke == SpeedAttacke.Max) anim.SetBool("SpeedMax", true); else anim.SetBool("SpeedMax", false);

                    anim.SetBool("Hit", true);
                }
                else anim.SetBool("Hit", false);
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                anim.SetBool("Hit", false);
                anim.SetBool("BowAttacke", false);
            }
        }
        mTransform.rotation = Quaternion.Euler(mTransform.rotation.eulerAngles.x, mainCamera.rotation.eulerAngles.y, mTransform.rotation.eulerAngles.z); // Устанавливаем поворот персонажа когда камера поворачивается 
    }

    void FixedUpdate()
    {
        Vector3 camF = mainCamera.forward; // Здесь мы задаем движение персонажа в зависимости от направления в которое смотрит камера
        Vector3 camR = mainCamera.right; // Сохраняем направление вперед и вправо от камеры

        camF.y = 0; // Чтобы направления вперед и вправо не зависили от того смотрит ли камера вверх или вниз, иначе когда мы смотрим вперед, персонаж будет идти быстрее чем когда смотрит вверх или вниз
        camR.y = 0; // Можете сами проверить что будет убрав camF.y = 0 и camR.y = 0 :)

        Vector3 movingVector;
        movingVector = Vector3.ClampMagnitude(camF.normalized * (vertical + Input.GetAxis("Vertical")) * currentSpeed + camR.normalized * (horizontal + Input.GetAxis("Horizontal")) * currentSpeed, currentSpeed); // Тут мы умножаем наше нажатие на кнопки W & S на направление камеры вперед и прибавляем к нажатиям на кнопки A & D и умножаем на направление камеры вправо

        rig.velocity = new Vector3(movingVector.x, rig.velocity.y, movingVector.z); // Здесь мы двигаем персонажа! Устанавливаем движение только по x & z потому что мы не хотим чтобы наш персонаж взлетал в воздух
        rig.angularVelocity = Vector3.zero; // У меня был баг, что персонаж крутился на месте и это исправил с помощью этой строки
    }

    public void OnScrin()
    {
        if (Input.GetKeyDown(KeyCode.P)) ScreenCapture.CaptureScreenshot("Starve Neon.png");
    }

    public void OnCursor()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None; // Прекрепляем курсор к середине экрана
                Cursor.visible = true; // и делаем его невидимым
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked; // Прекрепляем курсор к середине экрана
                Cursor.visible = false; // и делаем его невидимым
            }
        }
    }

    public void OnJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CanJump == true) rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); //Прыгаем
        }
    }

    public void Hit()
    {
        foreach (Transform item in quickslotInventory.allWeapons)
        {
            if (item.gameObject.activeSelf)
            {
                if (currentGatherItem != item)
                {
                    currentGatherItem = item;
                    currentGatherResources = item.GetComponent<GatherResources>();
                }
                currentGatherResources.GatherResource();
                craftManager.currentCraftItem.FillItemDetails();
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("WeatherSnow") || col.CompareTag("WeatherSand"))
        {
            if (indicators.StormDamage && indicators.StormArmor < 6)
            {
                if (indicators.StormArmor > 0)
                {
                    if (indicators.StormArmor == 1) indicators.healthAmount -= 18 * Time.deltaTime;
                    if (indicators.StormArmor == 2) indicators.healthAmount -= 14 * Time.deltaTime;
                    if (indicators.StormArmor == 3) indicators.healthAmount -= 10 * Time.deltaTime;
                    if (indicators.StormArmor == 4) indicators.healthAmount -= 6 * Time.deltaTime;
                    if (indicators.StormArmor == 5) indicators.healthAmount -= 2 * Time.deltaTime;
                }
                else indicators.healthAmount -= 22 * Time.deltaTime;
            }
        }
        if (col.CompareTag("Damage"))
        {
            if (indicators.Armor > 0)
            {
                if (indicators.Armor == 1) indicators.healthAmount -= 36 * Time.deltaTime;
                if (indicators.Armor == 2) indicators.healthAmount -= 32 * Time.deltaTime;
                if (indicators.Armor == 3) indicators.healthAmount -= 28 * Time.deltaTime;
                if (indicators.Armor == 4) indicators.healthAmount -= 24 * Time.deltaTime;
                if (indicators.Armor == 5) indicators.healthAmount -= 22 * Time.deltaTime;
                if (indicators.Armor == 6) indicators.healthAmount -= 20 * Time.deltaTime;
            }
            else indicators.healthAmount -= 40 * Time.deltaTime;
        }
        if (col.CompareTag("Fire"))
        {
            if (indicators.сoldAmount >= 200 && indicators.ColdArmor != 4)
            {
                indicators.healthAmount -= 8 * Time.deltaTime;
            }
            if (indicators.сoldAmount <= 200) indicators.сoldAmount += 10 * Time.deltaTime;
        }
        if (col.CompareTag("Cold"))
        {
            if (indicators.ColdArmor < 3 && indicators.сoldAmount > 0)
            {
                if (indicators.ColdArmor == 2) indicators.сoldAmount -= 4 * Time.deltaTime;
                if (indicators.ColdArmor == 1) indicators.сoldAmount -= 6 * Time.deltaTime;
                if (indicators.ColdArmor == 0) indicators.сoldAmount -= 8 * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other) //if (Multiplayer == true) if (!_photonView.IsMine) return;
    {
        if (other.gameObject.layer == 4) //Входим ли мы в воду ?
        {
            indicators.isWater = 1;
            once = true;
            //_waterLevel = other.transform.position.y; //+ other.GetComponent<BoxCollider>().size.y/2;
            if (noRunEffect == false) NoRunEffect();
        }
        if (other.CompareTag("Damage")) indicators.isDamage = 1;//Входим ли мы в зону атаки мобов ?
        if (other.CompareTag("Infected")) //Входим ли мы в зону заражения ?
        {
            indicators.isInfection = 1;
            indicators.isCheckInfection = 1;
        }
        if (other.CompareTag("Web")) //Входим ли мы в паутину ?
        {
            indicators.isWeb = 1;
            indicators.isCheckWeb = 1;
        }
        if (other.CompareTag("Fire")) indicators.isInСold2 = true;//Входим ли мы в зону тепла ?
        if (other.CompareTag("Cold")) indicators.isInСold3 = true;//Входим ли мы в зону холода ?
        if (other.CompareTag("WeatherSnow")) indicators.SnowBiom = 1;
        if (other.CompareTag("WeatherSand")) indicators.SandBiom = 1;
        if (other.CompareTag("MapEnd")) indicators.MapEnd = 0;
    }

    private void OnTriggerExit(Collider other) //if (Multiplayer == true) if (!_photonView.IsMine) return;
    {
        if (other.gameObject.layer == 4)
        {
            indicators.isWater = 0; //Выходим из этих зон?
            //_waterLevel = -1; //isNeckUnderWater = false;
        }
        if (other.CompareTag("Damage")) indicators.isDamage = 0;
        if (other.CompareTag("Infected")) indicators.isCheckInfection = 0;//Выходим из зоны заражения ?
        if (other.CompareTag("Web")) indicators.isCheckWeb = 0;// Выходим из паутины ?
        if (other.CompareTag("Fire"))
        {
            indicators.isInСold2 = false;
            indicators.isFire = 0;
        }
        if (other.CompareTag("Cold")) indicators.isInСold3 = false;
        if (other.CompareTag("WeatherSnow")) indicators.SnowBiom = 0;
        if (other.CompareTag("WeatherSand")) indicators.SandBiom = 0;
        if (other.CompareTag("MapEnd")) indicators.MapEnd = 1;
    }

    private void OnCollisionEnter(Collision collision) //if (Multiplayer == true) if (!_photonView.IsMine) return;
    {
        if (collision.collider.CompareTag("Ground")) //Стоим ли мы на земле?
        {
            CanJump = true;
            ground = true;
        }
        if (collision.collider.CompareTag("AD")) //Стоим ли мы на адской земле?
        {
            CanJump = true;
            ad = true;
        }
        if (collision.collider.CompareTag("Rock")) //Стоим ли мы на камнях?
        {
            CanJump = true;
            rock = true;
        }
        if (collision.collider.CompareTag("Snow")) //Стоим ли мы на снегу?
        {
            CanJump = true;
            snow = true;
        }
        if (collision.collider.CompareTag("Sand")) //Стоим ли мы на песке?
        {
            CanJump = true;
            sand = true;
        }
        if (collision.collider.CompareTag("Infection")) //Стоим ли мы на заражённой земле?
        {
            CanJump = true;
            infection = true;
        }
        if (collision.collider.CompareTag("Lava")) indicators.isDamage = 1;//Если стоим на лаве то получаем урон
        if (collision.collider.CompareTag("Infected")) //Если стоим на зоне заражения
        {
            indicators.isInfection = 1;
            indicators.isCheckInfection = 1;
        }
        if (collision.collider.CompareTag("Web")) //Если стоим на паутине
        {
            indicators.isWeb = 1;
            indicators.isCheckWeb = 1;
        }
    }

    private void OnCollisionExit(Collision collision) // (Multiplayer == true) if (!_photonView.IsMine) return;
    {
        if (collision.collider.CompareTag("Ground")) //Прыгнули
        {
            CanJump = false;
            ground = false;
        }
        if (collision.collider.CompareTag("AD"))
        {
            CanJump = false;
            ad = false;
        }
        if (collision.collider.CompareTag("Rock"))
        {
            CanJump = false;
            rock = false;
        }
        if (collision.collider.CompareTag("Snow"))
        {
            CanJump = false;
            snow = false;
        }
        if (collision.collider.CompareTag("Sand"))
        {
            CanJump = false;
            sand = false;
        }
        if (collision.collider.CompareTag("Infection"))
        {
            CanJump = false;
            infection = false;
        }
        if (collision.collider.CompareTag("Lava")) indicators.isDamage = 0;//Вышли из лавы
        if (collision.collider.CompareTag("Infected")) indicators.isCheckInfection = 0;//Вышли из зоны заражения
        if (collision.collider.CompareTag("Web")) indicators.isCheckWeb = 0; //Вышли из паутины
    }

    public void MobailHit()
    {
        anim.SetBool("Hit", false); //Этот метод произходит при нажатии на кнопку удара на телефоне. Отключение чтобы перс не бил бесконечно

        if (quickslotInventory.activeSlot != null && quickslotInventory.activeSlot.item != null && quickslotInventory.activeSlot.item.itemType == ItemType.Instrument && inventoryManager.isOpened == false && craftManager.isOpened == false && inventoryManager.isOpenedEsc == false && inventoryManager.isOpenedMap == false)
        {
            if (quickslotInventory.activeSlot.item.speedAttacke == SpeedAttacke.Medium) anim.SetBool("SpeedMedium", true); else anim.SetBool("SpeedMedium", false);
            if (quickslotInventory.activeSlot.item.speedAttacke == SpeedAttacke.Low) anim.SetBool("SpeedLow", true); else anim.SetBool("SpeedLow", false);
            if (quickslotInventory.activeSlot.item.speedAttacke == SpeedAttacke.Max) anim.SetBool("SpeedMax", true); else anim.SetBool("SpeedMax", false);

            anim.SetBool("Hit", true);
        }
        else anim.SetBool("Hit", false);
    }

    public void RunEffectManager()
    {
        if (run == true && CanJump == true)
        {
            once = true;

            if (ground == true) runGroundEffect.SetActive(true); else runGroundEffect.SetActive(false);
            if (sand == true) runSandEffect.SetActive(true); else runSandEffect.SetActive(false);
            if (snow == true) runSnowEffect.SetActive(true); else runSnowEffect.SetActive(false);
            if (infection == true) runInfectionEffect.SetActive(true); else runInfectionEffect.SetActive(false);
            if (ad == true) runADEffect.SetActive(true); else runADEffect.SetActive(false);
            if (rock == true) runRockEffect.SetActive(true); else runRockEffect.SetActive(false);
        }
    }

    public void NoRunEffect()
    {
        runGroundEffect.SetActive(false);
        runSandEffect.SetActive(false);
        runWaterEffect.SetActive(false);
        runSnowEffect.SetActive(false);
        runInfectionEffect.SetActive(false);
        runADEffect.SetActive(false);
        runRockEffect.SetActive(false);
        once = false;
    }

    public void AnimaAddItem() => anim.SetBool("addItem", false);
    public void EndSmenaItem() => anim.SetBool("Smena", false);

    public void MobHitAttacke(float damage)
    {
        if (indicators.Armor > 0)
        {
            if (indicators.Armor == 1) indicators.healthAmount -= (damage - 1);
            if (indicators.Armor == 2) indicators.healthAmount -= (damage - 2);
            if (indicators.Armor == 3) indicators.healthAmount -= (damage - 3);
            if (indicators.Armor == 4) indicators.healthAmount -= (damage - 4);
            if (indicators.Armor == 5) indicators.healthAmount -= (damage - 5);
            if (indicators.Armor == 6) indicators.healthAmount -= (damage - 6);
        }
        else indicators.healthAmount -= damage;
        indicators.damageEffect.SetActive(true);
    }
}