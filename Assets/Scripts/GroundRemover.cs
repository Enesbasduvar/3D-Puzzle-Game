
using UnityEngine;

public class GroundRemover : MonoBehaviour
{
    GroundSpawner groundSpawner;
    public Animator door;
    public GameObject indoor;
    public GameObject exitDoor;
    void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
    }

    void OnTriggerExit (Collider other)
    {
        if(other.tag == "Player")
        {
            ScoreManager.instance.AddLevel(1);
            Destroy(groundSpawner.willBeDestroyed1);
            Destroy(groundSpawner.willBeDestroyed2);
            groundSpawner.destroyPuzzleChecker();
            groundSpawner.generateFloor();
            Destroy(gameObject);
        }
        if(other.tag=="Puzzle")
        {
            exitDoor.SetActive(false);
        }
    }
    void OnTriggerEnter (Collider other)
    {
        if(other.tag =="Player")
        {
            indoor.SetActive(true);
        }
        if(other.tag=="Puzzle")
        {
            exitDoor.SetActive(true);
        }
    }
    public void DisableDoor()
    {
        door.SetBool("doorOpen", true);
        // Trigger the animation to move the door upwards
    }
}
