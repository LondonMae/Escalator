using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIScript : MonoBehaviour
{
    private GameObject rig, topPlatform, escalators, strtPlatform;
    private Vector3 topPos, btmPos;
    private int frameCounter = 0;
    private bool done = false;

    void Update()
    {
        if (frameCounter == 2 && !done)
        {
            rig = GameObject.Find("[CameraRig]");
            topPlatform = GameObject.Find("TopPlatform");
            strtPlatform = GameObject.Find("StartingPlatform");
            escalators = GameObject.Find("Escalators");
            topPos = new Vector3(rig.transform.position.x, topPlatform.transform.position.y, rig.transform.position.z);
            btmPos = new Vector3(rig.transform.position.x, rig.transform.position.y, rig.transform.position.z);

            done = true;
        }
        else
        {
            frameCounter++;
        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 70, 100, 30), "Move to Top"))
        {
			escalators.GetComponent<Timer>().stopButton = true;

            rig.transform.position = topPos;
            rig.transform.parent = topPlatform.transform;

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

        if (GUI.Button(new Rect(10, 110, 100, 30), "Move to Bottom"))
        {

            rig.transform.position = btmPos;
            rig.transform.parent = strtPlatform.transform;

            foreach (Transform escalator in escalators.transform)
            {
                if (escalator.gameObject.tag == "Escalator")
                {
                    if (escalator.GetComponent<EscalatorScript>().stepSpeed < 0)
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
