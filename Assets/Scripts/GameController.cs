using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject enemy;
    public GameObject knife;
    public float spawnTime;
    private float tempSpawnTime;
    int m_score;
    bool m_isGameOver;
    void Start()
    {
        m_score = 0;
        tempSpawnTime = 0;
        setGameOver(false);
    }

    // Update is called once per frame
    void Update()
    {
        tempSpawnTime -= Time.deltaTime;
        if(tempSpawnTime <= 0)
        {
            if (Random.Range(0, 2) == 1)
                SpawnEnemy(9.28f, 2.32f);
            else
                SpawnEnemy(9.28f, -1.92f);
            tempSpawnTime = spawnTime;
        }
        if (m_isGameOver == true)
        {
            return;
        }
    }
    public void Replay()
    {
        m_isGameOver = false;
        m_score = 0;
    }
    public void SpawnEnemy(float x, float y)
    {
        Vector2 spawnPos = new Vector2(x, y);
        if (enemy)
        {
            Instantiate(enemy, spawnPos, Quaternion.identity);
        }
    }
    public void SpawnKnife(Vector2 spawnPos)
    {
        if (knife)
        {
            Instantiate(knife, spawnPos, Quaternion.identity);
        }
    }
    public void setScore(int val)
    {
        m_score = val;
    }
    public int getScore()
    {
        return m_score;
    }
    public void increateScore()
    {
        m_score++;
    }
    public void setGameOver(bool state)
    {
        m_isGameOver = state;
    }
    public bool getGameOver()
    {
        return m_isGameOver;
    }
}
