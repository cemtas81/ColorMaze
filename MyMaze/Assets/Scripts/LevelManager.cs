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

}
