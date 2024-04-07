using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class GridMeshRenderer : MonoBehaviour
{
    public int GridSize;
    [SerializeField] private Camera _camera;
    [SerializeField] private Color _color;
    private int HalfSize;
    private MeshRenderer meshRenderer;
    
    void Awake()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        var mesh = new Mesh();
        var verticies = new List<Vector3>();

        var indicies = new List<int>();
        var HalfSize = GridSize / 2;
        for (int i = 0; i < GridSize; i++)
        {
            verticies.Add(new Vector3(i - HalfSize, -HalfSize, 0));
            verticies.Add(new Vector3(i - HalfSize, HalfSize, 0));

            indicies.Add(4 * i + 0);
            indicies.Add(4 * i + 1);

            verticies.Add(new Vector3(-HalfSize, i - HalfSize, 0));
            verticies.Add(new Vector3(HalfSize, i - HalfSize, 0));

            indicies.Add(4 * i + 2);
            indicies.Add(4 * i + 3);
        }

        mesh.vertices = verticies.ToArray();
        mesh.SetIndices(indicies.ToArray(), MeshTopology.Lines, 0);
        filter.mesh = mesh;
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
        meshRenderer.material.color = _color;
        meshRenderer.enabled = false;
    }

    void Update()
    {
        clampGridToCamera();
        toggleGridView();
    }

    void clampGridToCamera()
    {
        var _cameraPos = _camera.transform.position;
        _cameraPos.z = 0;

        _cameraPos.x = Mathf.Round(_cameraPos.x - HalfSize);
        _cameraPos.y = Mathf.Round(_cameraPos.y - HalfSize);
        transform.position = _cameraPos;

    }

    void toggleGridView()
    {
        if (Input.GetKeyDown(KeyCode.RightAlt) && meshRenderer.enabled)
        {
            meshRenderer.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightAlt))
        {
            meshRenderer.enabled = true;
        }
    }
}