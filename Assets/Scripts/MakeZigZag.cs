using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Makes a zig-zag of moving escalators
public class MakeZigZag : MonoBehaviour
{
    public int numFlights = 1;
    public float pathLength = 3;

    public GameObject pathFab;
    
    
    void Start()
    {
        GameObject escalator = GameObject.Find("Escalator");
        EscalatorScript escalatorScript = escalator.GetComponent<EscalatorScript>();
        GameObject escalatorParent = GameObject.Find("Escalators");

        Quaternion forward = new Quaternion(0, 0, 0, 0) * escalator.transform.rotation, backward = new Quaternion(0,1,0,0) * escalator.transform.rotation;


        float stepHeight = escalatorScript.stepHeight;
        float stepWidth = escalatorScript.stepWidth;
        float stepLength = escalatorScript.stepLength;
        int numSteps = escalatorScript.numSteps;
        int numWalk = escalatorScript.numWalk;

        float maxHeight = stepHeight * numSteps;

        Vector3 pathDimensions = new Vector3(pathLength*1.1f, stepHeight * 1.2f, stepWidth * 2.5f);

        for (int i = 1; i < numFlights; i++)
        {
            if ((i%2) == 1)
            {
                GameObject tempEsc = Instantiate(escalator, new Vector3(pathLength - stepLength + escalator.transform.position.x, 
                                                   maxHeight * i + escalator.transform.position.y, 
                                                   stepWidth * numSteps + escalator.transform.position.z), backward);
                GameObject tempPath = Instantiate(pathFab, new Vector3((pathLength - stepLength) / 2 + escalator.transform.position.x,
                                                   maxHeight * i + escalator.transform.position.y,
                                                   stepWidth * (numSteps + numWalk + 0f) + escalator.transform.position.z), backward);
                tempPath.transform.localScale = pathDimensions;

                tempEsc.transform.parent = escalatorParent.transform;
                tempPath.transform.parent = escalatorParent.transform;

            }
            else
            {
                GameObject tempEsc = Instantiate(escalator, new Vector3(escalator.transform.position.x,
                                                   maxHeight * i + escalator.transform.position.y,
                                                   escalator.transform.position.z), forward);
                GameObject tempPath = Instantiate(pathFab, new Vector3((pathLength - stepLength) / 2 + escalator.transform.position.x,
                                                  maxHeight * i + escalator.transform.position.y,
                                                  -1 * stepWidth * (numWalk + 0f) + escalator.transform.position.z), forward);
                tempPath.transform.localScale = pathDimensions;

                tempEsc.transform.parent = escalatorParent.transform;
                tempPath.transform.parent = escalatorParent.transform;

            }
        }

        if ((numFlights % 2) == 1) //makes the path block at the top
        {
            GameObject tempPath = Instantiate(pathFab, new Vector3((pathLength - stepLength) / 2 + escalator.transform.position.x,
                                                   maxHeight * numFlights + escalator.transform.position.y,
                                                   stepWidth * (numSteps + numWalk + 0f) + escalator.transform.position.z), backward);
            tempPath.transform.localScale = pathDimensions;
            tempPath.name = "TopPlatform";

            tempPath.transform.parent = escalatorParent.transform;
        }

        else
        {
            GameObject tempPath = Instantiate(pathFab, new Vector3((pathLength - stepLength) / 2 + escalator.transform.position.x,
                                              maxHeight * numFlights + escalator.transform.position.y,
                                              -1 * stepWidth * (numWalk + 0f) + escalator.transform.position.z), forward);
            tempPath.transform.localScale = pathDimensions;
            tempPath.name = "TopPlatform";

            tempPath.transform.parent = escalatorParent.transform;
        }
    }

}
