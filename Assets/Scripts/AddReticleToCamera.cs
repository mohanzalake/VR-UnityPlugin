using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddReticleToCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject cam = GameObject.FindGameObjectWithTag ("MainCamera");
		var initialTrans = this.transform.localPosition; 
		if (cam != null) {
			this.transform.SetParent (cam.transform);
			this.transform.localPosition=new Vector3(initialTrans.x,initialTrans.y,initialTrans.z);

		} else {
			Debug.Log ("Oops! No camera found");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
