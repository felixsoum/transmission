using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public GameObject lightRed;
    public GameObject lightBlue;
    public GameObject lightGreen;
    public GameObject lightOrange;
    Environment environment;

    private void Awake()
    {
        environment = GameObject.FindGameObjectWithTag("Environment").GetComponent<Environment>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //lightRed.SetActive(true);

            environment.Swap();
            Destroy(gameObject);
        }
    }
}
