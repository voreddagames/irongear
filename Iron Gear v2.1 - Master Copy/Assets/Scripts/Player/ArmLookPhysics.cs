using UnityEngine;
using System.Collections;

public class ArmLookPhysics : MonoBehaviour
{
    public static ArmLookPhysics instance;

    public float aimSpeed = 10.0f;
    public float offset = 90.0f;

    private float rate;

    private AimMotor aimMotor;

    private GameObject arm;

    private Quaternion lastArmRot;
    public Quaternion initArmRot;

    public Transform neutralArmPos;

    private bool isResetting;

    #region trash code
    //public float counterEffectAngle;
    //public float turnSmoothing = 10.0f;
    //public float responsiveness = 10.0f;
    //public float clampAngle;
    //public float lookAngle;
    //public float maxAngle = 200;

    //public Vector3 angleDirection = Vector3.up;
    //public Vector3 angleLookDirection = Vector3.forward;
    //public Vector3 worldPos;
    //public float angle;
    //public Vector3 lookTarget;
        
    //private float cameraDiff;

    //private int invertFactor = 1;
    #endregion

    void Awake()
    {
        instance = this;

        aimMotor = GetComponent<AimMotor>();
    }

    void Start()
    {
        arm = GameObject.Find("LeftArm");

        initArmRot = arm.transform.rotation;
    }
    
