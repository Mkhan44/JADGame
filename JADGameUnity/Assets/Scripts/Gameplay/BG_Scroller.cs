using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Scroller : MonoBehaviour
{
    public BoxCollider2D collider;

    public Rigidbody2D rb;

    private float width;
    public float scrollSpeed;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        width = collider.size.x;

        collider.enabled = false;

        rb.linearVelocity = new Vector2(scrollSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Wave_Spawner.Instance.getIntroFinishedStatus())
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = new Vector2(scrollSpeed, 0);
        }

        if (transform.position.x < -width)
        {
            Vector2 resetPosition = new Vector2(width * 2f, 0);
            transform.position = (Vector2)transform.position + resetPosition;
        }
    }
}
