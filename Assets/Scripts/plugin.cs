using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Vexe.FastSave;
using Vexe.Runtime.Types;
using System;
using FSExamples;
using UnityEngine.UI;
using UTJ;
public class plugin : MonoBehaviour
{

    // Use this for initialization
    List<GameObject> rootObjects = new List<GameObject>();
    private Vector3 screenPoint;
    private Vector3 offset;
    public GameObject selectedObject;
    public bool isobjectSelected = false;


    public Material outlineMat;
    Material existMat;
    public Material highLightMat;
    public Renderer rendSelectedObj;
    public Renderer rendDupObj;
    public GameObject hitTarget;
    Transform camTrans;
    RaycastHit hitInfo = new RaycastHit();
    Ray ray;
    public Dictionary<String, GameObject> objSet = new Dictionary<String, GameObject>();
    public bool useMouse = false;
    bool hit = false;

	public bool modifyObjectsP=false;
	public bool modifyObjectsR=false;	
	public bool modifyObjectsS=false;
	public GameObject toggleAxis;

	public GameObject StartMenu;
	public GameObject FunctionMenu;


	public bool RecordVideo=true;
	IMovieRecorder m_recorder;
	public Shader videoShader;
	public bool debugMode;
	public GameObject BelowMenu;

	public GameObject TextLeft;
	public GameObject TextRight;
	public GameObject rotLeft;
	public GameObject rotRight;


    void Start()
    {
		
		toggleAxis.SetActive (false);
        outlineMat = Resources.Load("Outlined_Silhouetted_Diffuse", typeof(Material)) as Material;
        int i = 0;
        foreach (GameObject go in this.GetComponent<MarkedTest>().dupObjects)
        {
            objSet.Add(this.GetComponent<MarkedTest>().modifiableObjects[i].name, go);
            i++;
			if(go.GetComponent<Collider> ()!=null)
			go.GetComponent<Collider> ().enabled = false;
            go.SetActive(false);
        }


		//Menu Handle

		StartMenu.SetActive (false);
		FunctionMenu.SetActive (false);
		toggleAxis.SetActive (false);


		if (RecordVideo) {
			GameObject cam=GameObject.FindGameObjectWithTag ("MainCamera");
			cam.AddComponent<MP4Recorder>();
			cam.GetComponent<MP4Recorder> ().m_outputDir.m_root = DataPath.Root.CurrentDirectory;
			cam.GetComponent<MP4Recorder> ().m_resolutionWidth = 1080;
			cam.GetComponent<MP4Recorder> ().m_frameRateMode = MP4Recorder.FrameRateMode.Variable;
			cam.GetComponent<MP4Recorder> ().m_framerate = 30;
			cam.GetComponent<MP4Recorder> ().m_captureEveryNthFrame = 1;
			cam.GetComponent<MP4Recorder> ().m_videoBitrate = 8192000;
			cam.GetComponent<MP4Recorder> ().m_audioBitrate = 64000;
			cam.GetComponent<MP4Recorder> ().m_shCopy = videoShader;

			m_recorder = cam.GetComponent<MP4Recorder>();
			m_recorder.BeginRecording ();
		}

    }

