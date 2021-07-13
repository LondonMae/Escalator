using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeParent : MonoBehaviour
{
    private GameObject rig, escalators;
    private string parentTag = "dfklghsfjlkgn", grandparentTag = "slfjdal;fj";
    private string parentTagC = "dfklghsfjlkgn"; //Parent of the collider's tag
    private bool onPlatform = false;
    private Timer timer;
	public float maxHeight = 0;
	private int flightsTravelled = 0;
    // Start is called before the first frame update
    void Start()
    {
        rig = GameObject.Find("[CameraRig]");
		escalators = GameObject.Find("Escalators");
        timer = escalators.GetComponent<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rig.transform.parent.tag == "Path" && timer.isRunning){
			rig.transform.position = new Vector3(rig.transform.position.x, maxHeight, rig.transform.position.z);
		}

    }

    void OnTriggerStay(Collider collider)
    {
        bool isTravelled = false, isTravelledC = true;

        if (rig.transform.parent != null)
        {
            parentTag = rig.transform.parent.tag;
            if (rig.transform.parent.parent != null)
            {
				Transform grandParent = rig.transform.parent.parent.transform;
                grandparentTag = rig.transform.parent.parent.tag;
                if (grandparentTag == "Escalator")
                {
                    isTravelled = grandParent.gameObject.GetComponent<EscalatorScript>().travelled;

					if (grandParent.gameObject.GetComponent<EscalatorScript>().stepSpeed > 0){
						maxHeight = grandParent.TransformPoint(new Vector3 (grandParent.position.x, 
																		grandParent.gameObject.GetComponent<EscalatorScript>().maxHeight + grandParent.gameObject.GetComponent<EscalatorScript>().stepHeight/2,
																		grandParent.position.z)).y;
					}
					else{
					maxHeight = grandParent.TransformPoint(new Vector3 (grandParent.position.x, 
																		0 + grandParent.gameObject.GetComponent<EscalatorScript>().stepHeight/2,
																		grandParent.position.z)).y;
					}

                }
            }
        }

        if (collider.transform.parent != null)
        {
            parentTagC = collider.transform.parent.tag;
            if (parentTagC == "Escalator")
            {
                isTravelledC = collider.transform.parent.GetComponent<EscalatorScript>().travelled;
            }
            
        }


		if (collider.tag == "Stair" && parentTag != "Stair" && !isTravelledC && !onPlatform)
        //if (collider.tag == "Stair" && !isTravelledC && !onPlatform)
        {
            timer.isRunning = true;
            if (parentTag == "Path")
            {
                rig.transform.parent.GetComponent<MarkAsDone>().isDone = true;
            }

            rig.transform.parent = collider.transform;
			rig.transform.parent.parent.GetComponent<EscalatorScript>().travelled = true;

        }

        else if(collider.tag == "Path" && parentTag == "Stair" && !onPlatform && !collider.GetComponent<MarkAsDone>().isDone)
        {
            
			flightsTravelled++;
            rig.transform.parent = collider.transform;
			
            
        }

        if (collider.tag == "Path")
        {
            onPlatform = true;
        }



        parentTag = "dfklghsfjlkgn";
        grandparentTag = "slfjdal;fj";
        parentTagC = "dfklghsfjlkgn";
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Path")
        {
            onPlatform = false;
        }
    }
}
