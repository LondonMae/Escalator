using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HeightTest : MonoBehaviour
{
	private GameObject escalator, floor, tempEsc, cam, controller, rig;
	private Vector3 btmPos;
	private float controllerHeight = 0, oldHeight = 0;
	private float timeCount = 0;
	private bool isFirstFrame = true;

	public float guessedHeight = 0f;
	public bool callTest = false, testRunning = false;
	

	public SteamVR_Action_Boolean GrabPinch, DPadClick;
    // Start is called before the first frame update
    void Start()
    {
		rig = GameObject.Find("[CameraRig]");
		tempEsc = GameObject.Find("Escalator");
		floor = GameObject.Find("Floor");
		controller = GameObject.Find("Controller (right)");
		
		escalator = Instantiate(tempEsc, new Vector3(floor.transform.position.x, 
														 floor.transform.position.y - .5f*tempEsc.GetComponent<EscalatorScript>().stepHeight + .01f, 
														 floor.transform.position.z), new Quaternion(0, 0, 0, 0));
		escalator.GetComponent<EscalatorScript>().shouldMove = false;

		btmPos = new Vector3(rig.transform.position.x, rig.transform.position.y, rig.transform.position.z);
		
    }

    // Update is called once per frame
    void Update()
    {
		if(isFirstFrame){
			foreach (Transform step in escalator.transform){
				step.gameObject.GetComponent<Collider>().enabled = false;
			}
			isFirstFrame = false;
		}
        if (callTest)
		{
			cam = GameObject.Find("Camera");
			Quaternion camFacing = new Quaternion(0, cam.transform.rotation.y, 0, cam.transform.rotation.w);
		    escalator.transform.rotation = camFacing;
			escalator.transform.position = new Vector3(floor.transform.position.x, 
													   floor.transform.position.y - .5f*escalator.GetComponent<EscalatorScript>().stepHeight +.15f,
													   floor.transform.position.z);
			escalator.transform.Translate(Vector3.forward*5);
			callTest = false;
			testRunning = true;
			escalator.transform.localScale = new Vector3(escalator.transform.localScale.x, .01f, escalator.transform.localScale.z);
			oldHeight = controller.transform.position.y;
		}

		if(testRunning){
		
			controllerHeight = controller.transform.position.y;
			
			if(GrabPinch.state){
				escalator.transform.localScale += new Vector3(0, (controllerHeight-oldHeight)*3f , 0);
				escalator.transform.Translate(Vector3.down * .5f*escalator.GetComponent<EscalatorScript>().stepHeight * (controllerHeight-oldHeight) * 3f);
			}

			if(DPadClick.state){
				guessedHeight = escalator.transform.localScale.y;
				rig.transform.position = btmPos;

				using (System.IO.StreamWriter file =
				new System.IO.StreamWriter(@"Output.txt", true))
				{
					file.WriteLine("Height Guessed was " + guessedHeight.ToString() + " where 1 is exactly correct.");
				}
				testRunning = false;
			}
			
			oldHeight = controllerHeight;
		}

		
    }

}
