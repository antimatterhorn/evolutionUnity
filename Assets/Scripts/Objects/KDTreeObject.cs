
using UnityEngine;

[CreateAssetMenu(fileName = "New KDTree", menuName = "Tree")]
public class KDTreeObject : ScriptableObject
{
    public KdTree<Transform> tree = new KdTree<Transform>();
}
