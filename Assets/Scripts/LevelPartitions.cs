using UnityEngine;
using System.Collections;

public class LevelPartitions : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			other.GetComponent<MyCharacterController>().currentArea = transform.name;
		}
	}
}
