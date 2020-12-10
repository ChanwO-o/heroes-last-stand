using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum ENEMY {
    // NOTE
    // When writing levels, use these exact names
    waddle,
    ice_waddle,
    yellow_waddle
}

/// <summary>
/// This class is for managing a level
/// It takes in a level schema (a .json file) and outputs the levels accordingly
/// Ideally, levels will be loaded via pure JSON, but for now, PathNodes will be
/// planted statically, and the level manager will only be provided the Starts and End
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager ins;
    public UIManager UI;

    public List<PathNode> starts = null;
    public PathNode end;
    public float game_speed = 1.0f;
    public int player_health = 100;

    //gameover variables
    public int gamestate = 0; //1 = win //2 = lose
    public bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    // Win/Lose condition variables
    public bool LAST_ENEMY_SPAWNED = false;        // Last enemy, of last wave, has spawned (but NOT died)

    private string test_path = "Waves/test";


    // Variables for handling the wave. Might have
    // been better as an Object haha
    [SerializeField]
    public GameObject[] enemies = null;     // public array holding refs to prefabs
    public int enemy_count = 0;
    public float wave_spawn_speed = 1.5f;   // 1.5 seconds between spawns
    public int current_wave_i = -1;          // index of the current wave (out of N waves)
    public bool wave_in_progress = false;   // Must be manually changed to true to start the next wave

    private List<(ENEMY, int)> the_wave = null; // The current wave (waves are only loaded 1 at a time)
    private List<string> wave_strings = null;   // Raw strings. Each line = 1 wave
    private int wave_enemies_left = 0;          // For keeping track of how many are left to SPAWN
    private float tick = 0.0f;                  // internal var to keep track of spawn rate

    public int coin = 2000;
    public int health = 15;
    public int dead = 0;

    public event System.Action TakeDamage;


    // ====================
    // PUBLIC SETS AND GETS
    // ====================

    public int getCoin()
    {
        return coin;
    }
    public void setCoin(int coin) {
        this.coin = coin;
    }
    public int getHealth()
    {
        return health;
    }
    public void setHealth(int health)
    {
        this.health = health;
    }
    public int getDead()
    {
        return dead;
    }
    public void setDeath(int dead)
    {
        this.dead = dead;
    }
    public void incrDeathCount(){
        this.dead += 1;
        UI.setTextDeath(this.dead);
        enemy_count--;
    }
    public string getWaveString()
    {
        // Return a nicely formatted string
        // Ex: "2/5"
        int totalWaves = (int)wave_strings.Count;
        int thisWave = current_wave_i + 1;
        return System.String.Format("Waves: {0}/{1}", thisWave, totalWaves);
    }






    // Start is called before the first frame update
    void Start()
    {
        if (ins == null) {
            ins = this;
        }
        else {
            Destroy(this.gameObject);
        }

        wave_strings = ReadWavesFromText(test_path);
        Debug.Log("Wave 1: " + wave_strings[0]);


        // Initial UI setup
        UI.setTextCoins(coin);
        UI.setTextDeath(dead);
        UI.setTextWave( getWaveString() );
    }


    // Update is called once per frame
    void Update()
    {
        CheckInput();
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

        UpdateUI();
    }


    ///
    /// === PUBLIC ===
    ///

    /// <summary>
    /// By default, our game has to be TOLD to manually start the next wave
    /// </summary>
    public void StartNextWave()
    {
        if(LAST_ENEMY_SPAWNED){ return; }
        
        // Handle incrementing wave, and other things here
        // (I just feel like this will be important)
        current_wave_i++;
        wave_in_progress = true;
        the_wave = loadWave(wave_strings[current_wave_i]);
        UI.setTextWave( getWaveString() );

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
        //pauseMenuUI.SetActive(true);
        UI.displayPauseMenu(true, gamestate);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        Debug.Log("RESUMED");
        //pauseMenuUI.SetActive(false);
        UI.displayPauseMenu(false, gamestate);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    /// <summary>
    /// Have the enemies call this when they reach the last node
    /// </summary>
    public void endReached(){
        // NOTE: In the future, we can add things like displaying a message
        // "Oh no, an enemy got past" or something
        player_health -= 1;

        if (TakeDamage != null)
        {
            TakeDamage();
        }
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
        

        if (LAST_ENEMY_SPAWNED &&  enemy_count == 0) {

            // WIN conditions:
            //  ALL enemies are dead
            //  But you might have more enemies, or more waves SO
            //  Check if the last enemy has spawned yet.
            //  THEN its game over
            Debug.Log("YOU WON");
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
        if (wave_in_progress && !GameIsPaused)
        {
            tick += Time.deltaTime * game_speed;
            if(tick < wave_spawn_speed){
                return;
            }
            // reset the ticker
            tick = 0.0f;

            // spawn enemy
            enemy_count++;
            GameObject new_enemy = Instantiate(
                enemies[(int)the_wave[0].Item1 ], // grab the corresponding enemy using its enum -> int
                starts[0].transform.position,
                Quaternion.identity
            );
            new_enemy.GetComponent<PathFollower>().target = starts[0];

            // decrement!
            the_wave[0] = (the_wave[0].Item1, the_wave[0].Item2 - 1); // Immutable workaround for the_wave[0][1]--
            if( the_wave[0].Item2 == 0){
                the_wave.RemoveAt(0); // If we're out of enemies, pop the empty tuple
            }
            wave_enemies_left--;


            // check if this is the last wave!
            if (wave_enemies_left == 0)
            {
                // end of the wave, load the next one
                wave_in_progress = false;

                if (current_wave_i == wave_strings.Count - 1)
                {
                    // the last wave just finished!
                    // Cue, end of the level?
                    wavesOver();
                    return;
                }else{
                    // If it wasn't the last wave, DONT load the next:
                    // Instead wait for StartNextWave() to be called
                    
                    //the_wave = loadWave(wave_strings[current_wave_i]);
                }
            }
        }
    }


    // Logic for when the gamestate != 0
    private void gameOver()
    {
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


    // Handle any input like hotkeys or menu access here
    private void CheckInput(){
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESCAPED");
            if(GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    /// <summary>
    /// Update any UI related state here
    /// </summary>
    private void UpdateUI(){
        // We want to DISABLE the button when wave is in progress
        // Keep disabled if the last enemy has spawned
        UI.startWaveEnable(!wave_in_progress && !LAST_ENEMY_SPAWNED);
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
            //wave_enemies_left += System.Convert.ToInt32(str_arr[i]);
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
        var txt = Resources.Load(path); // Need this for final build
        var str = txt.ToString().Split('\n');
        List<string> waves = new List<string>();

        // Filter out comments;
        for (int i = 0; i < str.Length; i++){
            if(str[i][0] != '#'){
                waves.Add(str[i]);
            }
        }

        Debug.Log("Waves Loaded: " + waves[0]);
        return waves;
    }
}
