using System.Collections.Generic;
using UnityEngine;

public class ConstellationPuzzle : MonoBehaviour
{
    // saves the correct order to connect the dots/stars
    public List<int> correctOrder = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };

    // list that saves the order the player connected them in
    private List<int> playerInput = new List<int>();

    // draws lines between stars as player connects them
    public LineRenderer line;

    public progressBar playerProgress;

    [Header("Star Materials")]
    public Material yellowLightStars;

    // saves clicked star renderers in order
    private List<Renderer> clickedStarRenderers = new List<Renderer>();

    // saves each star's original material
    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>();

    // this function is called when a player interacts with a star
    public void AddStar(Vector3 pos, int id, Renderer starRenderer)
    {
        Debug.Log("Star num: " + id);

        // stop the same star from being clicked again
        if (playerInput.Contains(id))
            return;

        // adds star id to list
        playerInput.Add(id);

        // save renderer if not already saved
        if (starRenderer != null)
        {
            clickedStarRenderers.Add(starRenderer);

            if (!originalMaterials.ContainsKey(starRenderer))
            {
                originalMaterials.Add(starRenderer, starRenderer.material);
            }

            // change star material to yellow/light material
            starRenderer.material = yellowLightStars;
        }

        // increases the number of lines based on amount of interaction
        // checks if it is the first clicked point
        if (line.positionCount == 0)
        {
            line.positionCount = 1;
            line.SetPosition(0, pos);
        }
        else
        {
            int count = line.positionCount;
            line.positionCount = count + 1;
            line.SetPosition(count, pos);
        }

        // if all the stars have been clicked, check solution
        if (playerInput.Count == correctOrder.Count)
        {
            CheckSolution();
        }
    }

    void CheckSolution()
    {
        // loops through the dots position in the list and sees if they match
        for (int i = 0; i < correctOrder.Count; i++)
        {
            Debug.Log("Expected: " + correctOrder[i] + " | Player: " + playerInput[i]);

            if (playerInput[i] != correctOrder[i])
            {
                Debug.Log($"Mismatch at position {i}");
                ResetPuzzle();
                return;
            }
        }

        Debug.Log("yayyy");

        if (playerProgress != null)
        {
            playerProgress.UpdateProgress(+10);
        }
        else
        {
            Debug.LogError("Player has no progress script!");
        }
    }

    // clears puzzle for the player to try again
    void ResetPuzzle()
    {
        playerInput.Clear();

        // reset clicked stars back to original materials
        foreach (Renderer starRenderer in clickedStarRenderers)
        {
            if (starRenderer != null && originalMaterials.ContainsKey(starRenderer))
            {
                starRenderer.material = originalMaterials[starRenderer];
            }
        }

        clickedStarRenderers.Clear();
        originalMaterials.Clear();

        line.positionCount = 0;
    }
}
