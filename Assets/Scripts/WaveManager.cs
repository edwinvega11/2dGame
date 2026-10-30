using UnityEngine;
using UnityEngine.UI;

// spawn enemies and move through the ten waves
public class WaveManager : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Enemy enemyPrefab;
    public PlayerHealth player;
    public Transform[] spawnPoints;
    public float secondsBetweenSpawns = 0.8f;
    public int totalWaves = 10;
    public Text waveText;
    public GameObject waveClearPanel;
    public Text waveClearText;
    public GameObject victoryPanel;
    public UpgradeShop shop;

    public int CurrentWave { get; private set; }
    public int EnemiesRemaining { get; private set; }
    public bool WaveActive { get; private set; }
    public bool HasWon { get; private set; }

    int enemiesToSpawn;
    float nextSpawnTime;
    bool clearingWave;
    float clearTime;

    void Start()
    {
        waveClearPanel.SetActive(false);
        victoryPanel.SetActive(false);
        StartNextWave();
    }

    void Update()
    {
        if (clearingWave && Time.unscaledTime >= clearTime)
        {
            clearingWave = false;
            FinishWave();
        }
        if (!WaveActive || player.IsDead || Time.timeScale == 0f) return;
        if (enemiesToSpawn > 0 && Time.time >= nextSpawnTime)
        {
            Enemy enemy = Instantiate(enemyPrefab, PickSpawnPoint(), Quaternion.identity);
            enemy.name = "Creep";
            enemy.player = player.transform;
            enemy.waveManager = this;
            // increase speed each wave and health every other wave
            enemy.moveSpeed = enemyPrefab.moveSpeed + (CurrentWave - 1) * 0.12f;
            enemy.health = enemyPrefab.health + (CurrentWave - 1) / 2;
            enemiesToSpawn--;
            nextSpawnTime = Time.time + secondsBetweenSpawns;
        }
    }

    Vector3 PickSpawnPoint()
    {
        // avoid spawning right beside the player
        int start = Random.Range(0, spawnPoints.Length);
        Transform farthest = spawnPoints[start];
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform point = spawnPoints[(start + i) % spawnPoints.Length];
            float distance = Vector3.Distance(point.position, player.transform.position);
            if (distance >= 4f) return point.position;
            if (distance > Vector3.Distance(farthest.position, player.transform.position)) farthest = point;
        }
        return farthest.position;
    }

    public void StartNextWave()
    {
        if (WaveActive || clearingWave || player.IsDead || HasWon) return;
        waveClearPanel.SetActive(false);
        CurrentWave++;
        // start with three creeps and add two each wave
        EnemiesRemaining = 3 + (CurrentWave - 1) * 2;
        enemiesToSpawn = EnemiesRemaining;
        nextSpawnTime = Time.time + 1f;
        WaveActive = true;
        Time.timeScale = 1f;
        UpdateWaveDisplay();
        shop.RefreshDisplay();
    }

    public void EnemyDefeated()
    {
        if (!WaveActive || player.IsDead) return;
        shop.AddSoul();
        EnemiesRemaining--;
        UpdateWaveDisplay();
        // the count includes enemies that have not spawned yet
        if (EnemiesRemaining > 0) return;

        WaveActive = false;
        foreach (Bullet bullet in FindObjectsByType<Bullet>()) Destroy(bullet.gameObject);
        Time.timeScale = 0f;
        // give the final soul time to float up before showing the choices
        clearingWave = true;
        clearTime = Time.unscaledTime + 0.7f;
    }

    void FinishWave()
    {
        if (player.IsDead) return;
        if (CurrentWave == totalWaves)
        {
            HasWon = true;
            GameAudio.Instance?.Win();
            victoryPanel.SetActive(true);
        }
        else
        {
            player.HealFully();
            GameAudio.Instance?.ClearWave();
            waveClearText.text = "Your souls obtained have granted you an upgrade";
            waveClearPanel.SetActive(true);
            shop.RefreshDisplay();
        }
    }

    void UpdateWaveDisplay()
    {
        waveText.text = "WAVE " + CurrentWave + " / " + totalWaves + "   |   CREEPS LEFT: " + EnemiesRemaining;
    }
}
