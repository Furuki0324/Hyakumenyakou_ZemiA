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
    private Vector3 scale;


    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        localRotation = transform.rotation;
        scale = transform.localScale;
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
            scale.x = -1;
        }
        if(velocity2D.x > 0)
        {
            localRotation.y = 0;
            scale.x = 1;
        }

        //transform.rotation = localRotation;
        transform.localScale = scale;
        rigid2D.velocity = velocity2D * walkSpeed;
    }


}
