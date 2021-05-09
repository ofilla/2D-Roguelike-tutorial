using System;
using System.Collections.Generic; // Allows us to use Lists.
using UnityEngine;
using Random = UnityEngine.Random; // Tells Random to use the Unity Engine random number generator.

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count (int min, int max) {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8; // number of columns for playable field
    public int rows = 8; // number of rows for playable field
    public Count wallCount = new Count(5,9); // limits for walls on field
    public Count foodCount = new Count(1,5); // limits for food items on field
    // prefabs
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder; // A variable to store a reference to the transform of our Board object.
    private List<Vector3> gridPositions = new List<Vector3>(); // A list of possible locations to place tiles.

    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        int enemyCount = (int) Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        CreateExit();
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < columns + 1; y++)
            {
                GameObject toInstantiate;
                if (IsInOuterWall(x,y))
                {
                    toInstantiate = PickAtRandom(outerWallTiles);
                } else {
                    toInstantiate = PickAtRandom(floorTiles);
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    void InitializeList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum+1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject chosenTile = PickAtRandom(tileArray);
            Instantiate(chosenTile, randomPosition, Quaternion.identity);
        }
    }

    private void CreateExit() {
        Vector3 fixedPosition = new Vector3(columns-1, rows-1, 0f);
        Instantiate(exit, fixedPosition, Quaternion.identity);
    }

    Vector3 RandomPosition()
    {
        // return a random position, remove any existing items there
        int randomIndex = Random.Range(0, gridPositions.Count);

        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    private bool IsInOuterWall(int x, int y)
    {
        return x == -1 || x == columns || y == -1 || y == columns;
    }

    private GameObject PickAtRandom(GameObject[] list)
    {
        int idx = Random.Range(0, list.Length);
        return list[idx];
    }
}
