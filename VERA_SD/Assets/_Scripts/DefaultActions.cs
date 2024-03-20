using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class DefaultActions : MonoBehaviour
{

    #region VARIABLES


    [SerializeField] private GrabTracker grabHandler;
    private bool isGrabbing = false;
    private Renderer rend;
    private HingeJoint joint;//<----------------------------------------------------------------------
    private ConfigurableJoint gJoint;
    private bool reverse;//<----------------------------------------------------------------------
    private bool startP;
    private bool gReverse;
    private Vector3 currentThrowDirection;
    [SerializeField][Range(0, 90)] private float throwAngleChange = 10;
    [SerializeField][Range(0, 90)] private float throwForce = 10;
    private float radians;
    private float newVertChange;
    private float vertRadians;
    private float verticleAngle;
    private float newHorzChange;
    private float horzRadians;
    private float horizontalAngle;






    #endregion


    #region MONOBEHAVIOUR


    // Start
    //--------------------------------------//
    private void Awake()
    //--------------------------------------//
    {
        joint = GetComponent<HingeJoint>();//<----------------------------------------------------------------------
        reverse = false;//<----------------------------------------------------------------------
        startP = true;
        gReverse = false;
        radians = throwAngleChange * Mathf.PI / 180;
        grabHandler = FindObjectOfType<GrabTracker>();
        gJoint = GetComponent<ConfigurableJoint>();
        vertRadians = radians;
        verticleAngle = 0;
        newVertChange = throwAngleChange;
        horzRadians = radians;
        horizontalAngle = 0;
        newHorzChange = throwAngleChange;
        currentThrowDirection = Camera.main.transform.forward;
        if (grabHandler == null)
        {
            // Debug.LogError("GrabTracker is not assigned or not found.");
        }
        else
        {
            // Debug.Log("GrabTracker found.");
        }
        rend = GetComponent<Renderer>();
    } // END Start

    // Update
    //--------------------------------------//
    private void Update()
    //--------------------------------------//
    {

    }//End Update

    #endregion


    #region DEFAULT ASSIGNABLE FUNCTIONS
    //new = local rotation * global rotation
    //newnew = new*axis
    //newnew + motionaxis *

    // Grab/Release
    //--------------------------------------//
    public void GrabOrRelease()
    //--------------------------------------//
    {
        //Object Must be grabbable
        if (GetComponent<XRGrabInteractable>() != null)
        {
            if (isGrabbing == false)
            {
                //Checks if there is an Object grabbed or not
                //Can probably remove above if statement//Will remove later
                if (grabHandler.GetGrabbedObject() == null)
                {
                    //if not holding anything grab current object
                    GrabObject(gameObject);
                }
                else
                {
                    //if holding something drop grabbed object
                    ReleaseObject();
                }
            }
            else
            {
                ReleaseObject();
            }
        }
        else
        {
            Debug.LogError("Not A Grab Interactable");
        }
    }//END Grab/Release


    //Throw
    //--------------------------------------//
    public void Throw()
    //--------------------------------------//
    {
        //Get Grabbed object
        GameObject obj = grabHandler.GetGrabbedObject();

        //Checks if you are grabing because if you are not holding an object you cant throw 
        //maybe change to allow to grab if not holding object similar to grab/release
        if (obj != null)
        {
            isGrabbing = false;
            //Setting object parent back to original
            obj.transform.SetParent(grabHandler.GetGrabParent(), true);

            //if object had a different orientation set using Attach Transform in XRGrabInteractable then restore original parent for it
            XRGrabInteractable grabInteractable = obj.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null && grabInteractable.attachTransform != null)
            {
                grabInteractable.attachTransform.SetParent(obj.transform, true);
            }

            // Adjust the offset as needed//
            Vector3 offset = new Vector3(0.75f, 0.25f, 0.5f);
            Vector3 zero = Camera.main.ViewportToWorldPoint(Vector3.zero);
            Vector3 targetPosition = Camera.main.ViewportToWorldPoint(offset);
            //Checks if there is a collision between player and held object //to account for teleporting past a wall or something
            if (CheckCollisions(zero, targetPosition))
            {
                // If there is a collision, move to the point just before the collision
                Vector3 adjustedPosition = FindAdjustedPosition(zero, targetPosition, obj);
                obj.transform.position = adjustedPosition;
            }
            else
            {
                // If no collision, move to the target position
                obj.transform.position = targetPosition;
            }


            //Setting RigidBody back to normal
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //Gravity must be true else object wont fall to the ground
                rb.useGravity = true;
                rb.freezeRotation = grabHandler.GetfreezeRotation();
                //Kinematic must be false or else force cannot be acted on it
                rb.isKinematic = false;
                rb.interpolation = grabHandler.GetInterpolation();
                //Force acted in the direction the user is looking
                // Vector3 throwDirection = Camera.main.transform.forward;
                rb.AddForce(currentThrowDirection * throwForce, ForceMode.Impulse);
                // DisplayTrajectory.Instance.hideLine();
            }
            grabHandler.SetGrabbedObject(null);

        }
    }//End Throw

    public void aimThrow()
    {
        //show tragectory line with regular force
        currentThrowDirection = Camera.main.transform.forward;
        vertRadians = radians;
        verticleAngle = 0;
        newVertChange = throwAngleChange;
        horzRadians = radians;
        horizontalAngle = 0;
        newHorzChange = throwAngleChange;
        Vector3 forceDirection = currentThrowDirection * throwForce;
        Debug.Log(currentThrowDirection);
        Debug.Log(currentThrowDirection.normalized);
        GameObject obj = grabHandler.GetGrabbedObject();
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        DisplayTrajectory.Instance.calculateLine(forceDirection, rb, obj.transform.position);
    }

    public void aimUp()
    {
        //move tragectory line up n units
        if (verticleAngle < 90)
        {
            if (verticleAngle == -90)
            {
                if (currentThrowDirection == -Camera.main.transform.up)
                {
                    currentThrowDirection = Vector3.RotateTowards(currentThrowDirection, Camera.main.transform.up, -vertRadians, 0.0f);
                }
                else
                {
                    currentThrowDirection = Vector3.RotateTowards(currentThrowDirection, Camera.main.transform.up, vertRadians, 0.0f);
                }
                verticleAngle += newVertChange;
            }
            else
            {
                if (verticleAngle + throwAngleChange > 90)
                {
                    newVertChange = 90 - verticleAngle;
                    vertRadians = newVertChange * Mathf.PI / 180;
                    verticleAngle = 90;
                }
                else
                {
                    vertRadians = throwAngleChange * Mathf.PI / 180;
                    verticleAngle += throwAngleChange;
                    newVertChange = throwAngleChange;
                }
                Debug.Log(vertRadians);
                currentThrowDirection = Vector3.RotateTowards(currentThrowDirection, Camera.main.transform.up, vertRadians, 0.0f);
            }
        }
        currentThrowDirection.Normalize();
        Vector3 forceDirection = currentThrowDirection * throwForce;
        GameObject obj = grabHandler.GetGrabbedObject();
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        DisplayTrajectory.Instance.calculateLine(forceDirection, rb, obj.transform.position);
        //update Trajectory line
    }
    public void aimDown()
    {
        //move tragectory line down n units
        //IF CURRENTDIRECTION == UP DIRECTION ROTATE NEGATIVE FROM RADIANS
        if (verticleAngle > -90)
        {
            if (verticleAngle == 90)
            {
                if (currentThrowDirection == Camera.main.transform.up)
                {
                    currentThrowDirection = Vector3.RotateTowards(currentThrowDirection, -Camera.main.transform.up, -vertRadians, 0.0f);
                }
                else
                {
                    currentThrowDirection = Vector3.RotateTowards(currentThrowDirection, -Camera.main.transform.up, vertRadians, 0.0f);
                }
                verticleAngle -= newVertChange;
            }
            else
            {
                if (verticleAngle - throwAngleChange < -90)
                {
                    newVertChange = 90 + verticleAngle;
                    vertRadians = newVertChange * Mathf.PI / 180;
                    verticleAngle = -90;
                }
                else
                {
                    vertRadians = throwAngleChange * Mathf.PI / 180;
                    verticleAngle -= throwAngleChange;
                    newVertChange = throwAngleChange;
                }
                currentThrowDirection = Vector3.RotateTowards(currentThrowDirection, -Camera.main.transform.up, vertRadians, 0.0f);
            }

        }
        currentThrowDirection.Normalize();
        Vector3 forceDirection = currentThrowDirection * throwForce;
        GameObject obj = grabHandler.GetGrabbedObject();
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        DisplayTrajectory.Instance.calculateLine(forceDirection, rb, obj.transform.position);
        //update Trajectory line
    }
    public void aimRight()
    {
        //move tragectory line right n units
        if (horizontalAngle < 90)
        {
            if (horizontalAngle == -90)
            {
                if (currentThrowDirection == -Camera.main.transform.right)
                {
                    currentThrowDirection = Vector3.RotateTowards(currentThrowDirection, Camera.main.transform.right, -horzRadians, 0.0f);
                }
                else
                {
                    currentThrowDirection = Vector3.RotateTowards(currentThrowDirection, Camera.main.transform.right, horzRadians, 0.0f);
                }
                horizontalAngle += newHorzChange;
            }
            else
            {
                if (horizontalAngle + throwAngleChange > 90)
                {
                    newHorzChange = 90 - horizontalAngle;
                    horzRadians = newHorzChange * Mathf.PI / 180;
                    horizontalAngle = 90;
                }
                else
                {
                    horzRadians = throwAngleChange * Mathf.PI / 180;
                    horizontalAngle += throwAngleChange;
                    newHorzChange = throwAngleChange;
                }
                currentThrowDirection = Vector3.RotateTowards(currentThrowDirection, Camera.main.transform.right, horzRadians, 0.0f);
            }
        }
        currentThrowDirection.Normalize();
        Vector3 forceDirection = currentThrowDirection * throwForce;
        GameObject obj = grabHandler.GetGrabbedObject();
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        DisplayTrajectory.Instance.calculateLine(forceDirection, rb, obj.transform.position);
        //update Trajectory line
    }
    public void aimLeft()
    {
        //move tragectory line left n units
        if (horizontalAngle > -90)
        {
            if (horizontalAngle == 90)
            {
                if (currentThrowDirection == Camera.main.transform.right)
                {
                    currentThrowDirection = Vector3.RotateTowards(currentThrowDirection, -Camera.main.transform.right, -horzRadians, 0.0f);
                }
                else
                {
                    currentThrowDirection = Vector3.RotateTowards(currentThrowDirection, -Camera.main.transform.right, horzRadians, 0.0f);
                }
                horizontalAngle -= newHorzChange;
            }
            else
            {
                if (horizontalAngle - throwAngleChange < -90)
                {
                    newHorzChange = 90 + horizontalAngle;
                    horzRadians = newHorzChange * Mathf.PI / 180;
                    horizontalAngle = -90;
                }
                else
                {
                    horzRadians = throwAngleChange * Mathf.PI / 180;
                    horizontalAngle -= throwAngleChange;
                    newHorzChange = throwAngleChange;
                }
                currentThrowDirection = Vector3.RotateTowards(currentThrowDirection, -Camera.main.transform.right, horzRadians, 0.0f);
            }

        }
        currentThrowDirection.Normalize();
        Vector3 forceDirection = currentThrowDirection * throwForce;
        GameObject obj = grabHandler.GetGrabbedObject();
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        DisplayTrajectory.Instance.calculateLine(forceDirection, rb, obj.transform.position);
        //update Trajectory line
    }


    //back and throw will unshow tragectory line

    //OpenAndClose
    //--------------------------------------//
    public void OpenAndClose()//<----------------------------------------------------------------------
    //--------------------------------------//
    {
        // Debug.Log("joint.angle:" + joint.angle);
        //collision makes joint angle be offsetted from 0 so if its close to 0 then its closed so open
        if (Mathf.Floor(Mathf.Abs(joint.angle)) == 0)
        {
            //if has two way opening checks greater than 45 to make sure player can move through door
            if (joint.limits.max > 45 && joint.limits.min < -45)
            {
                // Debug.Log("entered Open");
                //depending on reverse state flop between directions
                if (reverse == false)
                {
                    this.transform.RotateAround(transform.TransformPoint(joint.anchor), joint.axis, joint.limits.max);
                }
                else
                {
                    this.transform.RotateAround(transform.TransformPoint(joint.anchor), joint.axis, joint.limits.min);
                }
            }
            //if has positive opening
            else if (joint.limits.max > 45)
            {
                this.transform.RotateAround(transform.TransformPoint(joint.anchor), joint.axis, joint.limits.max);
            }
            //if has negative opening
            else if (joint.limits.min < -45)
            {
                this.transform.RotateAround(transform.TransformPoint(joint.anchor), joint.axis, joint.limits.min);
            }
        }
        //is open so close
        else
        {
            // Debug.Log("entered Close");
            //reverse reverse
            reverse = !reverse;
            this.transform.RotateAround(transform.TransformPoint(joint.anchor), joint.axis, -joint.angle);
        }
        //has problems when collisions if kinetatic is disables
        //for better accessability, if it has the option to move outwards from player do that
        //but will require to know which direction the player is opening door from and has to decide based on that to open with min or max
        //might also want to have a smooth animation for opening
    }//End OpenAndClose

    public void Drawer()
    {
        Vector3 movementDirection = Vector3.zero;

        if (gJoint.xMotion != ConfigurableJointMotion.Locked)
        {
            movementDirection += this.transform.TransformDirection(gJoint.axis);
        }
        if (gJoint.yMotion != ConfigurableJointMotion.Locked)
        {
            movementDirection += this.transform.TransformDirection(gJoint.secondaryAxis);
        }
        if (gJoint.zMotion != ConfigurableJointMotion.Locked)
        {
            Vector3 crossAxis = Vector3.Cross(gJoint.axis, gJoint.secondaryAxis);
            movementDirection += this.transform.TransformDirection(crossAxis);
        }

        // Debug.Log(CheckCollisionAndMove(movementDirection, gJoint.linearLimit.limit));
        // Debug.Log(CheckCollisionAndMove(-movementDirection, gJoint.linearLimit.limit));

        // if (startP == true)
        // {
        //     if (CheckCollisionAndMove(movementDirection, gJoint.linearLimit.limit) == true && CheckCollisionAndMove(-movementDirection, gJoint.linearLimit.limit) == true)
        //     {
        //         if (gReverse == false)
        //         {
        //             this.transform.position += movementDirection * gJoint.linearLimit.limit;
        //         }
        //         else
        //         {
        //             this.transform.position -= movementDirection * gJoint.linearLimit.limit;
        //         }
        //         startP = false;
        //     }
        //     else
        //     {
        //         if (CheckCollisionAndMove(movementDirection, gJoint.linearLimit.limit) == true)
        //         {
        //             gReverse = false;
        //             this.transform.position += movementDirection * gJoint.linearLimit.limit;
        //             startP = false;
        //         }
        //         else
        //         {
        //             gReverse = true;
        //             this.transform.position -= movementDirection * gJoint.linearLimit.limit;
        //             startP = false;
        //         }
        //     }
        // }
        // else
        // {
        //     if (gReverse == false)
        //     {
        //         this.transform.position -= movementDirection * gJoint.linearLimit.limit;
        //     }
        //     else
        //     {
        //         this.transform.position += movementDirection * gJoint.linearLimit.limit;
        //     }
        //     startP = true;
        //     gReverse = !gReverse;
        // }

        // Debug.Log("positive: " + testFunction(movementDirection, transform.position, gJoint.linearLimit.limit));
        // Debug.Log("negative: " + testFunction(-movementDirection, transform.position, gJoint.linearLimit.limit));
        if (startP == true)
        {
            if (testFunction(movementDirection, gJoint.connectedAnchor, gJoint.linearLimit.limit) == true && testFunction(-movementDirection, gJoint.connectedAnchor, gJoint.linearLimit.limit) == true)
            {
                if (gReverse == false)
                {
                    this.transform.position += movementDirection * gJoint.linearLimit.limit;
                }
                else
                {
                    this.transform.position -= movementDirection * gJoint.linearLimit.limit;
                }
                startP = false;
            }
            else
            {
                if (testFunction(movementDirection, gJoint.connectedAnchor, gJoint.linearLimit.limit) == true)
                {
                    gReverse = false;
                    this.transform.position += movementDirection * gJoint.linearLimit.limit;
                    startP = false;
                }
                else if (testFunction(-movementDirection, gJoint.connectedAnchor, gJoint.linearLimit.limit) == true)
                {
                    gReverse = true;
                    this.transform.position -= movementDirection * gJoint.linearLimit.limit;
                    startP = false;
                }
                else
                {
                    Debug.Log("OOPS");
                }
            }
        }
        else
        {
            if (gReverse == false)
            {
                this.transform.position -= movementDirection * gJoint.linearLimit.limit;
            }
            else
            {
                this.transform.position += movementDirection * gJoint.linearLimit.limit;
            }
            startP = true;
            gReverse = !gReverse;
        }

    }
    #endregion

    //if positive direction and negative return true then maybe just do code above
    //else if positive direction have a code where it swaps from positive to starting
    //else if negative have a code where it swaps from negative to starting
    #region HELPER FUNCTIONS

    bool testFunction(Vector3 direction, Vector3 pos, float limit)
    {
        Physics.queriesHitBackfaces = true;
        Collider[] relaventColliders = this.GetComponentsInChildren<Collider>();
        Ray ray = new Ray(pos, direction);
        RaycastHit hit;
        if (Physics.Raycast(pos, direction, out hit, Mathf.Infinity))
        {
            // Physics.queriesHitBackfaces = false;
            Debug.Log("hitcollider: " + hit.collider);
            Debug.Log("hitpoint: " + hit.point);
            Debug.Log("hitdistnce: " + hit.distance);
            foreach (Collider c in relaventColliders)
            {
                Debug.Log("childCollider: " + c);
                if (hit.collider == c)
                {
                    return testFunction(direction, hit.point, limit);
                }
            }
            RaycastHit reverseHit;
            if (Physics.Raycast(hit.point, -direction, out reverseHit, Mathf.Infinity))
            {
                if (reverseHit.distance >= limit)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }
    //might have to find last collider then check from that position to the limit
    bool CheckCollisionAndMove(Vector3 direction, float limit)
    {
        // Ray ray = new Ray(transform.position, direction);
        // if (Physics.Raycast(ray, limit))
        // {
        //     // Collision detected, do not move
        //     Debug.Log("Collision detected, cannot move.");
        // }
        Collider[] relaventColliders = this.GetComponentsInChildren<Collider>();
        //Returns an unordered list of objects hit by the raycast from the player to the target position
        // Debug.Log(direction);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, limit);
        //Sort the Raycast List by distance from player
        System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
        // Debug.Log(hits.Length);
        // Debug.Log(relaventColliders.Length);
        // Debug.Log(relaventColliders[i]);
        if (hits.Length > 0)
        {
            Debug.Log(hits[hits.Length - 1].collider);

            for (int i = 0; i < relaventColliders.Length; i++)
            {
                // Debug.Log(relaventColliders[i]);
                // Debug.Log(hits[hits.Length - 1].collider);
                if (hits[hits.Length - 1].collider == relaventColliders[i])
                {
                    return true;
                }
            }
            Debug.Log("Collision detected");
            return false;
        }
        return true;

    }
    // GrabObject
    //--------------------------------------//
    void GrabObject(GameObject obj)
    //--------------------------------------//
    {
        isGrabbing = true;

        //Saving Values of Grabbed Object
        grabHandler.SetGrabbedObject(obj);
        grabHandler.SetGrabParent(transform.parent);
        grabHandler.SetRB(obj.GetComponent<Rigidbody>());

        //Change RigidBody Values
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.freezeRotation = true;
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation.None;
        }

        // Adjust the offset as needed
        Vector3 offset = new Vector3(0.75f, 0.25f, 0.5f);
        Vector3 targetPosition = Camera.main.ViewportToWorldPoint(offset);
        //if Attach Transform in XRGrabInteractable is not null adjust orientation accordingly
        XRGrabInteractable grabInteractable = obj.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null && grabInteractable.attachTransform != null)
        {
            //Adjusting vectors to the correct position relative to the camera
            Quaternion grabRotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);

            //Set AttachTransform object's parent to the camera
            grabInteractable.attachTransform.SetParent(Camera.main.transform, true);
            //Set Object's parent to the Attach Transform object
            transform.SetParent(grabInteractable.attachTransform, true);

            //Setting position to bottom right of pov and setting rotation to the correct orientation
            grabInteractable.attachTransform.position = targetPosition;
            grabInteractable.attachTransform.rotation = grabRotation;
        }
        //using default orientation
        else
        {
            //Adjusting vectors to the correct position relative to the camera
            Quaternion grabRotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
            //Set Object's parent to camera
            transform.SetParent(Camera.main.transform, true);
            //Setting position to bottom right of pov and setting rotation to the correct orientation
            obj.transform.position = targetPosition;
            obj.transform.rotation = grabRotation;

        }

    }//End GrabObject


    //ReleaseObject
    //--------------------------------------//
    void ReleaseObject()
    //--------------------------------------//
    {
        isGrabbing = false;
        //Get Grabbed Object
        GameObject obj = grabHandler.GetGrabbedObject();
        //Set objects parent back to original
        obj.transform.SetParent(grabHandler.GetGrabParent(), true);
        //If Attach Transform had an object connected revert to original parent/grabbedobject
        XRGrabInteractable grabInteractable = obj.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null && grabInteractable.attachTransform != null)
        {
            grabInteractable.attachTransform.SetParent(obj.transform, true);
        }

        // Placing the grabbed object in front of the player

        // Adjust the offset as needed// adjust for distance before drop
        Vector3 zero = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 offset = new Vector3(0.5f, 0.5f, 2.0f);
        Vector3 targetPosition = Camera.main.ViewportToWorldPoint(offset);
        // Check for collisions along the path
        if (CheckCollisions(zero, targetPosition))
        {
            // If there is a collision, move to the point just before the collision
            Vector3 adjustedPosition = FindAdjustedPosition(zero, targetPosition, obj);
            obj.transform.position = adjustedPosition;
        }
        else
        {
            // If no collision, move to the target position
            obj.transform.position = targetPosition;
        }
        //Setting RigidBody values back to original
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = grabHandler.GetGravity();
            rb.freezeRotation = grabHandler.GetfreezeRotation();
            rb.isKinematic = grabHandler.GetKinematic();
            rb.interpolation = grabHandler.GetInterpolation();
        }
        //Set Grabbed Object to none
        grabHandler.SetGrabbedObject(null);
    }//End ReleaseObject


    // CheckCollisions
    //--------------------------------------//
    bool CheckCollisions(Vector3 start, Vector3 end)
    //--------------------------------------//
    {
        // Check for collisions using Physics.Linecast
        // This function returns true if there is a collision along the line
        return Physics.Linecast(start, end);
    }//End CheckCollisions


    // FindAdjustedPosition
    //--------------------------------------//
    Vector3 FindAdjustedPosition(Vector3 start, Vector3 end, GameObject obj)
    //--------------------------------------//
    {
        //initiating value to the target position
        Vector3 adjustedPosition = end;

        // Perform a Raycast to find the first collision along the path

        //Returns a list of all objects with colliders that are children to the main object
        Collider[] relaventColliders = obj.GetComponentsInChildren<Collider>();

        //Returns an unordered list of objects hit by the raycast from the player to the target position
        RaycastHit[] hits = Physics.RaycastAll(start, (end - start), Vector3.Distance(start, end));
        //Sort the Raycast List by distance from player
        System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

        //for each object hit by the raycast check if it is one of the children colliders of the main object
        foreach (RaycastHit hitC in hits)
        {
            //initializing value to false each iteration
            bool isIn = false;
            foreach (Collider c in relaventColliders)
            {
                //if the raycast object collider is one of the children colliders break loop no need to check anymore
                if (hitC.collider == c)
                {
                    isIn = true;
                    break;
                }
            }
            //if raycast object is not in collider array of the main object children then set position to that objects position
            if (isIn == false)
            {
                adjustedPosition = hitC.point - (end - start).normalized * 0.1f;
            }
            //At any point of the loop if adjustedPosition is not at the targetposition that means we have first contact
            if (adjustedPosition != end)
            {
                break;
            }
        }
        return adjustedPosition;
    }//End FindAdjustedPosition

    #endregion


} // END DefaultActions.cs
