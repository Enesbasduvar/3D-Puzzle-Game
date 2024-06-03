using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class rank1simonPuzzle : MonoBehaviour
{
    SimonNotes simonNotes;

    private float reachRange = 5.0f;
    public Camera mainCamera;
    public Material Yellow, Blue, Red, Green;
    public GameObject rank1simonPuzzleParent;
    private float timeBetweenFlashes = 0.5f;
    private float waittime = 0.2f;
    private int stage = 0;
    private int pressStage = 0;
    private int maxStage = 3;
    private bool isButtonPressed = false;
    Transform button;
    private int[] sequence = new int[8];
    private bool[] allTrue = new bool[8];

    int score = 50;
    private void Awake()
        {
        simonNotes = GameObject.FindGameObjectWithTag("simonAudio").GetComponent<SimonNotes>();
        }
    // Start is called before the first frame update
    void Start()
    {
        
        Yellow.DisableKeyword("_EMISSION");
        Blue.DisableKeyword("_EMISSION");
        Red.DisableKeyword("_EMISSION");
        Green.DisableKeyword("_EMISSION");
        //randomly generate a sequence of 8 numbers between 0 and 3
        for (int i = 0; i < 8; i++)
        {
            sequence[i] = Random.Range(0, 4);
            allTrue[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //get the camera for drawing the ray
        Transform playerPrefab = GameObject.Find("player").transform;
        Transform cameraHolder = playerPrefab.Find("CameraHolder");
        Camera mainCamera = cameraHolder.GetComponentInChildren<Camera>();

        //flash the buttons in the sequence 1 second apart
        timeBetweenFlashes -= Time.deltaTime;
        if (timeBetweenFlashes <= 0)
        {
            if(stage < maxStage && !isButtonPressed)
            {
                if (sequence[stage] == 0)
                {
                    StartCoroutine(FlashYellow());
                }
                else if (sequence[stage] == 1)
                {
                    StartCoroutine(FlashBlue());
                }
                else if (sequence[stage] == 2)
                {
                    StartCoroutine(FlashRed());
                }
                else if (sequence[stage] == 3)
                {
                    StartCoroutine(FlashGreen());
                }
                stage++;
            }
            else
            {
                stage = 0;
            }
            timeBetweenFlashes = 0.5f;
        }

        //Check if a button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, reachRange) && hit.collider.CompareTag("SimonButton"))
            {
                button = hit.collider.transform;
                //change x position of button
                if (button.localPosition.x == 0.05f)
                {
                    StartCoroutine(ButtonPress());
                    isButtonPressed = true;
                    CheckIfPressedInRightOrder();
                }
                else if (button.localPosition.x == -0.05f)
                {
                    button.localPosition = new Vector3(0.05f, button.localPosition.y, button.localPosition.z);
                }
            }
            
        }
        
        

        IEnumerator ButtonPress()
        {
            button.localPosition = new Vector3(0.03f, button.localPosition.y, button.localPosition.z);
            //get name of button
            string buttonName = button.name;
            if (buttonName == "Yellow")
            {
                StartCoroutine(FlashYellow());
            }
            else if (buttonName == "Blue")
            {
                StartCoroutine(FlashBlue());
            }
            else if (buttonName == "Red")
            {
                StartCoroutine(FlashRed());
            }
            else if (buttonName == "Green")
            {
                StartCoroutine(FlashGreen());
            }
            yield return new WaitForSeconds(waittime);
            button.localPosition = new Vector3(0.05f, button.localPosition.y, button.localPosition.z);
        }

        IEnumerator FlashYellow()
        {
            Yellow.EnableKeyword("_EMISSION");
            simonNotes.PlaySFX(simonNotes.simonY);
            yield return new WaitForSeconds(waittime);
            Yellow.DisableKeyword("_EMISSION");
        }

        IEnumerator FlashBlue()
        {
            Blue.EnableKeyword("_EMISSION");
            simonNotes.PlaySFX(simonNotes.simonB);
            yield return new WaitForSeconds(waittime);
            Blue.DisableKeyword("_EMISSION");
        }

        IEnumerator FlashRed()
        {
            Red.EnableKeyword("_EMISSION");
            simonNotes.PlaySFX(simonNotes.simonR);
            yield return new WaitForSeconds(waittime);
            Red.DisableKeyword("_EMISSION");
        }

        IEnumerator FlashGreen()
        {
            Green.EnableKeyword("_EMISSION");
            simonNotes.PlaySFX(simonNotes.simonG);
            yield return new WaitForSeconds(waittime);
            Green.DisableKeyword("_EMISSION");
        }


    }

    void CheckIfPressedInRightOrder()
    {
        if (isButtonPressed)
        {
            if (sequence[pressStage] == 0)
            {
                if (button.name == "Yellow")
                {
                    allTrue[pressStage] = true;
                    pressStage++;
                }
                else
                {
                    pressStage = 0;
                    isButtonPressed = false;
                    timeBetweenFlashes = 0.5f;
                }
            }
            else if (sequence[pressStage] == 1)
            {
                if (button.name == "Blue")
                {
                    allTrue[pressStage] = true;
                    pressStage++;
                    timeBetweenFlashes = 0.5f;
                }
                else
                {
                    pressStage = 0;
                    isButtonPressed = false;
                    timeBetweenFlashes = 0.5f;
                }
            }
            else if (sequence[pressStage] == 2)
            {
                if (button.name == "Red")
                {
                    allTrue[pressStage] = true;
                    pressStage++;
                }
                else
                {
                    pressStage = 0;
                    isButtonPressed = false;
                    timeBetweenFlashes = 0.5f;
                }
            }
            else if (sequence[pressStage] == 3)
            {
                if (button.name == "Green")
                {
                    allTrue[pressStage] = true;
                    pressStage++;
                }
                else
                {
                    pressStage = 0;
                    isButtonPressed = false;
                    timeBetweenFlashes = 0.5f;
                }
            }
            switch (maxStage)
            {
                case 3:
                    if (allTrue[0] && allTrue[1] && allTrue[2])
                    {
                        isButtonPressed = false;
                        maxStage++;
                        pressStage = 0;
                        timeBetweenFlashes = 0.5f;
                    }
                    break;
                case 4:
                    if (allTrue[0] && allTrue[1] && allTrue[2] && allTrue[3])
                    {
                        isButtonPressed = false;
                        maxStage++;
                        pressStage = 0;
                        timeBetweenFlashes = 0.5f;
                    }
                    break;
                case 5:
                    if (allTrue[0] && allTrue[1] && allTrue[2] && allTrue[3] && allTrue[4])
                    {
                        isButtonPressed = false;
                        maxStage++;
                        pressStage = 0;
                        timeBetweenFlashes = 0.5f;
                    }
                    break;
                case 6:
                    if (allTrue[0] && allTrue[1] && allTrue[2] && allTrue[3] && allTrue[4] && allTrue[5])
                    {
                        isButtonPressed = false;
                        maxStage++;
                        pressStage = 0;
                        timeBetweenFlashes = 0.5f;
                    }
                    break;
                case 7:
                    if (allTrue[0] && allTrue[1] && allTrue[2] && allTrue[3] && allTrue[4] && allTrue[5] && allTrue[6])
                    {
                        isButtonPressed = false;
                        maxStage++;
                        pressStage = 0;
                        timeBetweenFlashes = 0.5f;
                    }
                    break;
                case 8:
                    if (allTrue[0] && allTrue[1] && allTrue[2] && allTrue[3] && allTrue[4] && allTrue[5] && allTrue[6] && allTrue[7])
                    {
                        
                        ScoreManager.instance.AddScore(score);
                        for (int i = 0; i < 8; i++)
                        {
                            allTrue[i] = false;
                            Destroy(rank1simonPuzzleParent, 0.3f);
                        }

                    }
                    break;
            }
        }
    }
}
