using UnityEngine;

public class MazeCell
{
    public Vector3 position;
    public Color color;
    public bool visited;

    public Mesh mesh;

    public MazeCell(Vector3 pos, Color col, Mesh msh)
    {
        position = pos;
        color = col;
        visited = false;
        mesh = msh;
    }
}

public class Wall
{
    public Vector3 position;
    public float height;
    public float width;

    public Wall(Vector3 pos, float h, float w)
    {
        position = pos;
        height = h;
        width = w;
    }
}