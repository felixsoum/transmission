using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwapper : MonoBehaviour
{
    public Renderer render;
    public Material originalMat;
    public Material swappedMat;

    float swapTimer;
	
	void Update()
    {
	    if (swapTimer <= 0)
        {
            return;
        }
        swapTimer -= Time.deltaTime;
        if (swapTimer <= 0)
        {
            render.material = originalMat;
        }
	}

    public void Swap()
    {
        render.material = swappedMat;
        swapTimer = 5f;
    }
}
