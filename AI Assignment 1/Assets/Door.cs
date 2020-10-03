//100653593 Nathan Boldy
// Oct 02 2020
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	//variables
	public bool hot, noisy, safe;
	public Material hotMat, normalMat;

	public GameObject winScreen, loseScreen;
  
	//initializes values based upon file determined base state upon instantiation
    void Start()
    {
		if (hot)
		{
			//enabling lights and setting to hot material if door is hot
			this.GetComponent<Light>().enabled = true;
			this.GetComponent<MeshRenderer>().material = hotMat;
		}
		else
		{
			//inverse of previous comment
			this.GetComponent<Light>().enabled = false;
			this.GetComponent<MeshRenderer>().material = normalMat;
		}

		if (noisy)
		{
			//enables sound if door is noisy
			this.GetComponent<AudioSource>().mute = false;
		}
		else
		{
			//inverse of previous comment
			this.GetComponent<AudioSource>().mute = true;
		}

    }
	
   
}
