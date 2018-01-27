using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class FirstPersonController : MonoBehaviour
{
    public float scannerDetectionRange = 20f;
    public float vibrationMax = 0.25f;
    public float vibrationMin = 0.01f;

	public const int vibrationBaseFreq = 60;
	public const int vibrationBaseDur = 20;
	private int vibrationUpper { get { return vibrationFreq + vibrationDur; } }
	private int vibrationFreq = vibrationBaseFreq;		// Number of frames between vibration
	private int vibrationDur = vibrationBaseDur;		// Number of frames to vibrate
	private int vibrationCounter = 0;					// Counts number of frames

    public int playerIndex = 1;
    public GameObject playerCamera;
    public FirstPersonController otherPlayer;
    public RectTransform compass;
    Rigidbody body;
    GameObject[] scanners;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    void Start()
    {
        scanners = GameObject.FindGameObjectsWithTag("Scanner");
	}
	
	void Update()
    {
        UpdateMove();
        UpdateRotate();
        UpdateScannerVibration();

		vibrationCounter++;
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

        Vector3 compassAngle = compass.localEulerAngles;
        compassAngle.z = transform.eulerAngles.y;
        compass.localEulerAngles = compassAngle;


        float vertical = Input.GetAxis("VerticalRight" + playerIndex);

        float cameraX = playerCamera.transform.localEulerAngles.x + -vertical * Time.deltaTime * 200f;
        if (cameraX > 180f)
        {
            cameraX -= 360f;
        }
        cameraX = Mathf.Clamp(cameraX, -90f, 90f);
        playerCamera.transform.localEulerAngles = new Vector3(cameraX, 0, 0);
    }

    void UpdateScannerVibration()
    {
        float closestDistance = float.MaxValue;
        foreach (GameObject scanner in scanners)
        {
            if (scanner)
            {
                closestDistance = Mathf.Min(Vector3.Distance(scanner.transform.position, transform.position), closestDistance);
            }
        }

        float strength = 0;
		if (closestDistance <= scannerDetectionRange && vibrationFreq <= vibrationCounter && vibrationCounter <= vibrationUpper) {
			strength = (1f - closestDistance / scannerDetectionRange) * (vibrationMax - vibrationMin) + vibrationMin;

			vibrationFreq =	vibrationBaseFreq - (int) ((1.2f - closestDistance / scannerDetectionRange) * (vibrationBaseFreq));
		} 

		if (vibrationCounter >= vibrationUpper) {
			vibrationCounter = 0;
		}

        otherPlayer.Vibrate(strength);
    }

    public void Vibrate(float strength)
    {
        GamePad.SetVibration((PlayerIndex)(playerIndex - 1), strength, strength);
    }

    private void OnDestroy()
    {
        GamePad.SetVibration((PlayerIndex)(playerIndex - 1), 0, 0);
    }
}
