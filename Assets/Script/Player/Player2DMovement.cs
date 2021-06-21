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


    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
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

        rigid2D.velocity = velocity2D * walkSpeed;
    }


}
