using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Damaged : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Image healthbar;


    [SerializeField]
    Text dead;

    private float startHealth = 100;
    public float health;
    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
        healthbar.fillAmount = health / startHealth;
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(health);

        healthbar.fillAmount = health / startHealth;
        if (health < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        dead.text = "You have been eliminated";
    }
}
