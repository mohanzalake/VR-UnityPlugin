using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSwitch : MonoBehaviour {
	public bool isDebug = false;
	public GameObject debugBtn;
	public GameObject normalBtn;
	//private Texture bTexture = new Texture2D(1, 1);
	void OnGUI() 
	{
		
	}

	// Use this for initialization
	void Start () {
		debugBtn.SetActive(false);
		normalBtn.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		if(OVRInput.GetUp(OVRInput.RawButton.Start))
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast(ray, out hit)) 
			{
				if (hit.collider.name == "normal") 
				{
					Debug.Log (hit.collider.name);
					isDebug = true;
					debugBtn.SetActive(true);
					normalBtn.SetActive(false);
				} 
				else if (hit.collider.name == "debug") 
				{
					Debug.Log (hit.collider.name);
					isDebug = false;
					debugBtn.SetActive(false);
					normalBtn.SetActive(true);
				}
			}  
		}
		/*
		if(Input.GetMouseButton(0))
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast(ray, out hit)) 
			{
				Debug.Log (hit.collider.name);
				Debug.Log (hit.transform.gameObject.transform.position);

				//translate the cubes position from the world to Screen Point
				var screenSpace = Camera.main.WorldToScreenPoint(hit.transform.gameObject.transform.position);
				//calculate any difference between the cubes world position and the mouses Screen position converted to a worldpoint
				var offset = hit.transform.gameObject.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
				var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
				//convert the screen mouse position to world point and adjust with offset
				var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
				//update the position of the object in the world
				//hit.transform.gameObject.transform.position = curPosition;
				Debug.Log(hit.transform.gameObject.name);
			}  
		}
		*/
	}
}
