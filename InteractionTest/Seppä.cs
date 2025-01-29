using UnityEngine;

public class Seppä : Interactable
{
    [SerializeField] private string npcName = "Seppä";
    [SerializeField]
    private string[] dialogueLines = {
        "Terve, matkalainen!",
        "Täällä takon maailman vahvimpia miekkoja.",
        "Tarvitsetko uuden aseen tai haarniskan?"
    };

    public override void Interact()
    {
        Debug.Log("Seppä aktivoitu!");
        DialogueSystem.Instance.AddNewDialogue(dialogueLines, npcName);
    }
}

