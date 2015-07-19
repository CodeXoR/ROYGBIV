using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour 
{
	public bool operational = false;
	public bool levelEnd = false;
	public GameObject door;
	Animation anim;
	void Start()
	{
		anim = door.GetComponent<Animation>();
	}

	public bool Operational { get { return operational; } set { operational = value; } }

	void Update()
	{
		if(operational)
			GetComponent<SphereCollider>().enabled = true;

		else
			GetComponent<SphereCollider>().enabled = false;
	}

	void OnTriggerEnter (Collider other) 
	{
		if(other.tag == "Player")
		{
			anim.Play("open");
			if(levelEnd)
				GameManager.Get().gameState = GameManager.GameStates.LevelComplete;
		}
	}
	
	void OnTriggerExit()
	{
		anim.Play("close");
	}
}
