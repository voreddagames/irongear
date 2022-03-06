using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Component script which will apply gravity to the entity
/// </summary>
public class ABC_Gravity : MonoBehaviour {

    // ************************ Settings ***********************

    /// <summary>
    /// How long the entity should be grounded for before this script is removed
    /// </summary>
    public float groundedRemoveTime = 2.5f; 

    // *************************** Variables ******************


    /// <summary>
    /// Transform of the entity this component is attached too
    /// </summary>
    private Transform meTransform;

    /// <summary>
    /// collider of the entity this component is attached too
    /// </summary>
    private Collider meCollider;

    /// <summary>
    /// Tracks the gravity velocity
    /// </summary>
    private Vector3 gravityVelocity;

    /// <summary>
    /// Will track the last time gravity was applied; 
    /// </summary>
    private float lastTimeGravityApplied = 0f; 


    // ********************* Private Methods ********************

    #region Private Methods

    /// <summary>
    /// Will apply gravity to the entity attached until they have been grounded for a short while
    /// </summary>
    private void ApplyGravity() {

        //Find all colliders in range but remove ourselves
        List<Collider> hitCol;
        hitCol = Physics.OverlapBox(meCollider.bounds.center, new Vector3(0f, meCollider.bounds.size.y - (meCollider.bounds.size.y/4), 0f)).ToList();
        hitCol.Remove(meCollider);

        //If we have not collided with anything then apply gravity 
        if (hitCol.Count() == 0) {

            this.gravityVelocity += Physics.gravity * Time.deltaTime;
            this.meTransform.position += this.gravityVelocity * Time.deltaTime;

            //track when we last applied gravity 
            this.lastTimeGravityApplied = Time.time; 

        }


        //If gravity has not been applied for last x seconds then remove script as we are not falling and have been grounded
        if (Time.time - this.lastTimeGravityApplied > this.groundedRemoveTime)
            OnDisable();


    }


    #endregion

    // ************************** Game **********************


    void OnEnable() {
        // record starting position transform and collider
        this.meTransform = transform;
        this.gravityVelocity = new Vector3();
        this.meCollider = this.meTransform.GetComponent<Collider>();

        //Start the tracking of when gravity was last applied (If not applied again in x seconds script will be removed
        this.lastTimeGravityApplied = Time.time;
    }



    void Update() {

        this.ApplyGravity();
    }

    private void OnDisable() {
        Destroy(this);
    }

}
