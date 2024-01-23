using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMazeGenerator : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f;
    public Color[] colors;
    public Mesh mesh;

    private MazeCell[,] maze;

    void Start()
    {
        maze = GenerateMaze();
        InstantiateMaze();
    }

    MazeCell[,] GenerateMaze()
    {
        MazeCell[,] maze = new MazeCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = new MazeCell(new Vector3(x, 0, y) * cellSize, GetRandomColor(), mesh);
            }
        }

        for (int x = 0; x < width; x += 2)
        {
            for (int y = 0; y < height; y += 2)
            {
                if (x < width - 1)
                {
                    maze[x, y].color = maze[x + 1, y].color;
                }
                if (y < height - 1)
                {
                    maze[x, y].color = maze[x, y + 1].color;
                }
            }
        }

        return maze;
    }

    void InstantiateMaze()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject cellObj = new GameObject();
                cellObj.transform.parent = transform;
                cellObj.transform.localPosition = maze[x, y].position;
                cellObj.transform.localScale = new Vector3(cellSize, 0.5f, cellSize);
                cellObj.name = "Maze Cell " + x + " " + y;
                cellObj.AddComponent<MeshRenderer>().material.color = maze[x, y].color;
                cellObj.AddComponent<MeshFilter>().mesh = maze[x, y].mesh;
                cellObj.AddComponent<MeshCollider>();
            }
        }
    }

    Color GetRandomColor()
    {
        return colors[Random.Range(0, colors.Length)];
    }
}