using UnityEngine;
using System.Collections;

public class Fly : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	this.gameObject.transform.Translate(new Vector3(0,(float)0.5,0),Space.World);
	}
}
