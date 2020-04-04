using UnityEngine;

public class Gatherable : MonoBehaviour
{
    public GameObject[] resource;
    public int hitsRequired;

    private bool isActive = false;
    private int hitsTaken;
    private Rigidbody rb;
    private float gatheringTimer;

    private void Start()
    {
        hitsTaken = 0;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

/*    private void Update()
    {
        gatheringTimer -= Time.deltaTime;    
    }*/

    public void TakeHit (Vector3 direction,float gatheringDelay)
    {
        if (!isActive)
        {
            isActive = true;
            hitsTaken++;
            if (hitsTaken >= hitsRequired)
            {
                DestroyResource(direction);
            }
            BeingGathered(gatheringDelay);
        }
    }

    private void DestroyResource(Vector3 direction)
    {
        // die 
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        /*rb.AddForce(transform.forward * 100);*/
        GetComponent<Rigidbody>().AddForce(direction);
        Instantiate(resource[0], transform.position, Quaternion.identity);
    }

    public void BeingGathered(float gatherReset)
    {
        isActive = true;
        Invoke("BeingGatheredFlag", gatherReset);
    }

    public void BeingGatheredFlag()
    {
        isActive = false;
    }
}
