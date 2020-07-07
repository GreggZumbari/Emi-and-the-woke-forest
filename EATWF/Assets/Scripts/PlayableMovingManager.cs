﻿/**
 * Manages all moving capabilies for the current playable character
 * Meant to be run on the hitbox of the character
 * @author Greggory Hickman, October 2019
 * @version alpha 1.0
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableMovingManager : MonoBehaviour
{
    public GameObject camera;
    public GameObject rigidbody;

    public float speed;
    public float jumpMagnitude;
    public float slowFallMult;

    public int multiJumpLimit; //Number of jumps in between rests
    public int airJumps;

    public bool midAir;

    // Start is called before the first frame update
    void Start()
    {
        camera.GetComponent<Camera>().orthographic = true; //Set player view to be orthographic by default
        GetComponent<Rigidbody>().isKinematic = false; //Kinematic = bad

        midAir = true;

        speed = 7;
        jumpMagnitude = 14;
        slowFallMult = 2;
        multiJumpLimit = 1;
        airJumps = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Moving (left and right only)
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-GetComponent<Transform>().right * Time.deltaTime * speed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(GetComponent<Transform>().right * Time.deltaTime * speed);
        }

        //If not moving anymore, slide and stop
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0.96f * GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
        }

        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && (airJumps < multiJumpLimit || !midAir)) //If space is pressed, and the player still has some air jumps left or if the player is on the ground
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, jumpMagnitude, 0);
            airJumps++;
        }
        if (!midAir)
        {
            airJumps = 0;
        }

        //Slow fall if the player holds down space
        if (GetComponent<Rigidbody>().velocity.y <= 0 && Input.GetKey(KeyCode.Space))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x,
            GetComponent<Rigidbody>().velocity.y / 1.08f + (0.02f * slowFallMult),
            GetComponent<Rigidbody>().velocity.z);
        }
    }
}
