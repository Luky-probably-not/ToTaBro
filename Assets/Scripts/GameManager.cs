using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Prefabs")]
    public GameObject accueilUIPrefab;
    public GameObject GameOverUIPrefab;
    public GameObject CreditsUIPrefab;
    public GameObject gameScenePrefab;
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
    private bool inGame = false;

    [Header("Boss Enemies Prefabs")]
    public GameObject slimePrefab;
    public GameObject gunnerPrefab;
    public GameObject slurpPrefab;

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
    public GameObject goldPotionPrefab;
    public GameObject xpPotionPrefab;

    [Header("Player Prefabs")]
    public GameObject playerPrefab;

    [Header("Merchant")]
    public GameObject merchantPrefab;
    public Transform merchantSpawnPoint;
    private Vector3[] itemSpawnParent = new Vector3[3];
    private List<GameObject> currentMerchantItems = new List<GameObject>();
    private GameObject currentMerchantInstance;
    
    public void setInGame(bool set) {
        inGame = set;
    }
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
        itemSpawnParent[0] = new Vector3(-5f, 0f, 0f);
        itemSpawnParent[1] = new Vector3(0f, 0f, 0f);
        itemSpawnParent[2] = new Vector3(5f, 0f, 0f);
        LoadAccueil();
    }

    void Update()
    {
        if (inGame)
        {
            GameObject existingPlayer = GameObject.FindGameObjectWithTag("Player");
            if (existingPlayer == null)
            {
                setInGame(false);
                GameOver();
            }
        }
    }
    public void GameOver() 
    {
        Reset();
        LoadScene(GameOverUIPrefab);
        StartCoroutine(WaitThenLoadAccueil());
    }

    private IEnumerator WaitThenLoadAccueil()
    {
        yield return new WaitForSeconds(5);
        LoadAccueil();
    }
    public void LoadAccueil()
    {
        LoadScene(accueilUIPrefab);
        
    }
    public void Reset()
    {
        // Réinitialisation des vagues
        currentWave = 1;

        // Suppression du joueur
        GameObject existingPlayer = GameObject.FindGameObjectWithTag("Player");
        if (existingPlayer != null)
        {
            Destroy(existingPlayer);
        }

        // Suppression des ennemis
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Ennemy");
        if (enemies.Length > 0)
        {
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
        }
        enemiesAlive = 0;

        // Suppression des armes
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

        // Suppression de l'XP
        GameObject[] xpOrbs = GameObject.FindGameObjectsWithTag("Xp");
        if (xpOrbs.Length > 0)
        {
            foreach (GameObject xp in xpOrbs)
            {
                Destroy(xp);
            }
        }

        // Suppression de l'argent
        GameObject[] goldCoins = GameObject.FindGameObjectsWithTag("Coin");
        if (goldCoins.Length > 0)
        {
            foreach (GameObject gold in goldCoins)
            {
                Destroy(gold);
            }
        }

        // Réinitialisation de la map
        selectedGameScenePrefab = null;
    }

    public void StartGame()
    {
        setInGame(true);
        selectedGameScenePrefab = gameScenePrefab;

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
        currentWeapon.GetComponent<Weapon>().LevelUp(currentWave);
        currentWeapon.GetComponent<Weapon>().LevelUp(currentWave);

        currentWave = 1;
        StartCoroutine(SpawnWave(1f));
    }

    public void LoadScene(GameObject scenePrefab)
    {
        if (currentUI != null) Destroy(currentUI);
        currentUI = Instantiate(scenePrefab,  FindFirstObjectByType<Canvas>().transform);
    }
    public void TogglePause()
    {
        if (Time.timeScale == 1f)
        {
            if (pauseUIPrefab != null)
            {
                Debug.Log(currentUI);
                Instantiate(pauseUIPrefab,  FindFirstObjectByType<Canvas>().transform);
            }
        }
    }
    private void TriggerMerchant()
    {
        setInGame(false);
        currentMerchantInstance = Instantiate(merchantPrefab, merchantSpawnPoint.position, Quaternion.identity);
    }
    private IEnumerator SpawnWave(float wait)
    {
        int enemyCount = 2;
        if (inGame) {
            yield return new WaitForSeconds(wait);

            if (currentWave > 50)
            {
                LoadAccueil();
                yield break;
            }

            enemiesAlive = 0;

            if (currentWave % 10 == 0)
            {
                if (currentWave == 50) {
                    Instantiate(slurpPrefab, GetRandomPosition(), Quaternion.identity);
                }
                else 
                {
                    GameObject chosenPrefab = Random.value < 0.5f ? slimePrefab : gunnerPrefab;
                    Instantiate(chosenPrefab, GetRandomPosition(), Quaternion.identity);
                }
                enemiesAlive = 1;
            }
            else if (currentWave % 5 == 0)
            {
                if (currentWave % 10 == 0)
                {
                    if (currentWave == 50) {
                        Instantiate(slurpPrefab, GetRandomPosition(), Quaternion.identity);
                    }
                    else 
                    {
                        GameObject chosenPrefab = Random.value < 0.5f ? slimePrefab : gunnerPrefab;
                        Instantiate(chosenPrefab, GetRandomPosition(), Quaternion.identity);
                    }
                    enemiesAlive = 1;
                }
                TriggerMerchant();
            }
            else
            {
                if (currentWave % 3 == 0) {
                    enemyCount++;
                }
                int maxEnemyTypes = Mathf.Min(currentWave, normalEnemies.Count);

                for (int i = 0; i < enemyCount; i++)
                {
                    GameObject enemyPrefab = normalEnemies[Random.Range(0, maxEnemyTypes)];
                    GameObject enemyTmp = Instantiate(enemyPrefab, GetRandomPosition(), Quaternion.identity);
                    enemyTmp.GetComponent<Ennemy>().Evoluate(currentWave);
                    enemiesAlive++;
                }
            }
        }
    }

    public void EnemyDefeated(float minus)
    {
        enemiesAlive=enemiesAlive-minus;
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
            }
            StartCoroutine(SpawnWave(3f));
        }
    }
    public void SetDroppedWeapon(GameObject weapon)
    {
        droppedWeapon = weapon;
    }
    private void DropNewWeapon()
    {
        if (weapons.Count > 0)
        {
            GameObject newWeapon = weapons[Random.Range(0, availableWeapons.Count)];
            droppedWeapon = Instantiate(newWeapon, Vector3.right * 2f, Quaternion.identity);
            droppedWeapon.GetComponent<Weapon>().LevelUp(currentWave);
            dropWeaponWave = currentWave;
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

    public void EnterMerchant(Merchant merchant)
    {
        
        currentMerchantItems.Clear();
        GameObject potion = Instantiate(healthPotionPrefab, itemSpawnParent[0], Quaternion.identity);
        currentMerchantItems.Add(potion);
        GameObject weapon = weapons[Random.Range(0, weapons.Count)];
        GameObject weaponObj = Instantiate(weapon);
        weaponObj.GetComponent<Weapon>().LevelUp(currentWave);
        currentMerchantItems.Add(weaponObj);
        bool giveWeapon = Random.value < 0.5f;
        GameObject randObj = giveWeapon ? weapons[Random.Range(0, weapons.Count)] : GetRandomPotion();
        GameObject randItem = Instantiate(randObj, itemSpawnParent[2], Quaternion.identity);
        if(giveWeapon)
        {
            randItem.GetComponent<Weapon>().LevelUp(currentWave);
        }
        currentMerchantItems.Add(randItem);
    }
    public void ExitMerchant()
    {
        foreach (GameObject item in currentMerchantItems)
        {
            if (item != null)
                Destroy(item);
        }

        currentMerchantItems.Clear();
        if (currentMerchantInstance != null)
        {
            Destroy(currentMerchantInstance);
        }
        setInGame(true);
        currentWave +=1;
        StartCoroutine(SpawnWave(3f));
    }
    public void removeFromMarchent(GameObject item)
    {
        if (currentMerchantItems.Contains(item))
        {
            currentMerchantItems.Remove(item);
        }
    }
    private GameObject GetRandomPotion()
    {
        int rand = Random.Range(0, 5);
        switch (rand)
        {
            case 0: return healthPotionPrefab;
            case 1: return speedPotionPrefab;
            case 2: return goldPotionPrefab;
            case 3: return xpPotionPrefab;
            default: return fireRatePotionPrefab;
        }
    }

    public void Credits()
    {
        LoadScene(CreditsUIPrefab);
    }

}
