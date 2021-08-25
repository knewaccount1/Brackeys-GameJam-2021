using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public List<Interactable> allInteractables;

    public int interactablesToWin;
    public int score;

    [HideInInspector] public int destroyedInteractables;

    private void Start()
    {
        allInteractables = new List<Interactable>();

        
        allInteractables.AddRange(FindObjectsOfType<Interactable>());
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha0))
        {
            SceneManager.LoadScene(1);
        }

        
    }

    public void CheckWinCondition()
    {
        if (destroyedInteractables >= interactablesToWin)
        {
            Debug.Log("Player has won, load next scene");
            SceneManager.LoadScene(1);
        }

    }


}
