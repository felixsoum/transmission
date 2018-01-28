using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Const variables
    private const float MIN_CATCH_SPEED_DAMP = 0f;
    private const float MAX_CATCH_SPEED_DAMP = 1f;
    private const float MIN_ROTATION_SMOOTHING = 0f;
    private const float MAX_ROTATION_SMOOTHING = 30f;

    // Serializable fields
    [SerializeField]
    private Transform target = null; // The target to follow

    [SerializeField]
    [Range(MIN_CATCH_SPEED_DAMP, MAX_CATCH_SPEED_DAMP)]
    private float catchSpeedDamp = MIN_CATCH_SPEED_DAMP;

    [SerializeField]
    [Range(MIN_ROTATION_SMOOTHING, MAX_ROTATION_SMOOTHING)]
    [Tooltip("How fast the camera rotates around the pivot")]
    private float rotationSmoothing = 15.0f;

    // private fields
    private Transform rig; // The root transform of the camera rig
    private Transform pivot; // The point at which the camera pivots around
    private Quaternion pivotTargetLocalRotation; // Controls the X Rotation (Tilt Rotation)
    private Quaternion rigTargetLocalRotation; // Controlls the Y Rotation (Look Rotation)
    private Vector3 cameraVelocity; // The velocity at which the camera moves

	private int playerIndex;
	private Camera camera;

    protected virtual void Awake()
    {
        this.pivot = this.transform.parent;
        this.rig = this.pivot.parent;

        this.transform.localRotation = Quaternion.identity;
		playerIndex = target.GetComponent<FirstPersonController> ().playerIndex;

		camera = GetComponent<Camera> ();
    }

	private float lookAngle = 0f;
	private float tiltAngle = 0f;
	private float mouseSensitivity = 3f;
	private float minTiltAngle = -75f;
	private float maxTiltAngle = 45f;

    protected virtual void Update()
    {

		float mouseX = Input.GetAxis("HorizontalRight" + playerIndex);
		float mouseY = Input.GetAxis("VerticalRight" + playerIndex);

		//mouseX = 0f;

		// Adjust the look angle (Y Rotation)
		lookAngle += mouseX * mouseSensitivity;
		lookAngle %= 360f;

		// Adjust the tilt angle (X Rotation)
		tiltAngle += mouseY * mouseSensitivity;
		tiltAngle %= 360f;
		tiltAngle = ClampAngle(tiltAngle, minTiltAngle, maxTiltAngle);

		var controlRotation = Quaternion.Euler(-tiltAngle, lookAngle, 0f);


        //var controlRotation = PlayerInput.GetMouseRotationInput();
        this.UpdateRotation(controlRotation);
    }

	public static float ClampAngle(float angle, float min, float max)
	{
		while (angle < -360f || angle > 360f)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			else if (angle > 360f)
			{
				angle -= 360f;
			}
		}

		return Mathf.Clamp(angle, min, max);
	}

    protected virtual void LateUpdate()
    {
        this.FollowTarget();
    }

    //public void SetDistanceToTarget(float distanceToTarget)
    //{
    //    Vector3 cameraTargetLocalPosition = Vector3.zero;
    //    cameraTargetLocalPosition.z = -distanceToTarget;
    //    this.transform.localPosition = cameraTargetLocalPosition;
    //}

    private void FollowTarget()
    {
        if (this.target == null)
        {
            return;
        }

        this.rig.position = Vector3.SmoothDamp(this.rig.position, this.target.transform.position, ref this.cameraVelocity, this.catchSpeedDamp);
    }

    private void UpdateRotation(Quaternion controlRotation)
    {
		//Vector3 displacement = new Vector2 (Input.GetAxisRaw("HorizontalRight" + playerIndex), Input.GetAxis("VerticalRight" + playerIndex));

		//print (displacement);

		//displacement = camera.transform.TransformDirection(displacement);
		//displacement.y = 0;

		//print (displacement);

		//Vector3 worldForward = transform.TransformDirection(Vector3.forward);
		//Vector3 moveDir = Vector3.RotateTowards(worldForward, displacement, RotationSpeed * Time.deltaTime, 0.0f);
		//Quaternion rotation = Quaternion.LookRotation(moveDir);
		//transform.rotation = rotation;

        if (this.target != null)
        {
            // Y Rotation (Look Rotation)
            this.rigTargetLocalRotation = Quaternion.Euler(0f, controlRotation.eulerAngles.y, 0f);

            // X Rotation (Tilt Rotation)
            this.pivotTargetLocalRotation = Quaternion.Euler(controlRotation.eulerAngles.x, 0f, 0f);

			//float vertical = Input.GetAxis("VerticalRight" + playerIndex);

			//float cameraX = playerCamera.transform.localEulerAngles.x + -vertical * Time.deltaTime * 200f;
			//if (cameraX > 180f)
			//{
			//    cameraX -= 360f;
			//}
			//cameraX = Mathf.Clamp(cameraX, -90f, 90f);
			//playerCamera.transform.localEulerAngles = new Vector3(cameraX, 0, 0);

            if (this.rotationSmoothing > 0.0f)
            {
                this.pivot.localRotation =
                    Quaternion.Slerp(this.pivot.localRotation, this.pivotTargetLocalRotation, this.rotationSmoothing * Time.deltaTime);
				
                this.rig.localRotation =
                    Quaternion.Slerp(this.rig.localRotation, this.rigTargetLocalRotation, this.rotationSmoothing * Time.deltaTime);
            }
            else
            {
                this.pivot.localRotation = this.pivotTargetLocalRotation;
                this.rig.localRotation = this.rigTargetLocalRotation;
            }
        }

    }



}
