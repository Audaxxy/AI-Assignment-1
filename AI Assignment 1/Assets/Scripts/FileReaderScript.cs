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
	public GameObject UIpanel,errorLog,menu;
	public GameObject door;
	public TMP_InputField pathField;
	string filePath;
	string[] data;
	List<doorData> doorDataContainer= new List<doorData>();
	
	char seperatorChar = '\t' ;
	const float errorTolerance = 0.0000001f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

		
		

    }
	public void getFilePath()
	{
		try
		{
			filePath = pathField.text;

			data = File.ReadAllLines(filePath);
			foreach (string line in data)
			{
				Debug.Log(line);
			}
		}
		catch (FileNotFoundException)
		{
			Debug.Log("File not Found");
		}

		convertData();

	}
	public void convertData()
	{
		doorData.totalChance = 0;
		for (int i=0; i<data.Length-1; i++)
		{
			string[] dataArray = data[i+1].Split(seperatorChar);
			doorData currentDoor = new doorData();
			currentDoor.hot = (char.Parse(dataArray[0])=='Y');
			currentDoor.noisy = (char.Parse(dataArray[1]) == 'Y');
			currentDoor.safe = (char.Parse(dataArray[2]) == 'Y');
			currentDoor.chance = float.Parse(dataArray[3]);
			doorDataContainer.Add(currentDoor);
			doorData.totalChance += currentDoor.chance;
		}
		
		
		Debug.Log(doorData.totalChance);
		if ((doorData.totalChance-1f)<=errorTolerance)
		{
			Debug.Log("Probabilities  total to 100%. Valid file.");
			UIpanel.SetActive(false);
			GenerateDoors();
		}
		else
		{
			Debug.Log("Probabilities do not total to 100%. Invalid file.");
			menu.SetActive(false);
			errorLog.SetActive(true);
		}
		
	}
	public void GenerateDoors()
	{
		for (int i = 0; i < 20; i++)
		{

			GameObject newDoor = Instantiate(door,new Vector3(i*2.1f,0,0),Quaternion.identity);
			doorData selectedData = null;
			float randomChance = Random.Range(float.Epsilon, doorData.totalChance);
			for (int j = 0; j < doorDataContainer.Count; j++)
			{
				randomChance -= doorDataContainer[j].chance;

				if (randomChance <= 0)
				{
					selectedData = doorDataContainer[j];
					break;
				}
			}

			newDoor.GetComponent<Door>().hot = selectedData.hot;
			newDoor.GetComponent<Door>().noisy = selectedData.noisy;
			newDoor.GetComponent<Door>().safe = selectedData.safe;
			
		}
	}
}
