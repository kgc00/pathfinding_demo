using UnityEngine;

[System.Serializable]
public class Monster : Unit
{
    private Movement movement;
    [SerializeField] public UnitTypes TypeReference;

    public void Initialize(Board board, Point pos, UnitTypes r)
    {
        Position = pos;
        Board = board;
        TypeReference = r;
        movement = gameObject.AddComponent<Movement>();
    }
}