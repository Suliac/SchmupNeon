using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnEndAnimation : MonoBehaviour {

    bool wantToDisable = false;

	public void DisableObject()
    {
        wantToDisable = true;
    }
	
	// Update is called once per frame
	void Update () {
		if(wantToDisable)
        {
            wantToDisable = false;
            gameObject.SetActive(false);
        }
	}
}
