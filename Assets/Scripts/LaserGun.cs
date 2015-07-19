using UnityEngine;
using System.Collections;

public class LaserGun : MonoBehaviour
{
	public GameObject bullet;
	public Transform laserBarrel;
	public LineRenderer aimGuide;
	// Laser SFX
	public AudioClip sfx;

	void Start()
	{
		aimGuide.enabled = false;
	}

	public void ActivateLaserGuide()
	{
		// laserGuide
		Ray ray = new Ray(laserBarrel.position, laserBarrel.forward);
		RaycastHit hit;
		aimGuide.SetPosition(0, ray.origin);
		if(Physics.Raycast(ray, out hit, 10.0f))
		{
			aimGuide.enabled = true;
			aimGuide.SetPosition(1, hit.point);
		}
		
		else
			aimGuide.SetPosition(1, ray.GetPoint(10f));
	}

	public void DeactivateLaserGuide()
	{
		aimGuide.enabled = false;
	}

	public void ShootProjectile()
	{
		GetComponent<AudioSource>().PlayOneShot (sfx, .2f);
		GameObject proj = (GameObject)Instantiate(bullet, laserBarrel.position, laserBarrel.rotation);
	}
}
