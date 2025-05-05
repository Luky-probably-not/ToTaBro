using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Prefabs")]
    public GameObject accueilUIPrefab;
    public GameObject gameScenePrefab1;
    public GameObject gameScenePrefab2;
    private GameObject selectedGameScenePrefab;
    public GameObject pauseUIPrefab;
    private GameObject currentUI;

    [Header("Normals Enemies Prefabs")]
    public GameObject botPrefab; 
    public GameObject speedyPrefab;
    public GameObject tankPrefab;
    public GameObject rangedPrefab;
    private float enemiesAlive;
    private int currentWave = 1;
    private List<GameObject> normalEnemies = new List<GameObject>();

    [Header("Boss Enemies Prefabs")]
    public GameObject slimeyPrefab;

    [Header("Weapon Prefabs")]
    public GameObject gunniePrefab;
    public GameObject laserPrefab;
    public GameObject shotgunPrefab;
    public GameObject swordPrefab;
    private GameObject droppedWeapon;
    private int dropWeaponWave = -1;
    private List<GameObject> weapons = new List<GameObject>();
    private GameObject currentWeapon;
    [Header("Potion Prefabs")]
    public GameObject healthPotionPrefab;
    public GameObject speedPotionPrefab;
    public GameObject fireRatePotionPrefab;

    [Header("Player Prefabs")]
    public GameObject playerPrefab;
    

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
        LoadAccueil(0);
    }

    public void LoadAccueil(int accueil)
    {
        if (accueil!=0){
            Reset();
        }
        LoadScene(accueilUIPrefab);
    }
    public void Reset() {
        //réinitialisation des vagues
        currentWave = 1;
        //suppression du joueur
        GameObject existingPlayer = GameObject.FindGameObjectWithTag("Player");
        if (existingPlayer != null)
        {
            Destroy(existingPlayer);
        }
        //suppression des ennemies
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
        enemiesAlive = 0;
        // supression des armes
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
            currentWeapon = null;
        }
        if (droppedWeapon != null)
        {
            Destroy(droppedWeapon);
            droppedWeapon = null;
        }
        //suppression de l'exp
        foreach (var xp in GameObject.FindGameObjectsWithTag("XP"))
        {
            Destroy(xp);
        }
        // supression de l'argent
        foreach (var gold in GameObject.FindGameObjectsWithTag("Gold"))
        {
            Destroy(gold);
        }
        //reinitialisation de la map
        selectedGameScenePrefab = null;
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
        StartCoroutine(SpawnWave(1f));
    }

    public void LoadScene(GameObject scenePrefab)
    {
        if (currentUI != null) Destroy(currentUI);
        currentUI = Instantiate(scenePrefab, FindObjectOfType<Canvas>().transform);
    }
    public void TogglePause()
    {
        if (Time.timeScale == 1f)
        {
            if (pauseUIPrefab != null)
            {
                Debug.Log(currentUI);
                Instantiate(pauseUIPrefab, FindObjectOfType<Canvas>().transform);
            }
        }
    }

    private IEnumerator SpawnWave(float wait)
    {
        yield return new WaitForSeconds(wait);

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
        Debug.Log("Vague " + currentWave);
    }

    public void EnemyDefeated(float minus)
    {
        enemiesAlive=enemiesAlive-minus;
        Debug.Log("Enemy defeated. Restants: " + enemiesAlive);
        if (enemiesAlive <= 0)
        {
            if (currentWave % 10 == 0)
            {
                DropNewWeapon();
            }

            currentWave++;
            if (droppedWeapon != null && currentWave >= dropWeaponWave + 3)
            {
                Destroy(droppedWeapon);
                droppedWeapon = null;
                Debug.Log("Arme dropée non ramassée détruite après 3 vagues.");
            }
            StartCoroutine(SpawnWave(3f));
        }
    }
    public void SetDroppedWeapon(GameObject weapon)
    {
        droppedWeapon = weapon;
        Debug.Log("Arme déposée : " + weapon.name);
    }
    private void DropNewWeapon()
    {
        List<GameObject> availableWeapons = weapons.FindAll(w => w.name != currentWeapon.name.Replace("(Clone)", ""));
        if (availableWeapons.Count > 0)
        {
            GameObject newWeapon = availableWeapons[Random.Range(0, availableWeapons.Count)];
            droppedWeapon = Instantiate(newWeapon, Vector3.right * 2f, Quaternion.identity);
            dropWeaponWave = currentWave;

            Debug.Log("Nouvelle arme dropée : " + newWeapon.name);
        }
    }

    private Vector3 GetRandomPosition()
    {
        Camera cam = Camera.main;
        Vector3 randomPosition = Vector3.zero;
        bool validPosition = false;

        while(!validPosition)
        {
            float x = Random.Range(0.1f, 0.9f);
            float y = Random.Range(0.1f, 0.9f);
            Vector3 screenPos = new Vector3(x * Screen.width, y * Screen.height, cam.nearClipPlane);
            Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
            worldPos.z = 0;

            if (Mathf.Abs(worldPos.x) > 2f || Mathf.Abs(worldPos.y) > 2f)
            {
                randomPosition = worldPos;
                validPosition = true;
            }
        }

        return randomPosition;
    }
}
