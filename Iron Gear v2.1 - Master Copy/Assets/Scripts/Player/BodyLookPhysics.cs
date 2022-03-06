using UnityEngine;
using System.Collections;

[System.Serializable]
public class BendingSegment
{
    public Transform firstTransform;
    public Transform lastTransform;
    public float thresholdAngleDifference = 0;
    public float bendingMultiplier = 0.6f;
    public float maxAngleDifference = 30;
    public float maxBendingAngle = 80;
    public float responsiveness = 5;
    internal float angleH;
    internal float angleV;
    internal Vector3 dirUp;
    internal Vector3 referenceLookDir;
    internal Vector3 referenceUpDir;
    internal int chainLength;
    internal Quaternion[] origRotations;
}

[System.Serializable]
public class NonAffectedJoints
{
    public Transform joint;
    public float effect = 0;
}

public class BodyLookPhysics : MonoBehaviour
{
    public static BodyLookPhysics instance;

    public Transform rootNode;
    public Transform neutralBodyPos;
    
    public BendingSegment[] segments;
    public NonAffectedJoints[] nonAffectedJoints;

    public Vector3 lookVector = Vector3.forward;
    public Vector3 upVector   = Vector3.up;
    public Vector3 target     = Vector3.zero;

    public float effect = 1;

    public bool overrideAnimation = false;

    public float maxBend         = 30.0f;
    public float aimSpeed        = 6.0f;
    public float offsetFromMouse = 10.0f;

    private GameObject baseJoint;
//    private GameObject neckJoint;
    
    private bool isResetting;

    public float angleLimit;
    public float rate;
//    private float cameraDiff;

    private AimMotor aimMotor;

    public Quaternion lastHeadPos;

    void Awake()
    {
        instance = this;

        aimMotor = GetComponent<AimMotor>();
    }

    void Start()
    {
        baseJoint = GameObject.Find("Spine");
//        neckJoint = GameObject.Find("Neck");

        //angleLimit = baseJoint.transform.eulerAngles;
        #region trash
        //cameraDiff = Camera.main.transform.position.x - transform.position.x;
        
        //if (rootNode == null)
        //{
        //    rootNode = transform;
        //}

        //// Setup segments
        //foreach (BendingSegment segment in segments)
        //{
        //    Quaternion parentRot = segment.firstTransform.parent.rotation;
        //    Quaternion parentRotInv = Quaternion.Inverse(parentRot);

        //    segment.referenceLookDir = parentRotInv * rootNode.rotation * lookVector.normalized;
        //    segment.referenceUpDir = parentRotInv * rootNode.rotation * upVector.normalized;

        //    segment.angleH = 0;
        //    segment.angleV = 0;

        //    segment.dirUp = segment.referenceUpDir;

        //    segment.chainLength = 1;

        //    Transform t = segment.lastTransform;

        //    while (t != segment.firstTransform && t != t.root)
        //    {
        //        segment.chainLength++;
        //        t = t.parent;
        //    }

        //    segment.origRotations = new Quaternion[segment.chainLength];

        //    t = segment.lastTransform;

        //    for (int i = segment.chainLength - 1; i >= 0; i--)
        //    {
        //        segment.origRotations[i] = t.localRotation;
        //        t = t.parent;
        //    }
        //}
        #endregion
    }

