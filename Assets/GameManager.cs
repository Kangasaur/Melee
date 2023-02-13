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
    [SerializeField] Transform redFlag;
    [SerializeField] Transform blueFlag;
    [SerializeField] LayerMask groundMask;
    [SerializeField] GameObject winText;

    [HideInInspector] public List<GameObject> redTeam = new List<GameObject>();
    [HideInInspector] public List<GameObject> blueTeam = new List<GameObject>();

    bool isPlaying = false;

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
                    GameObject newKnight = Instantiate(redKnight, hit.point, Quaternion.identity);
                    redTeam.Add(newKnight);
                    newKnight.GetComponent<Knight>().goal = blueFlag;
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Vector2 screenPos = Input.mousePosition;
                Ray currentRay = Camera.main.ScreenPointToRay(screenPos);
                RaycastHit hit;
                if (Physics.Raycast(currentRay, out hit, 3000, groundMask))
                {
                    GameObject newKnight = Instantiate(blueKnight, hit.point, Quaternion.identity);
                    blueTeam.Add(newKnight);
                    newKnight.GetComponent<Knight>().goal = redFlag;
                }
            }
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
