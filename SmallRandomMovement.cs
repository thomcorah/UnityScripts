using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallRandomMovement : MonoBehaviour
{

    /* Creates a small random back-and-forth movement of about 2deg in a 
     * direction perpendicular to the direction to the player. This is done in
     * order to increase the authenticity of binaurally rendered sounds, 
     * where a random movement of 2deg in the sound source has been shown to
     * reduce the rate of front/back errors.
     */

    /*
     * We've got two booleans to track state. moving tracks whether the 
     * object is currently moving. If it's not, we create the new target
     * location and speed. If it is moving, we move it towards the target
     * at the set speed.
     */
    private bool moving = false;
    private bool movingAway = true;
    
    /*
     * We'll want to store the start position so that the object can
     * keep returning to it. We also need a target location that's 
     * used throughout. 
     */
    private Vector3 startPosition;
    private Vector3 targetLocation;
    
    /* distanceStep stores the randomly generated distance the
     * object moves for each frame.
     */
    private float distanceStep;
    
    /*
     * In order to know where to move to, we need to know where the 
     * player avatar is, so we create a serialized field to link
     * the avatar object to.
     */
    [SerializeField]
    private GameObject avatar;
    
    
    // Start is called before the first frame update
    void Start()
    {
      // when the object loads, we save the location.
      startPosition = transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        // If the object is not currently moving, we need to start it moving. 
        if(!moving){
          /* By default, the target location will be the start position of the 
           * of the object. If the object is meant to be moving away from its 
           * start position, this will be overwritten a little later.
           */
          targetLocation = startPosition;
          
          /* We generate a random number for the distance the object moves
           * on each frame. This is set for the beginning of each movement. We
           * want this distance to be relative to the distance between the object
           * and the avatar, so that the speed at which it moves decreases as the 
           * player gets closer. 
           * To do this, we multiply it by the distance to the player avatar.
           */
          distanceStep = Random.Range(0.1f, 1.0f);
          float distance = Vector3.Distance(avatar.transform.position, transform.position); 
          distanceStep *= distance * 0.1f;
          
          /* If the object is meant to be moving away from its initial position, we need to 
           * calculate the location that it is moving to. This point should be perpendicular
           * to the direction of the avatar, and relative to the distance to it. 
           */
          if(movingAway){
            // We get a vector that represents the direction to the avatar.
            Vector3 heading = avatar.transform.position - transform.position;
            
            // We then work out a perpendicular 2D vector based on this.
            Vector2 perpendicularDirection = Vector2.Perpendicular(new Vector2(heading.x, heading.z));
            
            // We normalize this vector to get a maximum magnitude of 1. This now
            // represents the direction without anything in the way of distance.
            perpendicularDirection = perpendicularDirection.normalized;

            /* We want the distance to represent around 2deg of movement from the point of 
             * view of the avatar. To get that, we work out the circumference of a circle 
             * that has the avatar at its centre, and this object on its perimeter.
             * We then multiply this by 0.006 to get the distance of around 2deg of this
             * distance.
             */
            float circumference = distance * 2.0f * Mathf.PI;
            float movementDistance = circumference * 0.006f;

            // We then use this 2deg of distance to scale the direction vector we got earlier.
            Vector2 scaledDirection = perpendicularDirection * movementDistance;
            
            // From this 2D vector, we create a 3D vector representing the target location.
            targetLocation = new Vector3(startPosition.x + scaledDirection.x, startPosition.y, startPosition.z + scaledDirection.y);
          } // End of if(movingAway)
          
          // Once we've set up the target location, we set moving to true.
          moving = true;
          
          // We flip the state of movingAway.
          movingAway = !movingAway;
          
        } else { // if moving is currently true...
        
          // We scale the distance to move in this frame according to the time elapsed since the
          // last frame. This gives us a distance that takes into account changes in frame rate.
          float scaledDistanceStep = distanceStep * Time.deltaTime;
          
          // We then move the object towards the target location by the scaled amount. 
          transform.position = Vector3.MoveTowards(transform.position, targetLocation, scaledDistanceStep);
          
          /* We need to know when the object reaches the target location so that 
           * we can start moving in the other direction. To do this, we find out the
           * distance to the target location. If it's less than 0.1 we consider that
           * we've reached where we want to go and simply set moving to false.
           * This will trigger the generation of a new target location on the next frame and 
           * set the object off towards that. 
           */
          float distanceToGo = Vector3.Distance(transform.position, targetLocation);
          if(distanceToGo < 0.1f){
            moving = false;
          }
        }
    }
}