    void LateUpdate()
    {

        //if (Input.GetJoystickNames().Length > 0)
        //{
        //    Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(crosshair.position.x, crosshair.position.y, cameraDiff));
        //    Vector3 lookDir = new Vector3(transform.position.x, worldPos.y, worldPos.z);

        //    target = lookDir;
        //}
        //else
        //{
        #region blah
        //Vector3 mousePos = Input.mousePosition;

        //    Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cameraDiff));
        //    Vector3 lookToDir = new Vector3(transform.position.x, worldPos.y, worldPos.z);

        //    target = lookToDir;
        ////}

        //if (Time.deltaTime == 0)
        //    return;

        //// Remember initial directions of joints that should not be affected
        //Vector3[] jointDirections = new Vector3[nonAffectedJoints.Length];

        //for (int i = 0; i < nonAffectedJoints.Length; i++)
        //{
        //    foreach (Transform child in nonAffectedJoints[i].joint)
        //    {
        //        jointDirections[i] = child.position - nonAffectedJoints[i].joint.position;
        //        break;
        //    }
        //}

        //// Handle each segment
        //foreach (BendingSegment segment in segments)
        //{
        //    Transform t = segment.lastTransform;

        //    if (overrideAnimation)
        //    {
        //        for (int i = segment.chainLength - 1; i >= 0; i--)
        //        {
        //            t.localRotation = segment.origRotations[i];
        //            t = t.parent;
        //        }
        //    }

        //    Quaternion parentRot = segment.firstTransform.parent.rotation;
        //    Quaternion parentRotInv = Quaternion.Inverse(parentRot);

        //    // Desired look direction in world space
        //    Vector3 lookDirWorld = (target - segment.lastTransform.position).normalized;

        //    // Desired look directions in neck parent space
        //    Vector3 lookDirGoal = (parentRotInv * lookDirWorld);

        //    // Get the horizontal and vertical rotation angle to look at the target
        //    float hAngle = AngleAroundAxis(segment.referenceLookDir, lookDirGoal, segment.referenceUpDir);

        //    Vector3 rightOfTarget = Vector3.Cross(segment.referenceUpDir, lookDirGoal);

        //    Vector3 lookDirGoalinHPlane = lookDirGoal - Vector3.Project(lookDirGoal, segment.referenceUpDir);

        //    float vAngle = AngleAroundAxis(lookDirGoalinHPlane, lookDirGoal, rightOfTarget);

        //    // Handle threshold angle difference, bending multiplier,
        //    // and max angle difference here
        //    float hAngleThr = Mathf.Max(0, Mathf.Abs(hAngle) - segment.thresholdAngleDifference) * Mathf.Sign(hAngle);

        //    float vAngleThr = Mathf.Max(0, Mathf.Abs(vAngle) - segment.thresholdAngleDifference) * Mathf.Sign(vAngle);

        //    hAngle = Mathf.Max(Mathf.Abs(hAngleThr) * Mathf.Abs(segment.bendingMultiplier), Mathf.Abs(hAngle) - segment.maxAngleDifference) * Mathf.Sign(hAngle) * Mathf.Sign(segment.bendingMultiplier);

        //    vAngle = Mathf.Max(Mathf.Abs(vAngleThr) * Mathf.Abs(segment.bendingMultiplier), Mathf.Abs(vAngle) - segment.maxAngleDifference) * Mathf.Sign(vAngle) * Mathf.Sign(segment.bendingMultiplier);

        //    // Handle max bending angle here
        //    hAngle = Mathf.Clamp(hAngle, -segment.maxBendingAngle, segment.maxBendingAngle);
        //    vAngle = Mathf.Clamp(vAngle, -segment.maxBendingAngle, segment.maxBendingAngle);

        //    Vector3 referenceRightDir = Vector3.Cross(segment.referenceUpDir, segment.referenceLookDir);

        //    // Lerp angles
        //    segment.angleH = Mathf.Lerp(segment.angleH, hAngle, Time.deltaTime * segment.responsiveness);
        //    segment.angleV = Mathf.Lerp(segment.angleV, vAngle, Time.deltaTime * segment.responsiveness);

        //    // Get direction
        //    lookDirGoal = Quaternion.AngleAxis(segment.angleH, segment.referenceUpDir) * Quaternion.AngleAxis(segment.angleV, referenceRightDir) * segment.referenceLookDir;

        //    // Make look and up perpendicular
        //    Vector3 upDirGoal = segment.referenceUpDir;
        //    Vector3.OrthoNormalize(ref lookDirGoal, ref upDirGoal);

        //    // Interpolated look and up directions in neck parent space
        //    Vector3 lookDir = lookDirGoal;

        //    segment.dirUp = Vector3.Slerp(segment.dirUp, upDirGoal, Time.deltaTime * 5);

        //    Vector3.OrthoNormalize(ref lookDir, ref segment.dirUp);

        //    // Look rotation in world space
        //    Quaternion lookRot = ((parentRot * Quaternion.LookRotation(lookDir, segment.dirUp)) * Quaternion.Inverse(parentRot * Quaternion.LookRotation(segment.referenceLookDir, segment.referenceUpDir)));

        //    // Distribute rotation over all joints in segment
        //    Quaternion dividedRotation = Quaternion.Slerp(Quaternion.identity, lookRot, effect / segment.chainLength);

        //    t = segment.lastTransform;

        //    for (int i = 0; i < segment.chainLength; i++)
        //    {
        //        t.rotation = dividedRotation * t.rotation;
        //        t = t.parent;
        //    }
        //}

        //// Handle non affected joints
        //for (int i = 0; i < nonAffectedJoints.Length; i++)
        //{
        //    Vector3 newJointDirection = Vector3.zero;

        //    foreach (Transform child in nonAffectedJoints[i].joint)
        //    {
        //        newJointDirection = child.position - nonAffectedJoints[i].joint.position;
        //        break;
        //    }

        //    Vector3 combinedJointDirection = Vector3.Slerp(jointDirections[i], newJointDirection, nonAffectedJoints[i].effect);

        //    nonAffectedJoints[i].joint.rotation = Quaternion.FromToRotation(newJointDirection, combinedJointDirection) * nonAffectedJoints[i].joint.rotation;
        //}
        #endregion
    }

