using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class rank1leverPuzzleRNG : MonoBehaviour
{
    //rank1boxPuzzle rank1BoxPuzzle;
    AudioManager audioManager;
    public GameObject rank1leverPuzzleParent;
    public Transform rank1leverPuzzle; // Reference to the rank1leverPuzzle prefab
    private List<int> randomNumbers;
    private List<int> orderOfNumbers;
    private List<int> clickedNumbers = new List<int>();
    Transform pivot;

    bool[] leverRotated = new bool[6];
    private int stage = 0;

    int score;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        score = 20;
        // Step 1: Generate random numbers
        GenerateRandomNumbers(6); // Change 6 to the desired count

        // Step 2: Assign numbers to Text components
        AssignNumbersToTextComponents();

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckClickedLever();
        }
        // Step 3: Check if pivots are rotated in the right order
        CheckPivotRotation();
    }

    void GenerateRandomNumbers(int count)
    {
        randomNumbers = new List<int>();
        for (int i = 1; i <= count; i++)
        {
            randomNumbers.Add(i);
        }
        // Shuffle the list to get a random order
        ShuffleList(randomNumbers);
        // find the index of 1
        orderOfNumbers = new List<int>();
        for (int i = 0; i < randomNumbers.Count; i++)
        {
            orderOfNumbers.Add(randomNumbers.IndexOf(i + 1) + 1);
        }
    }

    void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    void AssignNumbersToTextComponents()
    {
        if (rank1leverPuzzle != null)
        {
            Transform baseTransform = rank1leverPuzzle.Find("Base");

            for (int i = 0; i < randomNumbers.Count; i++)
            {
                string ltName = "LT" + (i + 1); // Construct LT name
                Transform ltTransform = baseTransform.Find(ltName);

                if (ltTransform != null)
                {
                    Text textComponent = ltTransform.GetComponentInChildren<Text>();
                    if (textComponent != null)
                    {
                        // Step 2: Assign the number to the Text component
                        textComponent.text = randomNumbers[i].ToString();
                    }
                }
            }
        }
    }

    void CheckPivotRotation()
    {
        if (rank1leverPuzzle != null)
        {
            Transform baseTransform = rank1leverPuzzle.Find("Base");
            if (clickedNumbers.Count == orderOfNumbers.Count)
            {
                for (int i = 0; i < orderOfNumbers.Count; i++)
                {
                    string leverName = "LeverC" + orderOfNumbers[i]; // Construct Lever name
                    Transform leverTransform = baseTransform.Find(leverName + "/Pivot");

                    if (leverTransform != null)
                    {
                        float targetRotation = -30.0f + i * 30.0f;
                        float currentRotation = leverTransform.localRotation.eulerAngles.z;
                        if (!Mathf.Approximately(currentRotation, targetRotation))
                        {
                            // Reset lever to original position (you can implement the reset logic here)
                            leverTransform.localRotation = Quaternion.Euler(0, 0, -30.0f);
                        }
                    }
                }
            }
        }
    }

    void CheckClickedLever()
    {
        Transform playerPrefab = GameObject.Find("player").transform;
        Transform cameraHolder = playerPrefab.Find("CameraHolder");
        Camera mainCamera = cameraHolder.GetComponentInChildren<Camera>();
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Check if the clicked object is one of the levers
            if (hit.collider.CompareTag("Lever") || hit.collider.CompareTag("LeverC"))
            {
                // Extract the number from the clicked lever
                int clickedNumber;
                if (int.TryParse(hit.collider.name.Substring(6), out clickedNumber) || int.TryParse(hit.collider.name.Substring(5), out clickedNumber))
                {
                    if(hit.collider.CompareTag("LeverC"))
                    {
                        pivot = hit.collider.transform.GetChild(0);
                    }
                    else if(hit.collider.CompareTag("Lever"))
                    {
                        pivot = hit.collider.transform.parent;
                    }

                    if (pivot != null)
                    {
                        // rotate the pivot object around the z-axis by -120 degrees
                        if (pivot.rotation.eulerAngles.z == 330)
                        {
                            clickedNumbers.Remove(clickedNumber);
                            leverRotated[clickedNumber - 1] = false;
                            
                            Debug.Log("lever 1");
                            audioManager.PlaySFX(audioManager.LeverOn);
                            stage--;
                            
                        }
                        else if (pivot.rotation.eulerAngles.z != 330)
                        {
                            // Store the clicked number
                            clickedNumbers.Add(clickedNumber);
                            leverRotated[clickedNumber - 1] = true;
                            stage++;
                            
                            Debug.Log("lever 2");
                            if (clickedNumbers[stage-1] != orderOfNumbers[stage-1] && stage > 0)
                            {
                                
                                //make all levers go back to original position 330 in euler angles
                                
                                Debug.Log("lever 3");
                                
                                audioManager.PlaySFX(audioManager.LeverOff);
                                for (int i = 0; i < 6; i++)
                                {
                                    string leverName = "LeverC" + (i + 1); // Construct Lever name
                                    Transform leverTransform = pivot.parent.parent.Find(leverName + "/Pivot");
                                    if (leverTransform != null)
                                    {
                                        leverTransform.localRotation = Quaternion.Euler(0, 0, 330);
                                        Debug.Log("lever 4");
                                    }
                                    leverRotated[i] = false;
                                }
                                clickedNumbers.Clear();
                                stage = 0;
                            }
                            else if(clickedNumbers[stage-1] == orderOfNumbers[stage-1])
                            {
                                if(leverRotated.All(x => x == true))
                                {
                                    ScoreManager.instance.AddScore(score);
                                    Destroy(rank1leverPuzzleParent,0.2f);
                                }
                            }
                        }
                        
                    }
                }
            }
        }
    }
}
