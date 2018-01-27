using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    Environment environment;

    private void Awake()
    {
        environment = GameObject.FindGameObjectWithTag("Environment").GetComponent<Environment>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            environment.Swap();
            Destroy(gameObject);
        }
    }
}
