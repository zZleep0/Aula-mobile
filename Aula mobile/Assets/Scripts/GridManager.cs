using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //Controle de grid
    public List<Sprite> sprites = new List<Sprite> ();
    public GameObject tilePrefab;
    public int gridDimension = 8;
    public float distance = 1.0f;
    private GameObject[,] grid;

    //GameManager
    public GameObject gameOverMenu;
    public TextMeshProUGUI movesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreEndText;

    public int startingMoves = 50;
    private int numMove;
    public int NumMove
    {
        get
        {
            return numMove;
        }
        set
        {
            numMove = value;
            movesText.text = "Moves: " + numMove.ToString(); //ou "" + numMove
        }
    }
    private int score;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            scoreText.text = "Score: " + score;
        }
    }

    public static GridManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
        Score = 0;
        NumMove = startingMoves;
        gameOverMenu.SetActive(false);
    }

    void Start()
    {

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

                //escolher a pe�a que vai nesse objeto
                List<Sprite> possibleSprites = new List<Sprite>(sprites);

                //verifica��o horizontal
                Sprite left1 = GetSpriteAt(column - 1, row);
                Sprite left2 = GetSpriteAt(column - 2, row);
                if (left1 != null && left1 == left2)
                {
                    possibleSprites.Remove(left1);
                }

                //verifica��o vertical
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

    //Verificar iguais para pontuar
    bool CheckMatches()
    {
        //Semelhante a lista. Calculo mais rapido. Por�m, nao aceita valores iguais
        HashSet<SpriteRenderer> matchedTiles = new HashSet<SpriteRenderer>();
        for (int row = 0; row < gridDimension; row++)
        {
            for (int column = 0; column < gridDimension; column++)
            {
                SpriteRenderer current = GetSpriteRendererAt(column, row);

                List<SpriteRenderer> horizontalMatches = FindColumnMatchForTile(column, row, current.sprite);
                if (horizontalMatches.Count >= 2)
                {
                    matchedTiles.UnionWith(horizontalMatches);
                    matchedTiles.Add(current);
                }

                List<SpriteRenderer> verticalMatches = FindRowMatchForTile(column, row, current.sprite);
                if (verticalMatches.Count >= 2)
                {
                    matchedTiles.UnionWith(verticalMatches);
                    matchedTiles.Add(current);
                }
            }
        }

        foreach (SpriteRenderer renderer in matchedTiles)
        {
            renderer.sprite = null;
        }

        Score += matchedTiles.Count;
        return matchedTiles.Count > 0;
    }

    List<SpriteRenderer> FindColumnMatchForTile (int col, int row, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = col + 1; i < gridDimension; i++)
        {
            SpriteRenderer nextCol = GetSpriteRendererAt(i, row);
            if (nextCol.sprite != sprite)
            {
                break;
            }
            result.Add(nextCol);
        }
        return result;
    }

    List<SpriteRenderer> FindRowMatchForTile(int col, int row, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = row + 1; i < gridDimension; i++)
        {
            SpriteRenderer nextRow = GetSpriteRendererAt(col, i);
            if (nextRow.sprite != sprite)
            {
                break;
            }
            result.Add(nextRow);
        }
        return result;
    }

    //Verificar sprites para nao haver iguais no inicio
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

    SpriteRenderer GetSpriteRendererAt(int column, int row)
    {
        if (column < 0 || column >= gridDimension || row < 0 || row >= gridDimension)
        {
            return null;
        }

        GameObject tile = grid[column, row];
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        return renderer;
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

        bool matches = CheckMatches();

        if (!matches)
        {
            temp = renderer1.sprite;
            renderer1.sprite = renderer2.sprite;
            renderer2.sprite = temp;
            SoundManager.instance.PlaySound(SoundManager.SoundType.TypeMove);
        }
        else
        {
            SoundManager.instance.PlaySound(SoundManager.SoundType.TypePop);
            NumMove--;
            do
            {
                FillHoles();
            } while (CheckMatches());

            if (NumMove <= 0)
            {
                NumMove = 0;
                GameOver();
            }
        }
    }

    void FillHoles()
    {
        for (int column = 0; column < gridDimension; column++)
        {
            for (int row = 0; row < gridDimension; row++)
            {
                while(GetSpriteRendererAt(column, row).sprite == null)
                {
                    SpriteRenderer current = GetSpriteRendererAt(column, row);
                    SpriteRenderer next = current;
                    for (int filler = row; filler < gridDimension - 1; filler++)
                    {
                        next = GetSpriteRendererAt(column, filler + 1);
                        current.sprite = next.sprite;
                        current = next;
                    }
                    next.sprite = sprites[Random.Range(0, sprites.Count)];
                }
            }
        }
    }
    void GameOver()
    {
        scoreEndText.text = "Final Score: " + Score.ToString();
        gameOverMenu.SetActive(true);
        SoundManager.instance.PlaySound(SoundManager.SoundType.TypeGameOver);
    }
}
