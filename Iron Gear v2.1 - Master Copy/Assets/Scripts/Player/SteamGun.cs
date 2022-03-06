using UnityEngine;
using System.Collections;

public class SteamGun : MonoBehaviour
{
    public float distance = 5.0f;
    public float speed = 20.0f;
    public float counterAngle = 0.0f;

    private GameObject player;

//    private PlayerPhysics playerPhysics;

    private Vector3 input;

    private float angle;

    public Vector3 AimInput() { return input; }

    void Awake()
    {
        player = GameObject.FindWithTag("Player");

//        playerPhysics = player.GetComponent<PlayerPhysics>();
    }

	// Update is called once per frame
	void Update ()
    {
        input = new Vector3(Input.GetAxisRaw("Joystick Cam X"), -Input.GetAxisRaw("Joystick Cam Y"), 0);

        if (player.transform.eulerAngles.y != 0)
        {
            input.x *= -1;
        }
        else
        {
            input.x *= 1;
        }
        
        if (input == Vector3.zero)
        {
            //input.x = 1;
            counterAngle = player.transform.eulerAngles.y;
            //if (input.x > 0)
            //{
            //    playerPhysics.faceDir = 1;
            //}
            //if (input.x < 0)
            //{
            //    playerPhysics.faceDir = -1;
            //}
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(180, player.transform.eulerAngles.y, 0)), speed);
        }
        else
        {

            //transform.eulerAngles = new Vector3(Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg, transform.eulerAngles.y, transform.eulerAngles.z);

            //if (playerPhysics.MoveDirection().z != 0)
            //{
            //    counterAngleY = 180;
            //}
            //if (input.x < 0 && playerPhysics.MoveDirection().z < 0)
            //{
            //    playerPhysics.faceDir = 1;
            //}
            //if (input.x < 0 && playerPhysics.MoveDirection().z > 0)
            //{
            //    playerPhysics.faceDir = -1;
            //}
        }

        

        //Calc(input.y, input.x);

        angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;

        Quaternion rot = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(-angle, 0, counterAngle)), speed);
        Vector3 pos = rot * new Vector3(0, 0, -distance) + player.transform.position;

        transform.rotation = rot;
        transform.position = pos;

        //transform.eulerAngles = new Vector3(Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg, transform.eulerAngles.y, transform.eulerAngles.z);

        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(input), 20 * Time.deltaTime);
        //transform.RotateAround(player.transform.position, Vector3.right, 20 * Time.deltaTime);

        //Vector3 mousePos = Input.mousePosition;
                
        //objPos = Camera.main.WorldToScreenPoint(transform.position);
        //angle = Mathf.Atan2(mousePos.y - objPos.y, mousePos.x - objPos.x) * Mathf.Rad2Deg;

        //Quaternion rot = Quaternion.Euler(new Vector3(-angle, 0, 0));
        //Vector3 pos = rot * new Vector3(0, 0, -distance) + player.transform.position;
                
        //transform.rotation = rot;
        //transform.position = pos;
	}

    //void Calc(float inputY, float inputX)
    //{
    //    angle = Mathf.Atan2(inputY, inputX) * Mathf.Rad2Deg;

    //    Quaternion rot = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(-angle, player.transform.eulerAngles.y, 0)), speed);
    //    Vector3 pos = rot * new Vector3(0, 0, -distance) + player.transform.position;

    //    transform.rotation = rot;
    //    transform.position = pos;
    //}
}
