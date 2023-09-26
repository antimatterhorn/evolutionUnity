
using UnityEngine;

[CreateAssetMenu(fileName = "New KDTree", menuName = "Scriptable Objects/Tree")]
public class KDTreeObject : ScriptableObject
{
    public KdTree<Transform> tree = new KdTree<Transform>();
}
