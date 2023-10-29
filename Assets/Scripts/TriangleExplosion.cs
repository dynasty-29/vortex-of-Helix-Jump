using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleExplosion : MonoBehaviour
{
    private MeshFilter meshFilter;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private MeshRenderer meshRenderer;
    private Collider collider;
    private int particleLayer = -1;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        meshRenderer = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();

        particleLayer = LayerMask.NameToLayer("Particle");
        if (particleLayer == -1)
        {
            Debug.LogError("Layer 'Particle' not found!");
        }
    }

    public IEnumerator SplitMesh(bool destroy = true)
    {
        Mesh M = null;
        Material[] materials = new Material[0];

        if (meshFilter)
        {
            M = meshFilter.mesh;
        }
        else if (skinnedMeshRenderer)
        {
            M = skinnedMeshRenderer.sharedMesh;
        }

        if (meshRenderer)
        {
            materials = meshRenderer.materials;
        }
        else if (skinnedMeshRenderer)
        {
            materials = skinnedMeshRenderer.materials;
        }

        if (collider)
        {
            collider.enabled = false;
        }

        Vector3[] verts = M.vertices;
        Vector3[] normals = M.normals;
        Vector2[] uvs = M.uv;
        for (int submesh = 0; submesh < M.subMeshCount; submesh++)
        {
            int[] indices = M.GetTriangles(submesh);
            for (int i = 0; i < indices.Length; i += 3)
            {
                Vector3[] newVerts = new Vector3[3];
                Vector3[] newNormals = new Vector3[3];
                Vector2[] newUv = new Vector2[3];
                for (int n = 0; n < 3; n++)
                {
                    int index = indices[i + n];
                    newVerts[n] = verts[index];
                    newNormals[n] = normals[index];
                    newUv[n] = uvs[index];
                }

                Mesh mesh = new Mesh();
                mesh.vertices = newVerts;
                mesh.normals = newNormals;
                mesh.uv = newUv;
                mesh.triangles = new int[] { 0, 1, 2, 2, 1, 0 };

                GameObject GO = new GameObject("Triangle " + (i / 3));
                GO.layer = particleLayer;
                GO.transform.position = transform.position;
                GO.transform.rotation = transform.rotation;
                GO.AddComponent<MeshRenderer>().material = materials[submesh];
                GO.AddComponent<MeshFilter>().mesh = mesh;
                GO.AddComponent<Rigidbody>().AddExplosionForce(Random.Range(300, 500), transform.position, 5);
                Destroy(GO, 5 + Random.Range(0.0f, 5.0f));

                if (i % 50 == 0) // Yield every 50 triangles to spread the load
                {
                    yield return null;
                }
            }
        }

        if (GetComponent<Renderer>())
        {
            GetComponent<Renderer>().enabled = false;
        }

        if (destroy)
        {
            Destroy(gameObject, 1);
        }
    }
}