    // Update is called once per frame
    void Update()
    {


		debugMode = GameObject.Find("ToggleSwitch").GetComponent<AddToggleSwitchToCamera>().DebugMode;
		if (debugMode && !isobjectSelected) {
			StartMenu.SetActive (true);
			FunctionMenu.SetActive (false);
		}
		if (debugMode && isobjectSelected) {
			StartMenu.SetActive (false);
			FunctionMenu.SetActive (true);
		}
		if (!debugMode) {
			StartMenu.SetActive (false);
			FunctionMenu.SetActive (false);
			unSelectObject ();
		}


        if (!useMouse)
        {
            camTrans = Camera.main.transform;
            ray = new Ray(camTrans.position, camTrans.forward);
            hit = Physics.Raycast(ray, out hitInfo);
        }
        else if (useMouse)
        {
            hitInfo = new RaycastHit();
            hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        }
		if ((hit && debugMode && hitInfo.transform.gameObject.tag!="Plugin" && (OVRInput.GetUp(OVRInput.Button.One)) || (hit && Input.GetMouseButtonDown(0))))
        {
			Debug.Log ("Root:"+hitInfo.transform.root.name);
            isobjectSelected = !isobjectSelected;
            hitTarget = hitInfo.transform.gameObject;
            if (isobjectSelected)
            {
				if (objSet.ContainsKey(hitTarget.name))
				{
					rendSelectedObj = hitTarget.GetComponent<Renderer>();
					existMat = rendSelectedObj.material;
					rendSelectedObj.material = outlineMat;

					objSet[hitTarget.name].SetActive(true);
					selectedObject = objSet[hitTarget.name];
				}

            }

            if (!isobjectSelected)
            {

				unSelectObject ();
            }

        }

		if (OVRInput.GetUp(OVRInput.Button.One) && hit && hitInfo.transform.gameObject.name=="PosQuad") {
			modifyObjectsP = true;
			modifyObjectsR = false;
			modifyObjectsS = false;
			GameObject.Find ("PositionButton").GetComponent<Image> ().color = new Color (1,1,0);
			GameObject.Find ("RotationButton").GetComponent<Image> ().color = new Color (47/255.0f,184/255.0f,206/255.0f);
			GameObject.Find ("SizeButton").GetComponent<Image> ().color = new Color (47/255.0f,184/255.0f,206/255.0f);
			toggleAxis.SetActive (true);

			} 
		if (OVRInput.GetUp(OVRInput.Button.One) && hit && hitInfo.transform.gameObject.name=="RotQuad") {
			modifyObjectsR = true;
			modifyObjectsP = false;
			modifyObjectsS = false;
			GameObject.Find ("PositionButton").GetComponent<Image> ().color = new Color (47/255.0f,184/255.0f,206/255.0f);
			GameObject.Find ("RotationButton").GetComponent<Image> ().color = new Color (1,1,0);
			GameObject.Find ("SizeButton").GetComponent<Image> ().color = new Color (47/255.0f,184/255.0f,206/255.0f);
			toggleAxis.SetActive (true);
		} 
		if (OVRInput.GetUp(OVRInput.Button.One) && hit && hitInfo.transform.gameObject.name=="SizeQuad") {
			modifyObjectsS = true;
			modifyObjectsP = false;
			modifyObjectsR = false;
			GameObject.Find ("PositionButton").GetComponent<Image> ().color = new Color (47/255.0f,184/255.0f,206/255.0f);
			GameObject.Find ("RotationButton").GetComponent<Image> ().color = new Color (47/255.0f,184/255.0f,206/255.0f);
			GameObject.Find ("SizeButton").GetComponent<Image> ().color = new Color (1,1,0);
			toggleAxis.SetActive (true);
		} 

		if (debugMode && isobjectSelected && modifyObjectsP) {
			moveObject_relative ();
		}
		if (debugMode && isobjectSelected && modifyObjectsR) {
			rotateObject ();
		}
		if (debugMode && isobjectSelected && modifyObjectsS) {
			scaleObject ();
		}
    }

