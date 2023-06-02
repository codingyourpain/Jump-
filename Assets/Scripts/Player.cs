using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    public GameController controller;

    [SerializeField] bool grounded;

    public float groundDistance = 0.1f;
    public LayerMask groundLayer;

    public ParticleSystem particle;

    Rigidbody2D rb;
    Animator anim;

    public Vector2 startPos;

    void Start()
    {
        startPos = transform.position;
        if (!controller) Debug.LogError("GameController is not defined");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (controller.GameState == "Game")
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Jump();
            }
            if (transform.position.x != Camera.main.transform.position.x)
            {
                if (Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > 10) controller.changeState("GameOver");
                else
                {
                    transform.position = Vector2.Lerp(
                        transform.position,
                        new Vector2(Camera.main.transform.position.x, transform.position.y),
                        Time.deltaTime / 2
                    );
                }
            }
            if (transform.position.y < -50) controller.changeState("GameOver");
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, -1, 0), Vector2.down, 0.1f, groundLayer);
            RaycastHit2D hit2 = Physics2D.CircleCast(transform.position, 0.51f, Vector2.right);

            // Check if the raycast hit the ground
            grounded = hit.collider != null;
            if (grounded)
            {
                if (!particle.isPlaying) particle.Play();
                anim.Play("Run");
            }
            else
            {
                if (hit2.collider != null && hit2.transform.name != transform.name)
                {
                    grounded = true;
                    //Debug.Log(hit2.transform.name);
                    //Jump();
                }
                if (!particle.isStopped) particle.Stop();
                anim.Play("Jump");
            }
            controller.score += Time.deltaTime * 2;
        }
    }
    private void LateUpdate()
    {
        Camera.main.transform.position = Vector3.Lerp(
            Camera.main.transform.position,
            new Vector3(
                Camera.main.transform.position.x,
                transform.position.y,
                Camera.main.transform.position.z
            ),
            Time.deltaTime
        );
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    grounded = true;
    //    anim.Play("Run");
    //}
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    grounded = false;
    //    anim.Play("Jump");
        
    //}

    public void Jump()
    {
        if (grounded)
        {
            rb.velocity = new Vector2(0, 10);
            grounded = false;
        }
    }
}
