using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Causes and object to float underneath another object. 
public class FollowUnderneath : MonoBehaviour
{
    public Transform toFollow;
    public float distance = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(toFollow.position.x, toFollow.position.y - distance, toFollow.position.z);
    }
}
