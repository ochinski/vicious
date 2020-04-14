using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody; 

    float xRotation = 0f;
    private GameObject aimingPoint;
    private bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        isActive = true;
        Cursor.lockState = CursorLockMode.Locked;
        /*Cursor.lockState = CursorLockMode.None;*/
        aimingPoint = GameObject.FindGameObjectWithTag("projectileSpawnPoint");
        /*Cursor.lockState = CursorLockMode.Locked;*/
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -40f, 40f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    private void UpdateAimPoint() {
        /*aimingPoint*/
    }
    public void ToggleMouseLook()
    {
        isActive = !isActive;
        Debug.Log(isActive);
    }

/*    private void RotateUpperBody()
    {
        var mousePos = Input.mousePosition;
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;

        upperBodyBone.rotation = Quaternion.Euler(
            upperBodyBone.transform.rotation.eulerAngles.x,
            upperBodyBone.transform.rotation.eulerAngles.y,
            upperBodyBone.transform.rotation.eulerAngles.z + (mousePos.y * chestRotateSpeed)
        );

        // TODO: Counter rotate the arms
    }*/

}
