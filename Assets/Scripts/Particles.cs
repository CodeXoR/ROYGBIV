using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Particles : MonoBehaviour 
{
	public SkinnedMeshRenderer renderer;
	Mesh mesh;
	public List<Vector3> vertPos;
	public GameObject particle;
	// Use this for initialization
	void Start () {
		mesh = renderer.sharedMesh;
		vertPos = new List<Vector3>();
		foreach(int index in mesh.triangles)
		{
			Vector3 pos = transform.TransformPoint(mesh.vertices[mesh.triangles[index]]);
			pos.z -= 1.2f;
			vertPos.Add(pos);
		}
		vertPos.Sort ((a,b) => a.y.CompareTo (b.y));
		StartCoroutine (Effect ());
	}
	
	IEnumerator Effect()
	{
		int itiration = 0;
		while(true)
		{
			if( itiration >= vertPos.Count )
				break;

			if( itiration < vertPos.Count )
			{
				GameObject p = Instantiate(particle, vertPos[itiration], particle.transform.rotation) as GameObject;
				GameObject q = Instantiate(particle, vertPos[itiration+1], particle.transform.rotation) as GameObject;
				GameObject r = Instantiate(particle, vertPos[itiration+2], particle.transform.rotation) as GameObject;
				itiration+=3;
				//yield return new WaitForSeconds(1f);
				Destroy(p, .5f);
				Destroy(q, .5f);
				Destroy(r, .5f);
				yield return 0;
			}

			else
				yield return 0;
		}
	}
}
