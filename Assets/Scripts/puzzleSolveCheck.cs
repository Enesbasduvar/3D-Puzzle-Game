using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleSolveCheck : MonoBehaviour
{
    AudioManager audioManager;

    bool allPuzzlesSolved = false;
    GroundRemover groundRemover;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        groundRemover = GameObject.FindObjectOfType<GroundRemover>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if(GameObject.FindGameObjectsWithTag("Puzzle").Length == 0)
        {
            Debug.Log("all puzzles solved!");
            if(allPuzzlesSolved == false)
            {
                allPuzzlesSolved = true;
                //groundRemover.exitDoor.SetActive(false);
                groundRemover.DisableDoor();
                audioManager.PlaySFX(audioManager.DoorOpen);
            }
        }
        
    }
}
