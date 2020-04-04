using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;

    public LayerMask enemyLayers;



    public Transform attackPoint;

    public Vector3 location;

    public GameObject[] items;
    public GameObject playerRightHand;
    public GameObject weaponSlot;

    public GameObject characterModel; 

    [SerializeField] float attackSpeed = 1f;
    [SerializeField] float attackDamage = 1;
    public float speed = 12f;
    public float attackRange;


    private GameObject weapon;
    private AnimController animController;

    private List<GameObject> ItemsInRange = new List<GameObject>();
    private void Start() {


        
        animController = characterModel.GetComponent<AnimController>();

        // find a better way to do this
        foreach (Transform child in weaponSlot.transform)
        {
            if (child.gameObject.tag == ("weapon"))
            {
                weapon = child.gameObject;
                break;
            }
        }
        //
    }
    

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // check to see if locomotion animation is active, if yes, no other animations are playing and character can move
        if (GetAnimationStatus("playerMovement"))
            controller.SimpleMove(move * speed);

        // check to see if character is not already in an attack animation
        if (Input.GetKeyDown(KeyCode.Mouse0) && !GetAnimationStatus("attackAnim")) {
            Attack();
        }

        // check to see if locomotion animation is active, if yes, only valid time to change inventory
        if (Input.GetKeyDown(KeyCode.Alpha1) && GetAnimationStatus("playerMovement")) {
            EquipItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && GetAnimationStatus("playerMovement")) {
            EquipItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && GetAnimationStatus("playerMovement"))
        {
            EquipItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && GetAnimationStatus("playerMovement"))
        {
            EquipItem(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && GetAnimationStatus("playerMovement"))
        {
            EquipItem(4);
        }

        if (Input.GetKeyDown(KeyCode.B) && GetAnimationStatus("playerMovement"))
        {
            ToggleInventory();
        }


        // check to see if locomotion animation is active, if yes, only valid time to loot
        if (Input.GetKeyDown(KeyCode.E) && GetAnimationStatus("playerMovement"))
        {
            Loot();
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("item"))
        {
            ItemsInRange.Add(col.gameObject);
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("item"))
        {
            ItemsInRange.Remove(col.gameObject);
        }

    }


    private bool GetAnimationStatus(string animation)
    {
        return animController.AttackAnimationStatus(animation);
    }

    private void ToggleInventory()
    {

    }

    private void Loot ()
    {
        if (ItemsInRange.Count > 0)
        {
            ItemsInRange[0].GetComponent<ItemPickup>().Pickup();
            ItemsInRange.RemoveAt(0);
        }
    }

    void EquipItem (int itemSlot)
    {
        GameObject item = items[itemSlot];

        foreach (Transform child in weaponSlot.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        GameObject itemInSlot = Instantiate(item, transform.position, Quaternion.identity);
        weapon.GetComponent<WeaponController>().ResetWeapon();

        weapon = itemInSlot;
        itemInSlot.transform.parent = playerRightHand.transform;
        itemInSlot.transform.localPosition = weapon.GetComponent<WeaponPosition>().pickPosition;
        itemInSlot.transform.localEulerAngles = weapon.GetComponent<WeaponPosition>().pickRotation;

        weapon.GetComponent<WeaponController>().SetTotalDamage(attackDamage);

        
        // to be changed to use a item type component
        SetAttackSpeed(weapon.GetComponent<WeaponController>().GetWeaponSpeed());
    }

    private void SetAttackSpeed(float weaponSpeeed)
    {
        attackSpeed = weaponSpeeed;
    }

    private void Attack()
    {
        animController.AttackAnimation(attackSpeed);
        weapon.GetComponent<WeaponController>().ResetWeapon();
    }


    // might be useless going forward
    private void ResetAttackFlag()
    {
/*        canAttack = true;*/
       /* weapon.GetComponent<Collider>().enabled = true;*/
    }

    // might be useless going forward
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
