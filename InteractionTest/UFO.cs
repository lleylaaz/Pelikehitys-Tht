using UnityEngine;

public class Ufo : Interactable
{
    [SerializeField] private string ufoName = "Xargon";
    [SerializeField]
    private string[] dialogueLines = {
        "Hei maan asukas!",
        "Tulen ulkoavaruudesta.",
        "Haluatko ett� s�det�n sinut?"
    };

    public override void Interact()
    {
        Debug.Log("UFO aktivoitu!");
        DialogueSystem.Instance.AddNewDialogue(dialogueLines, ufoName);
    }
}


