using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformElement : MonoBehaviour
{
    public GameObject player;
    public GameObject Pool;
    public GameController controller;
    public float destroyX;
    public int id;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.y < -10) transform.position = new Vector2(transform.position.x, -10);
        if (transform.position.y > 10) transform.position = new Vector2(transform.position.x, 10);
        if (controller.GameState == "Game") transform.position = Vector2.Lerp((Vector2)transform.position, (Vector2)transform.position - new Vector2(5, 0), Time.deltaTime);
        if (
            transform.position.x < player.transform.position.x &&
            Mathf.Abs((transform.position.x + transform.localScale.x/2) - player.transform.position.x) > 5
        )
        {
            transform.parent = Pool.transform;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().particle.Play();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().particle.Stop();
        }
    }
    void FixedUpdate()
    {
        //if (controller.GameState == "Game") transform.position = Vector2.Lerp((Vector2)transform.position, (Vector2)transform.position - new Vector2(5, 0), Time.deltaTime);
        //if (controller.GameState == "Game") rb.MovePosition((Vector2)transform.position - new Vector2(5, 0) * Time.fixedDeltaTime);
    }
}
