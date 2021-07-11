using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player2DMovement : MonoBehaviour
{
    //-----------------------Public----------------------
    public float walkSpeed = 1;

    //-----------------------Private------------------------
    private Rigidbody2D rigid2D;
    private Animator anim;
    private Quaternion localRotation;


    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        localRotation = transform.rotation;
    }


    void Update()
    {
        Walk();
    }

    private void Walk()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector2 velocity2D = new Vector2(inputX, inputY);

        if (velocity2D.magnitude != 0) anim.SetBool("isWalking", true);
        else anim.SetBool("isWalking", false);



        if(velocity2D.x < 0)
        {
            localRotation.y = 180;
        }
        if(velocity2D.x > 0)
        {
            localRotation.y = 0;
        }

        transform.rotation = localRotation;
        rigid2D.velocity = velocity2D * walkSpeed;
    }


}
