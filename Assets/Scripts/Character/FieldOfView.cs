using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius = 10f; // Distance of the FoV
    [Range(0, 360)]
    public float viewAngle = 90f; // Angle of the cone

    public LayerMask targetMask; // Layer for objects to detect
    public LayerMask obstacleMask; // Layer for obstacles blocking vision

    public List<Transform> targetsInFOV = new List<Transform>();// Track current targets

    public int segments = 50;
    public Color coneColor = new Color(1, 1, 0, 0.5f); // Semi-transparent yellow color

    private Mesh mesh; // Mesh for the cone
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

    private bool showCone = false;


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
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();

        // Create a transparent material using "Unlit/Transparent" shader
        Material coneMaterial = new Material(Shader.Find("Unlit/Transparent"));
        coneMaterial.color = new Color(1f, 1f, 0f, 0.5f); // Yellow with 50% transparency
        meshRenderer.material = coneMaterial;

        mesh = new Mesh();
        mesh.name = "Field of View Mesh";
        meshFilter.mesh = mesh;
    }
    void FindVisibleTargets()
    {
        showCone = false;
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        // List to keep track of targets found this frame
        List<Transform> currentFrameTargets = new List<Transform>();

        foreach (Collider target in targetsInViewRadius)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                // Check if there's an obstacle between player and target
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    currentFrameTargets.Add(target.transform);
                    showCone = true;

                    if(!targetsInFOV.Contains(target.transform))
                    {
                        target.gameObject.GetComponent<CatBehaviour>().FieldOfView_OnEnterFOV(target.transform);
                    }
                }
            }
        }
        foreach (Transform target in targetsInFOV) 
        {
            if (!currentFrameTargets.Contains(target)) {
                target.gameObject.GetComponent<CatBehaviour>().FieldOfView_OnExitFOV(target.transform);
            }
        }

        //Upload the list of target
        targetsInFOV = currentFrameTargets;
    }
    //Remove Object in targetsInFOV after pick up a object
    private void CatBehaviour_OnPickUpCat(Transform obj)
    {
        if (targetsInFOV.Contains(obj)) {
            targetsInFOV.Remove(obj);
        }
    }
    void DrawCone()
    {
        if (!showCone) {
            mesh.Clear();
            return; 
        }
        int vertexCount = segments + 2; // 1 for center, rest for arc
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(segments) * 3];

        // Set center of the cone at the player position
        vertices[0] = Vector3.zero;

        float angleStep = viewAngle / segments;
        float angle = -viewAngle / 2;

        // Generate vertices for the cone
        for (int i = 1; i <= segments + 1; i++)
        {
            Vector3 direction = DirFromAngle(angle, true);
            vertices[i] = direction * viewRadius;
            angle += angleStep;
        }

        // Generate triangles
        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0; 
            triangles[i * 3 + 1] = i + 1; 
            triangles[i * 3 + 2] = i + 2; 
        }

        // Update the mesh
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }


    //Editor Debug
    void OnDrawGizmos()
    {
        // Visualize FoV cone
        Gizmos.color = Color.yellow;
        Vector3 forwardLine = DirFromAngle(-viewAngle / 2, false) * viewRadius;
        Vector3 backwardLine = DirFromAngle(viewAngle / 2, false) * viewRadius;

        Gizmos.DrawLine(transform.position, transform.position + forwardLine);
        Gizmos.DrawLine(transform.position, transform.position + backwardLine);
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
