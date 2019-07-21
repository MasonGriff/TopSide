using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour {

    public string NextLevelSceneName = "";

    bool transitioningGo;

	// Use this for initialization
	void Start () {
        transitioningGo = false;
	}
	

    private void OnTriggerEnter(Collider myColl)
    {
        if(myColl.tag == "Player")
        {
            if (!transitioningGo)
            {
                transitioningGo = true;
                Invoke("ToNextLevel", 1f);
            }
        }
    }

    void ToNextLevel()
    {
        SceneManager.LoadScene(NextLevelSceneName);
    }
}
