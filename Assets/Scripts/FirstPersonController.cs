using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public int playerIndex;
    public GameObject playerCamera;
    Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        UpdateMove();
        UpdateRotate();

    }

    void UpdateMove()
    {
        float horizontal = Input.GetAxis("HorizontalLeft" + playerIndex);
        float vertical = Input.GetAxis("VerticalLeft" + playerIndex);

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        if (move.magnitude > 1)
        {
            move.Normalize();
        }
        body.AddForce(move * Time.deltaTime * 2000f);
    }

    void UpdateRotate()
    {
        float horizontal = Input.GetAxis("HorizontalRight" + playerIndex);

        transform.Rotate(0, horizontal * Time.deltaTime * 200f, 0);

        float vertical = Input.GetAxis("VerticalRight" + playerIndex);

        float cameraX = playerCamera.transform.localEulerAngles.x + -vertical * Time.deltaTime * 200f;
        if (cameraX > 180f)
        {
            cameraX -= 360f;
        }
        cameraX = Mathf.Clamp(cameraX, -90f, 90f);
        playerCamera.transform.localEulerAngles = new Vector3(cameraX, 0, 0);
    }
}
