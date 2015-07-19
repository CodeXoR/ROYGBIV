using UnityEngine;
using System.Collections;

public class TransparentDoor : MonoBehaviour 
{
	public Shader originalFrameShader;
	public Shader originalDoorShader;
	public MeshRenderer doorFrameRenderer;
	public MeshRenderer doorRenderer;
	public GameObject doorFrame;
	public GameObject door;
	public Shader transparentShader;
	// Use this for initialization
	void Start () 
	{
		doorFrameRenderer = doorFrame.GetComponent<MeshRenderer> ();
		doorRenderer = door.GetComponent<MeshRenderer> ();
		originalFrameShader = doorFrameRenderer.material.shader;
		originalDoorShader = doorRenderer.material.shader;
	}
}
