using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour {

    public float winRange = 10f;
    public GameObject player1;
    public GameObject player2;

    bool win = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("ButtonX"))
        {
            if (!win && Vector3.Distance(player1.transform.position, player2.transform.position) < winRange)
            {
                win = true;
                SceneManager.LoadScene("Win");
            }
        }
	}
}
