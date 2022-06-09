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

    [SerializeField] private float moveWhenHeavyAttack = 5.0f;
    private bool isAttacking = false;

    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        if (!isAttacking) { Walk(); }
    }

    private void Walk()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector2 velocity2D = new Vector2(inputX, inputY);

        if (velocity2D.magnitude != 0) anim.SetBool("isWalking", true);
        else anim.SetBool("isWalking", false);

        if(velocity2D.x > 0)
        {
            anim.SetFloat("Walk_Direction", velocity2D.x);
            anim.SetBool("Direction_Left", false);
        }
        if(velocity2D.x < 0)
        {
            anim.SetFloat("Walk_Direction", velocity2D.x);
            anim.SetBool("Direction_Left", true);
        }

        rigid2D.velocity = velocity2D * walkSpeed;
    }

    public void StartAttack(float attackDuration, bool isHeavyAttack = false)
    {
        if(!isAttacking)
        {
            isAttacking = true;
            Vector2 velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            velocity *= (isHeavyAttack) ? moveWhenHeavyAttack : walkSpeed * 1.5f;
            StartCoroutine(AttackMove(velocity, attackDuration));
        }
    }

    private IEnumerator AttackMove(Vector2 startVelocity, float duration)
    {
        if(duration < 0.1f) { duration = 0.15f; }
        Vector2 velocity = new Vector2(0, 0);
        for(float f = 0.0f; f < duration; f += Time.deltaTime)
        {
            velocity.x = Mathf.Lerp(startVelocity.x, 0, f / duration);
            velocity.y = Mathf.Lerp(startVelocity.y, 0, f / duration);

            rigid2D.velocity = velocity;

            yield return null;
        }
        isAttacking = false;
    }
}
