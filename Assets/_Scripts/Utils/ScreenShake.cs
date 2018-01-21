using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

    [SerializeField] private  float screenBump;
    [SerializeField] private float shakeDuration;

    private float shakeTimer;
    private Vector3 camPosition;
    private float xPos;
    private float yPos;

	// Use this for initialization
	void Start () {
        camPosition = transform.localPosition;
        shakeTimer = 0;
        xPos = 0;
        yPos = 0;
	}
	
	// Update is called once per frame
	void Update () {
	   //if (Input.GetMouseButtonDown(0))
    //    {
    //        Shake();
    //    }
	}

   
    public void Shake()
    {
        lock(this)
        {
        if (shakeTimer <= shakeDuration)
        {
            xPos = Random.Range(-1, 2) * screenBump;
            //print("xPos" + xPos);
            yPos = Random.Range(-1, 2) * screenBump;
            //print("yPos" + yPos);

            transform.localPosition = new Vector3(xPos, yPos, -13);

            shakeTimer++;
            StartCoroutine(ShakeWaiting());
        }
        else
        {
            transform.localPosition = camPosition;
            shakeTimer = 0;
        }
        }
    }
   
    IEnumerator ShakeWaiting()
    {
        yield return new WaitForSeconds(0.05f);
        Shake();
        yield return null;
    }
}

