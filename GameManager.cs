using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    //public TextMeshProUGUI gameOverText;
    //public GameObject tittleScreen;

    private float spawnRangeX1 = -30.0f;
    private float spawnRangeX2 = -3.0f;
    private float spawnRangeZ1 = 110.0f;
    private float spawnRangeZ2 = 35.0f;

    public GameObject enemyPrefab;
    public bool isGameActive;
    public int enemyCount;
    private int waveNumber = 1;


    // Start is called before the first frame update
    void Start()
    {
        isGameActive= true;
        
    }

    // Update is called once per frame
    void Update()
    {

        StartCoroutine(SpawnE());
    }
    IEnumerator SpawnE()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(0.5f);
            Spawner();
        }
    }

    void SpawnEnemies(int waveNumber)
    {
        for (int i = 0; i < waveNumber; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(spawnRangeX1, spawnRangeX2), 5, Random.Range(spawnRangeZ1, spawnRangeZ2));
            Instantiate(enemyPrefab, spawnPos, enemyPrefab.transform.rotation);
        }

    }



    public void Spawner()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0)
        {
            SpawnEnemies(waveNumber);
            waveNumber++;

        }
    }
}
