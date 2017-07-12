using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vexe.FastSave;
using Vexe.Runtime.Types;
using UTJ;
namespace FSExamples
{

    public class MarkedTest : BaseBehaviour
    {
        [HideInInspector]
        public string output;
        bool newoutput;

        public GameObject[] allObjects;
        public List<GameObject> dupObjects;
		public List<GameObject> modifiableObjects;
      //  public List<Material> modObjMat;
		public Shader videoShader;
	//	GameObject cam;
        [Show]
        public void PreProcess()
        {


            allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject go in allObjects)
            {
                // Debug.Log("name:" + go.name);
				if ((go.GetComponentInChildren<Camera>()==null) && go.transform.root.name!="Forest" && go.GetComponentInParent<Camera>() == null && go.tag!= "Plugin" && go.GetComponentInParent<OVRPlayerController>()==null && (go.GetComponent<Light>()==null))
                {
					GameObject instObj=Instantiate (go);
					dupObjects.Add(instObj);
					modifiableObjects.Add (go);
                   	go.AddComponent<FSMarker>();
                    go.GetComponent<FSMarker>().ComponentsToSave.Remove(typeof(MeshRenderer));
                    //if(go.GetComponent<Renderer>()!=null)
                   // modObjMat.Add(go.gameObject.GetComponent<Renderer>().material);
                    // go.AddComponent<FSReference>();
                }
            }
            foreach (GameObject dupo in dupObjects)
            {
               dupo.AddComponent<FSMarker>();
				//if(gameObject.GetComponent<Renderer>()!=null)
					//dupo.gameObject.SetActive(false);
                // dupo.AddComponent<FSReference>();
            }



        }
		[Show]public void SaveMarked()
        {
            output = Save.MarkedToMemory().GetString();
			newoutput = Save.MarkedToFile("Assets/a.xml");
            Debug.Log("Saved:"+newoutput);
        }

        [Show] void LoadMarked()
        {
            Load.MarkedFromMemory(output.GetBytes());
			newoutput = Load.MarkedFromFile("Assets/a.xml");
            Debug.Log("Loaded:"+newoutput);
        }

        [Show]
        public void AfterProcess()
        {
            allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (GameObject go in allObjects)
            {
                if (go.GetComponent<FSMarker>() != null)
                {
                    DestroyImmediate(go.GetComponent<FSMarker>());
                    //DestroyImmediate(go.GetComponent<FSReference>());
                }
            }
            foreach (GameObject dupo in dupObjects)
            {
                DestroyImmediate(dupo);
            }
			//DestroyImmediate(cam.GetComponent<MP4Recorder>());
            dupObjects.Clear();
			modifiableObjects.Clear ();
        }
			
    }


	}

