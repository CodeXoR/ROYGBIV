using UnityEngine;
using System.Collections;

public class ColorCrate : MonoBehaviour 
{
	public Canvas activatedIndicator;
	public string ColorKey;
	public bool activated = false;
	Color key;
	
	void Start()
	{
		key = new Color ();
		if (ColorKey == "yellow")
			key = Color.yellow;
		else if (ColorKey == "magenta")
			key = Color.magenta;
		else if (ColorKey == "cyan")
			key = Color.cyan;
	}

	void Update()
	{
		if(activated)
			activatedIndicator.enabled = true;
		else
			activatedIndicator.enabled = false;
	}

	public Color Key { get { return key; } set { key = value; } }
}
