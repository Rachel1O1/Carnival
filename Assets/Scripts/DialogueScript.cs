using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueScript : MonoBehaviour
{
    private TextMeshProUGUI textBox;
    private TextMeshProUGUI textSpeaker;
    private string talkName;
    private string[] lines;
    private float textSpeed;
    private int index;
    private bool talkMode;
    private bool isChoice;
    private int talkScriptNum;
    [SerializeField] private GameObject buttonHolder;

    //Choice dialogue only works with 1 line

    void Awake()
    {
        talkMode = false;
        textSpeed = .1f;
        isChoice = false;
        talkScriptNum = 0;
        buttonHolder = transform.Find("pickMe").gameObject;
        textBox = transform.Find("dialogueText").GetComponent<TextMeshProUGUI>();
        textSpeaker = transform.Find("Speaker").GetComponent<TextMeshProUGUI>();
        textBox.text = string.Empty;
        textSpeaker.text = string.Empty;
        buttonHolder.SetActive(false);
        gameObject.SetActive(false);
    }

    public void clickNext()
    {
        if (textBox.text == lines[index])
        {
            if (isChoice)
            {
                buttonHolder.SetActive(true);
            } else {
                NextLine();
            }
        } else {
            StopAllCoroutines();
            textBox.text = lines[index];
            if (isChoice)
            {
                buttonHolder.SetActive(true);
            }
        }
    }

    public bool getMode()
    {
        return talkMode;
    }

    public bool getIsChoice()
    {
        return isChoice;
    }

    public int getTalkScriptNum()
    {
        return talkScriptNum;
    }

    public void SwitchDialogue(int newNum)
    {
        talkScriptNum = newNum;
        if (newNum == 1)
        {
            //talked with Nel for funNEL cakes
            talkName = "Nel";
            lines = new string[] {"Hi! Would you like a Funnel Cake?"};
            isChoice = true;
        } else if (newNum == 2)
        {
            //talked with Cara for CARAmel apples
            talkName = "Cara";
            lines = new string[] {"Hi. Would you like a Caramel Apple?"};
            isChoice = true;
        } else if (newNum == 3)
        {
            //talked with Ferris for ???
            talkName = "Ferris";
            lines = new string[] {"Hi...", "The name's Ferris...",".....",".....",".....","Why are you still here...",".....","Leave me alone..."};
            isChoice = false;
        } else if (newNum == 4)
        {
            //talked with Rin for RINg bottle toss
            talkName = "Rin";
            lines = new string[] {"Hello! Do you want to play Ring Toss?"};
            isChoice = true;
        } else if (newNum == 5)
        {
            //talked with Charles for ping pong
            talkName = "Charles";
            lines = new string[] {"Hello. Do you want to play Ping Pong?"};
            isChoice = true;
        }
        StartDialogue();
    }

    public void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textBox.text = string.Empty;
            textSpeaker.text = string.Empty;
            textSpeaker.text = talkName;
            StartCoroutine(TypeLine());
        } else {
            StartCoroutine(WaitNow());
        }
    }

    public void StartDialogue()
    {
        buttonHolder.SetActive(false);
        index = 0;
        talkMode = true;
        textBox.text = string.Empty;
        textSpeaker.text = string.Empty;
        textSpeaker.text = talkName;
        gameObject.SetActive(true);
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        buttonHolder.SetActive(isChoice);
    }

    IEnumerator WaitNow()
    {
        yield return new WaitForSeconds(.1f);
        talkMode = false;
        gameObject.SetActive(false);
    }
}

//https://www.youtube.com/watch?v=8oTYabhj248