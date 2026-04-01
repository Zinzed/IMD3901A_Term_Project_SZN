using UnityEngine;

public class StarSelect : MonoBehaviour
{
    public StarNum number;
    public ConstellationPuzzle puzzle;

    private Renderer starRenderer;

    void Start()
    {
        // get renderer of the star
        starRenderer = GetComponent<Renderer>();

    }

    public void SelectStar()
    {
        puzzle.AddStar(transform.position, number.starID, starRenderer);
    }
}