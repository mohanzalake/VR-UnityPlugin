using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddFuncMenuToCamera : MonoBehaviour {

	GameObject cam;
	public GameObject startMenu;
	Vector3 initialRot;
	GameObject selectedObject;
	void Start () {
		//this.gameObject.GetComponent<Renderer>().enabled=false;
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
		var initialTrans = this.transform.localPosition; 
		initialRot = this.transform.localEulerAngles; 
		selectedObject=GameObject.Find ("Plugin").GetComponent<plugin> ().hitTarget;
		if (cam != null) {
			this.transform.SetParent (cam.transform);
			this.transform.localPosition=new Vector3(initialTrans.x,initialTrans.y,initialTrans.z);
			this.transform.localEulerAngles=new Vector3(initialRot.x,initialRot.y,initialRot.z);

		} else {
			Debug.Log ("Oops! No camera found");
		}
	}

	// Update is called once per frame
	void Update () {

		if (GameObject.Find ("Plugin").GetComponent<plugin> ().isobjectSelected && GameObject.Find ("Plugin").GetComponent<plugin> ().debugMode) {
			this.transform.SetParent (cam.transform.parent);
			//this.transform.localPosition = new Vector3 (startMenu.transform.position.x, startMenu.transform.position.y, startMenu.transform.position.z);
		}
		if (!GameObject.Find ("Plugin").GetComponent<plugin> ().isobjectSelected  && GameObject.Find ("Plugin").GetComponent<plugin> ().debugMode) {
			this.transform.SetParent (cam.transform);
			this.transform.localPosition=new Vector3(startMenu.transform.localPosition.x,startMenu.transform.localPosition.y,startMenu.transform.localPosition.z);
			this.transform.localEulerAngles=new Vector3(startMenu.transform.localEulerAngles.x,startMenu.transform.localEulerAngles.y,startMenu.transform.localEulerAngles.z);
		}
		
	}
}
