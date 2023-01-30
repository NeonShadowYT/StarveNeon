using EnvSpawn;
using UnityEngine;
public class MobStats : MonoBehaviour
{
    private Indicators indicators;
    private AIManager aIManager;
    private QuastGame quastGame;

    public MobScriptableObject mobScriptableObject;

    [Space]
    [Header("«доровье")]
    private float _health;
    public float health
    {
        get { return _health; }
        set
        {
            _health = value;
            if (aIManager != null && aIManager.onceDamage == true)
            {
                if(aIManager.onceDamage == true)
                {
                    if (aIManager.DamageForAgreAI == true)
                    {
                        if (_health != mobScriptableObject.Health)
                        {
                            aIManager.AgreAI = true;
                            aIManager.PanicAI = false;
                            aIManager.onceDamage = false;
                        }
                    }
                    if (aIManager.HealthForPanicAI == true)
                    {
                        if (_health <= 30)
                        {
                            aIManager.PanicAI = true;
                            aIManager.onceDamage = false;
                        }
                    }
                }
                if (aIManager.FractionAI == true && aIManager.FractionNPS != null && health != mobScriptableObject.Health) aIManager.FractionNPS.myFraction.FractionReputation -= 10;
            }
            if (health <= 0) StoneGathered();
        }
    }

    [Space]
    public float destroyTime = 2f;

    [Space]
    [Header("”рон")]
    private int StartDamage;

    [Space]
    [Header("–азное")]
    private Transform resourceSpawer, MyTransform;
    private EnviroSpawn_CS enviroSpawn;
    [SerializeField] private string spawnerName = "TreeSpawner";

    public GameObject forDestroy;
    private Animator anim;

    [Space]
    public bool arena = false;//нужно чтобы при уничтожении врага он не респавнилс€ занова
    public bool neobMob = false;

    public void Start()
    {
        if (aIManager == null) aIManager = GetComponent<AIManager>();
        if (quastGame == null) quastGame = FindObjectOfType<QuastGame>();
        if (mobScriptableObject == null) mobScriptableObject = aIManager.mobScriptableObject;

        if (arena == false)
        {
            if (resourceSpawer == null)
            {
                resourceSpawer = GameObject.Find(spawnerName).transform;
                enviroSpawn = resourceSpawer.GetComponent<EnviroSpawn_CS>();
            }
        }

        if (anim == null) anim = GetComponent<Animator>();
        if (indicators == null) indicators = FindObjectOfType<Indicators>();

        health = mobScriptableObject.Health;
        StartDamage = mobScriptableObject.Damage;

        MyTransform = gameObject.transform;
    }

    public void TreeFall()
    {
        Rigidbody rig = gameObject.AddComponent<Rigidbody>();
        rig.isKinematic = false;
        rig.useGravity = true;
        rig.mass = 200;
        rig.constraints = RigidbodyConstraints.FreezeRotationY;

        RespawnResource();
        Destroy(gameObject, destroyTime);
    }

    public void StoneGathered()
    {
        if (aIManager != null) aIManager.AIStop();

        for (int i = 0; i < quastGame.AcceptQuastList.Count; i++)
        {
            if (quastGame.AcceptQuastList[i].QuastMob == mobScriptableObject)
            {
                quastGame.quastAmount[i] += 1;

                if (quastGame.quastAmount[i] == quastGame.AcceptQuastList[i].Amount)
                {
                    Debug.Log("¬ыполнено задание с мобами");
                    quastGame.AddQuastReward(i);
                }
            }
        }

        if (anim != null)
        {
            anim.SetBool("Death", true);
        }
        else
        {
            Destroys();
        }
    }

    public void RespawnResource()
    {
        float randomX = Random.Range(resourceSpawer.position.x - enviroSpawn.dimensions.x / 2, resourceSpawer.position.x + enviroSpawn.dimensions.x / 2);
        float randomY = Random.Range(resourceSpawer.position.z - enviroSpawn.dimensions.y / 2, resourceSpawer.position.z + enviroSpawn.dimensions.y / 2);

        Vector3 rayPos = new Vector3(randomX, 100, randomY);
        RaycastHit hit;

        if (Physics.SphereCast(rayPos, 2, Vector3.down, out hit, 200, enviroSpawn.avoidMask)) //enviroSpawn.avoidMask можно добавить если не хочешь делать мобов которые спавн€тс€ на деревь€х
        {
            MyTransform.position = hit.point;

            anim.SetBool("Death", false);

            health = mobScriptableObject.Health;
            StartDamage = mobScriptableObject.Damage;

            aIManager.AIWalk();

            gameObject.SetActive(true);
        }
    }

    public void HitForMe(float damage)
    {
        if (mobScriptableObject.FireArmor < 1) health -= damage;
        else
        {
            if (mobScriptableObject.FireArmor == 1) health -= damage / 1.25f;
            if (mobScriptableObject.FireArmor == 2) health -= damage / 1.5f;
            if (mobScriptableObject.FireArmor == 3) health -= damage / 1.75f;
            if (mobScriptableObject.FireArmor == 4) health -= damage / 2f;
        }
    }

    public void AtackePlayer()
    {
        if (indicators == null) indicators = FindObjectOfType<Indicators>();

        StartDamage = mobScriptableObject.Damage;

        if (indicators.Armor == 6) StartDamage -= 25;
        if (indicators.Armor == 5) StartDamage -= 23;
        if (indicators.Armor == 4) StartDamage -= 19;
        if (indicators.Armor == 3) StartDamage -= 13;
        if (indicators.Armor == 2) StartDamage -= 8;
        if (indicators.Armor == 1) StartDamage -= 4;

        indicators.healthAmount -= StartDamage;
        StartDamage = mobScriptableObject.Damage;

        if (indicators.healthAmount <= 0)
        {
            aIManager._animator.SetBool("Damage", false);
        }
    }

    public void Destroys()
    {
        foreach (GameObject lut in mobScriptableObject.Lut)
        {
            Instantiate(lut, MyTransform.position, Quaternion.identity); //спавним лут
        }

        if (arena == true)
        {
            Destroy(gameObject);
        }
        else
        {
            if (neobMob == true) forDestroy.SetActive(false); else gameObject.SetActive(false);
            RespawnResource();
        }
    }
}