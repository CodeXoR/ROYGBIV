using UnityEngine;
using System.Collections;

public class TeleportPad : MonoBehaviour 
{
	public GameObject destination;

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			Teleporter tele = other.GetComponent<Teleporter>();
			if(tele.teleport==false)
			{
				// deactivate player controller and deactivate destination pad collider
				other.GetComponent<CharacterController>().enabled = false;
				destination.GetComponent<Collider>().enabled = false;
				tele.originPad = gameObject;
				tele.destPad = destination;
				tele.teleport = true;
			}
		}
	}
}
