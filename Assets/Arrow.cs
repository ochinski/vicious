using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject dust;
    public AudioClip hitSound;

    private bool hitSomething = false;

    [SerializeField] float force = 1f;
    [SerializeField] float damage = 15f;

    private Rigidbody rb;
    private AudioSource m_audio;
    // Start is called before the first frame update
    void Start()
    {
        m_audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!hitSomething) {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    public void SetDamage(float toAdd) {
        damage += toAdd;
    }

    private void OnCollisionEnter(Collision c) {
        hitSomething = true;
        rb.isKinematic = true;
        rb.useGravity = false;
        
        Stick();
        OnHitSound();
        
        gameObject.GetComponent<Collider>().enabled = false;
        Instantiate(dust,transform.position,Quaternion.identity);

        if (c.gameObject.CompareTag("enemy")) {
            gameObject.GetComponent<Transform>().SetParent(c.gameObject.transform);
            GameObject enemy = c.gameObject;
            Vector3 direction = (c.transform.position - transform.position).normalized;
            enemy.GetComponent<Enemy>().TakeDamage(damage, 0f, direction * force);
        }
        
    }

    private void OnHitSound() {
        m_audio.Stop();
        m_audio.PlayOneShot(hitSound);
/*        m_audio.clip = hitSound;*/
        /*m_audio.Stop();*/
    }
    private void Stick() {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
