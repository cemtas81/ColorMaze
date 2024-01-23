using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private SimpleMazeGenerator generator;
    void Start()
    {
        generator = FindObjectOfType<SimpleMazeGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        generator.currentCell = PlayerPrefs.GetInt("painted");
        if (generator.currentCell<=0)
        {
            generator.FinishCurrentLevel();
        }
    }
}
