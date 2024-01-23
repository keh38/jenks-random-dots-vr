using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheFullMonty : MonoBehaviour
{
    GameObject _cylinder;
    GameObject _refCylinder;

    GameObject _sceneRenderQuad;
    GameObject _simulatedProjection;

    TMPro.TextMeshProUGUI _infoLabel;

    float _cylinderHeight = 1f;
    float _cylinderAngle = 60f;

    float _cylinderRadius = 1.14f;

    float _actualPitch = 36.5f;
    float _actualYOffset = 0;
    float _actualZOffset = 0.3f;

    float _compensationPitch = 0f;
    float _compensationHeight = 0f;
    float _compensationOffset = 0f;
    float _horizontalFOV = 51f;

    float _screenWidth_wu;
    float _screenHeight_wu;
    float _aspectRatio;

    float[] _xvert;
    float[] _yvert;

    XboxCompensationController _xbox;

    private void Awake()
    {
        _cylinder = GameObject.Find("Scene/Cylinder");
        _refCylinder = GameObject.Find("Simulated Projection/Cylinder");

        _sceneRenderQuad = GameObject.Find("Scene Render/Quad");
        _simulatedProjection = GameObject.Find("Simulated Projection/Quad");

        _infoLabel = GameObject.Find("Canvas/Info").GetComponent<TMPro.TextMeshProUGUI>();

        _xbox = gameObject.GetComponent<XboxCompensationController>();
    }

    void Start()
    {
        Application.targetFrameRate = 60;
 
        _aspectRatio = (float) Screen.width / Screen.height;

        _screenHeight_wu = 2 * Mathf.Tan(0.5f * 60 * Mathf.Deg2Rad) * 1;
        _screenWidth_wu = _screenHeight_wu * _aspectRatio;

        CreateDefaultNormalizedMeshVertices(1, 1);

        _cylinder.GetComponent<MeshFilter>().mesh = CreateCylinderMesh(_cylinderRadius, _cylinderHeight, _cylinderAngle);
        _refCylinder.GetComponent<MeshFilter>().mesh = CreateCylinderMesh(_cylinderRadius, 3, 120);

        _sceneRenderQuad.GetComponent<MeshFilter>().mesh = InitializeMesh();
        _simulatedProjection.GetComponent<MeshFilter>().mesh = InitializeMesh();

        ApplyProjectionMesh(_simulatedProjection, _cylinderRadius, _actualPitch, _actualYOffset, _actualZOffset, 1.05f, 1.05f / _aspectRatio);

        UpdateProjection();
    }

    void Update()
    {
        if (Input.GetButtonUp("XboxB"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Visual Scene", UnityEngine.SceneManagement.LoadSceneMode.Single);
            return;
        }

        var delta = _xbox.GetDeltas();

        if (delta.pitch != 0)
        {
            _compensationPitch += delta.pitch;
            UpdateProjection();
        }

        if (delta.height != 0)
        {
            _compensationHeight += delta.height;
            UpdateProjection();
        }

        if (delta.offset != 0)
        {
            _compensationOffset -= delta.offset;
            UpdateProjection();
        }

        if (delta.fov != 0)
        {
            _horizontalFOV += delta.fov;
            UpdateProjection();
        }
    }

    private void UpdateProjection()
    {
        ApplyHorizontalFOV(_horizontalFOV, _aspectRatio);
        ApplyCompensationMesh(_sceneRenderQuad, _cylinderRadius, _compensationPitch, _compensationHeight, _compensationOffset, 1.05f, 1.05f / _aspectRatio);
        UpdateDisplayText();
    }

    private void UpdateDisplayText()
    { 
        string text = "Pitch = " +  _compensationPitch.ToString("F1") + "°" + System.Environment.NewLine;
        text += "Height = " + (100 * _compensationHeight).ToString("F1") + " cm" + System.Environment.NewLine;
        text += "Offset = " + (100 * _compensationOffset).ToString("F1") + " cm" + System.Environment.NewLine;
        text += "FOV = " + _horizontalFOV.ToString("F1") + "°";
        _infoLabel.text = text;
    }

    private void ApplyHorizontalFOV(float hfov, float aspectRatio)
    {
        float verticalFOV = Mathf.Rad2Deg * 2 * Mathf.Atan(Mathf.Tan((hfov * Mathf.Deg2Rad) / 2f) / aspectRatio);
        float orthoSize = Mathf.Tan(verticalFOV / 2 * Mathf.Deg2Rad);
        GameObject.Find("Scene/View Camera").GetComponent<Camera>().fieldOfView = verticalFOV;
        GameObject.Find("Scene/Texture Camera").GetComponent<Camera>().orthographicSize = orthoSize;
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

                float v = (yw*Kc*Rp * cos_th + Rp * sin_th) / (cos_th - yw * Kc * sin_th);
                float u = xw * (v * sin_th + Rp * cos_th) * Kc;

                verts[iv] = new Vector3(u, v, 0);
                iv++;
            }
        }

        Mesh mesh = quad.GetComponent<MeshFilter>().mesh;
        mesh.vertices = verts;
    }

    private void ApplyProjectionMesh(GameObject quadObject, float radius, float pitch, float dh, float delta, float width, float height)
    {
        pitch *= Mathf.Deg2Rad;

        int nx = _xvert.Length;
        int ny = _yvert.Length;

        float cos_th = Mathf.Cos(pitch);
        float sin_th = Mathf.Sin(pitch);

        float Rc = radius;
        float Rp = Rc + delta;

        float c = delta * delta - Rc * Rc;

        Vector3[] verts = new Vector3[nx * ny];

        float h = Rp * Mathf.Tan(pitch) + dh;

        int iv = 0;
        for (int ky = 0; ky < ny; ky++)
        {
            float y0 = _yvert[ky] * height;

            float yr = y0 * cos_th - Rp * sin_th;
            float zr = y0 * sin_th + Rp * cos_th;// + delta;

            float b = delta * zr;

            for (int kx = 0; kx < nx; kx++)
            {
                float x0 = _xvert[kx] * width;
                float xr = x0;
                float a = xr * xr + zr * zr;

                float K = (b + Mathf.Sqrt(b * b - a * c)) / a;

                float xw = K * xr;
                float yw = K * yr + h;
                float zw = K * zr - delta;

                verts[iv] = new Vector3(xw, yw, zw - 1);
                iv++;
            }
        }
        Mesh mesh = quadObject.GetComponent<MeshFilter>().mesh;
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
                float theta = 90 + angle/2 - kx * angleStep;
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
