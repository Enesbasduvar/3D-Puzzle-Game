using System.Net.NetworkInformation;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    AudioManager audioManager;
    public GameObject gate;
    public GameObject rank1boxPuzzle;
    public GameObject rank1boxPuzzlePlatform;
    public GameObject rank1leverPuzzle;
    public GameObject puzzleChecker;
    public GameObject willBeDestroyed1;
    public GameObject willBeDestroyed2;
    public GameObject willBeDestroyed3;
    public GameObject willBeDestroyed4;
    public GameObject rank1simonPuzzle;
    Transform gatePos;
    Vector3 spawnPoint;
    Vector3 spawnPoint2;
    Vector3 spawnPointWall1;
    Vector3 spawnPointWall2;
    Vector3 spawnPointWall3;
    Vector3 spawnPointWall4;
    
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void destroyPlatform()
    {
        Destroy(willBeDestroyed2);
    }

    public void generateFloor()
    {
        audioManager.PlaySFX(audioManager.lightswitch);
        audioManager.PlaySFXinLoop(audioManager.buzzing);
        GameObject temp = Instantiate(gate, spawnPoint, Quaternion.identity);
        spawnPoint = temp.transform.GetChild(1).transform.position;
        gatePos = gate.transform;
        spawnPoint2 = temp.transform.GetChild(2).transform.position;
        spawnPointWall1 = temp.transform.GetChild(5).transform.position;
        spawnPointWall2 = temp.transform.GetChild(6).transform.position;
        spawnPointWall3 = temp.transform.GetChild(7).transform.position;
        spawnPointWall4 = temp.transform.GetChild(8).transform.position;
        Vector3 randomPos1 = new Vector3(Random.Range(-6, 6), -4 , Random.Range(-6, 6));
        Vector3 randomPos2 = new Vector3(-randomPos1.x, -4.9f , -randomPos1.z);
        Vector3 wall1SpawnZone = new Vector3(0.15f, 0 , Random.Range(-13, 13));
        Vector3 wall3SpawnZone = new Vector3(-0.15f, 0 , Random.Range(-13, 13));
        Quaternion wall3Rotation = Quaternion.Euler(0, 180, 0);
        willBeDestroyed1 = Instantiate(rank1boxPuzzle, spawnPoint2+randomPos1, Quaternion.identity);
        willBeDestroyed2 = Instantiate(rank1boxPuzzlePlatform, spawnPoint2+randomPos2, Quaternion.identity);
        willBeDestroyed3 = Instantiate(rank1leverPuzzle, spawnPointWall1+wall1SpawnZone, Quaternion.identity);
        willBeDestroyed3 = Instantiate(rank1simonPuzzle, spawnPointWall3+wall3SpawnZone, wall3Rotation);
        willBeDestroyed4 = Instantiate(puzzleChecker, spawnPointWall2 + wall1SpawnZone, Quaternion.identity);
    }
    void Start()
    {
        generateFloor();
    }
    public void destroyPuzzleChecker()
    {
        Destroy(willBeDestroyed4);
    } 
}
