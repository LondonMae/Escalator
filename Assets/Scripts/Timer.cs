using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public bool isRunning = false, startChecked = false, stopButton = false;

    public int intervalTime = 120;

    public float timePassed = 0, heightChange = 0, startHeight = 0, timeOnPath = 0, totalRotation = 0;

    private int frameCount;
    private Vector3 btmPos;
	private Vector3 testPos;
    private GameObject rig, camera, escalators, startingPlatform, originalEscalator, floor, heightTest;

    private Quaternion oldRot, currRot;
    private string startingTime;


    void Start()
    {
        rig = GameObject.Find("[CameraRig]");
        camera = GameObject.Find("Camera");

        escalators = GameObject.Find("Escalators");
        startingPlatform = GameObject.Find("StartingPlatform");
        originalEscalator = GameObject.Find("Escalator");

		floor = GameObject.Find("Floor");
		heightTest = GameObject.Find("HeightTest");

        btmPos = new Vector3(rig.transform.position.x, rig.transform.position.y, rig.transform.position.z);

		testPos = new Vector3(floor.transform.position.x, floor.transform.position.y, floor.transform.position.z);

        oldRot = camera.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        frameCount++;
        if (isRunning)
        {
            if (!startChecked)
            {
                startChecked = true;
                startHeight = rig.transform.position.y;
                startingTime = DateTime.Now.ToString();
            }

            if (rig.transform.parent.tag == "Path")
            {
                timeOnPath += Time.deltaTime;
            }

            if (frameCount >= 5)
            {
                currRot = camera.transform.rotation;
                frameCount = 0;
                totalRotation += Abs(oldRot.x - currRot.x + oldRot.y - currRot.y + oldRot.z - currRot.z);
                oldRot = currRot;
            }

            timePassed += Time.deltaTime;

            if (timePassed >= intervalTime)
            {
                stopButton = true;
                heightChange = rig.transform.position.y - startHeight;
                foreach (Transform escalator in escalators.transform)
                {
                    if (escalator.gameObject.tag == "Escalator")
                    {
                        escalator.GetComponent<EscalatorScript>().travelled = false;
                    }
                    if (escalator.gameObject.tag == "Path")
                    {
                        escalator.GetComponent<MarkAsDone>().isDone = false;
                    }
                }

            }
        }

        if (stopButton) // Resets values between tests
        {
            rig.transform.position = testPos;
			heightTest.GetComponent<HeightTest>().callTest = true;
            rig.transform.parent = startingPlatform.transform;

            startChecked = false;
            isRunning = false;
            timePassed = 0;

            stopButton = false;
            EscalatorScript escScript = originalEscalator.GetComponent<EscalatorScript>();


            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter("Output.txt", true))
            {
                file.WriteLine("\n --------------- ^_^ --------------- \n");
                file.WriteLine("Test Began at " + startingTime + " and ended at " + DateTime.Now.ToLongTimeString());
                file.WriteLine("Trial Length was " + intervalTime + "s.");

                if (escScript.stepSpeed >= 0)
                {
                    file.WriteLine("Subject was moving upwards.");
                }
                else
                {
                    file.WriteLine("Subject was moving downwards.");
                }

                if (escScript.beStairs)
                {
                    file.WriteLine("Test was done in Stair Mode.");
                }
                else
                {
                    file.WriteLine("Test was done in Escalator Mode.");
                }

                file.WriteLine("Subject moved " + heightChange.ToString() + " meters vertically.");
                file.WriteLine("Subjects head rotated " + totalRotation.ToString() + " units(?) in total.");
                file.WriteLine("Subject spent " + timeOnPath.ToString() + "s on 'Path' blocks.");

                file.WriteLine("");
            }

            heightChange = 0;
            totalRotation = 0;
            timeOnPath = 0;
        }
    }

    //Returns the absolute value of a float
    float Abs(float num)
    {
        if (num < 0)
        {
            return num * -1;
        }
        return num;
    }
}
