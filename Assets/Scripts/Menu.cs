﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    [SerializeField] GameObject hero;
    [SerializeField] GameObject soldier;
    [SerializeField] GameObject tanker;
    [SerializeField] GameObject ranger;


    private Animator heroAnim;
    private Animator tankerAnim;
    private Animator soldierAnim;
    private Animator rangerAnim;

    private void Awake() 
    {
        Assert.IsNotNull(hero);
        Assert.IsNotNull(soldier);
        Assert.IsNotNull(tanker);
        Assert.IsNotNull(ranger);


	}


    // Use this for initialization
    void Start () {
        heroAnim = hero.GetComponent<Animator>();
        tankerAnim = tanker.GetComponent<Animator>();
        soldierAnim = soldier.GetComponent<Animator>();
        rangerAnim = ranger.GetComponent<Animator>();

        StartCoroutine(showcase());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator showcase () {
        yield return new WaitForSeconds(1f);
        heroAnim.Play("SpinAttack");
        yield return new WaitForSeconds(1f);
        tankerAnim.Play("Attack");
		yield return new WaitForSeconds(1f);
        soldierAnim.Play("Attack");
		yield return new WaitForSeconds(1f);
        rangerAnim.Play("Attack");

        StartCoroutine(showcase());
    }


    public void Battle () {
        SceneManager.LoadScene("level");

    }


	public void Quit()
	{
        Application.Quit();

	}
}
