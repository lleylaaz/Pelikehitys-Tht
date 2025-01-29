using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionTest : MonoBehaviour
{
    private void Update()
    {
        GetInteraction();
    }

    // Metodi tarkistaa onko objekti vuorovaikutteinen, johon hiirell� napautetaan
    private void GetInteraction()
    {
        // Luetaan hiiren sijainti n�yt�ll�
        Vector3 mousePosition = Mouse.current.position.ReadValue();

        // Luod��n s�de joka kulkee kamerasta hiiren sijaintiin
        Ray interactionRay = Camera.main.ScreenPointToRay(mousePosition);

        // Kohde johon s�de osuus
        RaycastHit interactionInfo;

        // Tutkitaan osuuko s�de johonkin objektiin
        if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity))
        {
            // Objekti, jonka collideriin s�de osuu
            GameObject interactableObject = interactionInfo.collider.gameObject;

            // Tarkistetaan onko objekti vuorovaikutteinen
            if (interactableObject.GetComponent<Interactable>() != null)
            {
                // Jos oli, niin kutsutaan objektin Interact() -metodia
                interactableObject.GetComponent<Interactable>().Interact();
            }
            else
            {
                print("Objekti ei ole vuorovaikutteinen");
            }
        }
    }
}
