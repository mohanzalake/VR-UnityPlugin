using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToggleSwitchToCamera : MonoBehaviour {

	// Use this for initialization
	public bool DebugMode=false;
	public Texture2D toggleNormal;
	public Texture2D toggleDebug;
	GameObject cam ;
	void Start () {
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
		var initialTrans = this.transform.localPosition; 
		if (cam != null) {
			this.transform.SetParent (cam.transform.parent);
			this.transform.localPosition=new Vector3(initialTrans.x,initialTrans.y,initialTrans.z);
			//this.transform.Translate(Vector3.forward,Camera.main.transform);

		} else {
			Debug.Log ("Oops! No camera found");
		}

	}

	// Update is called once per frame
	void Update () {
		//this.transform.position (Vector3.forward);
		Transform camTrans = Camera.main.transform;
		Ray ray = new Ray(camTrans.position, camTrans.forward);
		RaycastHit hitInfo = new RaycastHit();
		bool hit = Physics.Raycast(ray, out hitInfo);
		var playerController = this.transform.GetComponentInParent<OVRPlayerController> ();
		if (OVRInput.GetUp(OVRInput.Button.One) && hit && hitInfo.transform.gameObject.name=="ToggleSwitch") {
			DebugMode = !DebugMode;
			if (DebugMode) {
				if(playerController!=null)
				playerController.enabled = false;
				
				this.GetComponent<Renderer> ().material.mainTexture = toggleDebug;
			} else {
				if(playerController!=null)
				playerController.enabled = true;
				
				this.GetComponent<Renderer> ().material.mainTexture = toggleNormal;
			}
		}

	}
}
