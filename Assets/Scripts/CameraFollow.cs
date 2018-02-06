﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraFollow : MonoBehaviour {


    [SerializeField] private Transform target;
    [SerializeField] private float smoothing = 5f;
    private Vector3 offset;

    private void Awake()
    {
        Assert.IsNotNull(target);
    }

    // Use this for initialization
    void Start () {

        offset = transform.position - target.position;


	}
	
	// Update is called once per frame
	void Update () {

        Vector3 targetCameraPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCameraPos, smoothing * Time.deltaTime);


	}
}
