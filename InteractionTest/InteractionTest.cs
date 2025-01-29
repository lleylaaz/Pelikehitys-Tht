using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionTest : MonoBehaviour
{
    private void Update()
    {
        GetInteraction();
    }

    // Metodi tarkistaa onko objekti vuorovaikutteinen, johon hiirellä napautetaan
    private void GetInteraction()
    {
        // Luetaan hiiren sijainti näytöllä
        Vector3 mousePosition = Mouse.current.position.ReadValue();

        // Luodään säde joka kulkee kamerasta hiiren sijaintiin
        Ray interactionRay = Camera.main.ScreenPointToRay(mousePosition);

        // Kohde johon säde osuus
        RaycastHit interactionInfo;

        // Tutkitaan osuuko säde johonkin objektiin
        if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity))
        {
            // Objekti, jonka collideriin säde osuu
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
