using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialogSystem : Sounds
{
    [SerializeField] private bool startEducation;
    [SerializeField] private GameObject[] objectsToSpawn;
    [SerializeField] private GameObject[] destroyableObjects;
    [FormerlySerializedAs("CollisionToOpen")] [SerializeField] private GameObject collisionToOpen;
    [FormerlySerializedAs("MovementTips")] public GameObject movementTips;
    [FormerlySerializedAs("ContinueDialog")] public GameObject continueDialog;
    [FormerlySerializedAs("FightingTips")] public GameObject fightingTips;
    public string[] lines;
    public float speedText;
    public Text dialogText;
    public int index;
    private bool isDialogActive;

    private void Start ()
    {
        isDialogActive = false;
        index = 0;
        StartDialog();
    }

    private void StartDialog()
    {
        isDialogActive = true;
        dialogText.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        for (var i = 0; i < lines[index].Length; i++)
        {
            dialogText.text += lines[index][i];
            yield return new WaitForSeconds(speedText);
            if (i % 2 == 0)
                PlaySound(objectSounds[0], volume: 0.8f, fadeInTime: 0);
        }
        isDialogActive = false;
    }

    private void NextLines()
    {
        if (index < lines.Length - 1) 
            index++;
        else
        {
            if (objectsToSpawn.Length > 0)
                SpawnObjects();
            StopAllCoroutines();
            if (startEducation)
            {
                fightingTips.SetActive(true);
                collisionToOpen.SetActive(false);
            }
            foreach (var destroyableObject in destroyableObjects)
                Destroy(destroyableObject);
        }
    }

    private void SpawnObjects()
    {
        foreach (var obj in objectsToSpawn)
            obj.SetActive(true);
    }
    
    public void Update()
    {
        if (startEducation)
            MakeStartEducation();
        if (Input.GetKeyDown(KeyCode.Space) && movementTips != null && movementTips.activeSelf) return;
        if (index == 2 && movementTips == null && !isDialogActive && startEducation)
        {
            NextLines();
            StartDialog();
        }
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
        if (Input.GetKeyDown(KeyCode.Space) && continueDialog != null)
            DestroyImmediate(continueDialog);
        if ( continueDialog != null && index == 0 && !isDialogActive)
            continueDialog.SetActive(true);
        if ( movementTips != null && index == 2 && !isDialogActive)
            movementTips.SetActive(true);
    }
}
