using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerWorld : MonoBehaviour
{
    [SerializeField] private int dialogueNumber;

    public int getDialogueNumber()
    {
        return dialogueNumber;
    }
}
