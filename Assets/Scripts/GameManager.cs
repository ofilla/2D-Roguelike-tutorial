using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public BoardManager boardScript;
    public int firstLevel = 3; // start with 3 because first enemy appears here
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true; // TODO is this an annotation?

    private int level;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this) {
            Destroy(this);
        }
        DontDestroyOnLoad(this);

        level = firstLevel;
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        boardScript.SetupScene(level);
    }

    public void GameOver() {
        enabled = false;
    }
}
