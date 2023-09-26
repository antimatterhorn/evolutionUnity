using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName = "Food")]
public class FoodObject : ScriptableObject
{
    public Sprite sprite;
    public bool causesReproduction;
    public float changeAge;

    //public KdTree<Transform> tree;
}
