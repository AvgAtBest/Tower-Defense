using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; // targets coordinates in scene
    public bool hideCursor = true; // is cursor hidden
    [Header("Orbit")]
    public Vector3 offset = new Vector3(0, 0, 0);
    public float xSpeed = 120f; //Speed moving on X axis
    public float ySpeed = 120f; //Speed moving on Y axis
    public float yMinLimit = -20f; //minimum limit for looking on y axis
    public float yMaxLimit = 80f; //maximum limit for looking on y axis
    public float distanceMin = 0.5f; //minimum distance to target
    public float distanceMax = 15f; //max distance to target
    [Header("Collision")]
    public bool camCollison = true; //if the camera can collide with walls/objects (which is true)
    public float camRadius = 0.3f; //the radius the camera can visually see
    public LayerMask ignoreLayers; //ignores layers (like minimaps)

    private Vector3 originalOffset; //origial offset at start of game
    private float distance; //current distance
    private float rayDistance = 1000f; //max distance ray can check of collisions
    private float x = 0f; //x degrees of rotation
    private float y = 0f; //y degress of rotation
    public bool detachFromParent = false;
	void Start ()
    {
        //Detach camera from parent
        if (detachFromParent)
        {
            transform.SetParent(null);
        }
        //target = GameObject.Find("Player").GetComponent<Transform>();
        //is cursor supposed to be hidden?
        if(hideCursor)
        {
            //Lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            //hide cursor from player
            Cursor.visible = false;

        }
        //calculate original offset from target position
        originalOffset = transform.position - target.position;
        //Set raydistance to current distance magnitude of the camera
        rayDistance = originalOffset.magnitude;
        //Camera rotation
        Vector3 angles = transform.eulerAngles;
        //Set x and Y degrees to current cam rotation
        x = angles.y;
        y = angles.x;

	}
    void FixedUpdate()
    {
        //if target has been set
        if (target)
        {
            //is camera collision enabled
            if (camCollison)
            {
                //creates a ray starting from targets position and point backward toward camera
                Ray camRay = new Ray(target.position, -transform.forward);
                RaycastHit hit;
                //shoot a sphere in definded ray direction
                if(Physics.SphereCast(camRay, camRadius, out hit, rayDistance, ~ignoreLayers, QueryTriggerInteraction.Ignore))
                {
                    //set current camera distance to hit objects distance
                    distance = hit.distance;
                    //exit function
                    return;
                }
            }
            //Set distance to original distance
            distance = originalOffset.magnitude;
        }
    }
    void Update ()
    {
		//if target  has been set
        if (target)
        {
            //rotate the camera baseed on X and Y inputs
            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
            
            //clamps y angle using 'ClampAngle' function
            y = ClampAngle(y, yMinLimit, yMaxLimit);
            //rotate the transform of camera by quaternion euler angles (y for x and x for y)
            transform.rotation = Quaternion.Euler(y, x, 0);
        }
	}
    void LateUpdate()
    {
        //if target has been set
        if (target)
        {
            //calculate local offset from offset
            Vector3 localOffset = transform.TransformDirection(offset);
            //moves camera to new position based off distance and offset
            transform.position = (target.position + localOffset) + -transform.forward * distance;

        }
    }
    public static float ClampAngle(float angle, float min, float max)
    {
        if(angle < -360f)
        {
            angle += 360f;
        }
        if(angle > 360f)
        {
            angle -= -360f;
        }
        return Mathf.Clamp(angle, min, max);
    }
}
