using UnityEngine;

namespace Code.Gameplay.Common.View.UI
{
    public class LineRendererCollider : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private MeshCollider _meshCollider;
        [SerializeField] private float squareSize;

        public void UpdateMeshCollider()
        { 
            Mesh mesh = new Mesh();

            int pointsCount = _lineRenderer.positionCount;
            Vector3[] vertices = new Vector3[pointsCount * 4];
            int[] triangles = new int[(pointsCount - 1) * 24];

            for (int i = 0; i < pointsCount; i++)
            {
                Vector3 point = _lineRenderer.GetPosition(i);
                Vector3 up = Vector3.up * squareSize / 2;
                Vector3 right = Vector3.right * squareSize / 2;
                
                vertices[i * 4] = point + up + right;
                vertices[i * 4 + 1] = point + up - right;
                vertices[i * 4 + 2] = point - up - right;
                vertices[i * 4 + 3] = point - up + right;
            }
            
            for (int i = 0; i < pointsCount - 1; i++)
            {
                int vertIndex = i * 4;
                int triIndex = i * 24;
                
                triangles[triIndex] = vertIndex;
                triangles[triIndex + 1] = vertIndex + 4;
                triangles[triIndex + 2] = vertIndex + 1;
                triangles[triIndex + 3] = vertIndex + 1;
                triangles[triIndex + 4] = vertIndex + 4;
                triangles[triIndex + 5] = vertIndex + 5;
                
                triangles[triIndex + 6] = vertIndex + 2;
                triangles[triIndex + 7] = vertIndex + 6;
                triangles[triIndex + 8] = vertIndex + 3;
                triangles[triIndex + 9] = vertIndex + 3;
                triangles[triIndex + 10] = vertIndex + 6;
                triangles[triIndex + 11] = vertIndex + 7;
                
                triangles[triIndex + 12] = vertIndex;
                triangles[triIndex + 13] = vertIndex + 3;
                triangles[triIndex + 14] = vertIndex + 4;
                triangles[triIndex + 15] = vertIndex + 4;
                triangles[triIndex + 16] = vertIndex + 3;
                triangles[triIndex + 17] = vertIndex + 7;
                
                triangles[triIndex + 18] = vertIndex + 1;
                triangles[triIndex + 19] = vertIndex + 5;
                triangles[triIndex + 20] = vertIndex + 2;
                triangles[triIndex + 21] = vertIndex + 2;
                triangles[triIndex + 22] = vertIndex + 5;
                triangles[triIndex + 23] = vertIndex + 6;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            mesh.RecalculateNormals();
            
            _meshCollider.sharedMesh = null;
            _meshCollider.sharedMesh = mesh;
        }
    }
}
