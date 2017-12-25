using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void Play()
    {
        SceneManager.LoadScene("Game_Guillaume");
    }


    public void LevelRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void GoToMain()
    {
        SceneManager.LoadScene(0);
    }
}
