using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SimpleMazeGenerator : MonoBehaviour
{
    public int width = 10, height = 10, currentCell;
    public GameObject completed, groundPrefab, playerPrefab, trapPrefab, leftSpacePrefab, rightSpacePrefab, outsidePrefab, warpSmoke, wallPrefab, warpPrefab, ufo;
    public Text levelText, levelText2;
    public TMP_Text diamond;
    private int currentLevel;
    public float instantiationProbability = .5f, randomNumber, randomNumberUfo, ufoProbability;
    private List<Vector3> groundPositions = new(); // Store positions of ground prefabs
    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt("lastLevel", 1); // Default to level 1 if not set
        GenerateMazeForCurrentLevel();
        levelText.text = ("Level" + ":" + currentLevel.ToString());
        completed.SetActive(false);
    }
    void GenerateMazeForCurrentLevel()
    {
        int[,] mazeData = ReadMazeDataFromText(currentLevel);

        if (mazeData != null)
        {
            // Generate or instantiate maze based on mazeData
            GenerateMaze(mazeData, 20);
        }
        else
        {
            Debug.LogError("Failed to generate maze for level " + currentLevel);
        }

    }
    void GenerateMaze(int[,] mazeData, int v)
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
                        Instantiate(wallPrefab, new Vector3(i, Random.Range(1, 1.1f), j), Quaternion.identity);
                        break;
                    case 1:
                        // Instantiate ground and add its position to the list
                        GameObject ground = Instantiate(groundPrefab, new Vector3(i, 0, j), Quaternion.identity);
                        groundPositions.Add(ground.transform.position);
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

        // Choose a random ground position from the list
        if (groundPositions.Count > 0)
        {
            randomNumber = Random.value; randomNumberUfo = Random.value;
            // Check if the random number is within the probability threshold
            if (randomNumber <= instantiationProbability)
            {
                Vector3 randomGroundPosition = groundPositions[Random.Range(0, groundPositions.Count)];

                // Instantiate your object on the random ground position
                Instantiate(warpPrefab, randomGroundPosition, Quaternion.identity);
            }
            if (randomNumberUfo <= ufoProbability)
            {
                ufo.SetActive(true);
            }
        }

        // Fill the area outside the maze with another prefab
        for (int x = -21; x < width + v; x++)
        {
            for (int y = -21; y < height + v; y++)
            {
                if (x < 0 || y < 0 || x >= width || y >= height)
                {
                    // Instantiate the prefab outside the maze bounds
                    Instantiate(outsidePrefab, new Vector3(x, Random.Range(1, 3.5f), y), Quaternion.identity);
                }
            }
        }

        // Check if player's starting position is found
        if (playerStartX != -1 && playerStartY != -1)
        {
            // Instantiate ground under the player
            Instantiate(groundPrefab, new Vector3(playerStartX, 0, playerStartY), Quaternion.identity);

            // Instantiate player or perform other actions based on the starting position
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

    public void NextLevel()
    {
        currentLevel++;
        PlayerPrefs.SetInt("lastLevel", currentLevel);
        //GenerateMazeForCurrentLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void FinishCurrentLevel()
    {
        // Add your logic for finishing the current level
        Debug.Log("Finished");
        // Proceed to the next level
        //NextLevel();
        diamond.enabled = true;
        diamond.gameObject.GetComponent<Animator>().enabled = true;
        StartCoroutine(LevelEnd());

        Time.timeScale = 0;
    }
    IEnumerator LevelEnd()
    {
        yield return new WaitForSecondsRealtime(1);
        diamond.gameObject.GetComponent<Animator>().enabled = false;
        completed.SetActive(true);
        levelText2.text = levelText.text;
    }
    public void Warp(GameObject passenger)
    {
        Vector3 randomGroundPosition = groundPositions[Random.Range(0, groundPositions.Count)];
        randomGroundPosition.y = passenger.transform.position.y;
        passenger.transform.position = randomGroundPosition;
        Instantiate(warpSmoke, new(randomGroundPosition.x, 2, randomGroundPosition.z), Quaternion.identity);
    }
    public void UfoMove(GameObject ufo)
    {
        Vector3 randomGroundPosition = groundPositions[Random.Range(0, groundPositions.Count)];
        randomGroundPosition.y = ufo.transform.position.y;

        // Move UFO to random ground position
        ufo.transform.DOMove(randomGroundPosition, 2)
            .SetEase(Ease.InOutSine) // Customize easing as needed
            .OnComplete(() => EnableCollider(ufo.GetComponent<Collider>()))
            .Play();
    }

    private void EnableCollider(Collider collider)
    {
        if (collider != null)
            collider.enabled = true;
        ufo.GetComponent<Ufo>().particle.Play();
    }
}
