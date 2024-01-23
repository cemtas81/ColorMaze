using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleMazeGenerator : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public GameObject groundPrefab,playerPrefab,trapPrefab,leftSpacePrefab,rightSpacePrefab;
    public GameObject wallPrefab;

 
    private int currentLevel;
    public int currentCell;
    void Start()
    {

        currentLevel = PlayerPrefs.GetInt("lastLevel",0); // Default to level 1 if not set
        GenerateMazeForCurrentLevel();

    }

    void GenerateMazeForCurrentLevel()
    {
        int[,] mazeData = ReadMazeDataFromText(currentLevel);

        if (mazeData != null)
        {
            // Generate or instantiate maze based on mazeData
            GenerateMaze(mazeData);
        }
        else
        {
            Debug.LogError("Failed to generate maze for level " + currentLevel);
        }
    }
    void GenerateMaze(int[,] mazeData)
    {
        if (mazeData == null)
        {
            Debug.LogError("Invalid maze data.");
            return;
        }

        // Variables to store the player's starting position
        int playerStartX = -1;
        int playerStartY = -1;

        // Instantiate GameObjects based on mazeData
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                switch (mazeData[i, j])
                {
                    case -1:
                        // Save player's starting position
                        playerStartX = i;
                        playerStartY = j;
                        break;
                    case 0:
                        // Instantiate wall at a higher position
                        Instantiate(wallPrefab, new Vector3(i, 1, j), Quaternion.identity);
                        break;
                    case 1:
                        // Instantiate ground
                        Instantiate(groundPrefab, new Vector3(i, 0, j), Quaternion.identity);                      
                        break;
                    case 2:
                        // Instantiate trap or handle the trap logic
                        Instantiate(trapPrefab, new Vector3(i, 0, j), Quaternion.identity);
                        break;
                    case 8:
                        // Instantiate left space
                        Instantiate(leftSpacePrefab, new Vector3(i, 0, j), Quaternion.identity);
                        break;
                    case 9:
                        // Instantiate right space
                        Instantiate(rightSpacePrefab, new Vector3(i, 0, j), Quaternion.identity);
                        break;
                    // Add other cases for different tile types if needed
                    default:
                        Debug.LogWarning($"Unknown tile type {mazeData[i, j]} at ({i}, {j}).");
                        break;
                }
            }
        }

        // Check if player's starting position is found
        if (playerStartX != -1 && playerStartY != -1)
        {
            // Instantiate ground under the player
            Instantiate(groundPrefab, new Vector3(playerStartX, 0, playerStartY), Quaternion.identity);

            // Instantiate player or perform other actions based on the starting position
            // For example, instantiate a player GameObject
            Instantiate(playerPrefab, new Vector3(playerStartX, 1.1f, playerStartY), Quaternion.identity);
        }
    }

    int[,] ReadMazeDataFromText(int level)
    {
        int[,] mazeData = null;

        // Construct the file path based on the level and file extension
        string fileName = level.ToString() + ".text";
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                string[] lines = json.Split('\n');
                mazeData = new int[lines.Length - 1, width]; // Adjust the array size

                for (int i = 1; i < lines.Length; i++) // Start from 1 to skip the first line
                {
                    string[] values = lines[i].Trim().Split(',');

                    for (int j = 0; j < width; j++)
                    {
                        if (int.TryParse(values[j], out int parsedValue))
                        {
                            mazeData[i - 1, j] = parsedValue; // Adjust the array index
                        }
                        else
                        {
                            Debug.Log($"Error parsing value at line {i}, position {j + 1} in file '{filePath}'. Value: {values[j]}");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Error reading file '{filePath}': {e.Message}");
            }
        }
        else
        {
            Debug.Log($"File not found: {filePath}");
        }

        return mazeData;
    }

    void NextLevel()
    {
        currentLevel++;
        PlayerPrefs.SetInt("lastLevel", currentLevel);
        //GenerateMazeForCurrentLevel();
        //DrawMaze();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void FinishCurrentLevel()
    {
        // Add your logic for finishing the current level
        // ...
        Debug.Log("Finished");
        // Proceed to the next level
        NextLevel();
    }
}
