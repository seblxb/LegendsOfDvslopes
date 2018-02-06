using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour {

    [SerializeField] private int startingHealth = 20;
    [SerializeField] private float timeSinceLastHit = 0.5f;
    [SerializeField] private float dissapearSpeed = 2f;

    private AudioSource audioo;
    private int currentHealth;
    private float timer = 0f;
    private Animator anim;
    private NavMeshAgent nav;
    private bool isAlive;
    private Rigidbody rigidBody;
    private CapsuleCollider capsuleCollider;
    private bool dissapearEnemy = false;
    private ParticleSystem blood;

    public bool IsAlive {
        get {
            return isAlive;
        }
    }

	// Use this for initialization
	void Start () {


        GameManager.instance.RegisterEnemy(this);
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioo = GetComponent<AudioSource>();
        isAlive = true;
        currentHealth = startingHealth;
        blood = GetComponentInChildren<ParticleSystem>();

	}
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;

        if (dissapearEnemy == true)
        {
            transform.Translate(-Vector3.up * dissapearSpeed * Time.deltaTime );
        }

	}

    private void OnTriggerEnter(Collider other)
    {
        if (timer >= timeSinceLastHit && !GameManager.instance.GameOver) 
        {
            if (other.tag == "PlayerWeapon") 
            {
                takeHit();
                timer = 0f;
            }
        }
    }

    void takeHit () {
        if (currentHealth > 0) {
            audioo.PlayOneShot(audioo.clip);
            anim.Play("GetHurt");
            currentHealth -= 10;
            blood.Play();
        } else {
            isAlive = false;
            KillEnemy();
        }
    }

    void KillEnemy () {
        
        GameManager.instance.KillEnemy(this);
        capsuleCollider.enabled = false;
        nav.enabled = false;
        anim.SetTrigger("EnemyDie");
        rigidBody.isKinematic = true;
        StartCoroutine (removeEnemy());
        blood.Play();
    }

    IEnumerator removeEnemy () {
        yield return new WaitForSeconds(4f);
        dissapearEnemy = true;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject); 
    } 



}
