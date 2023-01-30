using UnityEngine;
public class GatherResources : MonoBehaviour
{
    [Space]
    [Header("Важное")]
    public Camera mainCamera;
    public LayerMask layerMask;
    public InventoryManager inventoryManager;
    public QuickslotInventory quickslotInventory;

    [Space]
    [Header("Характеристики")]
    public int damage;
    public bool NoDamage = false;
    public float attackeDistance = 7f, sizeAttacke = 0.2f;
    private float realAttackeDistance;

    [Space]
    public int Lvls;
    private int fixLvls;

    [Space]
    [SerializeField] private ItemDurability _itemDurability;

    public AudioSource audioSource; // источник звука

    public void GatherResource()
    {
        realAttackeDistance = attackeDistance;

        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

        if (inventoryManager.ViewMode == true) realAttackeDistance += 3;

        if (Physics.SphereCast(ray, sizeAttacke, out hit, realAttackeDistance, layerMask))
        {
            if(hit.collider.GetComponent<ResourceHealth>() != null)
            {
                ResourceHealth resourceHealth = hit.collider.GetComponent<ResourceHealth>();
                resourceHealth.AudioDamage();

                if (Lvls >= resourceHealth.Lvl)
                {
                    Instantiate(resourceHealth.hitFix, hit.point, Quaternion.LookRotation(hit.normal)); //_itemDurability.SubtractDurabilityPerHit();

                    fixLvls = Lvls;

                    if (Lvls == resourceHealth.Lvl)
                    {
                        inventoryManager.AddItem(resourceHealth.resourceType, 1);
                    }

                    if (Lvls > resourceHealth.Lvl)
                    {
                        Lvls -= resourceHealth.Lvl;
                        if (Lvls < 2)
                        {
                            inventoryManager.AddItem(resourceHealth.resourceType, 2);
                        }
                        else
                        {
                            inventoryManager.AddItem(resourceHealth.resourceType, Lvls);
                        }
                    }

                    Lvls = fixLvls;
                    inventoryManager.money.ScoreAdd();
                }
            }
            else if(hit.collider.GetComponent<MobStats>() != null)
            {
                MobStats mobHealth = hit.collider.GetComponent<MobStats>();

                Instantiate(mobHealth.mobScriptableObject.hitFIX, hit.point, Quaternion.LookRotation(hit.normal)); //_itemDurability.SubtractDurabilityPerHit();

                if (NoDamage == false)
                {
                    mobHealth.health -= damage;
                }

                if (hit.collider.gameObject.layer == 17)
                {
                    inventoryManager.money.ScoreAdd(); //Мобы
                }

                if (hit.collider.gameObject.layer == 8 || hit.collider.gameObject.layer == 30)
                {
                    SoundManager.snd.PlaySounds3();//Сундук
                }
            }
        }
    }
}
