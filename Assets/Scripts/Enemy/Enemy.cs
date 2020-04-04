using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    public float maxHealth = 100;    
    public float thrust = 1.0f;

    

    public Canvas healthbarUI;
    public Image healthBar;
    public GameObject characterModel;

    private float currentHealth;
    private GameObject player;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
    }

    private void Update()
    {
        Vector3 targetPostition = new Vector3(player.transform.position.x,
                                       healthbarUI.transform.position.y,
                                       player.transform.position.z);
        healthbarUI.transform.LookAt(targetPostition);

    }

    public void TakeDamage(float damage, float delay, Vector3 direction)
    {
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / maxHealth;
        // play hurt animation;
        if (currentHealth <= 0)
        {
            Die(direction);
        }
/*        StartCoroutine(CalcDamage(damage, delay, direction));*/
    }



    // CalcDamage
    private IEnumerator CalcDamage(float damage, float delayTime, Vector3 direction)
    {
        
        if (delayTime != 0) 
            yield return new WaitForSeconds(delayTime); 

        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / maxHealth;
        // play hurt animation;
        if (currentHealth <= 0)
        {
            Die(direction);
        }
    }


    // die
    private void Die(Vector3 direction)
    {
       
        rb.useGravity = true;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        characterModel.GetComponent<Animator>().enabled = false;
        GetComponent<Rigidbody>().AddForce(direction);
        healthbarUI.enabled = false;
    }


}
