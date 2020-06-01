using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float rot_speed = 0.1f;

    Animator Anim;
    Rigidbody rb;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        PointCamera();
    }

    void Movement()
    {
        float vertical = Input.GetAxis("Vertical");
        bool isSprint = Input.GetButton("Sprint");
        float speed = vertical;
        if (isSprint)
        {
            speed += 1;
        }
        Anim.SetFloat("Pace", speed);
    }

    void PointCamera()
    {
        float InputX = Input.GetAxis("Horizontal");
        float InputZ = Input.GetAxis("Vertical");

        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * InputZ + right * InputX;
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rot_speed);
    }
}
