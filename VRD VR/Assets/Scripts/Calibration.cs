using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibration : MonoBehaviour
{
    CompensationRig _compensationRig;

    float _cylinderHeight = 0.5f;
    float _cylinderAngle = 60f;

    float _cylinderRadius = 1.14f;

    TMPro.TextMeshProUGUI _messageLabel;

    bool _promptShowing = false;
    bool _exitPending = false;

    private void Awake()
    {
        _compensationRig = GameObject.Find("Compensation Rig").GetComponent<CompensationRig>();
        _messageLabel = GameObject.Find("Canvas/Message").GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Start()
    {
        GameObject.Find("Calibration/Cylinder").GetComponent<MeshFilter>().mesh = CreateCylinderMesh(_cylinderRadius, _cylinderHeight, _cylinderAngle);
        var label = GameObject.Find("Canvas/Info").GetComponent<TMPro.TextMeshProUGUI>();
        _compensationRig.EnableCalibration(label);
    }

    private void Update()
    {
        if (_exitPending) return;

        if (Input.GetButtonUp("XboxB"))
        {
            if (_promptShowing)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Visual Scene", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
            else
            {
                _compensationRig.DisableCompensation();
                _messageLabel.text = "Press <color=\"green\"><b><size=120%> A </color></size> to save projector settings" + System.Environment.NewLine +
                    "Press <color=\"red\"><b><size=120%> B </color></size> to quit without saving";
                _promptShowing = true;
            }
        }

        if (Input.GetButtonUp("XboxA") && _promptShowing)
        {
            _exitPending = true;
            _compensationRig.SaveProjectorSettings();
            StartCoroutine(ShowMessageAndExit());
        }
    }

    IEnumerator ShowMessageAndExit()
    {
        _messageLabel.text = "Projector settings saved.";
        yield return new WaitForSeconds(2);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Visual Scene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    private Mesh CreateCylinderMesh(float radius, float height, float angle)
    {
        int ns = 181;
        float angleStep = angle / ns;


        int ny = 2;

        float dy = height / (ny - 1);

        Vector3[] verts = new Vector3[ns * ny];
        Vector2[] uv = new Vector2[ns * ny];

        int iv = 0;
        for (int ky = 0; ky < ny; ky++)
        {
            float y = -0.5f * height + ky * dy;
            for (int kx = 0; kx < ns; kx++)
            {
                float theta = 90 + angle / 2 - kx * angleStep;
                float x = radius * Mathf.Cos(theta * Mathf.Deg2Rad);
                float z = radius * Mathf.Sin(theta * Mathf.Deg2Rad);

                verts[iv] = new Vector3(x, y, z - 1);
                uv[iv] = new Vector2((float)kx / (ns - 1), (float)ky / (ny - 1));
                iv++;
            }
        }

        int ntri = 3 * 2 * (ns - 1) * (ny - 1);
        int[] triangles = new int[ntri];

        int itri = 0;
        for (int ky = 0; ky < ny - 1; ky++)
        {
            for (int kx = 0; kx < ns - 1; kx++)
            {
                triangles[itri++] = ky * ns + kx;
                triangles[itri++] = (ky + 1) * ns + kx + 1;
                triangles[itri++] = ky * ns + kx + 1;

                triangles[itri++] = (ky + 1) * ns + kx + 1;
                triangles[itri++] = ky * ns + kx;
                triangles[itri++] = (ky + 1) * ns + kx;

            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }


}
