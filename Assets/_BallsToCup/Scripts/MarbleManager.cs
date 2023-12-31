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
        [SerializeField] Material _grey;


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
            if (LevelManager.Active)
                RenderMarbles();

        }


        private void RenderMarbles()
        {

            for (int i = 0; i < _marbles.Length; i++)
            {
                Transform marbleTransform = _marbles[i];
                marbleTransform.GetPositionAndRotation(out _positionCache, out _rotationCache);

                float marbleSize = Level.MarbleSize * marbleTransform.localScale.z;//Using localscale.z to individually scale marble renders
                Matrix4x4 matrix = Matrix4x4.TRS(_positionCache, _rotationCache, marbleSize * Vector3.one);
                _matrices[i % ColorManager.ColorCount][i / ColorManager.ColorCount] = matrix;
            }


            for (int i = 0; i < ColorManager.ColorCount; i++)
                for (int j = 0; j < _matrices[i].Length; j++)
                {
                    bool isWasted = (_matrices[i][j][2, 2] < 0);
                    Graphics.DrawMesh(_sphereMesh, _matrices[i][j], isWasted ? _grey : _materials[i], 0);
                }  



        }

    }
}
