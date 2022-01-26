using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour {
  Mesh mesh;
  MeshCollider meshCollider;
  Vector3[] vertices;
  int[] triangles;
  private int xSize = 40;
  private int zSize = 40;
  private int yMax = 10;
  public Gradient gradient;
  void Start() {
    mesh = GetComponent<MeshFilter>().mesh;
    meshCollider = GetComponent<MeshCollider>();
    MeshMash();
  }
  void MeshMash() {
    mesh.Clear();
    vertices = new Vector3[(xSize + 1) * (zSize + 1)];
    Color[] colors = new Color[vertices.Length];
    for (int i = 0, z = 0; z <= zSize; z++) {
      for (int x = 0; x <= xSize; x++) {
        // float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 20f;
        float y = Random.Range(0, yMax);
        vertices[i] = new Vector3(x, y, z);
        colors[i] = gradient.Evaluate(y / yMax);
        i++;
      }
    }
    triangles = new int[xSize * zSize * 6];
    int vert = 0;
    int tris = 0;
    for (int z = 0; z < zSize; z++) {
      for (int x = 0; x < xSize; x++) {
        triangles[tris + 0] = vert + 0;
        triangles[tris + 1] = vert + xSize + 1;
        triangles[tris + 2] = vert + 1;
        triangles[tris + 3] = vert + 1;
        triangles[tris + 4] = vert + xSize + 1;
        triangles[tris + 5] = vert + xSize + 2;
        vert++;
        tris += 6;
      }
      vert++;
    }
    mesh.vertices = vertices;
    mesh.triangles = triangles;
    // meshCollider.cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation | MeshColliderCookingOptions.EnableMeshCleaning | MeshColliderCookingOptions.WeldColocatedVertices | MeshColliderCookingOptions.UseFastMidphase;
    meshCollider.convex = false;
    meshCollider.sharedMesh = mesh;
    meshCollider.enabled = true;
    // Optimisation: colors32 is more efficient apparently
    mesh.colors = colors;
    //
    mesh.RecalculateNormals();
    mesh.RecalculateBounds(); // what does this do again?
    //
    // Optimisation: All this stuff can and needs to be optimised all to shit when updating terrain, like only affecting necessary colours and whathaveyou
  }
  private void OnDrawGizmos() {
    if (vertices == null) return;
    // for (int i = 0; i < vertices.Length; i++) {
    //   Gizmos.DrawSphere(vertices[i], 0.1f);
    // }
  }
}