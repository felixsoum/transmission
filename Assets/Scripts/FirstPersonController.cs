using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class FirstPersonController : MonoBehaviour
{
    public float scannerDetectionRange = 20f;
    public float vibrationMax = 0.25f;
    public float vibrationMin = 0.01f;

	// Vibrates once every vibrationBaseFreq frames for vibrationBaseDur frames.
	public const int vibrationBaseFreq = 60;
	public const int vibrationBaseDur = 20;
	private int vibrationFreq = vibrationBaseFreq;		// Number of frames between vibration
	private int vibrationDur = vibrationBaseDur;		// Number of frames to vibrate
	private int vibrationCounter = 0;					// Counts number of frames
	private int vibrationUpper { get { return vibrationFreq + vibrationDur; } }

    public int playerIndex = 1;
    //public GameObject playerCamera;
    public FirstPersonController otherPlayer;
    public RectTransform compass;
    Rigidbody body;
    GameObject[] scanners;

    public GameObject meshObject;
	public Camera camera;
	private Vector3 direction = new Vector3(0,0,1);

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
        //		print (Input.GetAxis ("VerticalRight1"));
    }

    void UpdateMove()
    {
        if (transform.position.y > 5)
        {
            return;
        }
        float horizontal = Input.GetAxis("HorizontalLeft" + playerIndex);
        float vertical = Input.GetAxis("VerticalLeft" + playerIndex);

        Vector3 projectedRight = camera.transform.right;
        projectedRight.y = 0;
        projectedRight.Normalize();

        Vector3 projectedForward = camera.transform.forward;
        projectedForward.y = 0;
        projectedForward.Normalize();

        Vector3 move = projectedRight * horizontal + projectedForward * vertical;

		//direction = camera.transform.TransformDirection(move);
		//direction = new Vector3 (direction.x, 0, direction.z).normalized;

		//move = move.magnitude * direction;

		//move = playerIndex == 1 ? -move : move;		// bug: player 1 movement direction is inverted.

        if (move.magnitude > 1)
        {
            move.Normalize();
        }
        body.AddForce(move * Time.deltaTime * 5000f);

		direction = move;

        meshObject.transform.forward = Vector3.Lerp(meshObject.transform.forward, new Vector3(move.x, 0, move.z).normalized, Time.deltaTime * 10f);
    }

    void UpdateRotate()
    {
        //float horizontal = Input.GetAxis("HorizontalRight" + playerIndex);

        //transform.Rotate(0, horizontal * Time.deltaTime * 200f, 0);

		//Quaternion rotation = Quaternion.LookRotation (direction, new Vector3(0,1,0));
		//this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, 5f * Time.deltaTime);
		//transform.rotation = rotation;

		//if (direction != Vector3.zero) {
		//	Quaternion rotation = Quaternion.LookRotation (direction, Vector3.up);
		//	this.transform.rotation = Quaternion.Slerp (this.transform.rotation, rotation, 5f * Time.deltaTime);
		//}

		//if (vibrationCounter == 50)
		//	transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), 5f * Time.deltaTime);

        Vector3 compassAngle = compass.localEulerAngles;
        compassAngle.z = camera.transform.eulerAngles.y;
        compass.localEulerAngles = compassAngle;


        //float vertical = Input.GetAxis("VerticalRight" + playerIndex);

        //float cameraX = playerCamera.transform.localEulerAngles.x + -vertical * Time.deltaTime * 200f;
        //if (cameraX > 180f)
        //{
        //    cameraX -= 360f;
        //}
        //cameraX = Mathf.Clamp(cameraX, -90f, 90f);
        //playerCamera.transform.localEulerAngles = new Vector3(cameraX, 0, 0);

        
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

        Vibrate(strength);
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