	public void unSelectObject(){
		toggleAxis.SetActive (false);
		if(rendSelectedObj!=null)
		rendSelectedObj.material = existMat;
		isobjectSelected = false;
		modifyObjectsS = false;
		modifyObjectsP = false;
		modifyObjectsR = false;
		toggleAxis.SetActive (false);
		if (GameObject.Find ("PositionButton") != null) {
			Debug.Log ("This");
			GameObject.Find ("PositionButton").GetComponent<Image> ().color = new Color (47 / 255.0f, 184 / 255.0f, 206 / 255.0f);
			GameObject.Find ("RotationButton").GetComponent<Image> ().color = new Color (47 / 255.0f, 184 / 255.0f, 206 / 255.0f);
			GameObject.Find ("SizeButton").GetComponent<Image> ().color = new Color (47 / 255.0f, 184 / 255.0f, 206 / 255.0f);
		}
	}
    void selectObject()
    {
        if (objSet.ContainsKey(hitTarget.name))
        {
			rendSelectedObj = hitTarget.GetComponent<Renderer>();
			existMat = rendSelectedObj.material;
			rendSelectedObj.material = outlineMat;

			objSet[hitTarget.name].SetActive(true);
			selectedObject = objSet[hitTarget.name];
        }


    }
	void moveObject_relative()
	{
		BelowMenu.SetActive (true);
		TextLeft.SetActive (true);
		TextRight.SetActive (true);
		rotLeft.SetActive (false);
		rotRight.SetActive (false);
		rendDupObj = selectedObject.GetComponent<Renderer> ();
		rendDupObj.material = highLightMat;
		if (!toggleAxis.GetComponent<thirdAxisSwitch> ().thirdAxis) {
			if (OVRInput.Get (OVRInput.Button.DpadUp) || Input.GetKey (KeyCode.UpArrow)) {
				selectedObject.transform.Translate(Vector3.up* Time.deltaTime, Camera.main.transform);
			}
			if (OVRInput.Get (OVRInput.Button.DpadDown) || Input.GetKey (KeyCode.DownArrow)) {
				selectedObject.transform.Translate(Vector3.down* Time.deltaTime, Camera.main.transform);
			}
			if (OVRInput.Get (OVRInput.Button.DpadLeft) || Input.GetKey (KeyCode.LeftArrow)) {
				selectedObject.transform.Translate(Vector3.left* Time.deltaTime, Camera.main.transform);
			}
			if (OVRInput.Get (OVRInput.Button.DpadRight) || Input.GetKey (KeyCode.RightArrow)) {
				selectedObject.transform.Translate(Vector3.right* Time.deltaTime, Camera.main.transform);
			}
		} else {

			if (OVRInput.Get (OVRInput.Button.DpadUp) || Input.GetKey (KeyCode.UpArrow)) {
				selectedObject.transform.Translate(Vector3.forward* Time.deltaTime, Camera.main.transform);
			}
			if (OVRInput.Get (OVRInput.Button.DpadDown) || Input.GetKey (KeyCode.DownArrow)) {
				selectedObject.transform.Translate(Vector3.back* Time.deltaTime, Camera.main.transform);
			}

		}
	}
    void moveObject()
    {
		//
	
				rendDupObj = selectedObject.GetComponent<Renderer> ();
				rendDupObj.material = highLightMat;
				if (!toggleAxis.GetComponent<thirdAxisSwitch> ().thirdAxis) {
					if (OVRInput.Get (OVRInput.Button.DpadUp) || Input.GetKey (KeyCode.UpArrow)) {
				selectedObject.transform.localPosition = new Vector3 (selectedObject.transform.localPosition.x, selectedObject.transform.localPosition.y + 0.05f, selectedObject.transform.localPosition.z);
					}
					if (OVRInput.Get (OVRInput.Button.DpadDown) || Input.GetKey (KeyCode.DownArrow)) {
				selectedObject.transform.localPosition = new Vector3 (selectedObject.transform.localPosition.x, selectedObject.transform.localPosition.y - 0.05f, selectedObject.transform.localPosition.z);
					}
					if (OVRInput.Get (OVRInput.Button.DpadLeft) || Input.GetKey (KeyCode.LeftArrow)) {
				selectedObject.transform.localPosition = new Vector3 (selectedObject.transform.localPosition.x - 0.05f, selectedObject.transform.localPosition.y, selectedObject.transform.localPosition.z);
					}
					if (OVRInput.Get (OVRInput.Button.DpadRight) || Input.GetKey (KeyCode.RightArrow)) {
				selectedObject.transform.localPosition = new Vector3 (selectedObject.transform.localPosition.x + 0.05f, selectedObject.transform.localPosition.y, selectedObject.transform.localPosition.z);
					}
				} else {
			
					if (OVRInput.Get (OVRInput.Button.DpadUp) || Input.GetKey (KeyCode.UpArrow)) {
				selectedObject.transform.localPosition = new Vector3 (selectedObject.transform.localPosition.x, selectedObject.transform.localPosition.y, selectedObject.transform.localPosition.z + 0.05f);
					}
					if (OVRInput.Get (OVRInput.Button.DpadDown) || Input.GetKey (KeyCode.DownArrow)) {
				selectedObject.transform.localPosition = new Vector3 (selectedObject.transform.localPosition.x, selectedObject.transform.localPosition.y, selectedObject.transform.localPosition.z - 0.05f);
					}
			
		}
    }
	void rotateObject()
	{
		BelowMenu.SetActive (true);
		rotLeft.SetActive (true);
		rotRight.SetActive (true);
		TextLeft.SetActive (false);
		TextRight.SetActive (false);
			rendDupObj = selectedObject.GetComponent<Renderer>();
			rendDupObj.material = highLightMat;
			if (!toggleAxis.GetComponent<thirdAxisSwitch> ().thirdAxis) {
				if (OVRInput.Get (OVRInput.Button.DpadLeft) || Input.GetKey (KeyCode.LeftArrow)) {
					selectedObject.transform.localEulerAngles = new Vector3 (selectedObject.transform.localEulerAngles.x, selectedObject.transform.localEulerAngles.y + 0.5f, selectedObject.transform.localEulerAngles.z);
				}
				if (OVRInput.Get (OVRInput.Button.DpadRight) || Input.GetKey (KeyCode.RightArrow)) {
					selectedObject.transform.localEulerAngles = new Vector3 (selectedObject.transform.localEulerAngles.x, selectedObject.transform.localEulerAngles.y - 0.5f, selectedObject.transform.localEulerAngles.z);
				}
				if (OVRInput.Get (OVRInput.Button.DpadUp) || Input.GetKey (KeyCode.UpArrow)) {
					selectedObject.transform.localEulerAngles = new Vector3 (selectedObject.transform.localEulerAngles.x + 0.5f, selectedObject.transform.localEulerAngles.y, selectedObject.transform.localEulerAngles.z);
				}
				if (OVRInput.Get (OVRInput.Button.DpadDown) || Input.GetKey (KeyCode.DownArrow)) {
					selectedObject.transform.localEulerAngles = new Vector3 (selectedObject.transform.localEulerAngles.x - 0.5f, selectedObject.transform.localEulerAngles.y, selectedObject.transform.localEulerAngles.z);
				}
			}
			else {
				if (OVRInput.Get (OVRInput.Button.DpadLeft) || Input.GetKey (KeyCode.LeftArrow)) {
					selectedObject.transform.localEulerAngles = new Vector3 (selectedObject.transform.localEulerAngles.x, selectedObject.transform.localEulerAngles.y, selectedObject.transform.localEulerAngles.z + 0.5f);
				}
				if (OVRInput.Get (OVRInput.Button.DpadRight) || Input.GetKey (KeyCode.RightArrow)) {
					selectedObject.transform.localEulerAngles = new Vector3 (selectedObject.transform.localEulerAngles.x, selectedObject.transform.localEulerAngles.y , selectedObject.transform.localEulerAngles.z - 0.5f);
				}
		}
	}

