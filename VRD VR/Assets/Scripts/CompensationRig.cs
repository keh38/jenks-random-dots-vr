using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompensationRig : MonoBehaviour
{
    GameObject _sceneQuad;

    TMPro.TextMeshProUGUI _infoLabel;

    float _cylinderHeight = 0.5f;
    float _cylinderAngle = 60f;

    float _cylinderRadius = 1.14f;

    ProjectorSettings _projectorSettings = new ProjectorSettings();

    float _screenWidth_wu;
    float _screenHeight_wu;
    float _aspectRatio;

    float[] _xvert;
    float[] _yvert;


    bool _isCalibrating = false;

    Camera _textureCamera;
    Camera _outputCamera;

    XboxCompensationController _xbox;

    private void Awake()
    {
        _sceneQuad = GameObject.Find("Compensation Rig/Scene/Quad");
        _textureCamera = GameObject.Find("Compensation Rig/Scene/Texture Camera").GetComponent<Camera>();
        _outputCamera = GameObject.Find("Projector Compensation/View Camera").GetComponent<Camera>();
        _xbox = gameObject.GetComponent<XboxCompensationController>();
    }

    void Start()
    {
        _outputCamera.enabled = false;

        _aspectRatio = (float)Screen.width / Screen.height;

        _screenHeight_wu = 2 * Mathf.Tan(0.5f * 60 * Mathf.Deg2Rad) * 1;
        _screenWidth_wu = _screenHeight_wu * _aspectRatio;

        CreateDefaultNormalizedMeshVertices(1, 1);

        _sceneQuad.GetComponent<MeshFilter>().mesh = InitializeMesh();
    }

    public void EnableCompensation(Transform parent)
    {
        _textureCamera.transform.parent = parent;

        _projectorSettings = ProjectorSettings.Restore();
        _isCalibrating = false;

        UpdateProjection();

        _outputCamera.enabled = true;
    }

    public void EnableCalibration(TMPro.TextMeshProUGUI label)
    {
        _projectorSettings = ProjectorSettings.Restore();

        _infoLabel = label;
        _isCalibrating = true;

        UpdateProjection();

        _outputCamera.enabled = true;
    }

    public void DisableCompensation()
    {
        _isCalibrating = false;
        _outputCamera.enabled = false;
    }

    public void SaveProjectorSettings()
    {
        _projectorSettings.Save();
    }

    void Update()
    {
        if (!_isCalibrating) return;

        var delta = _xbox.GetDeltas();

        if (delta.pitch != 0)
        {
            _projectorSettings.pitch += delta.pitch;
            UpdateProjection();
        }

        if (delta.height != 0)
        {
            _projectorSettings.yOffset += delta.height;
            UpdateProjection();
        }

        if (delta.offset != 0)
        {
            _projectorSettings.zOffset -= delta.offset;
            UpdateProjection();
        }

        if (delta.fov != 0)
        {
            _projectorSettings.fov += delta.fov;
            UpdateProjection();
        }
    }

    private void UpdateProjection()
    {
        ApplyHorizontalFOV(_projectorSettings.fov, _aspectRatio);
        ApplyCompensationMesh(_sceneQuad, _cylinderRadius, _projectorSettings.pitch, _projectorSettings.yOffset, _projectorSettings.zOffset, 1.05f, 1.05f / _aspectRatio);

        if (_isCalibrating)
        {
            UpdateDisplayText();
        }
    }

    private void UpdateDisplayText()
    {
        string text = "Pitch = " + _projectorSettings.pitch.ToString("F1") + "°" + System.Environment.NewLine;
        text += "Height = " + (100 * _projectorSettings.yOffset).ToString("F1") + " cm" + System.Environment.NewLine;
        text += "Offset = " + (100 * _projectorSettings.zOffset).ToString("F1") + " cm" + System.Environment.NewLine;
        text += "FOV = " + _projectorSettings.fov.ToString("F1") + "°";
        _infoLabel.text = text;
    }

    private void ApplyHorizontalFOV(float hfov, float aspectRatio)
    {
        float verticalFOV = Mathf.Rad2Deg * 2 * Mathf.Atan(Mathf.Tan((hfov * Mathf.Deg2Rad) / 2f) / aspectRatio);
        float orthoSize = Mathf.Tan(verticalFOV / 2 * Mathf.Deg2Rad);
        _textureCamera.orthographicSize = orthoSize;
    }

    private void ApplyCompensationMesh(GameObject quad, float radius, float pitch, float dh, float delta, float width, float height)
    {
        float cos_th = Mathf.Cos(pitch * Mathf.Deg2Rad);
        float sin_th = Mathf.Sin(pitch * Mathf.Deg2Rad);

        float Rc = radius;
        float Rp = Rc + delta;

        int nx = _xvert.Length;
        int ny = _yvert.Length;

        Vector3[] verts = new Vector3[nx * ny];

        float h = Rp * Mathf.Tan(pitch * Mathf.Deg2Rad) + dh;

        int iv = 0;
        for (int ky = 0; ky < ny; ky++)
        {
            float y = _yvert[ky] * height;

            for (int kx = 0; kx < nx; kx++)
            {
                float x = _xvert[kx] * width;

                float Kc = 1 / (Mathf.Sqrt(Rc * Rc - x * x) + delta);
                float xw = x;
                float yw = y - h;
                float zw = Rc;

                float v = (yw * Kc * Rp * cos_th + Rp * sin_th) / (cos_th - yw * Kc * sin_th);
                float u = xw * (v * sin_th + Rp * cos_th) * Kc;

                verts[iv] = new Vector3(u, v, 0);
                iv++;
            }
        }

        Mesh mesh = quad.GetComponent<MeshFilter>().mesh;
        mesh.vertices = verts;
    }


    private Mesh InitializeMesh()
    {
        int nx = _xvert.Length;
        int ny = _yvert.Length;

        Vector3[] verts = new Vector3[nx * ny];
        Vector2[] uv = new Vector2[nx * ny];

        int iv = 0;
        for (int ky = 0; ky < ny; ky++)
        {
            for (int kx = 0; kx < nx; kx++)
            {
                verts[iv] = new Vector3(_xvert[kx], _yvert[ky], 0);
                uv[iv] = new Vector2((float)kx / (nx - 1), (float)ky / (ny - 1));
                iv++;
            }
        }

        int ntri = 3 * 2 * (nx - 1) * (ny - 1);
        int[] triangles = new int[ntri];

        int itri = 0;
        for (int ky = 0; ky < ny - 1; ky++)
        {
            for (int kx = 0; kx < nx - 1; kx++)
            {
                triangles[itri++] = ky * nx + kx;
                triangles[itri++] = (ky + 1) * nx + kx + 1;
                triangles[itri++] = ky * nx + kx + 1;

                triangles[itri++] = (ky + 1) * nx + kx + 1;
                triangles[itri++] = ky * nx + kx;
                triangles[itri++] = (ky + 1) * nx + kx;

            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void CreateDefaultNormalizedMeshVertices(float width, float height)
    {
        int nx = 51;
        float dx = width / (nx - 1);

        int ny = Mathf.RoundToInt(height / dx) + 1;
        float dy = height / (ny - 1);

        _xvert = new float[nx];
        _yvert = new float[ny];

        for (int ky = 0; ky < ny; ky++)
        {
            float y = -height / 2 + ky * dy;
            _yvert[ky] = y;
        }

        for (int kx = 0; kx < nx; kx++)
        {
            float x = -width / 2 + kx * dx;
            _xvert[kx] = x;
        }
    }

}
