using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransparencyTracker : MonoBehaviour 
{
	public List<TransparentCube> cubes = new List<TransparentCube>();
	public TransparentDoor door;
	// Use this for initialization
	void Start () 
	{
		GameObject[] objs = GameObject.FindGameObjectsWithTag (gameObject.name);
		foreach( GameObject obj in objs )
			cubes.Add(obj.GetComponent<TransparentCube>());
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) 
	{
		if(other.tag == "Player")
		{
			foreach(TransparentCube tc in cubes)
				tc.GetComponent<Renderer>().material = tc.transparentMat;

			if(door!=null)
			{
				door.doorFrameRenderer.material.shader = door.transparentShader;
				door.doorRenderer.material.shader = door.transparentShader;
			}
		}
	}

	void OnTriggerExit (Collider other) 
	{
		if(other.tag == "Player")
		{
			foreach(TransparentCube tc in cubes)
				tc.GetComponent<Renderer>().material = tc.originalMat;

			if(door!=null)
			{
				door.doorFrameRenderer.material.shader = door.originalFrameShader;
				door.doorRenderer.material.shader = door.originalDoorShader;
			}
		}
	}
}
