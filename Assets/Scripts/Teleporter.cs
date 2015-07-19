using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour 
{
	public GameObject originPad;
	public GameObject destPad;
	public SkinnedMeshRenderer playerRenderer;
	public SkinnedMeshRenderer gunRenderer;
	public bool teleport = false;
	Material playerMat;
	Material gunMat;
	float timer = 0;
	
	void Start () 
	{
		playerMat = playerRenderer.material;
		gunMat = gunRenderer.material;
		timer = 0;
		StartCoroutine (Track ());
	}
	
	IEnumerator Track () 
	{
		while(true)
		{
			if(teleport==true)
			{
				yield return StartCoroutine(Teleport());
				teleport = false;
				// activate player controller and destination pad collider
				GetComponent<CharacterController>().enabled = true;
				destPad.GetComponent<Collider>().enabled = true;
				yield return 0;
			}
			else 
				yield return 0;
		}
	}

	public IEnumerator Teleport () 
	{
		bool ascending = true;
		ascending = timer == 0 ? true : false;
		while(true)
		{
			if(ascending)
			{
				// from origin to destination pad
				if(timer >= 0 && timer < 1)
				{
					timer += Time.deltaTime;
					transform.position += -originPad.transform.forward * Time.deltaTime;
				}

				if(timer > 1)
				{
					timer = 1;
					playerMat.SetFloat ("_Transparency", timer);
					gunMat.SetFloat ("_Transparency", timer);
					transform.position = destPad.transform.position;
					transform.rotation = destPad.transform.rotation;
					ascending = !ascending;
				}
			}
				
			if(!ascending)
			{
				// from destination to outside of collider range
				if(timer <= 1 && timer > 0)
				{
					timer -= Time.deltaTime;
					transform.position += destPad.transform.forward * Time.deltaTime;
				}

				if(timer < 0)
				{
					timer = 0;
					playerMat.SetFloat ("_Transparency", timer);
					gunMat.SetFloat ("_Transparency", timer);
					yield break;
				}
			}

			playerMat.SetFloat ("_Transparency", timer);
			gunMat.SetFloat ("_Transparency", timer);
			yield return 0;
		}
	}
}
