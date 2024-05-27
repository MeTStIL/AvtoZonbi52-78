using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    public string[] lines;
    public GameObject MovementTips;
    public GameObject ContinueDialog;
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
            index=0;
        }
    }

    public void Update()
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
}
