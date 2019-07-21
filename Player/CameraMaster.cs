using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMaster : MonoBehaviour {

    public static CameraMaster Instance { get; set; }
    public GameObject[] EnviroBlank, EnviroSide, Enviro;
    public Transform Player;

    public GameObject CamSide, CamTop, MainCam;

    public bool TopDown = true;

    // Use this for initialization
    private void Awake()
    {
        Instance = this;
    }

    void Start () {
        Enviro = GameObject.FindGameObjectsWithTag("EnvironmentBlock");
        EnviroBlank = Enviro;
        EnviroSide = Enviro;
        TopDown = false;
	}
	
	// Update is called once per frame
	void Update () {
        /*Vector3 NewPosition = transform.position;
        NewPosition.x = Player.position.x;
        NewPosition.z = Player.position.z;
        transform.position = NewPosition;*/
        if (TopDown)
        {
            Enviro = GameObject.FindGameObjectsWithTag("EnvironmentBlock");

            MainCam = CamTop;
            CamTop.SetActive(true);
            CamSide.SetActive(false);
        }
        else
        {
            EnviroSide = GameObject.FindGameObjectsWithTag("EnvironmentBlock");
            Enviro = EnviroSide;
            MainCam = CamSide;
            CamTop.SetActive(false);
            CamSide.SetActive(true);
        }
	}

    

}
