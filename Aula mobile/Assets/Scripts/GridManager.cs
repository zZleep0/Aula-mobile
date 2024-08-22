using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite> ();
    public GameObject tilePrefab;
    public int gridDimension = 8;
    public float distance = 1.0f;
    private GameObject[,] grid;

    public static GridManager instance { get; private set; }

    void Start()
    {
        instance = this;

        //grid = new GameObject[8,8]; mesma coisa
        grid = new GameObject[gridDimension, gridDimension];
        InitGrid();
    }

    void Update()
    {
        
    }

    void InitGrid()
    {
        Vector3 posOffset = transform.position - new Vector3(gridDimension * distance / 2.0f, gridDimension * distance / 2.0f, 0);

        for (int row = 0; row < gridDimension; row++)
        {
            for (int column = 0; column < gridDimension; column++)
            {
                GameObject newTile = Instantiate(tilePrefab);

                //escolher a peça que vai nesse objeto
                List<Sprite> possibleSprites = new List<Sprite>(sprites);

                //verificação horizontal
                Sprite left1 = GetSpriteAt(column - 1, row);
                Sprite left2 = GetSpriteAt(column - 2, row);
                if (left1 != null && left1 == left2)
                {
                    possibleSprites.Remove(left1);
                }

                //verificação vertical
                Sprite down1 = GetSpriteAt(column, row - 1);
                Sprite down2 = GetSpriteAt(column, row - 2);
                if (down1 != null && down1 == down2)
                {
                    possibleSprites.Remove(down1);
                }

                SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>();
                renderer.sprite = possibleSprites[Random.Range(0, possibleSprites.Count)];

                Tile tile = newTile.AddComponent<Tile>();
                tile.position = new Vector2Int(column, row);

                newTile.transform.parent = transform;
                newTile.transform.position = new Vector3(column * distance, row * distance, 0) + posOffset;
                grid[column, row] = newTile;

            }
        }
    }

    Sprite GetSpriteAt(int column, int row)
    {
        if (column < 0 || column >= gridDimension || row < 0 || row >= gridDimension)
        {
            return null;
        }

        GameObject tile = grid[column, row];
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        return renderer.sprite;
    }

    public void SwapTiles(Vector2Int tile1Position, Vector2Int tile2Position)
    {
        GameObject tile1 = grid[tile1Position.x, tile1Position.y];
        SpriteRenderer renderer1 = tile1.GetComponent<SpriteRenderer>();

        GameObject tile2 = grid[tile2Position.x, tile2Position.y];
        SpriteRenderer renderer2 = tile2.GetComponent<SpriteRenderer>();

        Sprite temp = renderer1.sprite;
        renderer1.sprite = renderer2.sprite;
        renderer2.sprite = temp;
    }
}