    public void LookAtControl()
    {
//        Vector3 mousePos = Input.mousePosition;

//        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cameraDiff));
//        Vector3 lookToDir = new Vector3(transform.position.x, worldPos.y, worldPos.z);

//        target = lookToDir;

        if (Time.deltaTime == 0)
            return;

        // Remember initial directions of joints that should not be affected
        Vector3[] jointDirections = new Vector3[nonAffectedJoints.Length];

        for (int i = 0; i < nonAffectedJoints.Length; i++)
        {
            foreach (Transform child in nonAffectedJoints[i].joint)
            {
                jointDirections[i] = child.position - nonAffectedJoints[i].joint.position;
                break;
            }
        }

        // Handle each segment
        foreach (BendingSegment segment in segments)
        {
            Transform t = segment.lastTransform;

            if (overrideAnimation)
            {
                for (int i = segment.chainLength - 1; i >= 0; i--)
                {
                    t.localRotation = segment.origRotations[i];
                    t = t.parent;
                }
            }

            Quaternion parentRot = segment.firstTransform.parent.rotation;
            Quaternion parentRotInv = Quaternion.Inverse(parentRot);

            // Desired look direction in world space
            Vector3 lookDirWorld = (target - segment.lastTransform.position).normalized;

            // Desired look directions in neck parent space
            Vector3 lookDirGoal = (parentRotInv * lookDirWorld);

            // Get the horizontal and vertical rotation angle to look at the target
            float hAngle = AngleAroundAxis(segment.referenceLookDir, lookDirGoal, segment.referenceUpDir);

            Vector3 rightOfTarget = Vector3.Cross(segment.referenceUpDir, lookDirGoal);

            Vector3 lookDirGoalinHPlane = lookDirGoal - Vector3.Project(lookDirGoal, segment.referenceUpDir);

            float vAngle = AngleAroundAxis(lookDirGoalinHPlane, lookDirGoal, rightOfTarget);

            // Handle threshold angle difference, bending multiplier,
            // and max angle difference here
            float hAngleThr = Mathf.Max(0, Mathf.Abs(hAngle) - segment.thresholdAngleDifference) * Mathf.Sign(hAngle);

            float vAngleThr = Mathf.Max(0, Mathf.Abs(vAngle) - segment.thresholdAngleDifference) * Mathf.Sign(vAngle);

            hAngle = Mathf.Max(Mathf.Abs(hAngleThr) * Mathf.Abs(segment.bendingMultiplier), Mathf.Abs(hAngle) - segment.maxAngleDifference) * Mathf.Sign(hAngle) * Mathf.Sign(segment.bendingMultiplier);

            vAngle = Mathf.Max(Mathf.Abs(vAngleThr) * Mathf.Abs(segment.bendingMultiplier), Mathf.Abs(vAngle) - segment.maxAngleDifference) * Mathf.Sign(vAngle) * Mathf.Sign(segment.bendingMultiplier);

            // Handle max bending angle here
            hAngle = Mathf.Clamp(hAngle, -segment.maxBendingAngle, segment.maxBendingAngle);
            vAngle = Mathf.Clamp(vAngle, -segment.maxBendingAngle, segment.maxBendingAngle);

            Vector3 referenceRightDir = Vector3.Cross(segment.referenceUpDir, segment.referenceLookDir);

            // Lerp angles
            segment.angleH = Mathf.Lerp(segment.angleH, hAngle, Time.deltaTime * segment.responsiveness);
            segment.angleV = Mathf.Lerp(segment.angleV, vAngle, Time.deltaTime * segment.responsiveness);

            // Get direction
            lookDirGoal = Quaternion.AngleAxis(segment.angleH, segment.referenceUpDir) * Quaternion.AngleAxis(segment.angleV, referenceRightDir) * segment.referenceLookDir;

            // Make look and up perpendicular
            Vector3 upDirGoal = segment.referenceUpDir;
            Vector3.OrthoNormalize(ref lookDirGoal, ref upDirGoal);

            // Interpolated look and up directions in neck parent space
            Vector3 lookDir = lookDirGoal;

            segment.dirUp = Vector3.Slerp(segment.dirUp, upDirGoal, Time.deltaTime * 5);

            Vector3.OrthoNormalize(ref lookDir, ref segment.dirUp);

            // Look rotation in world space
            Quaternion lookRot = ((parentRot * Quaternion.LookRotation(lookDir, segment.dirUp)) * Quaternion.Inverse(parentRot * Quaternion.LookRotation(segment.referenceLookDir, segment.referenceUpDir)));

            // Distribute rotation over all joints in segment
            Quaternion dividedRotation = Quaternion.Slerp(Quaternion.identity, lookRot, effect / segment.chainLength);
            
            
            t = segment.lastTransform;

            for (int i = 0; i < segment.chainLength; i++)
            {
                if (aimMotor.HasAimControl())
                {
                    if (!isResetting)
                    {
                        rate = 0.0f;
                        isResetting = true;
                    }
                    t.rotation = dividedRotation * t.rotation;
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
                    
                    Quaternion resetRotation = Quaternion.Slerp(t.rotation, neutralBodyPos.rotation, rate);
                    
                    t.rotation = resetRotation;
                    print("reset");
                }
                t = t.parent;
            }
        }

        // Handle non affected joints
        for (int i = 0; i < nonAffectedJoints.Length; i++)
        {
            Vector3 newJointDirection = Vector3.zero;

            foreach (Transform child in nonAffectedJoints[i].joint)
            {
                newJointDirection = child.position - nonAffectedJoints[i].joint.position;
                break;
            }

            Vector3 combinedJointDirection = Vector3.Slerp(jointDirections[i], newJointDirection, nonAffectedJoints[i].effect);

            nonAffectedJoints[i].joint.rotation = Quaternion.FromToRotation(newJointDirection, combinedJointDirection) * nonAffectedJoints[i].joint.rotation;
        }
    }

