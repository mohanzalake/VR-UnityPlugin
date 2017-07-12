using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdAxisSwitch : MonoBehaviour {

	public bool thirdAxis=false;
	public Texture2D toggleOff;
	public Texture2D toggleOn;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Transform camTrans = Camera.main.transform;
		Ray ray = new Ray(camTrans.position, camTrans.forward);
		RaycastHit hitInfo = new RaycastHit();
		bool hit = Physics.Raycast(ray, out hitInfo);

		if (OVRInput.GetUp(OVRInput.Button.One) && hit && hitInfo.transform.gameObject.name=="AxisToggle") {
			thirdAxis = !thirdAxis;
			if (thirdAxis) {
				this.GetComponent<Renderer> ().material.mainTexture = toggleOn;


			} 
			else {
				this.GetComponent<Renderer> ().material.mainTexture = toggleOff;
			}
		}
	}
}
