using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRules : MonoBehaviour
{
    //Grid Size
    [SerializeField] private int width = 6;
    [SerializeField] private int height = 6;

    //Cell Prefab
    [SerializeField] private Cell cellPrefab = null;

    //Buttons
    [SerializeField] private Button _rulesSet;
    [SerializeField] private Button _placeCells;
    [SerializeField] private Button _start;
    [SerializeField] private Button _stop;

    //Rules
    [SerializeField] private int _underPopulationLimit = 2;
    [SerializeField] private int _overPopulationLimit = 3;

    //Sliders to set various Values
    [SerializeField] private Slider _gridWidth;
    [SerializeField] private Slider _gridHeight;
    [SerializeField] private Slider _underPopulationUpperLimit;
    [SerializeField] private Slider _overPopulationLowerLimit;

    public float _frameSpeed = 0.1f;

    private float timer = 0f;
    private bool isStarted = false;

    Cell[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        _rulesSet.interactable = true;
        _placeCells.interactable = false;
        _start.interactable = false;
        _stop.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            if (timer > _frameSpeed)
            {
                timer = 0f;
                CountAliveNeighbors();
                ControllingCellsLife();
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    void PlaceCells()
    {
        for(int y=0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                Cell cell = Instantiate(cellPrefab, new Vector2(x-(width/2), y-(height/2)), Quaternion.identity);
                grid[x, y] = cell;
                grid[x, y].SetAlive(RandomizeCellLivelihood());
            }
        }
    }

    bool RandomizeCellLivelihood()
    {
        int randomValue = UnityEngine.Random.Range(0, 100);
        if(randomValue > 60)
        {
            return true;
        }
        return false;
    }

    void CountAliveNeighbors()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int neighbors = 0;
                if (y + 1 < height)
                {
                    if (grid[x, y + 1].isAlive)
                    {
                        neighbors++;
                    }
                }
                if (x + 1 < width)
                {
                    if (grid[x + 1, y].isAlive)
                    {
                        neighbors++;
                    }
                }
                if (y - 1 >= 0)
                {
                    if (grid[x, y - 1].isAlive)
                    {
                        neighbors++;
                    }
                }
                if (x - 1 >= 0)
                {
                    if (grid[x - 1, y].isAlive)
                    {
                        neighbors++;
                    }
                }
                if (y + 1 < height && x + 1 < width)
                {
                    if (grid[x + 1, y + 1].isAlive)
                    {
                        neighbors++;
                    }
                }
                if (y + 1 < height && x - 1 >= 0)
                {
                    if (grid[x - 1, y + 1].isAlive)
                    {
                        neighbors++;
                    }
                }
                if (y - 1 >= 0 && x + 1 < width)
                {
                    if (grid[x + 1, y - 1].isAlive)
                    {
                        neighbors++;
                    }
                }
                if (y - 1 >= 0 && x - 1 >= 0)
                {
                    if (grid[x - 1, y - 1].isAlive)
                    {
                        neighbors++;
                    }
                }

                grid[x, y].neighbours = neighbors;
            }
        }
    }

    void ControllingCellsLife()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                
                if (grid[x, y].isAlive)
                {
                    //fewer than two live neighbors dies 
                    //more than three live neighbours dies
                    if (grid[x, y].neighbours < _underPopulationLimit || grid[x, y].neighbours > _overPopulationLimit)
                    {
                        grid[x, y].SetAlive(false);
                    }
                    //two or three live neighbours lives on to next generation
                    /****Nothing Happens*****/
                }
                else
                {
                    //dead cells with exactly 3 live cells gets to life
                    if(grid[x, y].neighbours == _overPopulationLimit)
                    {
                        grid[x, y].SetAlive(true);
                    }
                }
                
            }
        }
    }

    public void SetRules()
    {
        width = (int)_gridWidth.value;
        height = (int)_gridHeight.value;
        _underPopulationLimit = (int)_underPopulationUpperLimit.value;
        _overPopulationLimit = (int)_overPopulationLowerLimit.value;

        _placeCells.interactable = true;
        _rulesSet.interactable = false;
    }

    public void StartPlacement()
    {
        grid = new Cell[width, height];
        PlaceCells();
        _start.interactable = true;
        _placeCells.interactable = false;
    }

    public void StartGame()
    {
        isStarted = true;
        _stop.interactable = true;
    }

    public void StopGame()
    {
        isStarted = false;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
