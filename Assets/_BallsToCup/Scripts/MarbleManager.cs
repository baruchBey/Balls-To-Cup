using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Baruch
{
    public class MarbleManager : Singleton<MarbleManager>, IInit
    {

        [SerializeField] Shader _marbleShader;
        [SerializeField] Mesh _sphereMesh;
        [SerializeField]Material _grey;


        Material[] _materials;


        Transform[] _marbles;
        Matrix4x4[][] _matrices;

        Vector3 _positionCache;
        Quaternion _rotationCache;

        static readonly int _colorHash = Shader.PropertyToID("_Color");


        public void Init()
        {
            LevelManager.OnLevelBuild += LevelManager_OnLevelBuild;

        }

        private void LevelManager_OnLevelBuild()
        {
            _marbles = LevelManager.Instance.CurrentLevel.GetMarbleTransforms();
            int marbleCount = _marbles.Length;
            int colorsCount = ColorManager.ColorCount;
            int marblesPerColor = marbleCount / colorsCount;
            int extraMarbles = marbleCount % colorsCount;

            _matrices = new Matrix4x4[colorsCount][];
            _materials = new Material[colorsCount];
            Color[] colors = new Color[colorsCount];

            for (byte i = 0; i < colorsCount; i++)
            {
                colors[i] = ColorManager.Instance.GetColor(i);
                int marblesForThisColor = marblesPerColor + (i < extraMarbles ? 1 : 0);
                _matrices[i] = new Matrix4x4[marblesForThisColor];
                _materials[i] = new Material(_marbleShader)
                {
                    enableInstancing = true
                };
                _materials[i].SetColor(_colorHash, colors[i]);
            }
        }



        void Update()
        {
            RenderMarbles();

        }
        public bool IsSphereInViewFrustum(Vector3 sphereCenter, float sphereRadius, Camera camera)
        {
            Matrix4x4 vpMatrix = camera.projectionMatrix * camera.worldToCameraMatrix;
            Vector4 sphereCenterClip = vpMatrix * new Vector4(sphereCenter.x, sphereCenter.y, sphereCenter.z, 1.0f);

            // If the sphere is behind the camera, it's not in view
            if (sphereCenterClip.w < 0f)
                return false;

            // Convert the sphere center to normalized device coordinates (NDC)
            Vector3 sphereCenterNDC = new Vector3(sphereCenterClip.x, sphereCenterClip.y, sphereCenterClip.z) / sphereCenterClip.w;

            // Check if the sphere is outside the frustum
            float distanceSquared = sphereCenterNDC.x * sphereCenterNDC.x + sphereCenterNDC.y * sphereCenterNDC.y + sphereCenterNDC.z * sphereCenterNDC.z;
            float radiusNDC = sphereRadius / sphereCenterClip.w;

            return (distanceSquared + radiusNDC * radiusNDC) <= 1.0f;
        }
        private void RenderMarbles()
        {
            int marblesPerColor = _marbles.Length / ColorManager.ColorCount;
            int extraMarbles = _marbles.Length % ColorManager.ColorCount;

            for (int i = 0; i < ColorManager.ColorCount; i++)
            {
                int marbleStartIndex = i * marblesPerColor + Mathf.Min(i, extraMarbles);
                int marbleCount = marblesPerColor + (i < extraMarbles ? 1 : 0);

                for (int j = 0; j < marbleCount; j++)
                {
                    int marbleIndex = marbleStartIndex + j;
                    if (marbleIndex < _marbles.Length)
                    {
                        Transform marbleTransform = _marbles[marbleIndex];
                        marbleTransform.GetPositionAndRotation(out _positionCache, out _rotationCache);
                        Matrix4x4 matrix = Matrix4x4.TRS(_positionCache, _rotationCache, Vector3.one);
                        _matrices[i][j] = matrix;
                    }
                }
            }

            for (int i = 0; i < ColorManager.ColorCount; i++)
            {
                Graphics.DrawMeshInstanced(_sphereMesh, 0, _materials[i], _matrices[i]);
            }
        }
    }
}
