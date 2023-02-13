using UnityEngine;

namespace Code.Scripts.Decrepated
{
    public class SetPlanetUV : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log($"set mesh uv to 0");
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            mesh.uv = new Vector2[mesh.vertexCount];
        }
    }
}
