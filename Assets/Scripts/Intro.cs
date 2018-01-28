using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public GameObject Text1;
    public GameObject Text2;
    public GameObject Text3;

    private void Awake()
    {
        Text1.SetActive(false);  
        Text2.SetActive(false);  
        Text3.SetActive(false);
    }

    void Start()
    {
        Invoke("Show1", 1);
        Invoke("Show2", 3);
        Invoke("Show3", 5);
        Invoke("Clear", 12);
    }

    void Show1()
    {
        Text1.SetActive(true);
    }

    void Show2()
    {
        Text2.SetActive(true);
    }

    void Show3()
    {
        Text3.SetActive(true);
    }

    void Clear()
    {
        Destroy(gameObject);
    }
}
