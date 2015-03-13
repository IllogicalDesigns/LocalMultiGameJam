using UnityEngine;
using System.Collections;

public class TestPlayer : MonoBehaviour {
	public float speed = 5.5f;						//our speed
	public int playerIntimidationValue = 10;		//10 is the defualt
	private float v;
	private float h;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		v = Input.GetAxis ("MoveVerticalMenu");
		h = Input.GetAxis ("MoveHorizontalMenu");
	}
	void FixedUpdate () {
		if(h != 0f)
		transform.Translate (transform.right * speed * h * Time.deltaTime);
		if(v != 0f)
		transform.Translate (transform.forward * speed * v * Time.deltaTime);
	
	}
}
