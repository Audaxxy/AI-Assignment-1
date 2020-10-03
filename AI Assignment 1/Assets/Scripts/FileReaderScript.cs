//100653593 Nathan Boldy
// Oct 02 2020
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class doorData
{
	public bool noisy, hot, safe;
	public float chance;
	public static float totalChance;
	
}

public class FileReaderScript : MonoBehaviour
{

	public GameObject UIpanel,probabilityError,noFile,menu; //UI element refs
	public GameObject door; //Door ref
	public TMP_InputField pathField; //input box for user input file path 
	string filePath; //Contains the user defined file path
	string[] data;//String array containing all lines of file
	List<doorData> doorDataContainer= new List<doorData>(); //List of door data
	
	char seperatorChar = '\t' ;//omits tabs from file, used with split function
	const float errorTolerance = 0.0000001f;//used when checking if probabilities total to 100% for error check

	//Get file path from user input field
	public void getFilePath()
	{
		try
		{
			//get path from field
			filePath = pathField.text;

			//read the file
			data = File.ReadAllLines(filePath);
			foreach (string line in data)
			{
				//output for testing
				Debug.Log(line);
				
			}
		}
		//if file not found
		catch (FileNotFoundException)
		{
			Debug.Log("File not Found");
			menu.SetActive(false);
			noFile.SetActive(true);
		}

		convertData();

	}
	//convert data from test to usable
	public void convertData()
	{
		//sum of elapsed chance reset
		doorData.totalChance = 0;
		//for the length of the data array
		for (int i=0; i<data.Length-1; i++)
		{
			//split each set of 4 values into an array of 4 values, char char char float
			string[] dataArray = data[i+1].Split(seperatorChar);
			doorData currentDoor = new doorData();
			//if Y, door is hot, set bool true
			currentDoor.hot = (char.Parse(dataArray[0])=='Y');
			//if Y, door is noisy, set bool true
			currentDoor.noisy = (char.Parse(dataArray[1]) == 'Y');
			//if Y, door is safe, set bool true
			currentDoor.safe = (char.Parse(dataArray[2]) == 'Y');
			//Take probability as a float
			currentDoor.chance = float.Parse(dataArray[3]);
			//add door to list
			doorDataContainer.Add(currentDoor);
			//add chance of door to totalchance used for debugging
			doorData.totalChance += currentDoor.chance;
		}
		
		
		Debug.Log(doorData.totalChance);
		// if total probabilities are either less than, or greater than 100%, the file is invalid and an error is thrown.
		if ((doorData.totalChance-1f)<=errorTolerance)
		{
			Debug.Log("Probabilities  total to 100%. Valid file.");
			//disables menu UI
			UIpanel.SetActive(false);
			// Locks cursor
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			//generates doors
			GenerateDoors();
		}
		else
		{
			Debug.Log("Probabilities do not total to 100%. Invalid file.");
			menu.SetActive(false);
			probabilityError.SetActive(true);
		}
		
	}
	public void GenerateDoors()
	{
		//generate 20 doors
		for (int i = 0; i < 20; i++)
		{
			//instantiate new door, at an offset based on iterator
			GameObject newDoor = Instantiate(door,new Vector3(0,0,i * 2.1f),Quaternion.identity);
			doorData selectedData = null;
			//Create a random float
			float randomChance = Random.Range(float.Epsilon, doorData.totalChance);
			//iterate through door data array until the first door to break the chance threshold occurs
			for (int j = 0; j < doorDataContainer.Count; j++)
			{
				randomChance -= doorDataContainer[j].chance;

				if (randomChance <= 0)
				{
					//load the current iteration of door container into the selected data var
					selectedData = doorDataContainer[j];
					break;
				}
			}

			//assign values of selected data to new door
			newDoor.GetComponent<Door>().hot = selectedData.hot;
			newDoor.GetComponent<Door>().noisy = selectedData.noisy;
			newDoor.GetComponent<Door>().safe = selectedData.safe;
			
		}
	}
}