	void scaleObject()
	{

		BelowMenu.SetActive (false);
			rendDupObj = selectedObject.GetComponent<Renderer>();
			rendDupObj.material = highLightMat;
			//if (toggleAxis.GetComponent<thirdAxisSwitch> ().thirdAxis) {
//				if (OVRInput.Get (OVRInput.Button.DpadUp) || Input.GetKey (KeyCode.UpArrow)) {
//					selectedObject.transform.localScale = new Vector3 (selectedObject.transform.localScale.x, selectedObject.transform.localScale.y + 0.005f, selectedObject.transform.localScale.z);
//				}
//				if (OVRInput.Get (OVRInput.Button.DpadDown) || Input.GetKey (KeyCode.DownArrow)) {
//					selectedObject.transform.localScale = new Vector3 (selectedObject.transform.localScale.x, selectedObject.transform.localScale.y - 0.005f, selectedObject.transform.localScale.z);
//				}
//				if (OVRInput.Get (OVRInput.Button.DpadLeft) || Input.GetKey (KeyCode.LeftArrow)) {
//					selectedObject.transform.localScale = new Vector3 (selectedObject.transform.localScale.x - 0.005f, selectedObject.transform.localScale.y, selectedObject.transform.localScale.z);
//				}
//				if (OVRInput.Get (OVRInput.Button.DpadRight) || Input.GetKey (KeyCode.RightArrow)) {
//					selectedObject.transform.localScale = new Vector3 (selectedObject.transform.localScale.x + 0.005f, selectedObject.transform.localScale.y, selectedObject.transform.localScale.z);
//				}
			//}
		//else if(!toggleAxis.GetComponent<thirdAxisSwitch> ().thirdAxis)
		{
				if (OVRInput.Get (OVRInput.Button.DpadUp) || Input.GetKey (KeyCode.UpArrow)) {
					selectedObject.transform.localScale = new Vector3 (selectedObject.transform.localScale.x+ 0.005f, selectedObject.transform.localScale.y + 0.005f, selectedObject.transform.localScale.z + 0.005f);
				}
				if (OVRInput.Get (OVRInput.Button.DpadDown) || Input.GetKey (KeyCode.DownArrow)) {
					selectedObject.transform.localScale = new Vector3 (selectedObject.transform.localScale.x - 0.005f, selectedObject.transform.localScale.y- 0.005f , selectedObject.transform.localScale.z- 0.005f);
				}

		}
	}
    private void OnApplicationQuit()
    {
        //Renderer rendObj;
        //foreach (KeyValuePair<string, Material> kvp in objMat)
        //{
        //    rendObj = objSet[kvp.Key].GetComponent<Renderer>();
        //    rendObj.material = kvp.Value;
        //}
		if(rendSelectedObj!=null)
        rendSelectedObj.material = existMat;
        this.GetComponent<MarkedTest>().SaveMarked();
		if(RecordVideo)
		m_recorder.EndRecording ();

    }
		
}
