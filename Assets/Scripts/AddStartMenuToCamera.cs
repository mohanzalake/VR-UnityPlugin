using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddStartMenuToCamera : MonoBehaviour {


	GameObject cam;
	// Use this for initialization
	void Start () {
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
		var initialTrans = this.transform.localPosition; 
		var initialRot = this.transform.localEulerAngles; 
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

	}
}
