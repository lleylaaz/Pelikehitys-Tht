using UnityEngine;

public class Kauppias : Interactable
{
    [SerializeField] private string npcName = "Kauppias";
    [SerializeField]
    private string[] dialogueLines = {
        "Tervetuloa kauppaan!",
        "Meillä on tarjouksessa maagisia juomia.",
        "Haluatko ostaa jotain?"
    };

    public override void Interact()
    {
        Debug.Log("Kauppias aktivoitu!");
        DialogueSystem.Instance.AddNewDialogue(dialogueLines, npcName);
    }
}

