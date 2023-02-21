using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject tutorial;
    [SerializeField] GameObject redKnight;
    [SerializeField] GameObject blueKnight;
    [SerializeField] GameObject redWizard;
    [SerializeField] GameObject blueWizard;
    [SerializeField] Transform redFlag;
    [SerializeField] Transform blueFlag;
    [SerializeField] LayerMask groundMask;
    [SerializeField] GameObject winText;

    [HideInInspector] public List<GameObject> redTeam = new List<GameObject>();
    [HideInInspector] public List<GameObject> blueTeam = new List<GameObject>();

    bool isPlaying = false;

    enum UnitType {Knight, Wizard};

    UnitType currType = UnitType.Knight;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 screenPos = Input.mousePosition;
                Ray currentRay = Camera.main.ScreenPointToRay(screenPos);
                RaycastHit hit;
                if (Physics.Raycast(currentRay, out hit, 3000, groundMask))
                {
                    switch (currType)
                    {
                        case UnitType.Knight:
                            GameObject newKnight = Instantiate(redKnight, hit.point, Quaternion.identity);
                            redTeam.Add(newKnight);
                            newKnight.GetComponent<Knight>().goal = blueFlag;
                            break;
                        case UnitType.Wizard:
                            GameObject newWizard = Instantiate(redWizard, hit.point, Quaternion.identity);
                            redTeam.Add(newWizard);
                            newWizard.GetComponent<Wizard>().goal = blueFlag;
                            break;
                    }
                    
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Vector2 screenPos = Input.mousePosition;
                Ray currentRay = Camera.main.ScreenPointToRay(screenPos);
                RaycastHit hit;
                if (Physics.Raycast(currentRay, out hit, 3000, groundMask))
                {
                    switch (currType)
                    {
                        case UnitType.Knight:
                            GameObject newKnight = Instantiate(blueKnight, hit.point, Quaternion.identity);
                            blueTeam.Add(newKnight);
                            newKnight.GetComponent<Knight>().goal = redFlag;
                            break;
                        case UnitType.Wizard:
                            GameObject newWizard = Instantiate(blueWizard, hit.point, Quaternion.identity);
                            blueTeam.Add(newWizard);
                            newWizard.GetComponent<Wizard>().goal = redFlag;
                            break;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1)) currType = UnitType.Knight;
            else if (Input.GetKeyDown(KeyCode.Alpha2)) currType = UnitType.Wizard;
        }
    }

    public void StartGame()
    {
        foreach (GameObject knight in GameObject.FindGameObjectsWithTag("BlueTeam"))
        {
            knight.GetComponent<NavMeshAgent>().isStopped = false;
            knight.GetComponent<Knight>().enemyTeam = redTeam.ToArray();
        }
        foreach (GameObject knight in GameObject.FindGameObjectsWithTag("RedTeam"))
        {
            knight.GetComponent<NavMeshAgent>().isStopped = false;
            knight.GetComponent<Knight>().enemyTeam = blueTeam.ToArray();
        }
        tutorial.SetActive(false);
        isPlaying = true;
    }
    public void EndGame(string winner)
    {
        foreach (GameObject knight in GameObject.FindGameObjectsWithTag("BlueTeam"))
        {
            knight.GetComponent<NavMeshAgent>().isStopped = true;
            knight.GetComponentInChildren<Animator>().SetBool("isWalking", false);
        }
        foreach (GameObject knight in GameObject.FindGameObjectsWithTag("RedTeam"))
        {
            knight.GetComponent<NavMeshAgent>().isStopped = true;
            knight.GetComponentInChildren<Animator>().SetBool("isWalking", false);
        }
        winText.SetActive(true);
        winText.GetComponent<TextMeshProUGUI>().text = winner + " Team wins!";
    }
}
