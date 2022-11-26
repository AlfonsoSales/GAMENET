using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
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

    
   
    bool dead = false;
    public Text deathLog;

    public Text scoreBox;
    private int score = 0;

    public float TimerKick = 20;

    public Text champion;




    // Start is called before the first frame update
    void Start()
    {
        
        health = startHealth;
        healthBar.fillAmount = health / startHealth;
        animator = this.GetComponent<Animator>();
        deathLog = GameObject.Find("Death Log").GetComponent<Text>();
        scoreBox = GameObject.Find("score").GetComponent<Text>();
        champion = GameObject.Find("Champ").GetComponent<Text>();

        

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(score);
        if (score >= 10)
        {
            champion.GetComponent<Text>().text = "You have won!";
            photonView.RPC("Finish", RpcTarget.AllBuffered);

            if (TimerKick > 0)
            {
                TimerKick -= Time.deltaTime;
            }
            else if (TimerKick <= 0)
            {
                PhotonNetwork.LeaveRoom();
            }
        }



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
                if (hit.collider.gameObject.GetComponent<Shooting>().health == 0)
                {
                    GetKill();
                }
            }

            
            
            
        }
    }

    [PunRPC]
    public void TakeDamage(int damage, PhotonMessageInfo info)
    {
      
        this.health -= damage;
        this.healthBar.fillAmount = health / startHealth;
        

        if (health <= 0)
        {
            Die();
            shooter = info.Sender.NickName;
            
            deadP = info.photonView.Owner.NickName;

           
            
            dead = true;
            photonView.RPC("DisplayDead", RpcTarget.AllBuffered);
  
        }


        if (photonView.IsMine && dead == true)
        {
            dead = false;
            
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
        Debug.Log(shooter + " killed" + deadP);
        deathLog.GetComponent<Text>().text = shooter + " killed " + deadP;
    }

    [PunRPC]
    public void Finish()
    {
        transform.GetComponent<PlayerMovementController>().enabled = false;


    }

    public void GetKill()
    {
        score++;
        scoreBox.GetComponent<Text>().text = score.ToString();
    }

    

   


}
