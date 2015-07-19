using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
	public GameObject laserReflected;
	public float laserSpeed;
	public float lifetime;
	Vector3 dir;

	void Awake()
	{
		dir = transform.forward;
		Destroy (gameObject, lifetime);
	}

	void Update()
	{
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;
		//Debug.DrawRay (transform.position, transform.forward * .5f, Color.green);
		if(Physics.Raycast(ray, out hit, .5f))
		{
			if(hit.transform.gameObject.layer == LayerMask.NameToLayer("ReflectiveObject"))
			{

				Color b = hit.transform.gameObject.GetComponent<MeshRenderer>().material.color;
				Color a = GetComponent<LineRenderer>().material.color;
				Color mix = new Color();
				if( a != Color.green && a != Color.blue && a != Color.red )
					mix = b;
				if( a == Color.green && b == Color.blue )
					mix = Color.cyan;
				else if( a == Color.blue && b == Color.red )
					mix = Color.magenta;
				else if( a == Color.red && b == Color.green )
					mix = Color.yellow;

				laserReflected.GetComponent<LineRenderer>().material.color = mix;
				Vector3 reflectForce = Vector3.Reflect(transform.forward, hit.normal);
				Quaternion rot = Quaternion.LookRotation(reflectForce);
				Instantiate(laserReflected, hit.point, rot);
				Destroy(gameObject);
			}

			else if( hit.transform.gameObject.layer == LayerMask.NameToLayer("Key") )
			{
				if( hit.transform.GetComponent<ColorCrate>().Key == GetComponent<LineRenderer>().material.color )
				{
					hit.transform.GetComponent<ColorCrate>().activated = true;
					Destroy(gameObject);
				}
				else
					Destroy(gameObject);
			}

			else if(hit.transform.gameObject.layer != LayerMask.NameToLayer("ReflectiveObject") &&
			   hit.transform.gameObject.layer != LayerMask.NameToLayer("Key"))
				Destroy(gameObject);
		}
		transform.position += dir.normalized * Time.deltaTime * laserSpeed;
	}
}
