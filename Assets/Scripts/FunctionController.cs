using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionController : MonoBehaviour
{
    private bool isSelect = false;
    private RaycastHit selectObject = new RaycastHit();
    //private Texture Remote;
    
    void OnGUI()
    {
        if(isSelect)
        {
            Texture Remote = Resources.Load("Remote", typeof(Texture)) as Texture;
            GUI.DrawTexture(new Rect(Screen.width / 2 - 360, Screen.height - 175, 250, 150), Remote);
        }
        

    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hitInfo = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //OnMouseDown
        //if(OVRInput.GetDown(OVRInput.Button.One))  Replace for Oculus input 
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                selectObject = hitInfo;
                Debug.Log("click" + hitInfo.collider.name);
                isSelect = true;

                int id = this.GetInstanceID();
                Debug.Log(hitInfo.transform.gameObject.GetInstanceID() + "in update");
            }
        }

        if ((Input.GetKeyDown(KeyCode.P) || OVRInput.GetDown(OVRInput.RawButton.DpadUp)) && isSelect)
        {
            Debug.Log("Activate position modification of object" + selectObject.collider.name);
        }
        if ((Input.GetKeyDown(KeyCode.S) || OVRInput.GetDown(OVRInput.RawButton.DpadDown)) && isSelect)
        {
            Debug.Log("Activate size modification of object" + selectObject.collider.name);
            isSelect = false;
        }
        if ((Input.GetKeyDown(KeyCode.T) || OVRInput.GetDown(OVRInput.RawButton.DpadRight)) && isSelect)
        {
            Debug.Log("Activate color modification of object" + selectObject.collider.name);
        }
        if ((Input.GetKeyDown(KeyCode.U) || OVRInput.GetDown(OVRInput.RawButton.DpadLeft)) && isSelect)
        {
            Debug.Log("Activate rotation modification of object" + selectObject.collider.name);
        }

    }
}
