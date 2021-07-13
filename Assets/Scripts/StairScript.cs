using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairScript : MonoBehaviour
{
    // Controls the players movement as they physically move up and down the stairs.

    private GameObject rig, camera, middleMarker, rightCollider;

    private float stepHeight, stepWidth, maxHeight, cameraOldZ, bound1, bound2, startingHeight, lockedY;
    private int numSteps, numWalk;
    private bool found = false, heightLocked = true, enteredBounds = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rig = GameObject.Find("[CameraRig]");
        camera = GameObject.Find("Camera");
        middleMarker = GameObject.Find("MiddleMarker");
		rightCollider = GameObject.Find("RightCollider");

        GameObject escalator = GameObject.Find("Escalator");
        
        EscalatorScript escalatorScript = escalator.GetComponent<EscalatorScript>();

        stepHeight = escalatorScript.stepHeight;
        stepWidth = escalatorScript.stepWidth;

        
        numSteps = escalatorScript.numSteps;
        numWalk = escalatorScript.numWalk;


        maxHeight = stepHeight * numSteps;
        cameraOldZ = camera.transform.position.z - rig.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (rig.transform.parent != null)
        {
            if (rig.transform.parent.tag == "Stair")
            {
                if (!found)
                {
                    Transform escTransform = GameObject.Find("Escalator").transform;
                    bound1 = escTransform.position.z;
                    bound2 = stepWidth * numSteps + escTransform.position.z;
                    startingHeight = escTransform.position.y;
                    found = true;
                }
                
				float dZ = 0;
				
				if (middleMarker.transform.position.x > camera.transform.position.x)
                    {
                        dZ = camera.transform.position.z - rig.transform.position.z - cameraOldZ;
                    }
                else
                    {
                        dZ = cameraOldZ - camera.transform.position.z + rig.transform.position.z;
                    }
				float dY = stepHeight * (dZ/stepWidth);

                //Debug.Log("Connected to Parent");
                if (camera.transform.position.z > bound1 && camera.transform.position.z  < bound2)
                {
                    //Debug.Log("Changing Height");
					enteredBounds = true;
                    rig.transform.Translate(Vector3.up * dY);
                }

				else if(enteredBounds){
					float heightGoal = rightCollider.GetComponent<MakeParent>().maxHeight;
					dY = Abs(dY);
					if (rig.transform.position.y < heightGoal - .05f){
						if(dY > .002){
							rig.transform.Translate(Vector3.up * dY);
						}
						else{
							rig.transform.Translate(Vector3.up * .002f);
						}
						
					}
					else if (rig.transform.position.y > heightGoal + .05f){
						if(dY > .002){
							rig.transform.Translate(Vector3.down * dY);
						}
						else{
							rig.transform.Translate(Vector3.down * .002f);
						}
					}
				}

                
            }

			else{ //parent is not a stair
				enteredBounds = false;
			}
        }

        cameraOldZ = camera.transform.position.z - rig.transform.position.z;
    }

    float Abs(float num)
    {
        if (num < 0)
        {
            return num * -1;
        }
        return num;
    }
}