    public void NewLookAtControl()
    {
        Vector3 mousePos = Input.mousePosition;
                
        Plane playerPlane = new Plane(Vector3.right, baseJoint.transform.position);
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
                Vector3 lookTarget = ray.GetPoint(hitDist);
                Quaternion targetRot = Quaternion.LookRotation(lookTarget - baseJoint.transform.position, Vector3.left * aimMotor.faceDir) * Quaternion.AngleAxis(offsetFromMouse, Vector3.left);

                rate += aimSpeed * Time.deltaTime;
                rate = Mathf.Clamp01(rate);
                
                //angleLimit = Vector3.Slerp(baseJoint.transform.eulerAngles, (lookTarget - baseJoint.transform.position) + Vector3.forward * characterFollow.faceDir, rate);
                baseJoint.transform.rotation = Quaternion.Slerp(baseJoint.transform.rotation, targetRot, rate);
          
                //RotateArm(targetRot, rate);
                //baseJoint.transform.rotation = Quaternion.Slerp(baseJoint.transform.rotation, targetRot, rate);
                //arm.transform.rotation = Quaternion.Slerp(arm.transform.rotation, targetRot, aimSpeed);

                lastHeadPos = targetRot;
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

            baseJoint.transform.rotation = Quaternion.Slerp(lastHeadPos, neutralBodyPos.rotation, rate);
        }
        baseJoint.transform.rotation = Quaternion.Euler(baseJoint.transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }

    // The angle between dirA and dirB around axis
    public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
    {
        // Project A and B onto the plane orthogonal target axis
        dirA = dirA - Vector3.Project(dirA, axis);
        dirB = dirB - Vector3.Project(dirB, axis);

        // Find (positive) angle between A and B
        float angle = Vector3.Angle(dirA, dirB);

        // Return angle multiplied with 1 or -1
        return angle * (Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0 ? -1 : 1);
    }
}