using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Animator anim;
    public Vector2 direction;
    private Vector2 lastDirection;

    public bool disableMovement; //will stop inputs from moving player

    void InputVector()
    {
        //takes in inputs to assign to xmov and ymov, then creates the vector direction that holds which way the player is facing 
        float XMovement = Input.GetAxisRaw("Horizontal");
        float YMovement = Input.GetAxisRaw("Vertical");

        direction = new Vector2(XMovement, YMovement);

        //uses the lastDirection vector to store the direction player was facing right before letting go of the arrow keys (the x and y inputs)
        //also pauses the animation on the first frame with anim.speed = 0 and resumes it according to whether player is moving - used for 
        if (XMovement != 0 && YMovement != 0 || direction.x != 0 || direction.y != 0)
        {
            lastDirection = direction;
            anim.speed = 0.45f;
        }
        else
        {
            anim.speed = 0;
        }
    }

    //literally just moves the player. Uses the rb.velocity component built into unity rigidbody,
    //which moves the player with direction velocity (capped to 1 in each of the 8 directions with.normalised) and multiplied by speed 
    void move()
    {
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
    }


    // Start is called before the first frame update
    void Start()
    {
        //allows to use components in code
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        InputVector();

        //sets the x and y values found in direction to the animation blend tree
        anim.SetFloat("Vertical", direction.y);
        anim.SetFloat("Horizontal", direction.x);
        anim.SetFloat("Magnitude", direction.magnitude);
        anim.SetFloat("LastVertical", lastDirection.y);
        anim.SetFloat("LastHorizontal", lastDirection.x);
    }
    //called at a fixed framerate regardless of a computer's processing power
    private void FixedUpdate()
    {
        if (!disableMovement)
        {
            move();
        }
    }
}

