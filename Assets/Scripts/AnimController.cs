using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    public GameObject playerObject;

    private CharacterController charController;
    private Animator anim;
    private bool canAttack = true;
    private bool canEqup = true;

    [SerializeField] float attackResetTimer = 50f;
    [SerializeField] float equipResetTimer = 50f;

    private float attackSpeed;
    private bool isSprinting = false;
    private bool isBlocking = false;

    private float motionX, motionZ;
    // Start is called before the first frame update

    private bool animState = false;
    private float sprintSpeed = 0;
    void Start()
    {
        anim = GetComponent<Animator>();
        charController = playerObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim == null) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 horizontalVelocity = charController.velocity;
        horizontalVelocity = new Vector3(charController.velocity.x, 0, charController.velocity.z);

        // The speed on the x-z plane ignoring any speed
        float horizontalSpeed = horizontalVelocity.magnitude;
        // The speed from gravity or jumping
        float verticalSpeed = charController.velocity.y;
        // The overall speed
        float overallSpeed = charController.velocity.magnitude;

        /*        if (motionZ > -1 && motionZ < 1) {
                    motionZ += z;
                }
                Debug.Log("horizontal : "  + x);
                Debug.Log("Vertical : " + z);
        */
        Move(x,z);
    }

    private void Move(float z) {
        if (isSprinting && sprintSpeed != 1) {
            sprintSpeed += 0.05f;
        } else if (!isSprinting && sprintSpeed != 0){
            sprintSpeed -= 0.05f;
        }
        anim.SetFloat("SprintSpeed", sprintSpeed);
        anim.SetFloat("Forward", z);
    }

    private void Move(float x,float z)
    {
        if (isSprinting && sprintSpeed <= 1) {
            sprintSpeed += 0.05f;
        } else if (!isSprinting && sprintSpeed >= 0) {
            sprintSpeed -= 0.05f;
        }
        anim.SetFloat("SprintSpeed", sprintSpeed);
        anim.SetFloat("Forward", z);
        anim.SetFloat("Horizontal", x);
    }

    public void WeaponEquipAnimation()
    {
        anim.SetTrigger("EquipTrigger");
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

    public void CrouchAnimation(bool isActive)
    {
        // if already crouching, set trigger to stand, else, set trigger to crouch
        if (isActive)
        {
            anim.SetBool("CrouchActive", false);
            anim.SetTrigger("CrouchTriggerUp");
        } else
        {
            anim.SetBool("CrouchActive", true);
            anim.SetTrigger("CrouchTriggerDown");
        }
    }
    public bool AnimationStatus(string animation)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsTag(animation);

    }

    public void SetLayer(string layer) {
        if (layer == "Ranger") {
            Debug.Log("switching to ranger layer");
            anim.SetLayerWeight(1, 1);
        }
        if (layer == "Ranger Drawing Power") {
            anim.SetLayerWeight(2, 1);
        }
    }
    public void SetLayer(string layer, float weight) {
        if (layer == "Melee") {
            anim.SetLayerWeight(0, weight);
        }
    }

    public void SetAnimation(string animation, bool isActive) {
        anim.SetBool(animation, isActive);
    }

    public void SprintAnimation(bool isActive)
    {
        isSprinting = isActive;
        anim.SetBool("SprintActive", isActive);
    }

    public void BlockingAnimation(bool isActive)
    {
        isBlocking = isActive;
        if (isBlocking)
        {
            anim.SetTrigger("BlockingTriggerActive");
        } else
        {
            anim.SetTrigger("BlockingTriggerNotActive");
        }
    }

    public void SideStep_Left() {
        anim.SetTrigger("SideStepLeftTrigger");
    }

    public void SideStep_Right() {
        anim.SetTrigger("SideStepRightTrigger");
    }

    public void RollAnimation(float rollSpeed)
    {
        anim.SetFloat("rollSpeed", rollSpeed);
        anim.SetTrigger("RollTrigger");
        var x = Input.GetAxis("Vertical");   
        Move(x);
    }

    public void SetAnimationFloat (string animName, float value) {
        anim.SetFloat(animName, value);
    }

    public void AnimationTrigger(string trigger) {
        anim.SetTrigger(trigger);
    }

    public void AttackRangedFire() {
        anim.SetTrigger("ShootArrowTrigger");
    }

    public void AttackRangedAiming(bool isActive) {
        anim.SetBool("rangedAiming", isActive);
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
