using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    private Animator anim;
    private bool canAttack = true;
    private bool canEqup = true;

    [SerializeField] float attackResetTimer = 50f;
    [SerializeField] float equipResetTimer = 50f;

    private float attackSpeed;
    // Start is called before the first frame update

    private bool animState = false;


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim == null) return;

        var x = Input.GetAxis("Vertical");   
        Move(x);
    }

    private void Move(float x)
    {
        anim.SetFloat("Forward", x);
    }

    // needs to be changed
    private void WeaponEquip()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && canEqup)
        {
            canEqup = false;
            anim.SetTrigger("EquipTrigger");
            Invoke("ResetEquipFlag", AnimToSeconds(equipResetTimer));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && canEqup)
        {
            canEqup = false;
            anim.SetTrigger("EquipTrigger");
            Invoke("ResetEquipFlag", AnimToSeconds(equipResetTimer));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && canEqup)
        {
            canEqup = false;
            anim.SetTrigger("EquipTrigger");
            Invoke("ResetEquipFlag", AnimToSeconds(equipResetTimer));
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && canEqup)
        {
            canEqup = false;
            anim.SetTrigger("EquipTrigger");
            Invoke("ResetEquipFlag", AnimToSeconds(equipResetTimer));
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && canEqup)
        {
            canEqup = false;
            anim.SetTrigger("EquipTrigger");
            Invoke("ResetEquipFlag", AnimToSeconds(equipResetTimer));
        }
    }

    public bool AttackAnimationStatus(string animation)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsTag(animation);

    }

    public void AttackAnimation(float weaponSpeed)
    {
        anim.SetFloat("attackSpeed", weaponSpeed);
        anim.SetTrigger("AttackTrigger");
    }

    // might be useless going forward
    private float AnimToSeconds(float animTimer)
    {
        return animTimer / 60;
    }

    // might be useless going forward
    private void ResetAttackFlag()
    {
        canAttack = true;
    }

    // might be useless going forward
    private void ResetEquipFlag()
    {
        canEqup = true;
    }
}
