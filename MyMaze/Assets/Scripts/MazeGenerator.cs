using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public GameObject groundPrefab;
    public GameObject wallPrefab;

    private int[,] maze;
    private Stack<Vector2Int> stack;
    private Vector2Int currentCell;

    void Start()
    {
        InitializeMaze();
        GenerateMaze();
        DrawMaze();
    }

    void InitializeMaze()
    {
        maze = new int[width, height];
        stack = new Stack<Vector2Int>();
        currentCell = new Vector2Int(0, 0);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                maze[i, j] = 1; // Set all cells as walls initially
            }
        }

        stack.Push(currentCell);
    }

    void GenerateMaze()
    {
        while (stack.Count > 0)
        {
            maze[currentCell.x, currentCell.y] = 0; // Mark the current cell as visited

            List<Vector2Int> neighbors = GetUnvisitedNeighbors(currentCell);

            if (neighbors.Count > 0)
            {
                stack.Push(currentCell);
                Vector2Int randomNeighbor = neighbors[Random.Range(0, neighbors.Count)];
                RemoveWall(currentCell, randomNeighbor);
                currentCell = randomNeighbor;
            }
            else if (stack.Count > 0)
            {
                currentCell = stack.Pop();
            }
        }

        // Set outer boundary walls
        for (int i = 0; i < width; i++)
        {
            maze[i, 0] = 1;         // Top boundary
            maze[i, height - 1] = 1; // Bottom boundary
        }

        for (int j = 0; j < height; j++)
        {
            maze[0, j] = 1;         // Left boundary
            maze[width - 1, j] = 1; // Right boundary
        }
    }

    List<Vector2Int> GetUnvisitedNeighbors(Vector2Int cell)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        // Check four neighboring cells
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighbor = cell + dir * 2; // Two steps to get neighbor

            if (IsInBounds(neighbor) && maze[neighbor.x, neighbor.y] == 1)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    void RemoveWall(Vector2Int current, Vector2Int next)
    {
        Vector2Int wall = current + (next - current) / 2;
        maze[wall.x, wall.y] = 0;
    }

    void DrawMaze()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Instantiate(groundPrefab, new Vector3(i, 0, j), Quaternion.identity);

                if (maze[i, j] == 1)
                {
                    Instantiate(wallPrefab, new Vector3(i, 1, j), Quaternion.identity);
                }
            }
        }
    }

    bool IsInBounds(Vector2Int cell)
    {
        return cell.x >= 0 && cell.x < width && cell.y >= 0 && cell.y < height;
    }
}
