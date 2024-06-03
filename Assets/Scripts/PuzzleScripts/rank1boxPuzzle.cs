using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class rank1boxPuzzle : MonoBehaviour
{
    GroundRemover groundRemover;
    GroundSpawner groundSpawner;
    PickUpControls pickUpControls;
    AudioManager audioManager;
    //public rank1boxPuzzlePlatform rank1boxPuzzleplatform;
    // take LT, T, RT, L, M, R, LB, B, RB objects and change their positions randomly in start between -1.5 and 1.5 with 1.5 distance between them in only x and z axis with each object having a different position and spawn the parent
    // Start is called before the first frame update
    public GameObject LT;
    public GameObject T;
    public GameObject RT;
    public GameObject L;
    public GameObject M;
    public GameObject R;
    public GameObject LB;
    public GameObject B;
    public GameObject RB;
    public GameObject rank1boxPuzzleParent;
    public GameObject PuzzleObjects;
    GameObject slot;
    //int i;
    int arraySize = 9;
    int score = 40;
    //bool isHolding = false;
    
    bool snapped = false;
    bool s1 = false;
    bool s2 = false;
    bool s3 = false;
    bool s4 = false;
    bool s5 = false;
    bool s6 = false;
    bool s7 = false;
    bool s8 = false;
    bool s9 = false;
    bool scoreCheck = false;
    Vector3[] randPosArray = new Vector3[9];
     //float space = 0.5f;
    float space;
    float finishSpace = 0.76f;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        slot = GameObject.Find("Slot");
        GameObject[] PuzzleObjects = GameObject.FindGameObjectsWithTag("Puzzle");

        groundRemover = GameObject.FindObjectOfType<GroundRemover>();
        pickUpControls = GameObject.FindObjectOfType<PickUpControls>();
        randPosArray[0]= new Vector3(-1.2f, 0, -1.2f);
        randPosArray[1]= new Vector3(0, 0, -1.2f);
        randPosArray[2]= new Vector3(1.2f, 0, -1.2f);
        randPosArray[3]= new Vector3(-1.2f, 0, 0);
        randPosArray[4]= new Vector3(0, 0, 0);
        randPosArray[5]= new Vector3(1.2f, 0, 0);
        randPosArray[6]= new Vector3(-1.2f, 0, 1.2f);
        randPosArray[7]= new Vector3(0, 0, 1.2f);
        randPosArray[8]= new Vector3(1.2f, 0, 1.2f);
        //a shuffled array contains numbers from 0 to 8 and every number occurs once
        int[] shuffledArray = new int[9];
        for (int i = 0; i < arraySize; i++)
        {
            shuffledArray[i] = i;
        }
        for (int i = 0; i < arraySize; i++)
        {
            int temp = shuffledArray[i];
            int randomIndex = UnityEngine.Random.Range(i, arraySize);
            shuffledArray[i] = shuffledArray[randomIndex];
            shuffledArray[randomIndex] = temp;
        }
        //console log the shuffled array
        for (int i = 0; i < arraySize; i++)
        {
            //Debug.Log(shuffledArray[i]);
        }
        LT.transform.position = rank1boxPuzzleParent.transform.position+randPosArray[shuffledArray[0]];
        T.transform.position = rank1boxPuzzleParent.transform.position+randPosArray[shuffledArray[1]];
        RT.transform.position = rank1boxPuzzleParent.transform.position+randPosArray[shuffledArray[2]];
        L.transform.position = rank1boxPuzzleParent.transform.position+randPosArray[shuffledArray[3]];
        M.transform.position = rank1boxPuzzleParent.transform.position+randPosArray[shuffledArray[4]];
        R.transform.position = rank1boxPuzzleParent.transform.position+randPosArray[shuffledArray[5]];
        LB.transform.position = rank1boxPuzzleParent.transform.position+randPosArray[shuffledArray[6]];
        B.transform.position = rank1boxPuzzleParent.transform.position+randPosArray[shuffledArray[7]];
        RB.transform.position = rank1boxPuzzleParent.transform.position+randPosArray[shuffledArray[8]];

        space = (rank1boxPuzzleParent.transform.localScale.x/2)+0.1f;
        
    }
   
    
 
    // Update is called once per frame
    void Update()
    {
        
        
        checkDistance();
        //snapToSlot();
        if (!snapped)
        {

            snapToNearestSlot(LT, ref s1);
            snapToNearestSlot(T, ref s2);
            snapToNearestSlot(RT, ref s3);
            snapToNearestSlot(L, ref s4);
            snapToNearestSlot(M, ref s5);
            snapToNearestSlot(R, ref s6);
            snapToNearestSlot(LB, ref s7);
            snapToNearestSlot(B, ref s8);
            snapToNearestSlot(RB, ref s9);
        }
        
    }


    void snapToNearestSlot(GameObject obj, ref bool Lsnapped)
    {
        GameObject[] slotObjects = GameObject.FindGameObjectsWithTag("Slot");

        float minDistance = Mathf.Infinity;
        GameObject nearestSlot = null;

        // Find the nearest slot object
        foreach (GameObject slotObject in slotObjects)
        {
            float distance = Vector3.Distance(obj.transform.position, slotObject.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestSlot = slotObject;
            }
        }

        // Snap to the nearest slot if it exists
        if (nearestSlot != null && minDistance < space && !Lsnapped)
        {
            obj.transform.position = nearestSlot.transform.position;
            obj.transform.rotation = nearestSlot.transform.rotation;
            if(minDistance < 0.1f)
            {
                Lsnapped = true;
                //Debug.Log("pop");
                audioManager.PlaySFX(audioManager.popin);
            }
            
            
        }
        if(Lsnapped)
        {
            obj.transform.position = nearestSlot.transform.position;
            obj.transform.rotation = nearestSlot.transform.rotation;
        }
        if (minDistance > space && Lsnapped)
        {
            Lsnapped = false;
            //Debug.Log("popout");
            audioManager.PlaySFX(audioManager.popout);
        }
        if (s1 && s2 && s3 && s4 && s5 && s6 && s7 && s8 && s9)
        {
            //Debug.Log("All snapped");
            snapped = true;
        }
        //Debug.Log("Distance: "+minDistance.ToString("F2")+", nearestSlot: "+nearestSlot.name+", snapped: "+snapped);
    }


    //if any of the objects are close to LT_slot or RT_slot or so on, they will snap to it. the void function is below
    void snapToSlot()
    {
        GameObject[] slotObjects = GameObject.FindGameObjectsWithTag("Slot");

        // Iterate through each "Slot" object
        foreach (GameObject slotObject in slotObjects)
        {
            Debug.Log("Distance: "+Vector3.Distance(LT.transform.position, slotObject.transform.position).ToString("F2")+", Treshold:"+space);
            
            if(Vector3.Distance(LT.transform.position, slotObject.transform.position) !< space)
            {
                s1 = false;
            }
            if (Vector3.Distance(LT.transform.position, slotObject.transform.position) < space)
            {
                LT.transform.position = slotObject.transform.position;
                LT.transform.rotation = slotObject.transform.rotation;
                s1 = true;
                //audioManager.PlaySFX(audioManager.popin);
            }  
            if (Vector3.Distance(T.transform.position, slotObject.transform.position) !< space)
            {
                s2 = false;
            }
            if (Vector3.Distance(T.transform.position, slotObject.transform.position) < space)
            {
                T.transform.position = slotObject.transform.position;
                T.transform.rotation = slotObject.transform.rotation;
                s2 = true;
                
            }
            if (Vector3.Distance(RT.transform.position, slotObject.transform.position) !< space)
            {
                s3 = false;
            }
            if (Vector3.Distance(RT.transform.position, slotObject.transform.position) < space)
            {
                RT.transform.position = slotObject.transform.position;
                RT.transform.rotation = slotObject.transform.rotation;
                s3 = true;
                
            }
            if (Vector3.Distance(L.transform.position, slotObject.transform.position) !< space)
            {
                s4 = false;
            }
            if (Vector3.Distance(L.transform.position, slotObject.transform.position) < space)
            {
                L.transform.position = slotObject.transform.position;
                L.transform.rotation = slotObject.transform.rotation;
                s4 = true;
                
            }
            if (Vector3.Distance(M.transform.position, slotObject.transform.position) !< space)
            {
                s5 = false;
            }
            if (Vector3.Distance(M.transform.position, slotObject.transform.position) < space)
            {
                M.transform.position = slotObject.transform.position;
                M.transform.rotation = slotObject.transform.rotation;
                s5 = true;
                
            }
            if (Vector3.Distance(R.transform.position, slotObject.transform.position) !< space)
            {
                s6 = false;
            }
            if (Vector3.Distance(R.transform.position, slotObject.transform.position) < space)
            {
                R.transform.position = slotObject.transform.position;
                R.transform.rotation = slotObject.transform.rotation;
                s6 = true;
                
            }
            if (Vector3.Distance(LB.transform.position, slotObject.transform.position) !< space)
            {
                s7 = false;
            }
            if (Vector3.Distance(LB.transform.position, slotObject.transform.position) < space)
            {
                LB.transform.position = slotObject.transform.position;
                LB.transform.rotation = slotObject.transform.rotation;
                s7 = true;
                
            }
            if (Vector3.Distance(B.transform.position, slotObject.transform.position) !< space)
            {
                s8 = false;
            }
            if (Vector3.Distance(B.transform.position, slotObject.transform.position) < space)
            {
                B.transform.position = slotObject.transform.position;
                B.transform.rotation = slotObject.transform.rotation;
                s8 = true;
                
            }
            if (Vector3.Distance(RB.transform.position, slotObject.transform.position) !< space)
            {
                s9 = false;
            }
            if (Vector3.Distance(RB.transform.position, slotObject.transform.position) < space)
            {
                RB.transform.position = slotObject.transform.position;
                RB.transform.rotation = slotObject.transform.rotation;
                s9 = true;
            }
        }
        if (s1 && s2 && s3 && s4 && s5 && s6 && s7 && s8 && s9)
        {
            Debug.Log("All snapped");
            snapped = true;
        }
        
        
    }

    void checkDistance()
    {
        
        if (Vector3.Distance(LT.transform.position, T.transform.position) < finishSpace)
        {
            if (Vector3.Distance(T.transform.position, RT.transform.position) < finishSpace)
            {
                if (Vector3.Distance(LT.transform.position, L.transform.position) < finishSpace)
                {
                    if (Vector3.Distance(L.transform.position, M.transform.position) < finishSpace)
                    {
                        if (Vector3.Distance(M.transform.position, R.transform.position) < finishSpace)
                        {
                            if (Vector3.Distance(R.transform.position, RB.transform.position) < finishSpace)
                            {
                                if (Vector3.Distance(RB.transform.position, B.transform.position) < finishSpace)
                                {
                                    if (Vector3.Distance(B.transform.position, LB.transform.position) < finishSpace)
                                    {
                                        if(snapped)
                                        {
                                            Destroy(rank1boxPuzzleParent, 0.2f);
                                            groundSpawner.destroyPlatform();
                                            Destroy(LT, 0.2f);
                                            Destroy(T, 0.2f);
                                            Destroy(RT, 0.2f);
                                            Destroy(L, 0.2f);
                                            Destroy(M, 0.2f);
                                            Destroy(R, 0.2f);
                                            Destroy(LB, 0.2f);
                                            Destroy(B, 0.2f);
                                            Destroy(RB, 0.2f);
                                            if (!scoreCheck)
                                            {
                                                scoreCheck = true;
                                                ScoreManager.instance.AddScore(score);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

}
