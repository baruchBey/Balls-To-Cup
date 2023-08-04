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

        Material[] _materials;

        Transform[] _marbles;
        Matrix4x4[][] matrices;

        Vector3 _positionCache;
        Quaternion _rotationCache;

        static readonly int _gradientSampleHash = Shader.PropertyToID("_GradientSample");
        public void Init()
        {
            LevelManager.OnLevelBuild += LevelManager_OnLevelBuild;
        }

        private void LevelManager_OnLevelBuild()
        {
            _marbles = LevelManager.Instance.CurrentLevel.GetMarbles();

            matrices = new Matrix4x4[6][];
            for (int i = 0; i < matrices.Length; i++)
                matrices[i] = new Matrix4x4[_marbles.Length];

            _materials = new Material[6];
            for (int i = 0; i < _materials.Length; i++)
            {
                _materials[i] = new(_marbleShader)
                {
                    enableInstancing = true
                };
                _materials[i].SetFloat(_gradientSampleHash, (float)i / (6 - 1));
            }
        }

     

        void Update()
        {
            for (int i = 0; i < _marbles.Length; i++)
            {
                Transform marbleTransform = _marbles[i];
                marbleTransform.GetPositionAndRotation(out _positionCache,out _rotationCache);
                Matrix4x4 matrix = Matrix4x4.TRS(_positionCache, _rotationCache, Vector3.one);
                matrices[i % 6][i] = matrix;
            }

            for (int i = 0; i < 6; i++)
            {
                Graphics.DrawMeshInstanced(_sphereMesh, 0, _materials[i], matrices[i]);

            }

        }

       
    }
}
