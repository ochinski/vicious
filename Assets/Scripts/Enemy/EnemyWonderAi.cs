using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWonderAi : MonoBehaviour
{
    public float wanderRadius;    
    public float speed;

    public GameObject characterModel;


    private Transform target;
    private NavMeshAgent agent;
    private AnimEnemyController animController;

    private float timer;
    private float wanderTimer;


    private void Start()
    {
        animController = characterModel.GetComponent<AnimEnemyController>();
        wanderTimer = GenerateWanderTimer();
    }

    // Use this for initialization
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }
        
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        /*Debug.Log(timer);*/
        // get movement velocity and pass it to the animation controller
        float movementVelocity = agent.velocity.magnitude / agent.speed;
        animController.Move(movementVelocity);

        
        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
            wanderTimer = GenerateWanderTimer();
        }
    }

    private float GenerateWanderTimer()
    {
        return Random.Range(0, 10.0f);
    }
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
