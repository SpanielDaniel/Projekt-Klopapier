
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts
{
    public class Street : Ground
    {

        public static List<Street> Streets = new List<Street>();
        
        private Vector3 PosTop;
        private Vector3 PosDown;
        private Vector3 PosRight;
        private Vector3 PosLeft;
        
        [SerializeField] private bool IsTopStreet;
        [SerializeField] private bool IsDownStreet;
        [SerializeField] private bool IsLeftStreet;
        [SerializeField] private bool IsRightStreet;
        public Vector3 GetPosTop => PosTop;
        public Vector3 GetPosDown => PosDown;
        public Vector3 GetPosLeft => PosLeft;
        public Vector3 GetPosRight => PosRight;

        public bool GetIsTopStreet => IsTopStreet;
        public bool GetIsDownStreet => IsDownStreet;
        public bool GetIsLeftStreet => IsLeftStreet;
        public bool GetIsRightStreet => IsRightStreet;

        [SerializeField] private SpriteRenderer LeftArrow;
        [SerializeField] private SpriteRenderer  RightArrow;
        [SerializeField] private SpriteRenderer  TopArrow;
        [SerializeField] private SpriteRenderer  DownArrow;

        [SerializeField] private Material CanBuild;
        [SerializeField] private Material CantBuild;
        
        private void Awake()
        {
            InitPositions();
            InitArrows();
            SetAllArrowsActive(false);
            Streets.Add(this);
        }

        private void InitPositions()
        {
            Vector3 localPos = transform.position;
            PosTop = localPos + Vector3.forward;
            PosDown = localPos + Vector3.back;
            PosLeft = localPos + Vector3.left;
            PosRight = localPos + Vector3.right;
        }

        private void InitArrows()
        {
            SetArrowMaterial(!IsLeftStreet,LeftArrow);
            SetArrowMaterial(!IsRightStreet,RightArrow);
            SetArrowMaterial(!IsTopStreet,TopArrow);
            SetArrowMaterial(!IsDownStreet,DownArrow);
        }

        private void SetArrowMaterial(bool _canBuild, SpriteRenderer _spriteRenderer)
        {
            if (_canBuild) _spriteRenderer.material = CanBuild;
            else _spriteRenderer.material = CantBuild;
        }

        public void SetAllArrowsActive(bool _isActive)
        {
            LeftArrow.gameObject.SetActive(_isActive);
            RightArrow.gameObject.SetActive(_isActive);
            TopArrow.gameObject.SetActive(_isActive);
            DownArrow.gameObject.SetActive(_isActive);
        }

    }
}
