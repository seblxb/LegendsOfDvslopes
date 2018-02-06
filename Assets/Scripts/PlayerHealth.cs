using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class PlayerHealth : MonoBehaviour {



    [SerializeField] private int startingHealth = 20;
    [SerializeField] private float timeSinceLastHit = 2f;
    [SerializeField] Slider healthSlider;

    private float timer = 0f;
    private CharacterController characterController;
    private Animator anim;
    private int currentHealth;
    private AudioSource audioo;
    private ParticleSystem blood;


    public int CurrentHealth {
        get { return currentHealth; }

        set {
            if (value < 0) {
                currentHealth = 0;
            } else {
                currentHealth = value;
            }
        }
    }

    private void Awake()
    {
        Assert.IsNotNull(healthSlider);
    }

    // Use this for initialization
    void Start () {
        blood = GetComponentInChildren<ParticleSystem>();
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        audioo = GetComponent<AudioSource>();
        currentHealth = startingHealth;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (timer >= timeSinceLastHit && !GameManager.instance.GameOver) {
            if ( other.tag == "Weapon") {
                takeHit();
                timer= 0;
                blood.Play();
            }

        }
    }

    void takeHit ()
    {
        if (currentHealth > 0)
        {
            GameManager.instance.PlayerHit(currentHealth);
            anim.Play("GetHurt");
            currentHealth -= 10;
            healthSlider.value = currentHealth;
            audioo.PlayOneShot(audioo.clip);

        }

        if (currentHealth <=0) {
            killPlayer();
        }
    }

    void killPlayer() {
        GameManager.instance.PlayerHit(currentHealth); 
        anim.SetTrigger("HeroDie");
        characterController.enabled = false; 
        audioo.PlayOneShot(audioo.clip);
    }

    public void PowerUpHealth () {
        if (currentHealth <= 70) {
            CurrentHealth += 30;
        } else if (currentHealth < startingHealth) {
            CurrentHealth = startingHealth;
        }

        healthSlider.value = currentHealth;
    }

}