    void LateUpdate()
    {
        Vector3 mousePos = Input.mousePosition;

        #region trash code
        //Vector3 target = mousePos;

        //clampAngle = Mathf.Clamp(clampAngle, -maxAngle, maxAngle);
        
        //lookAngle = Mathf.Lerp(lookAngle, clampAngle, responsiveness);

        //target = Quaternion.AngleAxis(lookAngle, angleDirection) * angleLookDirection;

        //Vector3 lookDir = target;

        //Quaternion lookRot = Quaternion.LookRotation(lookDir);
        ////Quaternion rot = Quaternion.Slerp(Quaternion.identity, lookRot, turnSmoothing);

        ////transform.rotation = rot;

        //worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        //Vector3 lookToDir = new Vector3(transform.position.x, worldPos.y, worldPos.z);

        //Vector3 objPos = Camera.main.WorldToScreenPoint(transform.position);
        
        ////on left side of character
        //if (objPos.x > mousePos.x)
        //{
        //    invertFactor = -1;
        //}
        //    //on right side of character
        //else
        //{
        //    invertFactor = 1;
        //}

        //angle = Mathf.Atan2(mousePos.y - objPos.y, mousePos.x - objPos.x) * Mathf.Rad2Deg;
        //angle = Mathf.Atan2(worldPos.y - objPos.y, worldPos.x - objPos.x) * Mathf.Rad2Deg;
        //facingDirection = angle * movementRight + angle * movementForward;

        //float rotationAngle = AngleAroundAxis(transform.right, facingDirection, Vector3.up);

        //Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(-angle - (counterEffectAngle * invertFactor), 0, 90 * invertFactor)), turnSmoothing);

        //RaycastHit hitInfo;
        //Vector3 lookDelta = (hitInfo.point - transform.position);
        
        //Quaternion targetRot = Quaternion.LookRotation(lookDelta);
        //Quaternion rot = Quaternion.RotateTowards(transform.rotation, targetRot, turnSmoothing);
        //float lookAngle = Mathf.LerpAngle(transform.eulerAngles.x, pos, turnSmoothing * Time.deltaTime);
        //transform.eulerAngles = new Vector3(lookAngle, 0, 90);
        //Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-angle, 0, 90), turnSmoothing);
        //transform.rotation = rot;

        //Vector3 pos = rot * new Vector3(0, 0, -distance) + player.transform.position;
        #endregion

        Plane playerPlane = new Plane(Vector3.forward, arm.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        
        float hitDist = 0.0f;

        if (aimMotor.HasAimControl())
        {
            if (!isResetting)
            {
                rate = 0.0f;
                isResetting = true;
            }

            if (playerPlane.Raycast(ray, out hitDist))
            {
                //find mouse position point along the plane
                Vector3 lookTarget = ray.GetPoint(hitDist);
                //have the arm look at the mouse position
                Quaternion targetRot = Quaternion.LookRotation(lookTarget - arm.transform.position, Vector3.right * aimMotor.faceDir);

                rate += aimSpeed * Time.deltaTime;
                rate = Mathf.Clamp01(rate);

                //rotate towards mouse
                arm.transform.rotation = Quaternion.Slerp(arm.transform.rotation, targetRot, rate);

                //log last rotation
                lastArmRot = targetRot;
            }
        }
        else
        {
            if (isResetting)
            {
                rate = 0.0f;
                isResetting = false;
            }

            rate += aimSpeed * Time.deltaTime;
            rate = Mathf.Clamp01(rate);

            //go back to original position
            arm.transform.rotation = Quaternion.Slerp(lastArmRot, initArmRot, rate);
        }
        arm.transform.rotation = Quaternion.Euler(arm.transform.eulerAngles.x, transform.eulerAngles.y, Quaternion.AngleAxis(offset, transform.forward).eulerAngles.z);
    }
    
    #region trash code
    //void LateUpdate()
    //{
        //Vector3 targetVel = moveDirection * walkSpeed;
        //Vector3 deltaVel = targetVel - rigidbody.velocity;

        //if (rigidbody.useGravity)
        //{
        //    deltaVel.y = 0;
        //}
        //rigidbody.AddForce(deltaVel * walkSnappiness, ForceMode.Acceleration);

        //if (faceDir == Vector3.zero)
        //{
        //    faceDir = moveDirection;
        //}

        //if (facingDirection == Vector3.zero)
        //{
        //    rigidbody.angularVelocity = Vector3.zero;
        //}
        //else
        //{
        //    float rotationAngle = AngleAroundAxis(transform.forward, facingDirection, Vector3.right);

        //    //transform.eulerAngles = (Vector3.right * rotationAngle * turnSmoothing);
        //    //rigidbody.angularVelocity = (Vector3.right * rotationAngle * turnSmoothing);
        //}
    //}

    //public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
    //{
    //    dirA = dirA - Vector3.Project(dirA, axis);
    //    dirB = dirB - Vector3.Project(dirB, axis);

    //    float angle = Vector3.Angle(dirA, dirB);

    //    return angle * (Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0 ? -1 : 1);
    //}

    //void FixedUpdate()
    //{
    //    Vector3 movementDir = Vector3.zero;
    //    Vector3 targetVel = 
    //}
    //public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    //public RotationAxes axes = RotationAxes.MouseXAndY;
    //public float mousesensitivityX = 15F;
    //public float mousesensitivityY = 15F;

    //public float joysensitivityX = 3F;
    //public float joysensitivityY = 3F;

    //public float minimumX = -360F;
    //public float maximumX = 360F;

    //public float minimumY = -60F;
    //public float maximumY = 60F;

    //float rotationY = 0F;

    //void Update()
    //{

    //    float Xon = Mathf.Abs(Input.GetAxis("Joystick Cam X"));
    //    float Yon = Mathf.Abs(Input.GetAxis("Joystick Cam Y"));

    //    if (axes == RotationAxes.MouseXAndY)
    //    {
    //        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mousesensitivityX;

    //        rotationY += Input.GetAxis("Mouse Y") * mousesensitivityY;
    //        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

    //        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
    //    }
    //    else if (axes == RotationAxes.MouseX)
    //    {
    //        if (Xon > .05)
    //        {
    //            transform.Rotate(0, Input.GetAxis("Joystick Cam X") * joysensitivityX, 0);
    //        }
    //        transform.Rotate(0, Input.GetAxis("Mouse X") * mousesensitivityX, 0);
    //    }
    //    else
    //    {
    //        if (Yon > .05)
    //        {
    //            rotationY += Input.GetAxis("Joystick Cam Y") * joysensitivityY;
    //        }
    //        rotationY += Input.GetAxis("Mouse Y") * mousesensitivityY;
    //        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

    //        transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);

    //    }
    //}

    //void Start()
    //{
    //    // Make the rigid body not change rotation
    //    if (rigidbody)
    //        rigidbody.freezeRotation = true;
    //}
    #endregion
}