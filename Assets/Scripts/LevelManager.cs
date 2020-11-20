using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum ENEMY {
    waddle,
    ice_waddle
}

/// <summary>
/// This class is for managing a level
/// It takes in a level schema (a .json file) and outputs the levels accordingly
/// Ideally, levels will be loaded via pure JSON, but for now, PathNodes will be
/// planted statically, and the level manager will only be provided the Starts and End
/// </summary>
public class LevelManager : MonoBehaviour
{
    public List<PathNode> starts = null;
    public PathNode end;
    public bool paused = false;
    public float game_speed = 1.0f;
    public int player_health = 1;

    //gameover variables
    public int gamestate = 0; //1 = win //2 = lose
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    // Win/Lose condition variables
    private bool LAST_ENEMY_SPAWNED = false;        // Last enemy, of last wave, has spawned (but NOT died)

    private string test_path = "Assets/Waves/test.txt";


    private List<string> wave_strings = null;
    public int current_wave_i = 0;
    public List<(ENEMY, int)> the_wave = null;
    public bool wave_in_progress = false;
    [SerializeField]
    public GameObject[] enemies = null;
    private int wave_enemies_left = 0;
    public float wave_spawn_speed = 1.5f; // 1.5 seconds between spawns
    private float tick = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
        wave_strings = ReadWavesFromText(test_path);
        Debug.Log("Wave 1: " + wave_strings[0]);

        // In the future, populate the enemies dict
        // programatically

        // populate the first wave
        the_wave = loadWave(wave_strings[current_wave_i]);
    }



    // Update is called once per frame
    void Update()
    {
        // 1. Check/transition gamestate
        checkGameState();

        // 2. Act based off new gamestate
        switch(gamestate){
            case 0: // game not over
                gameNotOver();
                break;
            case 1: // player WON
                gameOver();
                break;
            case 2: // player LOST
                gameOver(); // for now, in the future we might have a diff call for losing
                break;
        }
    }


    ///
    /// === PUBLIC ===
    ///

    /// <summary>
    /// By default, our game has to be TOLD to manually start the next wave
    /// </summary>
    public void StartNextWave()
    {
        // Handle incrementing wave, and other things here
        // (I just feel like this will be important)
        current_wave_i++;
        wave_in_progress = true;

        // Count the number of enemies so we can display
        foreach(var tuple in the_wave){
            wave_enemies_left += tuple.Item2;
        }
    }


    /// <summary>
    /// Use to pause the game, prevent spawning and stuff
    /// </summary>
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    /// <summary>
    /// Have the enemies call this when they reach the last node
    /// </summary>
    public void endReached(){
        // NOTE: In the future, we can add things like displaying a message
        // "Oh no, an enemy got past" or something
        player_health -= 1;
    }



    ///
    /// === PRIVATE ===
    ///

    // Step 1 of teh state machine
    private void checkGameState()
    {
        // What are our conditions for:
        // - winning
        // - losing
        // - neither?
        

        if (LAST_ENEMY_SPAWNED && enemies.Length == 0) {

            // WIN conditions:
            //  ALL enemies are dead
            //  But you might have more enemies, or more waves SO
            //  Check if the last enemy has spawned yet.
            //  THEN its game over
            gamestate = 1;

        }else if(player_health == 0){
            // LOSE conditions:
            //  Player's health is down to 0
            //  Anything else?
            gamestate = 2;
        }else{
            // If neither, then the game goes on!
            gamestate = 0;
        }
    }


    // Logic for when the gamestate == 0
    private void gameNotOver(){
        
        // We DONT want to do anything if the game is:
        // Paused, in progress
        if (wave_in_progress && !paused)
        {
            tick += Time.deltaTime;
            if(tick < wave_spawn_speed){
                return;
            }
            // reset the ticker
            tick = 0.0f;

            // spawn enemy
            GameObject new_enemy = Instantiate(
                enemies[(int)the_wave[0].Item1 ], // grab the corresponding enemy using its enum -> int
                starts[0].transform.position,
                Quaternion.identity
            );
            new_enemy.GetComponent<PathFollower>().target = starts[0];

            // decrement!
            the_wave[0] = (the_wave[0].Item1, the_wave[0].Item2 - 1); // Tuples are immutable so here we are
            if( the_wave[0].Item2 == 0){
                the_wave.RemoveAt(0); // If we're out of enemies, pop the empty tuple
            }
            wave_enemies_left--;
            Debug.Log("Enemies Left: " + wave_enemies_left.ToString());


            // check if this is the last wave!
            if (wave_enemies_left == 0)
            {
                // end of the wave, load the next one
                wave_in_progress = false;
                current_wave_i++;

                if (current_wave_i == wave_strings.Count)
                {
                    // the last wave just finished!
                    // Cue, end of the level?
                    wavesOver();
                    return;
                }else{
                    // If it wasn't the last wave, load the next
                    the_wave = loadWave(wave_strings[current_wave_i]);
                }
            }
        }
    }


    // Logic for when the gamestate != 0
    private void gameOver()
    {
        Debug.Log("game over");
        if(!GameIsPaused)
        {
            Pause();
        }
    }



    // Meant to be called only when ALL waves are spawned
    // NOTE enemies are PROBABLY still alive at this point
    // This is DIFFERENT from all enemies being dead
    private void wavesOver()
    {
        // Just used to handle any logic needed when the last enemy is spawned
        LAST_ENEMY_SPAWNED = true;
        Debug.Log("Last enemy has spawned!");
    }







    ///
    /// === UTILITY & HELPER FUNCTIONS ===
    ///

    // Using the supplied string
    // Creates a new wave
    // [(enemy_enum, int),]
    private List<(ENEMY, int)> loadWave(string wave_string)
    {
        // Make a new dict [{enemy_name: count}]
        List<(ENEMY, int)> new_wave = new List<(ENEMY, int)>();
        var str_arr = wave_string.TrimEnd().Split(' ');

        // Load the data from the strings we split
        // [count1, enemy1, count2, enemy2, ... ]
        for (int i = 0; i < str_arr.Length; i += 2)
        {
            wave_enemies_left += System.Convert.ToInt32(str_arr[i]);
            new_wave.Add((
                (ENEMY) System.Enum.Parse( typeof(ENEMY) ,str_arr[i + 1] ),
                System.Convert.ToInt32(str_arr[i])
            ));
        }

        // Finally, return the waves
        return new_wave;
    }

    /// Takes a path and returns a list of waves as strings
    private List<string> ReadWavesFromText(string path)
    {
        //Read the text from directly from the test.txt file
        var string_array = File.ReadAllLines(path);
        List<string> waves = new List<string>(string_array);
        return waves;
    }
}
