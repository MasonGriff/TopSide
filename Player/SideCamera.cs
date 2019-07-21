using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCamera : MonoBehaviour {
    public GameObject[] Enviro;
    public Transform CameraViewPoint;


	// Use this for initialization
	void Start () {
        Enviro = CameraMaster.Instance.Enviro;

    }
	
	// Update is called once per frame
	void Update () {
		if (!CameraMaster.Instance.TopDown)
        {
            //Enviro = GameObject.FindGameObjectsWithTag("EnvironmentBlock");
            Enviro = CameraMaster.Instance.Enviro;
            foreach (GameObject obj in Enviro)
            {
                if (obj.transform.position.z < CameraViewPoint.position.z)
                {
                    obj.SetActive(false);
                }
            }
        }
        else
        {
            //Enviro = GameObject.FindGameObjectsWithTag("EnvironmentBlock");
            foreach (GameObject obj in Enviro)
            {
                obj.SetActive(true);
            }
        }
	}
}
