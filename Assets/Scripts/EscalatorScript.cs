using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class EscalatorScript : MonoBehaviour {

    public int numSteps= 10, numWalk= 2; //The numer of steps that make up the escalator and how many steps should make up the 'Lead up' for the ramp
    public float stepHeight=.1f, stepSpeed=.5f, stepWidth=.07f, stepLength = 1; //Step Speed is speed in m/s
    public GameObject stepFab; // the step prefab
    public bool travelled = false;
    public bool beStairs = false;
	public float maxHeight;

    private List<GameObject> steps = new List<GameObject>();
    private float maxDistance, minDistance;
    private Vector3 direction;
    private GameObject rig;
    public bool shouldMove = true;
    private float dTime;
    


	
	void Start () {

        dTime = Time.deltaTime;
        rig = GameObject.Find("[CameraRig]");

        if (beStairs)
        {
            rig.GetComponent<StairScript>().enabled = true;
        }

        //Creates a list of steps in appropriate positions.
        float posX = 0, posY = 0, posZ = 0;
        maxHeight = stepHeight * numSteps;
        maxDistance = stepWidth * (numSteps + (numWalk)); //numWalk doesn't have to be multiplied because 'zero' is the location  where the steps start moving up, not where the steps start
        minDistance = 0 - (stepWidth * numWalk);

        direction = new Vector3(0, stepHeight, stepWidth).normalized;
        Vector3 stepDimensions = new Vector3(stepLength, stepHeight * 1.1f, stepWidth * 1.01f); //step width and height are slightly oversized to prevent gaps

        Quaternion forward = new Quaternion(0, 0, 0, 0);
        for (int i=0; i < numSteps; i++)
        {
            Vector3 pos = transform.TransformPoint(new Vector3(posX, posY, posZ));
            GameObject newStep = Instantiate(stepFab, pos, forward, transform);
            newStep.transform.localScale = stepDimensions;
            steps.Add(newStep); //creates a new step

            posY += stepHeight;
            posZ += stepWidth;
        }

        for (int i=0; i < numWalk; i++)
        {
            Vector3 pos = transform.TransformPoint(new Vector3(0, 0, stepWidth*-1 - (stepWidth * i)));
            GameObject newStep = Instantiate(stepFab, pos, forward, transform);
            newStep.transform.localScale = stepDimensions;
            steps.Add(newStep); //creates a new step

            pos = transform.TransformPoint(new Vector3(0, maxHeight, stepWidth * (numSteps) + (stepWidth * i)));
            newStep = Instantiate(stepFab, pos, forward, transform);
            newStep.transform.localScale = stepDimensions;
            steps.Add(newStep); //creates a new step
        }

        /*
        // Creates motionless steps at the top and bottom of the escalator.
        Vector3 endStepDimensions = new Vector3(stepLength, stepHeight, stepWidth);

        Vector3 baseStepPos = transform.TransformPoint(new Vector3(0, 0, 0));
        Instantiate(stepFab, baseStepPos, forward, transform).transform.localScale = endStepDimensions;
        Vector3 topStepPos = transform.TransformPoint(new Vector3(0, maxHeight, stepWidth* numSteps));
        Instantiate(stepFab, topStepPos, forward, transform).transform.localScale = endStepDimensions;
        */

    }

	
	// Update is called once per frame
	void Update () {
        dTime = Time.deltaTime;

        // Either teleports a step back to the bottom, or moves it up and forward
        if (shouldMove && !beStairs)
        {
            float roundProtect = direction[2] * stepSpeed * dTime / 2f;
            float disIter = (direction[2] * stepSpeed * dTime);

            foreach (GameObject step in steps)
            {
                
                if (transform.InverseTransformPoint(step.transform.position)[2] + disIter - roundProtect>= maxDistance) //roundProtect is there to prevent rounding errors.
                {
                    if (step.transform.childCount > 0)
                    {
                        rig.transform.parent = null;
                        travelled = true;
                    }

                    step.transform.position = transform.TransformPoint(new Vector3(0, 0, -1 * (stepWidth * numWalk) + transform.InverseTransformPoint(step.transform.position)[2] + disIter - maxDistance));

                }
                else if (transform.InverseTransformPoint(step.transform.position)[2] + disIter + roundProtect <= minDistance)
                {
                    if (step.transform.childCount > 0)
                    {
                        rig.transform.parent = null;
                        travelled = true;
                    }

                    step.transform.position = transform.TransformPoint(new Vector3(0, maxHeight, (stepWidth * (numWalk + numSteps) + transform.InverseTransformPoint(step.transform.position)[2] + disIter - minDistance)));

                }
                else // moves thes steps forward
                {
                    if ((transform.InverseTransformPoint(step.transform.position)[2] < (0)) ||
                        transform.InverseTransformPoint(step.transform.position)[2] >= (numSteps * stepWidth))
                    {
                        step.transform.Translate(Vector3.forward * disIter);
                    }
                    else
                    {
                        step.transform.Translate(direction * stepSpeed * dTime);
                    }

                }
            }
        }
	} 
}

