using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TeleportPad : MonoBehaviour
{
    public SteamVR_Action_Boolean teleport;

    void OnTriggerStay(Collider collider)
    {
        if (teleport.state)
        {
            GameObject rig = GameObject.Find("[CameraRig]");
            GameObject topPlatform = GameObject.Find("TopPlatform");
            
            Vector3 topPos = new Vector3(rig.transform.position.x, topPlatform.transform.position.y, rig.transform.position.z);

            rig.transform.position = topPos;
            rig.transform.parent = topPlatform.transform;

            GameObject escalators = GameObject.Find("Escalators");

            foreach (Transform escalator in escalators.transform)
            {
                if (escalator.gameObject.tag == "Escalator")
                {
                    if (escalator.GetComponent<EscalatorScript>().stepSpeed > 0)
                    {
                        escalator.GetComponent<EscalatorScript>().stepSpeed *= -1;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
