using UnityEngine;
using System.Collections;

public class PlayerIndicator : MonoBehaviour 
{
	public MeshRenderer dirCenterRenderer;
	public MeshRenderer dirIndicatorRenderer;
	GameObject player;
	Animator anim;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		transform.position = player.transform.position;
		anim = GetComponent<Animator> ();
	}

	void Update () 
	{
		float x = Input.GetAxis ("Horizontal");
		float z = Input.GetAxis ("Vertical");
		float mag = Mathf.Abs (x) + Mathf.Abs (z);
		anim.SetFloat("MoveMag",mag);

		if(player.GetComponent<MyCharacterController>().isGrounded &&
		   player.GetComponent<Teleporter>().teleport == false)
		{
			dirCenterRenderer.enabled = true;
			dirIndicatorRenderer.enabled = true;
		}

		else
		{
			dirCenterRenderer.enabled = false;
			dirIndicatorRenderer.enabled = false;
		}

		transform.position = player.transform.position;
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, player.transform.eulerAngles.y+90f, transform.eulerAngles.z);
	}
}
