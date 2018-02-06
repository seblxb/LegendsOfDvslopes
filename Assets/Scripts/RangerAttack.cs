using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAttack : MonoBehaviour
{


	[SerializeField] private float range = 3f;
	[SerializeField] private float timeBetweenAttacks = 1f;
    [SerializeField] private Transform fireLocation;

	private Animator anim;
	private GameObject player;
	private bool playerInRange;
	private BoxCollider[] weaponColliders;
	private EnemyHealth enemyHealth;
    private GameObject arrow;


	// Use this for initialization
	void Start()
	{
        arrow = GameManager.instance.Arrow;
		enemyHealth = GetComponent<EnemyHealth>();

		player = GameManager.instance.Player;
		anim = GetComponent<Animator>();
		StartCoroutine(attack());
	}

	// Update is called once per frame
	void Update()
	{
		if (Vector3.Distance(transform.position, player.transform.position) < range
			&& enemyHealth.IsAlive)
		{
			playerInRange = true;
            RotateTowards(player.transform);
            anim.SetBool("playerInRange", true);

		}
		else
		{
			playerInRange = false;
            anim.SetBool("playerInRange", false);
		}
		//print(playerInRange); 
	}

	IEnumerator attack()
	{
		//print("gameover" + GameManager.instance.GameOver); 
		if (playerInRange && GameManager.instance.GameOver == false)
		{
			//print("Attack!");
			anim.Play("Attack");
			yield return new WaitForSeconds(timeBetweenAttacks);
		}
		yield return null;

		StartCoroutine(attack());
	} 

    private void RotateTowards(Transform player) 
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotaion = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotaion, Time.deltaTime * 10f);

    }

    public void FireArrow()
    {
        GameObject newArrow = Instantiate(arrow) as GameObject;
        newArrow.transform.position = fireLocation.position;
        newArrow.transform.rotation = transform.rotation;
        newArrow.GetComponent<Rigidbody>().velocity = transform.forward * 25f;
        //print(newArrow.GetComponent<Rigidbody>().velocity);
    }


}
