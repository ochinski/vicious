using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEnemyController : MonoBehaviour
{
    private Animator anim;
    private bool canAttack = true;
    private bool canEqup = true;

    [SerializeField] float attackResetTimer = 0.5f;
    [SerializeField] float equipResetTimer = 0.5f;
    [SerializeField] float attackDistance = 0.5f;

    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Move(float x)
    {
        anim.SetFloat("Forward", x);
    }

}
