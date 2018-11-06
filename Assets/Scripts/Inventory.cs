using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    private const int numSlots = 5;
    public Pickable[] Inv { get; set; }

	// Use this for initialization
	void Start () {
        Inv = new Pickable[numSlots];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
