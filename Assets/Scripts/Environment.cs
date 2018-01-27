using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    MaterialSwapper[] swappers;

    void Awake()
    {
        swappers = transform.GetComponentsInChildren<MaterialSwapper>();
    }

    public void Swap()
    {
        foreach (MaterialSwapper swapper in swappers)
        {
            if (swapper)
            {
                swapper.Swap();
            }
        }
    }
}
