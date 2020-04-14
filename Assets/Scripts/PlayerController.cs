using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {
    public CharacterController controller;

    public LayerMask enemyLayers;
    public Transform attackPoint;

    public Texture2D crosshairImage;

    public Vector3 location;

    public GameObject[] items;

    public GameObject playerLeftHand;
    public GameObject playerRightHand;

    public Transform playerLeftShoulder;
    public Transform playerRightShoulder;

    public GameObject weaponSlot;
    public GameObject weaponSlotLeft;

    public GameObject characterModel;
    public GameObject inventoryUI;
    public GameObject characterUI;

    public Transform aimingPos;
    public Transform normalPos;

    public GameObject mainCamera;
    public Transform upperBodyBone;

    [SerializeField] float attackSpeed = 1f;
    [SerializeField] float attackDamage = 1;
    [SerializeField] float rangedDamage = 1;
    [SerializeField] float chestRotateSpeed = 1;

    public float speed = 12f;
    public float attackRange;

    private float crouchSpeed;
    private float walkingSpeed;
    private float runningSpeed;
    private float sprintingSpeed;
    private float shootForce = 100;
    private float drawPower = 0f;
    private float maxDraw = 100f;

    private bool movement;

    private Vector3 currentLocalRotation;

    private float ButtonCooler = 0.5f; // Half a second before reset
    private int ButtonCount = 0;

    private float ButtonCoolerW = 0.5f; // Half a second before reset
    private int ButtonCountW = 0;

    private float ButtonCoolerA = 0.5f; // Half a second before reset
    private int ButtonCountA = 0;

    private float ButtonCoolerS = 0.5f; // Half a second before reset
    private int ButtonCountS = 0;

    private float ButtonCoolerD = 0.5f; // Half a second before reset
    private int ButtonCountD = 0;

    private bool isCrouching = false;
    private bool isBlocking = false;

    private bool isMelee = true;
    private bool isRanged = false;
    private bool isAiming = false;

    private bool movingCharacter = false;

    private GameObject itemRightHand;
    private GameObject itemLeftHand;

    private GameObject weapon;
    private AnimController animController;
    private WeaponController weaponController;

    private List<GameObject> ItemsInRange = new List<GameObject>();

    private Transform mainCameraLocation;
    private void Start() {
        movement = false;
        isMelee = true;
        // this is bad, fix this.
        // only in for testing atm
        crouchSpeed = speed / 2f;
        runningSpeed = speed;
        sprintingSpeed = speed * 1.5f;
        mainCameraLocation = mainCamera.transform;
        animController = characterModel.GetComponent<AnimController>();
        // find a better way to do this
        foreach (Transform child in weaponSlot.transform) {
            if (child.gameObject.tag == ("weapon")) {
                weapon = child.gameObject;
                break;
            }
        }
    }

    void Update() {
        UiToggled();
        AnimMovement(movement);
        MoveCamera();
        RangerDrawPower();
        AdjustRangedDamage();

/*        if (!inventoryUI.activeSelf && !characterUI.activeSelf) {*/

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            Cursor.visible = false;

            if (x == 0 && z == 0) {
                movement = false;
            } else {
                movement = true;
            }


            if (GetAnimationStatus("playerRoll")) {
                MovePlayer("forward", 1.5f);
            }

            if (GetAnimationStatus("playerStepLeft")) {
                Debug.Log("moving left");
                MovePlayer("left", 1f);
            }

            if (GetAnimationStatus("playerStepRight")) {
                MovePlayer("right", 1f);
            }

            // check to see if locomotion animation is active, if yes, no other animations are playing and character can move
            if (GetAnimationStatus("playerMovement") | GetAnimationStatus("playerCrouch")) {
                if (!GetAnimationStatus("playerStepLeft") && !GetAnimationStatus("playerRoll") && !GetAnimationStatus("playerStepRight"))
                    controller.SimpleMove(move * speed);
            }

            if (Input.GetKeyDown(KeyCode.W) && GetAnimationStatus("playerMovement") && !GetAnimationStatus("playerRoll") && !GetAnimationStatus("attackAnim")) {
                Debug.Log("we in here too");
                /*controller.SimpleMove(move * speed);*/
                if (ButtonCoolerW > 0 && ButtonCountW == 1/*Number of Taps you want Minus One*/) {
                    /*isStamUsage = true;*/
                    ButtonCountW = 0;
                    Roll();
                } else {
                    ButtonCoolerW = 0.5f;
                    ButtonCountW += 1;
                }
            }
            if (ButtonCoolerW > 0) {

                ButtonCoolerW -= 1 * Time.deltaTime;

            } else {
                ButtonCountW = 0;
            }

            if (Input.GetKeyDown(KeyCode.A) && GetAnimationStatus("playerMovement") && !GetAnimationStatus("playerDodge") && !GetAnimationStatus("attackAnim")) {
                controller.SimpleMove(move * speed);
                if (ButtonCoolerA > 0 && ButtonCountA == 1/*Number of Taps you want Minus One*/) {
                    ButtonCountA = 0;
                    SideStep("left");
                } else {
                    ButtonCoolerA = 0.5f;
                    ButtonCountA += 1;
                }
            }
            if (ButtonCoolerA > 0) {

                ButtonCoolerA -= 1 * Time.deltaTime;

            } else {
                ButtonCountA = 0;
            }

            if (Input.GetKeyDown(KeyCode.D) && GetAnimationStatus("playerMovement") && !GetAnimationStatus("playerDodge") && !GetAnimationStatus("attackAnim")) {
                controller.SimpleMove(move * speed);
                if (ButtonCoolerD > 0 && ButtonCountD == 1/*Number of Taps you want Minus One*/) {
                    ButtonCountD = 0;
                    SideStep("right");
                } else {
                    ButtonCoolerD = 0.5f;
                    ButtonCountD += 1;
                }
            }
            if (ButtonCoolerD > 0) {

                ButtonCoolerD -= 1 * Time.deltaTime;

            } else {
                ButtonCountD = 0;
            }

            if (Input.GetKeyDown(KeyCode.S) && GetAnimationStatus("playerMovement") && !GetAnimationStatus("playerRoll") && !GetAnimationStatus("attackAnim")) {
                controller.SimpleMove(move * speed);
                if (ButtonCoolerS > 0 && ButtonCountS == 1/*Number of Taps you want Minus One*/) {
                    /*SideStep("right");*/
                } else {
                    ButtonCoolerS = 0.5f;
                    ButtonCountS += 1;
                }
            }
            if (ButtonCoolerS > 0) {

                ButtonCoolerS -= 1 * Time.deltaTime;

            } else {
                ButtonCount = 0;
            }

            if (Input.GetKeyDown(KeyCode.Z) && GetAnimationStatus("playerMovement") && !GetAnimationStatus("playerRoll") && !GetAnimationStatus("attackAnim")) {
                EquipGear();
            }


            if (Input.GetKeyDown(KeyCode.LeftControl) && (GetAnimationStatus("playerMovement") | GetAnimationStatus("playerCrouch"))) {
                CrouchToggle();
            }


            if (Input.GetKey(KeyCode.LeftShift) && (GetAnimationStatus("playerMovement") | GetAnimationStatus("playerRoll") | GetAnimationStatus("playerCrouch"))) {

                Sprint(true);
                CrouchToggleUp();
                speed = sprintingSpeed;
                /*controller.SimpleMove(move * speed);*/

            } else {
                speed = runningSpeed;
                Sprint(false);
            }

            /*&& !GetAnimationStatus("aimingBow")*/
            /*            if (isRanged && Input.GetKey(KeyCode.Mouse0) && !GetAnimationStatus("drawArrow") && !isAiming && !GetAnimationStatus("shootArrow")) {
                            AttackRangerAim(true);
                            isAiming = true;
                        }*/

            Debug.Log(drawPower);
            if (isRanged && Input.GetKey(KeyCode.Mouse0) && drawPower == 0f) {
                AttackRangerAim(true);
                isAiming = true;
                weaponController.AnimateDrawStart();
        } else if (isRanged && Input.GetKey(KeyCode.Mouse0) && drawPower >= 249f) {
                AttackRangerAim(false);
                WithdrawRangerAim();
                isAiming = false;
            }
            if (isRanged && Input.GetKeyUp(KeyCode.Mouse0) && isAiming && drawPower >= 30f) {
                AttackRangerAim(false);
                AttackRangeFire();
                isAiming = false;
            } else if (isRanged && Input.GetKeyUp(KeyCode.Mouse0) && isAiming && drawPower < 30f) {
                AttackRangerAim(false);
                WithdrawRangerAim();
                isAiming = false;
            }
            // check to see if character is not already in an attack animation
            /*            if (Input.GetKeyDown(KeyCode.Mouse0) && !GetAnimationStatus("attackAnim")) {
                            if (isRanged && Input.GetKey(KeyCode.Mouse0)) {
                                Debug.Log("getting key Down");
                                AttackRangerAim(true);
                            } else if (isRanged && Input.GetKeyUp(KeyCode.Mouse0)){
                                AttackRangerAim(false);
                                AttackRangeFire();
                            } else {
                                Attack();
                            }

                        }*/

            // check to see if character is not already in an attack animation
            if (Input.GetKeyDown(KeyCode.Mouse1) && !isBlocking) {
                isBlocking = !isBlocking;
                Blocking(isBlocking);
            } else if (Input.GetKeyUp(KeyCode.Mouse1) && isBlocking) {
                isBlocking = !isBlocking;
                Blocking(isBlocking);
            }

            // check to see if locomotion animation is active, if yes, only valid time to change inventory
            if (Input.GetKeyDown(KeyCode.Alpha1) && GetAnimationStatus("playerMovement")) {
                EquipItem(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && GetAnimationStatus("playerMovement")) {
                EquipItem(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && GetAnimationStatus("playerMovement")) {
                EquipItem(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && GetAnimationStatus("playerMovement")) {
                EquipItem(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) && GetAnimationStatus("playerMovement")) {
                EquipItem(4);
            }

            /*if (Input.GetKeyDown(KeyCode.B) && GetAnimationStatus("playerMovement")) {
                ToggleInventory();
            }*/
/*        } else {
            Cursor.visible = true;
        }*/

        // check to see if locomotion animation is active, if yes, only valid time to loot
        if (Input.GetKeyDown(KeyCode.E) && GetAnimationStatus("playerMovement")) {
            Loot();
        }
    }

    // to be render after animation
    private void LateUpdate() {
        RotateUpperBody();
        if (isAiming) {
            RotateShoulderssWhenAiming();
        }
    }

    void OnGUI() {
        float xMin = (Screen.width / 2) - (crosshairImage.width / 2);
        float yMin = (Screen.height / 2) - (crosshairImage.height / 2);
        GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width, crosshairImage.height), crosshairImage);
    }

    private void UiToggled() {
        if (Input.GetKeyDown(KeyCode.B)) {
            mainCamera.GetComponent<MouseLook>().ToggleMouseLook();
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            mainCamera.GetComponent<MouseLook>().ToggleMouseLook();
        }
    }

    private void WithdrawRangerAim() {
        animController.AnimationTrigger("noShootArrowTrigger");
    }
    private void RangerDrawPower() {
        
        if (isAiming) {
            weaponController.AnimateDrawStrength(drawPower);
            /*weaponController.drawStrength = drawPower;*/
            animController.SetAnimationFloat("drawPower", drawPower);
            if (drawPower < maxDraw && drawPower < 40f) {
                drawPower += (Time.deltaTime * 30);
            } else if (drawPower >= 40f) {
                drawPower += (Time.deltaTime * 80);
            }
        } else if (drawPower != 0f) {
            ResetDrawPower();
        }
    }

    private void AdjustRangedDamage() {
        rangedDamage = drawPower * 0.3f;
    }

    private void ResetDrawPower() {
        drawPower = 0f;
        animController.SetAnimationFloat("drawPower", drawPower);
        weaponController.AnimateDrawStrength(drawPower);
        weaponController.AnimateDrawEnd();
    }

    private void MoveCamera() {
        if (isAiming) {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, aimingPos.position, Time.deltaTime * 2f);
        } else {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, normalPos.position, Time.deltaTime * 3.5f);
            /*mainCamera.transform.position = normalPos.position;*/
        }
    }

    private void MovePlayer(string direction, float multiple) {
        if (direction == "forward") {
            controller.SimpleMove(transform.forward * speed * multiple);
        } else if (direction == "left") {
            controller.SimpleMove(-transform.right * speed * multiple);
        } else if (direction == "right") {
            controller.SimpleMove(transform.right * speed * multiple);
        }
    }

    // check if item is within range
    public void OnTriggerEnter(Collider col) {
        if (col.gameObject.CompareTag("item")) {
            ItemsInRange.Add(col.gameObject);
        }
    }

    // check if item is out of range
    public void OnTriggerExit(Collider col) {
        if (col.gameObject.CompareTag("item")) {
            ItemsInRange.Remove(col.gameObject);
        }

    }

    private bool GetAnimationStatus(string animation) {
        return animController.AnimationStatus(animation);
    }

    private void RotateShoulderssWhenAiming() {
        
            var mousePos = Input.mousePosition;
            mousePos.x -= Screen.width / 2;
            mousePos.y -= Screen.height / 2;
        /*if (GetAnimationStatus("aimingBow")) {*/
            /*mousePos.y = Mathf.Clamp(mousePos.y, -90f, 90f);*/
            playerLeftShoulder.rotation = Quaternion.Euler(
                playerLeftShoulder.transform.rotation.eulerAngles.x,
                playerLeftShoulder.transform.rotation.eulerAngles.y,
                /*playerLeftShoulder.transform.rotation.eulerAngles.z + (mainCamera.transform.rotation.x)*/
                mainCamera.transform.rotation.eulerAngles.x
                );
            playerRightShoulder.rotation = Quaternion.Euler(
                playerRightShoulder.transform.rotation.eulerAngles.x,
                playerRightShoulder.transform.rotation.eulerAngles.y,
                playerRightShoulder.transform.rotation.eulerAngles.z
                /*mainCamera.transform.rotation.eulerAngles.x*/
                );
        /*}*/
    }

    private void RotateUpperBody() {
        var mousePos = Input.mousePosition;
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;
        mousePos.y = Mathf.Clamp(mousePos.y, -90f, 90f);
/*        Debug.Log(mousePos.y);*/
        upperBodyBone.rotation = Quaternion.Euler(
            upperBodyBone.transform.rotation.eulerAngles.x,
            upperBodyBone.transform.rotation.eulerAngles.y,
            upperBodyBone.transform.rotation.eulerAngles.z + (mousePos.y * chestRotateSpeed)
        );

        // TODO: Counter rotate the arms
    }

    private void CrouchToggle() {
        animController.CrouchAnimation(isCrouching);
        isCrouching = !isCrouching;
        if (isCrouching) {
            speed = crouchSpeed;
        } else {
            speed = runningSpeed;
        }
    }

    private void CrouchToggleUp() {
        if (isCrouching) {
            speed = runningSpeed;
            isCrouching = false;
            animController.CrouchAnimation(isCrouching);
        }
    }

    private void Roll() {
        animController.RollAnimation(5f);
    }

    private void SideStep(string direction) {
        if (direction == "left") {
            animController.SideStep_Left();
        } else {
            animController.SideStep_Right();
        }
    }

    private void Sprint(bool isActive) {
        animController.SprintAnimation(isActive);
    }

    private void Blocking(bool isActive) {
        animController.BlockingAnimation(isActive);
    }


    private void Loot () {
        if (ItemsInRange.Count > 0) {
            ItemsInRange[0].GetComponent<ItemPickup>().Pickup();
            ItemsInRange.RemoveAt(0);
        }
    }


    private void EquipGear() {
        animController.WeaponEquipAnimation();
    }

    private void EquipItem (int itemSlot) {
        GameObject item = items[itemSlot];

        foreach (Transform child in weaponSlot.transform) {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Transform child in weaponSlotLeft.transform) {
            GameObject.Destroy(child.gameObject);
        }

        GameObject itemInSlot = Instantiate(item, transform.position, Quaternion.identity);

        animController.WeaponEquipAnimation();
        weapon = itemInSlot;

        weaponController = weapon.GetComponent<WeaponController>();
        weaponController.ResetWeapon();

        if (weaponController.isRanged) {
            SetRanger();

            itemInSlot.transform.parent = playerLeftHand.transform;
            itemInSlot.transform.localPosition = weapon.GetComponent<WeaponPosition>().pickPosition;
            itemInSlot.transform.localEulerAngles = weapon.GetComponent<WeaponPosition>().pickRotation;
        } else {
            SetMelee();

            itemInSlot.transform.parent = playerRightHand.transform;
            itemInSlot.transform.localPosition = weapon.GetComponent<WeaponPosition>().pickPosition;
            itemInSlot.transform.localEulerAngles = weapon.GetComponent<WeaponPosition>().pickRotation;
        }
        


        weapon.GetComponent<WeaponController>().SetTotalDamage(attackDamage);
        
        // to be changed to use a item type component
        SetAttackSpeed(weapon.GetComponent<WeaponController>().GetWeaponSpeed());
    }

    private void SetRanger() {
        animController.SetAnimation("Ranged", true);
        animController.SetAnimation("Melee", false);
        animController.SetLayer("Ranger");
        animController.SetLayer("Ranger Drawing Power");
        isRanged = true;
        isMelee = false;
    }

    private void SetMelee() {
        animController.SetAnimation("Ranged", false);
        animController.SetAnimation("Melee", true);
        isRanged = false;
        isMelee = true;
    }

    private void SetAttackSpeed(float weaponSpeeed) {
        attackSpeed = weaponSpeeed;
    }

    private void AnimMovement(bool isActive) {
        if (isActive) {
            animController.SetAnimation("Movement", true);
            Debug.Log("setting to true");
        } else {
            animController.SetAnimation("Movement", false);
        }
    }

    private void AttackRangeFire() {
        animController.AttackRangedFire();
        weaponController.Shoot(drawPower, rangedDamage);
        /*bulletCapInstance.GetComponent<Rigidbody>().AddForce(bulletCapTransform.right * 10000);*/

        ResetDrawPower();
        //This will send a raycast straight forward from your camera centre.
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
    }

    private void AttackRangerAim(bool isActive) {
        if (isActive) {
            animController.SetLayer("Ranged",1);
            animController.AttackRangedAiming(true);
        } else {
            animController.SetLayer("Ranged", 0);
            animController.AttackRangedAiming(false);
        }
    }

    private void Attack() {
        animController.AttackAnimation(attackSpeed);
        weapon.GetComponent<WeaponController>().ResetWeapon();
    }


    // might be useless going forward
    private void Delay (float secondDelay) {
/*        canAttack = true;*/
       /* weapon.GetComponent<Collider>().enabled = true;*/
    }

    // might be useless going forward
    private void OnDrawGizmosSelected() {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
