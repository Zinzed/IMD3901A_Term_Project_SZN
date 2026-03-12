using UnityEngine;


public class StarSelect : MonoBehaviour
{
    public StarNum number;
    public ConstellationPuzzle puzzle;

    public void SelectStar()
    {
        puzzle.AddStar(transform.position, number.starID);
    }
}
