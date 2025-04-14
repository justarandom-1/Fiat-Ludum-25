using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;

    [SerializeField] private GameObject tower;
    [SerializeField] private int towerType;

    private string filePath = Application.persistentDataPath + "/gameData.txt";

    void Start()
    {
        saveButton.onClick.AddListener(SaveData);
        loadButton.onClick.AddListener(LoadData);
        deleteButton.onClick.AddListener(DeleteData);
    }

    // runs when save button is clicked
    void SaveData()
    {
        // called to hold tower coords
        Vector3 position = tower.transform.position;
        // save game objects to the list
        string gameDataStr;
        string towerCoords = $"{position.x},{position.y},{position.z}";
        gameDataStr = $"{towerType} {towerCoords}";
        // save list to filePath
        File.WriteAllText(filePath, gameDataStr);
    }

    // called when load button is clicked
    public void LoadData()
    {
        // if filePath can be found:
        if (File.Exists(filePath))
        {
            // open streamreader
            StreamReader dataReader = new StreamReader(filePath);
            // read line
            string line = dataReader.ReadLine();
            dataReader.Close();
            // parse + move existing tower to same position
            string[] myData = line.Split(' ');
            int towerType = int.Parse(myData[0]);
            string[] coords = myData[1].Split(',');
            Vector3 towerCoordinates = new Vector3(
                float.Parse(coords[0]), 
                float.Parse(coords[1]), 
                float.Parse(coords[2])
                );

            tower.transform.position = towerCoordinates;
        }
        else
        {
            // otherwise return error:
            Debug.LogError("You need to save an existing game before you can load a game");
        }
    }

    // called when delete data is clicked
    void DeleteData()
    {
        // open filePath
        // delete all data
    }
}
