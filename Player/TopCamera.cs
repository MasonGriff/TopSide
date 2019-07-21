using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopCamera : MonoBehaviour {
    public GameObject[] Enviro;
    // Use this for initialization
    void Start () {
        Enviro = CameraMaster.Instance.Enviro;
    }
	
	// Update is called once per frame
	void Update () {
        if (CameraMaster.Instance.TopDown)
        {
            //Enviro = GameObject.FindGameObjectsWithTag("EnvironmentBlock");
            Enviro = CameraMaster.Instance.Enviro;
            foreach (GameObject obj in Enviro)
            {
                obj.SetActive(true);
            }
            foreach(GameObject obj in CameraMaster.Instance.EnviroBlank)
            {
                obj.SetActive(true);
            }
        }
    }
}
