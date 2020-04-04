using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPosition : MonoBehaviour
{
    public Vector3 pickPosition;
    public Vector3 pickRotation;

    // Start is called before the first frame update
    public Vector3 GetPickPosition()
    {
        return pickPosition;
    }

    public Vector3 GetPickRotation()
    {
        return pickRotation;
    }
}
