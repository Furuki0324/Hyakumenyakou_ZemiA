using UnityEngine;
using System.Collections;

public class VoiceTest : MonoBehaviour {

 public Material material;

 void Start()
 {
  Vector3[] vertices = {
   new Vector3(-1f, -1f, 0),
   new Vector3(-1f,  1f, 0),
   new Vector3( 1f,  1f, 0),
   new Vector3( 1f, -1f, 0)
  };

  int[] triangles = { 0, 1, 2, 0, 2, 3 };

  Mesh mesh = new Mesh();
  mesh.vertices = vertices;
  mesh.triangles = triangles;
  
  mesh.RecalculateNormals();

  MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
  if(!meshFilter) meshFilter = gameObject.AddComponent<MeshFilter>();

  MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
  if(!meshRenderer) meshRenderer = gameObject.AddComponent<MeshRenderer>();

  meshFilter.mesh = mesh;
  meshRenderer.sharedMaterial = material;
 }
}