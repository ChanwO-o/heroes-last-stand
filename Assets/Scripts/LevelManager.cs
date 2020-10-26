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
        // according to the timer, instantiate enemies.
        // Best way to keep track of enemies/spawn them?

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
    /// Takes a path and returns a list of waves as strings
    /// </summary>
    List<string> ReadWavesFromText(string path)
    {
        //Read the text from directly from the test.txt file
        var string_array = File.ReadAllLines(path);
        List<string> waves = new List<string>(string_array);
        return waves;
    }


    // Using the supplied string
    // Creates a new wave
    // [(enemy_enum, int),]
    List<(ENEMY, int)> loadWave(string wave_string)
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



    void wavesOver()
    {
        // Just used to handle any logic needed when the last enemy is spawned
        Debug.Log("Last enemy has spawned!");
    }
}
