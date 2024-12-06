using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Speeds { Slow = 0, Normal = 1, Fast = 2, Faster = 3, Fastest = 4 };
public enum Gamemodes { Cube = 0, Ship = 1, Ball = 2, Wave = 3 };

public class Movement : MonoBehaviour
{
    public Speeds CurrentSpeed;
    public Gamemodes CurrentGamemode;
    //                       0      1      2       3      4
    float[] SpeedValues = { 8.6f, 10.4f, 12.96f, 15.6f, 19.27f };
    [System.NonSerialized] public int[] screenHeightValues = { 11, 10, 8, 10 };
    [System.NonSerialized] public float yLastPortal = -2.3f;

    public float GroundCheckRadius;
    public LayerMask GroundMask;
    public Transform Sprite;
    Vector2 startPosition;

    Rigidbody2D rb;

    public int Gravity = 1;
    public bool clickProcessed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        transform.position += Vector3.right * SpeedValues[(int)CurrentSpeed] * Time.deltaTime;
        Invoke(CurrentGamemode.ToString(), 0);
    }


    public bool OnGround()
    {
        return Physics2D.OverlapBox(transform.position + Vector3.down * Gravity * 0.5f, Vector2.right * 1.1f + Vector2.up * GroundCheckRadius, 0, GroundMask);
    }

    bool TouchingWall()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + (Vector2.right * 0.55f), Vector2.up * 0.8f + (Vector2.right * GroundCheckRadius), 0, GroundMask);
    }

    public void Die()
    {
        transform.position = startPosition;
    }

    void Cube()
    {
        general.createGamemode(rb, this, true, 19.5269f, 9.057f, true, false, 409.1f);
    }


    void Ship()
    {
        transform.rotation = Quaternion.Euler(0, 0, rb.velocity.y * 2);

        if (Input.GetMouseButton(0))
            rb.gravityScale = -4.314969f;
        else
            rb.gravityScale = 4.314969f;

        rb.gravityScale = rb.gravityScale * Gravity;
    }


    void Ball()
    {
        general.createGamemode(rb, this, true, 0, 6.2f, false, true);
    }

    void Wave()
    {
        rb.gravityScale = 0;
        rb.velocity = new Vector2(0, SpeedValues[(int)CurrentSpeed] * (Input.GetMouseButton(0) ? 1 : -1) * Gravity);
    }

    public void ChangeThroughPortal(Gamemodes Gamemode, Speeds Speed, int gravity, int State, float yPortal)
    {
        switch (State)
        {
            case 0:
                CurrentSpeed = Speed;
                break;
            case 1:
                yLastPortal = yPortal;
                CurrentGamemode = Gamemode;
                break;
            case 2:
                Gravity = gravity;
                rb.gravityScale = Mathf.Abs(rb.gravityScale) * gravity;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Portal portal = collision.gameObject.GetComponent<Portal>();
        if (portal)
            portal.initiatePortal(this);
    }

}