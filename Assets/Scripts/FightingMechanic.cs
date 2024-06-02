using System.Collections.Generic;
using Fighting;
using UnityEngine;

public class FightingMechanic : MonsterTypeIdentifier
{
    private ButtonSequenceGen buttonGenerator;
    private List<GameObject> buttonInstances;
    private Queue<char> buttonSequence;
    private int buttonCount;
    public bool isButtonGenerated;
    private float timeForAttack;
    private void Start()
    {
        buttonSequence = new Queue<char>();
        buttonInstances = new List<GameObject>();
        isButtonGenerated = false;
        buttonGenerator = new ButtonSequenceGen();
        buttonCount = other.ButtonCount;
    }

    private void Update()
    {
        CheckPlayerMissClick();
        other.IsCorrectClick = buttonGenerator.CheckForCorrectClick(buttonSequence, buttonInstances);
        if (!(Vector3.Distance(transform.position, other.player.position) <= other.visibleRadius)
            || isButtonGenerated || other.MonsterInfo.LivesCount <= 0) return;
        GenerateButtonSequence();
        timeForAttack = Time.time;
    }

    private void LateUpdate()
    {
        CheckForAttack();
    }

    private void CheckPlayerMissClick()
    {
        if (other.IsCorrectClick == false)
        {
            timeForAttack = Time.time;
            other.playerHealth.TakeDamage(1);
        }
    }

    private void GenerateButtonSequence()
    {
        var buttons = buttonGenerator.GenerateButtonSeq(buttonCount);
        var buttonPosition = other.sprite.transform.position + new Vector3((-buttonCount)*0.3f, 1, 0);
        foreach (var letter in buttons)
        {
            var newButton = Instantiate(other.buttonPrefab, buttonPosition, Quaternion.identity, transform);
            var buttonInfo = new ButtonInfo(newButton, letter, other.sprite);
            buttonGenerator.GenerateSprite(buttonInfo, buttonSequence, buttonInstances, buttonCount, buttonPosition);
            buttonPosition += new Vector3(1*0.8f, 0, 0);
        }
        isButtonGenerated = true;
    }
        
    private void CheckTimeToAttack()
    {
        isButtonGenerated = false;
        buttonInstances = new List<GameObject>();
        buttonSequence = new Queue<char>();
        DestroyButtons();
        timeForAttack = Time.time;
        if (other.objectSounds.Length > 0)
            other.PlaySound(other.objectSounds[0]);
        other.playerHealth.TakeDamage(1);
        
    }
        
    private void CheckForAttack()
    {
        if (!isButtonGenerated) return;
        if (Time.time - timeForAttack > 3.5f)
            CheckTimeToAttack();
        else if (buttonSequence.Count == 0)
            GiveDamageToMonster();
    }
        
    private void GiveDamageToMonster()
    {
        isButtonGenerated = false;
        other.MonsterInfo.LivesCount--;
        buttonInstances = new List<GameObject>();
        DestroyButtons();
    }

    private void DestroyButtons()
    {
        while (gameObject.transform.childCount > 0)
            DestroyImmediate(gameObject.transform.GetChild(0).gameObject);
    }
}