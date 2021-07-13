using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A program to make an object follow a different object without being effected by the other objects rotation.
public class LockRotation : MonoBehaviour
{

    public bool lockXRotation = false, lockYRotation = false, lockZRotation = false;


    private float rX, rY, rZ; // difference of the X Y and Z components of the transforms.
    // Start is called before the first frame update
    void Start()
    {
        rX = transform.rotation.x;
        rY = transform.rotation.y;
        rZ = transform.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        float nX, nY, nZ;

        if (lockXRotation)
        {
            nX = rX;
        }
        else
        {
            nX = transform.rotation.x;
        }

        if (lockYRotation)
        {
            nY = rY;
        }
        else
        {
            nY = transform.rotation.y;
        }

        if (lockZRotation)
        {
            nZ = rZ;
        }
        else
        {
            nZ = transform.rotation.z;
        }

        transform.rotation.Set(nX, nY, nZ, transform.rotation.w);
    }
}
