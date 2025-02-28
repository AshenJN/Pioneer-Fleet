using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Bullet : MonoBehaviour {

	public float velocity = 20f;
	public int damage = 40;
	public int shieldDamage=40;
//	public Rigidbody2D rb;
	public GameObject impactEffect;
	public float angle;
	public int hp=1;
	// Use this for initialization
	void Start () {
	
	}
	void Update()
	{
		Vector3 newPos = transform.position;
		newPos.x += velocity * Time.deltaTime;
		//(float)transform.rotation.y*(float).5
		newPos.y += velocity * Time.deltaTime*angle ;
		transform.position = newPos;
		

	}
	void OnTriggerEnter2D (Collider2D hitInfo)
	{
		 if (hitInfo.CompareTag("Attack")) // check if the collider's tag is "Attack"
    {
        return; // if it is, exit the method without doing anything
    }
		Enemy enemy = hitInfo.GetComponent<Enemy>();
		if (enemy != null)
		{
			enemy.TakeDamage(shieldDamage,damage);
		}

		Instantiate(impactEffect, transform.position, transform.rotation);
		hp--;
		//Debug.Log("TEST3");
		if(hp<=0)
		{
		foreach (Transform child in transform)
		{
			//Debug.Log("TEST" + child.gameObject.name);
			Destroy(child.gameObject);
		}
	//	Debug.Log("TEST2" + gameObject.name);
		Destroy(gameObject);
		//Debug.Log("TEST3");
		}
	}
	
}
