using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blades : MonoBehaviour
{
    //public float rotateSpeed;
    public FloatVariable FloatVariable;

    public float rotateSpeed;
    public float staminaHold;

    public Rigidbody2D rb;
    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    //public float launchSpeed;
    //public float launchSpeedLeft;
    //public float launchSpeedRight;
    // Start is called before the first frame update
    void Start()
    {
        //launchSpeedLeft = launchSpeed;
        //launchSpeedRight = launchSpeed * -1;

        //if (KatanaMuramasa.Rotation == 1)
        //{
        //    rotateSpeed = launchSpeedRight;
        //}

        rotateSpeed = FloatVariable.SpinSpeed;
        if (FloatVariable.Stamina == 2)
        {
            staminaHold = 0.01f;
        }
        else if (FloatVariable.Stamina == 1)
        {
            staminaHold = 0.001f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void FixedUpdate()
    {
        transform.Rotate(0f, 0f, rotateSpeed);

        if(rotateSpeed > 0 && rotateSpeed != 0)
        {
            rotateSpeed = rotateSpeed - staminaHold;
        }
        else if(rotateSpeed < 0 && rotateSpeed != 0)
        {
            rotateSpeed = rotateSpeed + staminaHold;
        }

        if (rotateSpeed < 1 && rotateSpeed > -1)
        {
            rotateSpeed = 0;
        }

        if (FloatVariable.Stamina == 1)
        {
            if (rb.position.x > 0 && rb.position.y < 0)
            {
                rb.MovePosition(rb.position + new Vector2(-1f, 1f) * Time.deltaTime);
            }
            else if (rb.position.x < 0 && rb.position.y > 0)
            {
                rb.MovePosition(rb.position + new Vector2(1f, -1f) * Time.deltaTime);
            }
            else if (rb.position.x < 0 && rb.position.y < 0)
            {
                rb.MovePosition(rb.position + new Vector2(1f, 1f) * Time.deltaTime);
            }
            else if (rb.position.x > 0 && rb.position.y > 0)
            {
                rb.MovePosition(rb.position + new Vector2(-1f, -1f) * Time.deltaTime);
            }
        }

        if (FloatVariable.Stamina == 2)
        {
            if (rb.position.x > 0 && rb.position.y < 0)
            {
                rb.velocity = new Vector2(-2f, 2f);
            }
            else if (rb.position.x < 0 && rb.position.y > 0)
            {
                rb.velocity = new Vector2(2f, -2f);
            }
            else if (rb.position.x < 0 && rb.position.y < 0)
            {
                rb.velocity = new Vector2(2f, 2f);
            }
            else if (rb.position.x > 0 && rb.position.y > 0)
            {
                rb.velocity = new Vector2(-2f, -2f);
            }
        }
    }

    //public void LauncherShot()
    //{
    //    launchSpeed = 100;
    //}
}
