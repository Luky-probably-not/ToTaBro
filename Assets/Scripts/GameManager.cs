using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Prefabs")]
    public GameObject accueilUIPrefab;
    public GameObject gameScenePrefab1;
    public GameObject gameScenePrefab2;
    private GameObject selectedGameScenePrefab;

    [Header("Normals Enemies Prefabs")]
    public GameObject botPrefab; 
    public GameObject speedyPrefab;
    public GameObject tankPrefab;
    public GameObject rangedPrefab;

    [Header("Boss Enemies Prefabs")]
    public GameObject slimeyPrefab;

    [Header("Weapon Prefabs")]
    public GameObject gunniePrefab;
    public GameObject laserPrefab;
    public GameObject shotgunPrefab;
    public GameObject swordPrefab;

    [Header("Player Prefabs")]
    public GameObject playerPrefab;

    private GameObject currentUI;
    private int currentWave = 1;
    private int enemiesAlive;
    private List<GameObject> normalEnemies = new List<GameObject>();
    private List<GameObject> weapons = new List<GameObject>();
    private GameObject currentWeapon;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
        }
        else
        {
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        LoadAccueil();
    }

    public void LoadAccueil()
    {
        currentWave = 1;
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
            currentWeapon = null;
        }
        selectedGameScenePrefab = null;
        LoadScene(accueilUIPrefab);
    }

    public void StartGame()
    {
        selectedGameScenePrefab = (Random.value < 0.5f) ? gameScenePrefab1 : gameScenePrefab2;

        LoadScene(selectedGameScenePrefab);
        if (playerPrefab != null)
        {
            Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }

        normalEnemies.Clear();
        normalEnemies.Add(botPrefab);
        normalEnemies.Add(speedyPrefab);
        normalEnemies.Add(tankPrefab);
        normalEnemies.Add(rangedPrefab);

        weapons.Clear();
        weapons.Add(laserPrefab);
        weapons.Add(shotgunPrefab);
        weapons.Add(swordPrefab);
        currentWeapon = Instantiate(gunniePrefab, Vector3.zero, Quaternion.identity);

        currentWave = 1;
        StartCoroutine(SpawnWave());
    }

    public void LoadScene(GameObject scenePrefab)
    {
        if (currentUI != null) Destroy(currentUI);
        currentUI = Instantiate(scenePrefab, FindObjectOfType<Canvas>().transform);
    }

    private IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(1f);

        if (currentWave > 50)
        {
            LoadAccueil();
            yield break;
        }

        enemiesAlive = 0;

        if (currentWave % 10 == 0)
        {
            Instantiate(slimeyPrefab, GetRandomPosition(), Quaternion.identity);
            enemiesAlive = 1;
        }
        else
        {
            int enemyCount = 2 + (currentWave - 1);
            int maxEnemyTypes = Mathf.Min(currentWave, normalEnemies.Count);

            for (int i = 0; i < enemyCount; i++)
            {
                GameObject enemyPrefab = normalEnemies[Random.Range(0, maxEnemyTypes)];
                Instantiate(enemyPrefab, GetRandomPosition(), Quaternion.identity);
                enemiesAlive++;
            }
        }
    }

    public void EnemyDefeated()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            if (currentWave % 10 == 0)
            {
                DropNewWeapon();
            }

            currentWave++;
            StartCoroutine(SpawnWave());
        }
    }

    private void DropNewWeapon()
    {
        List<GameObject> availableWeapons = weapons.FindAll(w => w.name != currentWeapon.name.Replace("(Clone)", ""));

        if (availableWeapons.Count > 0)
        {
            GameObject newWeapon = availableWeapons[Random.Range(0, availableWeapons.Count)];
            Destroy(currentWeapon);
            currentWeapon = Instantiate(newWeapon, Vector3.zero, Quaternion.identity);
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomPosition = Vector3.zero;
        bool validPosition = false;

        while (!validPosition)
        {
            float x = Random.Range(-18f, 18f); 
            float y = Random.Range(-18f, 18f); 

            if (Mathf.Abs(x) > 2f || Mathf.Abs(y) > 2f)
            {
                randomPosition = new Vector3(x, y, 0f);
                validPosition = true;
            }
        }

        return randomPosition;
    }
}
