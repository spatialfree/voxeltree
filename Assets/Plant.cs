using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
  public Transform cameraMount;

  public Material material;
  public Mesh meshCube;

  Voxel baseVoxel;
  public class Voxel
  {
    public int state;
    public Voxel[] inside;

    public Voxel (int state)
    {
      this.state = state;
    }
  }

  void Start()
  {
    baseVoxel = new Voxel(1);

    for (int x = 0; x < 2; x++)
    {
      for (int y = 0; y < 2; y++)
      {
        for (int z = 0; z < 2; z++)
        {
          renderPos.Add(new Vector3(x, y, z));
        }
      }
    }
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.R))
    {
      baseVoxel = new Voxel(1);
    }

    if (Input.GetKey(KeyCode.Space))
    {
      NewInside(baseVoxel, 64);
    }

    Render(baseVoxel, Vector3.zero, 64);

    if (Time.time > rotTime)
    {
      toRot = Random.rotation;
      rotTime = Time.time + 3;
    }
    cameraMount.rotation = Quaternion.Slerp(cameraMount.rotation, toRot, Time.deltaTime);
  }
  Quaternion toRot;
  float rotTime;

  void NewInside(Voxel voxel, int scale)
  {
    if (voxel.state == 0 || scale == 2) { return; }
    if (voxel.inside != null)
    {
      NewInside(voxel.inside[Random.Range(0, 8)], scale / 2);
    }
    else
    {
      Grow(voxel);
    }
  }

  void Grow(Voxel voxel)
  {
    voxel.inside = new Voxel[8];
    for (int i = 0; i < voxel.inside.Length; i++)
    {
      Voxel v = voxel.inside[i] = new Voxel(Random.Range(0, 3));
      v.inside = null;
    }
  }


  // Copy -> Paste the Render Method (no time to explain just yet...)
  // scale steps: 64x 32x 16x 8x 4x 2x
  Matrix4x4 m4 = new Matrix4x4();
  List<Vector3> renderPos = new List<Vector3>();
  void Render(Voxel voxel, Vector3 drawPosition, int scale)
  {
    if (voxel.inside == null && voxel.state > 0)
    {
      m4.SetTRS(
        drawPosition,
        Quaternion.identity, 
        Vector3.one * scale
      );
      Graphics.DrawMesh(meshCube, m4, material, 0);
    }

    if (voxel.inside != null)
    {
      for (int i = 0; i < voxel.inside.Length; i++)
      {
        Vector3 offset = Vector3.one * (scale / 4);
        Render(
          voxel.inside[i], 
          drawPosition - offset + renderPos[i] * (scale / 2), 
          scale / 2
        );
      }
    }
  }

  // some way to preserve the bigger voxels, a hierarchy not a takeover
  // generate mesh for mesh collider

  // void RenderMesh(Voxel voxel, Vector3 drawPosition, int scale)
  // {

  // }

  // void Face(Vector3 dir, List<Vector3> vertices, List<int> triangles, List<> )
  // {
    
  // }
}
