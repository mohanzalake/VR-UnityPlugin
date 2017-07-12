using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HideMenuFunction : MonoBehaviour {

	public GameObject TopMenu;
	// Use this for initialization
	Transform camTrans;
	RaycastHit hitInfo = new RaycastHit();
	Ray ray;
	bool hit=false;
	bool hideMenu=false;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		camTrans = Camera.main.transform;
		ray = new Ray(camTrans.position, camTrans.forward);
		hit = Physics.Raycast(ray, out hitInfo);

		if (hit && hitInfo.collider.name=="HideQuad" && (OVRInput.GetUp(OVRInput.Button.One)) )
		{
			hideMenu = !hideMenu;
			if (hideMenu) {
				TopMenu.SetActive (false);
				this.GetComponentInChildren<Text> ().text = "Show Menu";
			} else if (!hideMenu) {
				TopMenu.SetActive (true);
				this.GetComponentInChildren<Text> ().text = "Hide Menu";
			}

		}
	}
}
