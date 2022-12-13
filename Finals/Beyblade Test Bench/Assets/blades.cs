using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class blades : MonoBehaviourPunCallbacks
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
            staminaHold = 0.1f;
        }
        else if (FloatVariable.Stamina == 1)
        {
            staminaHold = 0.01f;
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

    [PunRPC]
    void Damage(Collision collision)
    {
        if (collision.gameObject.tag == "Attack Ring")
        {
            if (rotateSpeed < 1)
            {
                rotateSpeed = rotateSpeed + 20;
            }
            else if (rotateSpeed > 1)
            {
                rotateSpeed = rotateSpeed - 20;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(gameObject.name == "ArenaBounds")
        {
            Debug.Log("Ring Out!");
        }
    }

    
}
