using UnityEngine;
using System.Collections;

public class Detonator : MonoBehaviour 
{
	public bool defused = false;
	public Door doorLink;
	public GameObject[] keys;
	public GameObject projector;

	void Update () 
	{
		int count = 0;
		foreach(GameObject key in keys)
		{
			if(key.GetComponent<ColorCrate>().activated)
				count++;
		}
		if( count == keys.Length )
			defused = true;

		if(defused)
		{
			doorLink.Operational = true;
			projector.GetComponent<Projector>().enabled = true;
			foreach(GameObject key in keys)
			{
				key.GetComponent<ColorCrate>().activated = false;
				key.layer = LayerMask.NameToLayer("StaticObject");
			}
			enabled = false;
		}

		else
			doorLink.Operational = false;
	}
}
