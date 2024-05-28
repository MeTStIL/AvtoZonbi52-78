using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] private bool startEducation;
    [SerializeField] private GameObject[] objectsToSpawn;
    [SerializeField] private GameObject[] destroyableObjects;
    [SerializeField] private GameObject CollisionToOpen;
    public string[] lines;
    public GameObject MovementTips;
    public GameObject ContinueDialog;
    public GameObject FightingTips;
    public float speedText;
    public Text dialogText;
    public int index;
    private bool isDialogActive;

    void Start ()
    {
        isDialogActive = false;
        index = 0;
        StartDialog();
    }

    void StartDialog()
    {
        isDialogActive = true;
        dialogText.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (var c in lines[index]) 
        {
            dialogText.text += c;
            yield return new WaitForSeconds (speedText);
        }

        isDialogActive = false;
    }
    
    public void NextLines()
    {
        if (index < lines.Length - 1) 
        {
            index++;
        } 
        else
        {
            if (objectsToSpawn.Length > 0)
                SpawnObjects();
            StopAllCoroutines();
            if (startEducation)
            {
                
                FightingTips.SetActive(true);
                CollisionToOpen.SetActive(false);
            }

            foreach (var destroyableObject in destroyableObjects)
            {
                Destroy(destroyableObject);
            }
            
        }
    }

    private void SpawnObjects()
    {
        Debug.Log("СПАВНЮ");
        foreach (var obj in objectsToSpawn)
            obj.SetActive(true);
    }
    public void Update()
    {
        if (startEducation)
            MakeStartEducation();

        if (Input.GetKeyDown(KeyCode.Space) && MovementTips != null && MovementTips.activeSelf) return;
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        if (isDialogActive)
        {
            StopAllCoroutines();
            dialogText.text = lines[index];
            isDialogActive = false;
        }
        else
        {
            
            NextLines();
            StartDialog();
        }
    }

    private void MakeStartEducation()
    {
        if (Input.GetKeyDown(KeyCode.Space) && ContinueDialog != null)
            DestroyImmediate(ContinueDialog);
        if ( ContinueDialog != null && index == 0 && !isDialogActive)
        {
            ContinueDialog.SetActive(true);
        }
        if ( MovementTips != null && index == 2 && !isDialogActive)
        {
            MovementTips.SetActive(true);
        }
        
    }
}
