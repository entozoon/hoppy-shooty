using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour {
  Mesh mesh;
  MeshCollider meshCollider;
  Vector3[] vertices;
  int[] triangles;
  private int sizeX = 40;
  private int sizeZ = 40;
  private int maxY = 10;
  public Gradient gradient;
  void Start() {
    mesh = GetComponent<MeshFilter>().mesh;
    meshCollider = GetComponent<MeshCollider>();
    MeshMash();
  }
  void MeshMash() {
    mesh.Clear();
    vertices = new Vector3[(sizeX + 1) * (sizeZ + 1)];
    Color[] colors = new Color[vertices.Length];
    for (int z = 0, i = 0; z <= sizeZ; z++) {
      for (int x = 0; x <= sizeX; x++) {
        // float y = (i % 3f) / 3f * maxY;
        // float y = Random.Range(0, maxY);
        float y = 0;
        if (x > sizeX * .4 && x < sizeX * .6 &&
           z > sizeZ * .4 && z < sizeZ * .6) {
          y = 0;
        } else if (x == 1 || z == 1 || x == sizeX - 1 || z == sizeZ - 1) {
          y = 0;
        } else if (x == 0 || z == 0 || x == sizeX || z == sizeZ) {
          y = 30;
        } else if (x < sizeX * .5) {
          y = Mathf.PerlinNoise(x * .3f, z * .3f) * maxY * 1.5f;
        } else if (x > sizeX * .5) {
          y = (i % 3f) / 3f * maxY;
        }
        vertices[i] = new Vector3(x, y, z);
        colors[i] = gradient.Evaluate(y / maxY);
        i++;
      }
    }
    triangles = new int[sizeX * sizeZ * 6];
    int vert = 0;
    int tris = 0;
    for (int z = 0; z < sizeZ; z++) {
      for (int x = 0; x < sizeX; x++) {
        triangles[tris + 0] = vert + 0;
        triangles[tris + 1] = vert + sizeX + 1;
        triangles[tris + 2] = vert + 1;
        triangles[tris + 3] = vert + 1;
        triangles[tris + 4] = vert + sizeX + 1;
        triangles[tris + 5] = vert + sizeX + 2;
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