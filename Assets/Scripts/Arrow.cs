using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {


  //  void OnCollisionEnter(Collision collision)
  //  {
		//print("hahaha");
		//Destroy(gameObject);
    //}

    void OnTriggerEnter(Collider col) {
		//print("hahaha");
		Destroy(gameObject);
	}
}
