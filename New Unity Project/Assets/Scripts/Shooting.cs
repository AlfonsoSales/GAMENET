using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Shooting : MonoBehaviourPunCallbacks
{
    public Camera camera;
    public GameObject hitEffectPrefab;

    

    [Header("HP Related Stuff")]
    public float startHealth = 100;
    private float health;
    public Image healthBar;
    private string deadP;
    private string shooter;

    private Animator animator;

    GameObject scoreBox;
    private int score = 0;
   
    bool dead = false;
    public GameObject deathLog = GameObject.Find("Death Log");

    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
        healthBar.fillAmount = health / startHealth;
        animator = this.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        DisplayDead();
    }

    public void Fire()
    {
        RaycastHit hit;
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out hit, 200))
        {
            Debug.Log(hit.collider.gameObject.name);

            photonView.RPC("CreateHitEffects", RpcTarget.All, hit.point);

            if (hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
            {
                hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 25);
            }
            
        }
    }

    [PunRPC]
    public void TakeDamage(int damage, PhotonMessageInfo info)
    {
        GameObject scoreBox = GameObject.Find("score");
        this.health -= damage;
        this.healthBar.fillAmount = health / startHealth;
        

        if (health <= 0)
        {
            Die();
            shooter = info.Sender.NickName;
            deadP = info.photonView.Owner.NickName;
            
            Debug.Log(info.Sender.NickName + " killed" + info.photonView.Owner.NickName);
            dead = true;
            DisplayDead();
        }

        if (photonView.IsMine && dead == true)
        {
            dead = false;
            score = score + 1;
            scoreBox.GetComponent<Text>().text = score.ToString(); //changed it to have player who died 10 times get kicked from room
            if (score >= 10)
            {
                PhotonNetwork.LeaveRoom();
            }
        }

    }

    [PunRPC]
    public void CreateHitEffects(Vector3 position)
    {
        GameObject hitEffectGameObject = Instantiate(hitEffectPrefab, position, Quaternion.identity);
        Destroy(hitEffectGameObject, 0.2f);
    }

    public void Die()
    {
        if (photonView.IsMine)
        {
            animator.SetBool("isDead", true);
            StartCoroutine(RespawnCountdown());
        }
    }

    IEnumerator RespawnCountdown()
    {
        GameObject respawnText = GameObject.Find("Respawn Text");
        float respawnTime = 5.0f;
        
        while (respawnTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            respawnTime--;

            transform.GetComponent<PlayerMovementController>().enabled = false;
            respawnText.GetComponent<Text>().text = "You are killed. Respawning in " + respawnTime.ToString(".00");
        }

        animator.SetBool("isDead", false);
        respawnText.GetComponent<Text>().text = "";

        int randomPointX = Random.Range(-20, 20);
        int randomPointZ = Random.Range(-20, 20);

        this.transform.position = new Vector3(randomPointX, 0, randomPointZ);
        transform.GetComponent<PlayerMovementController>().enabled = true;

        photonView.RPC("RegainHealth", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void RegainHealth()
    {
        health = 100;
        healthBar.fillAmount = health / startHealth;
    }


    [PunRPC]
    public void DisplayDead()
    {
        deathLog.GetComponent<Text>().text = shooter + " killed" + deadP;
    }
}
