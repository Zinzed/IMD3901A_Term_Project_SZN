using System.Collections.Generic;
using UnityEngine;



public class ConstellationPuzzle : MonoBehaviour
{
    //saves the correct order to connect the dots/stars
    public List<int> correctOrder = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
    //list that saves the order the player connected them in
    private List<int> playerInput = new List<int>();

    //draws lines between stars as player connects them
    public LineRenderer line;

    public progressBar playerProgress;

    //this function is called when a player interacts with a star
    public void AddStar(Vector3 pos, int id)
    {
        Debug.Log("Star num: " + id);
        //adds star id to list
        playerInput.Add(id);

        //increases the number of lines based on amount of interaction
        //checks if it is the first clicked point(to avoid drawing a line from origin to this point)
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
        //is all the stars have been clicked it checks if the order is right
        if (playerInput.Count == correctOrder.Count)
        {
            CheckSolution();
        }
    }

    void CheckSolution()
    {
        //loops through the dots position in the list and sees if they match, if not is resets the puzzle
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
            playerProgress.UpdateProgress(+10); // Increment progress
        }
        else
        {
            Debug.LogError("Player has no progress script!"); // Debug missing component
        }
    }
    //clears puzzle for the player to try again
    void ResetPuzzle()
    {
        playerInput.Clear();


        line.positionCount = 0;
    }
}
