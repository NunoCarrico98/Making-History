using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    private const int numSlots = 5;
    public GameObject[] Inv { get; set; }

	// Use this for initialization
	void Start () {
        Inv = new GameObject[numSlots];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
