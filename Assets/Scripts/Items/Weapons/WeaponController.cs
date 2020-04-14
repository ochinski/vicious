using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] float attackResetTimer = 50f;
    [SerializeField] float damage = 25;
    [SerializeField] float delay = 0f;
    [SerializeField] float force = 1f;
    [SerializeField] float weaponSpeed = 1f;
    [SerializeField] float gatheringDelay = 50f;

    public bool isRanged = false;
    public GameObject projectileObject;

    private GameObject projectileSpawnPoint;

    private Animator anim;

    private float totalDamage = 0f;
    private bool isActive = false;
    private Collider m_Collider;
    
    public bool canHit = true;

    public float drawStrength;
    private List<GameObject> objectsHit = new List<GameObject>();

    // get collider and what else is needed
    private void Awake()
    {
        projectileSpawnPoint = GameObject.FindGameObjectWithTag("projectileSpawnPoint");
        m_Collider = GetComponent<Collider>();
        m_Collider.enabled = false;
        if (isRanged) {
            anim = GetComponent<Animator>();
        }
    }

    public void AnimateDrawStrength(float drawPower) {
        anim.SetFloat("drawPower", drawPower);
    }
    public void AnimateDrawStart() {
        anim.SetTrigger("drawStart");
    }
    public void AnimateDrawEnd() {
        anim.SetTrigger("drawEnd");
    }

    // get the damage of the weapon multiplied by character stats
    public void SetTotalDamage(float characterDamage)
    {
        totalDamage = characterDamage * damage;
    }

    public float GetWeaponSpeed()
    {
        return weaponSpeed;
    }

    private void OnCollisionEnter(Collision c)
    {
        // check if it's an enemy and not recently hit in the list
        if (c.gameObject.CompareTag("enemy") && isActive && !objectsHit.Contains(c.gameObject))
        {
            // add the enemy to the list
            objectsHit.Add(c.gameObject);
            m_Collider.enabled = false;

            // get the enemy component and call take damage as well as the direction being hit
            GameObject enemy = c.gameObject;
            Vector3 direction = ( c.transform.position - transform.position ).normalized;
            enemy.GetComponent<Enemy>().TakeDamage(totalDamage, delay, direction * force);
        }
        if (c.gameObject.CompareTag("tree") && isActive)
        {
            m_Collider.enabled = false;
            GameObject tree = c.gameObject;
            Vector3 direction = (c.transform.position - transform.position).normalized;
            tree.GetComponent<Gatherable>().TakeHit(direction, gatheringDelay);
        }
    }


    public void ResetWeapon()
    {
        /*isActive = true;*/
        objectsHit.Clear();
        m_Collider.enabled = true;
    }

    public void Shoot(float force, float playerRangedDamage) {

        /*RaycastHit hit;*/
        GameObject projectile = Instantiate(projectileObject, projectileSpawnPoint.transform.position, projectileSpawnPoint.transform.rotation) as GameObject;

        projectile.GetComponent<Arrow>().SetDamage(playerRangedDamage);

        projectile.GetComponent<Rigidbody>().AddForce(projectileSpawnPoint.transform.forward * force);
    }

    // potential useless method
    public void Attacking(float attackResetTimer)
    {
        isActive = true;
        Invoke("ResetAttackFlag", attackResetTimer);
    }

    // potential useless method
    public void ResetAttackFlag()
    {
        m_Collider.enabled = true;
        isActive = false;
    }
}
