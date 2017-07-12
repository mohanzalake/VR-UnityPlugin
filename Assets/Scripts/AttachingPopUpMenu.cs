using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AttachingPopUpMenu : MonoBehaviour {
    public GameObject MenuPanel;
	// Use this for initialization
	void Start () {
        List<GameObject> rootObjects = new List<GameObject>();
        Scene activeScene = SceneManager.GetActiveScene();
        activeScene.GetRootGameObjects(rootObjects);
        for (int i=0; i<rootObjects.Count; ++i)
        {
            GameObject gameObject = rootObjects[i];
            Debug.Log(gameObject.name + gameObject.GetComponent("AttachingPopUpMenu") + "out");
            if(gameObject.GetComponent<AttachingPopUpMenu> () == null)
            {
                gameObject.AddComponent<AttachingPopUpMenu>();
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hitInfo = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //OnMouseDown
        if(Input.GetMouseButtonDown(0))
        {
                MenuPanel.SetActive(false);
            if(Physics.Raycast(ray, out hitInfo))
            {
                int id = this.GetInstanceID();
                Debug.Log(hitInfo.transform.gameObject.GetInstanceID() + "in update");
                MenuPanel.SetActive(true);
            }
        }
		
	}
}
