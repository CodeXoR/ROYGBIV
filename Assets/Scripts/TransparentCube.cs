using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransparentCube : MonoBehaviour 
{
	public enum CheckDirection { forward = 0, right = 1 } 
	public List<TransparentCube> adjacentObjs = new List<TransparentCube> ();
	public Material transparentMat;
	public Material originalMat;
	MeshRenderer renderer;
	public CheckDirection rayDir = CheckDirection.forward;
	public bool checkLAdjs = true;
	public bool checkRAdjs = true;
	// Use this for initialization
	void Start () 
	{
		//GetAdjacentObjs (rayDir);
		renderer = GetComponent<MeshRenderer> ();
		originalMat = renderer.material;
	}

	void GetAdjacentObjs(CheckDirection dir)
	{
		if(rayDir==CheckDirection.forward)
		{
			Ray rayF = new Ray(transform.position + transform.forward, transform.forward);
			Ray rayB = new Ray(transform.position + transform.forward/2, -transform.forward);
			//Debug.DrawRay(rayF.origin, rayF.direction, Color.red);
			//Debug.DrawRay(rayB.origin, rayB.direction, Color.red);
			RaycastHit hit;
			if(Physics.Raycast(rayF, out hit, 1.0f))
			{
				TransparentCube tC = hit.transform.GetComponent<TransparentCube>();
				if( tC != null )
					adjacentObjs.Add(hit.transform.GetComponent<TransparentCube>());
			}
			if(Physics.Raycast(rayB, out hit, 1.0f))
			{
				TransparentCube tC = hit.transform.GetComponent<TransparentCube>();
				if( tC != null )
					adjacentObjs.Add(hit.transform.GetComponent<TransparentCube>());
			}
		}
		else
		{
			Ray rayR = new Ray(transform.position + transform.forward * .7f, transform.right);
			Ray rayL = new Ray(transform.position + transform.forward * .7f, -transform.right);
			//Debug.DrawRay(rayR.origin, rayR.direction, Color.red);
			//Debug.DrawRay(rayL.origin, rayL.direction, Color.red);
			RaycastHit hit;
			if(checkRAdjs)
			{
				if(Physics.Raycast(rayR, out hit, 1.0f))
				{
					TransparentCube tC = hit.transform.GetComponent<TransparentCube>();
					if( tC != null )
						adjacentObjs.Add(hit.transform.GetComponent<TransparentCube>());
				}
			}
			if(checkLAdjs)
			{
				if(Physics.Raycast(rayL, out hit, 1.0f))
				{
					TransparentCube tC = hit.transform.GetComponent<TransparentCube>();
					if( tC != null )
						adjacentObjs.Add(hit.transform.GetComponent<TransparentCube>());
				}
			}
		}
	}
}
