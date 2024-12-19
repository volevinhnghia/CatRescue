using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class FieldOfView : MonoBehaviour
{
    public float ViewRadius = 10f; // Distance of the FoV
    [Range(0, 360)]
    public float ViewAngle = 90f; // Angle of the cone

    public LayerMask TargetMask; // Layer for objects to detect
    public LayerMask ObstacleMask; // Layer for obstacles blocking vision

    public List<Transform> TargetsInFOV = new List<Transform>();// Track current targets

    public int Segments = 50;
    public Color ConeColor = new Color(1, 1, 0, 0.5f); // Semi-transparent yellow color

    private Mesh _mesh; // Mesh for the cone
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;

    private bool _showCone = false;


    void Start()
    {
        coneRenderInit();
        CatBehaviour.OnPickUpCat += CatBehaviour_OnPickUpCat;
    }

    void Update()
    {
        FindVisibleTargets();
        DrawCone();
    }
    private void OnDestroy()
    {
        CatBehaviour.OnPickUpCat -= CatBehaviour_OnPickUpCat;
    }
    private void coneRenderInit() 
    {
        // Add components if not already present
        _meshFilter = gameObject.AddComponent<MeshFilter>();
        _meshRenderer = gameObject.AddComponent<MeshRenderer>();

        // Create a transparent material using "Unlit/Transparent" shader
        Material coneMaterial = new Material(Shader.Find("Unlit/Transparent"));
        coneMaterial.color = new Color(1f, 1f, 0f, 0.5f); // Yellow with 50% transparency
        _meshRenderer.material = coneMaterial;

        _mesh = new Mesh();
        _mesh.name = "Field of View Mesh";
        _meshFilter.mesh = _mesh;
    }
    void FindVisibleTargets()
    {
        _showCone = false;
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, ViewRadius, TargetMask);
        // List to keep track of targets found this frame
        List<Transform> currentFrameTargets = new List<Transform>();

        foreach (Collider target in targetsInViewRadius)
        {
            //Null Exception
            if (target == null) continue;

            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < ViewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                // Check if there's an obstacle between player and target
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, ObstacleMask))
                {
                    currentFrameTargets.Add(target.transform);
                    _showCone = true;

                    if(!TargetsInFOV.Contains(target.transform))
                    {
                        target.gameObject.GetComponent<CatBehaviour>().FieldOfView_OnEnterFOV(target.transform);
                    }
                }
            }
        }

        //In case target out of Player FOV, call function restart coroutine
        foreach (Transform target in TargetsInFOV) 
        {
            //Null Exception
            if (target == null) continue;

            if (!currentFrameTargets.Contains(target)) {
                target.gameObject.GetComponent<CatBehaviour>().FieldOfView_OnExitFOV(target.transform);
            }
        }

        //Upload the list of target
        TargetsInFOV = currentFrameTargets;
    }
    //Remove Object in targetsInFOV after pick up a object
    private void CatBehaviour_OnPickUpCat(Transform obj)
    {
        if (TargetsInFOV.Contains(obj)) {
            TargetsInFOV.Remove(obj);
        }
    }
    void DrawCone()
    {
        if (!_showCone) {
            _mesh.Clear();
            return; 
        }
        int vertexCount = Segments + 2; // 1 for center, rest for arc
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(Segments) * 3];

        // Set center of the cone at the player position
        vertices[0] = Vector3.zero;

        float angleStep = ViewAngle / Segments;
        float angle = -ViewAngle / 2;

        // Generate vertices for the cone
        for (int i = 1; i <= Segments + 1; i++)
        {
            Vector3 direction = DirFromAngle(angle, true);
            vertices[i] = direction * ViewRadius;
            angle += angleStep;
        }

        // Generate triangles
        for (int i = 0; i < Segments; i++)
        {
            triangles[i * 3] = 0; 
            triangles[i * 3 + 1] = i + 1; 
            triangles[i * 3 + 2] = i + 2; 
        }

        // Update the mesh
        _mesh.Clear();
        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();
    }


    //Editor Debug
    /*void OnDrawGizmos()
    {
        // Visualize FoV cone
        Gizmos.color = Color.yellow;
        Vector3 forwardLine = DirFromAngle(-ViewAngle / 2, false) * ViewRadius;
        Vector3 backwardLine = DirFromAngle(ViewAngle / 2, false) * ViewRadius;

        Gizmos.DrawLine(transform.position, transform.position + forwardLine);
        Gizmos.DrawLine(transform.position, transform.position + backwardLine);
        Gizmos.DrawWireSphere(transform.position, ViewRadius);
    }*/

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
