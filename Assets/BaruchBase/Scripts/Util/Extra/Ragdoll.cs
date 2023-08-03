using Baruch.Extension;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Baruch
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Baruch/Ragdoll")]
    public class Ragdoll : MonoBehaviour
    {
        private enum Preset
        {
            Realistic,
            Casual
        }
        private readonly string[] _boneNames = new string[] { "Hips", "Spine", "Head", "LeftUpLeg", "LeftLeg", "RightUpLeg", "RightLeg", "LeftArm", "LeftForeArm", "RightArm", "RightForeArm" };



        private Rigidbody[] _boneRbs = new Rigidbody[11];
        private Transform[] _boneTransforms = new Transform[11];
        private Collider[] _boneCls = new Collider[11];

        [SerializeField] bool _isActive;
        [Min(0f)]
        [SerializeField] float _weight = 25f;


        [SerializeField] Preset _preset;

        bool _isBuilded => _boneRbs[0];



        /// <summary>Activates Ragdoll(Requires <c>Animator</c>).        
        /// </summary>
        public void Activate()
        {
            if (!_isBuilded)
            {
                Debug.LogError("Ragdoll Not Builded");
                return;
            }
            if (TryGetComponent<Animator>(out Animator animator))
                Destroy(animator);

            else
                Debug.LogWarning("Couldnt Find Animator");
            _isActive = true;
            Toggle();
        }

        /// <summary>Ragdoll force effect on an Activated ragdoll.
        /// <para>Example:</para>
        /// <example>
        /// <code>
        /// ragdoll.Activate();        
        /// <para>ragdoll.Hit(Bullet.position,100f,false);</para>
        /// </code>
        /// </example>
        /// </summary>

        public void Hit(Vector3 position, float force = 1000f, bool reaction = true)
        {

            (List<int> sortedIndexes, List<Vector3> PositionTO) = ClosestBones(position);
            for (int i = 0; i < sortedIndexes.Count; i++)
            {
                _boneRbs[sortedIndexes[i]].AddForce(PositionTO[i] * Remap(i) * force);

            }

            float Remap(int x)
            {
                float z = (10 - x) / 10f;
                if (reaction)
                    return -0.07f * z * z + 0.119f * z + 1.19f;

                return z;
            }
        }



        private (List<int>, List<Vector3>) ClosestBones(Vector3 position)
        {
            List<Transform> boneList = _boneTransforms.ToList();
            boneList.Sort(ByDistance);
            int ByDistance(Transform a, Transform b)
            {
                float dstToA = Vector3.Distance(position, a.transform.position);
                float dstToB = Vector3.Distance(position, b.transform.position);
                return dstToA.CompareTo(dstToB);
            }

            List<Vector3> positionTO = new List<Vector3>();

            List<int> boneIndex = new List<int>();
            List<Transform> bones = _boneTransforms.ToList();

            foreach (Transform item in boneList)
            {
                positionTO.Add((item.position - position).normalized);
                boneIndex.Add(bones.IndexOf(item));
            }

            return (boneIndex, positionTO);
        }

        public void Toggle()
        {
            for (int i = 0; i < _boneRbs.Length; i++)
            {
                _boneCls[i].enabled = _isActive;
                _boneRbs[i].isKinematic = !_isActive;
            }
        }

        #region Inspector
        [HideInInspector] public bool showInspector = true;
        public void ShowInspector()
        {
            showInspector = true;
        }

        public bool HasRagdoll()
        {
            return !(_boneRbs[0] is null);
        }

        public void Clear()
        {
            foreach (string name in _boneNames)
            {
                Transform Bone = transform.FindDeepChild(NameToBone(name));

                if (Bone.TryGetComponent<CharacterJoint>(out CharacterJoint cj))
                {
                    DestroyImmediate(cj);
                }
                if (Bone.TryGetComponent<Rigidbody>(out Rigidbody r))
                {
                    DestroyImmediate(r);
                }
                if (Bone.TryGetComponent<Collider>(out Collider c))
                {
                    DestroyImmediate(c);
                }


            }

            _boneRbs = new Rigidbody[11];
            _boneCls = new Collider[11];
            ShowInspector();
        }
        public void Build()
        {
            Create();
            switch (_preset)
            {
                case Preset.Realistic:
                    BuildRealistic();
                    break;
                case Preset.Casual:
                    BuildCasual();

                    break;
                default:
                    break;
            }

        }
        // "Hips", "Spine", "Head", "LeftUpLeg", "LeftLeg", "RightUpLeg", "RightLeg", "LeftArm", "LeftForeArm", "RightArm", "RightForeArm"

        private void BuildCasual()
        {
            Collider c;
            Rigidbody r;

            c = _boneCls[0];//Hips
            (c as BoxCollider).center = new Vector3(0, 0.01f, 0);
            (c as BoxCollider).size = new Vector3(0.22f, 0.07f, 0.1f);
            r = _boneRbs[0];
            r.mass = _weight * 0.156f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
            //----
            c = _boneCls[1];//Spine
            (c as BoxCollider).center = new Vector3(0, 0.07f, 0);
            (c as BoxCollider).size = new Vector3(0.22f, 0.14f, 0.1f);
            r = _boneRbs[1];
            r.mass = _weight * 0.156f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
            //----
            c = _boneCls[2];//Head
            (c as SphereCollider).center = new Vector3(0, 0.17f, 0.014f);
            (c as SphereCollider).radius = .197f;
            r = _boneRbs[2];
            r.mass = _weight * 0.0624f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
            //----
            c = _boneCls[3];//LeftUpLeg
            (c as CapsuleCollider).center = new Vector3(0, 0.09f, 0);
            (c as CapsuleCollider).radius = .056f;
            (c as CapsuleCollider).height = .186f;
            r = _boneRbs[3];
            r.mass = _weight * 0.094f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
            //----
            c = _boneCls[4];//LeftLeg
            (c as CapsuleCollider).center = new Vector3(0, 0.109f, 0);
            (c as CapsuleCollider).radius = .0546f;
            (c as CapsuleCollider).height = .218f;
            r = _boneRbs[4];
            r.mass = _weight * 0.094f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
            //----
            c = _boneCls[5];//RightUpLeg
            (c as CapsuleCollider).center = new Vector3(0, 0.09f, 0);
            (c as CapsuleCollider).radius = .056f;
            (c as CapsuleCollider).height = .186f;
            r = _boneRbs[5];
            r.mass = _weight * 0.094f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
            //----
            c = _boneCls[6];//RightLeg
            (c as CapsuleCollider).center = new Vector3(0, 0.109f, 0);
            (c as CapsuleCollider).radius = .0546f;
            (c as CapsuleCollider).height = .218f;
            r = _boneRbs[6];
            r.mass = _weight * 0.094f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
            //----
            c = _boneCls[7];//LeftArm
            (c as CapsuleCollider).center = new Vector3(0, 0.052f, 0);
            (c as CapsuleCollider).radius = .0261f;
            (c as CapsuleCollider).height = .104f;
            r = _boneRbs[7];
            r.mass = _weight * 0.0624f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
            //----
            c = _boneCls[8];//LeftForeArm
            (c as CapsuleCollider).center = new Vector3(0, 0.13f, 0);
            (c as CapsuleCollider).radius = .052f;
            (c as CapsuleCollider).height = .263f;
            r = _boneRbs[8];
            r.mass = _weight * 0.0624f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
            //----
            c = _boneCls[9];//RightArm
            (c as CapsuleCollider).center = new Vector3(0, 0.0521f, 0);
            (c as CapsuleCollider).radius = .0261f;
            (c as CapsuleCollider).height = .104f;
            r = _boneRbs[9];
            r.mass = _weight * 0.0624f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
            //----
            c = _boneCls[10];//RightForeArm
            (c as CapsuleCollider).center = new Vector3(0, 0.13f, 0);
            (c as CapsuleCollider).radius = .052f;
            (c as CapsuleCollider).height = .263f;
            r = _boneRbs[10];
            r.mass = _weight * 0.0624f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
        }

        private void BuildRealistic()
        {
            Collider c;
            Rigidbody r;

            c = _boneCls[0];
            (c as BoxCollider).center = new Vector3(0, 0.02f, -0.02f);
            (c as BoxCollider).size = new Vector3(0.3f, 0.12f, 0.2f);
            r = _boneRbs[0];
            r.mass = _weight * 0.156f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
            //----
            c = _boneCls[1];
            (c as BoxCollider).center = new Vector3(0, 0.15f, 0);
            (c as BoxCollider).size = new Vector3(0.3f, 0.3f, 0.2f);
            r = _boneRbs[1];
            r.mass = _weight * 0.156f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
            //----
            c = _boneCls[2];
            (c as SphereCollider).center = new Vector3(0, 0.1f, 0.026f);
            (c as SphereCollider).radius = .123f;
            r = _boneRbs[2];
            r.mass = _weight * 0.0624f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
            //----
            c = _boneCls[3];
            (c as CapsuleCollider).center = new Vector3(0, 0.22f, 0);
            (c as CapsuleCollider).radius = .13f;
            (c as CapsuleCollider).height = .45f;
            r = _boneRbs[3];
            r.mass = _weight * 0.094f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
            //----
            c = _boneCls[4];
            (c as CapsuleCollider).center = new Vector3(0, 0.26f, 0);
            (c as CapsuleCollider).radius = .13f;
            (c as CapsuleCollider).height = .52f;
            r = _boneRbs[4];
            r.mass = _weight * 0.094f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
            //----
            c = _boneCls[5];
            (c as CapsuleCollider).center = new Vector3(0, 0.22f, 0);
            (c as CapsuleCollider).radius = .13f;
            (c as CapsuleCollider).height = .45f;
            r = _boneRbs[5];
            r.mass = _weight * 0.094f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
            //----
            c = _boneCls[6];
            (c as CapsuleCollider).center = new Vector3(0, 0.26f, 0);
            (c as CapsuleCollider).radius = .13f;
            (c as CapsuleCollider).height = .52f;
            r = _boneRbs[6];
            r.mass = _weight * 0.094f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
            //----
            c = _boneCls[7];
            (c as CapsuleCollider).center = new Vector3(0, 0.116f, 0);
            (c as CapsuleCollider).radius = .058f;
            (c as CapsuleCollider).height = .23f;
            r = _boneRbs[7];
            r.mass = _weight * 0.0624f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
            //----
            c = _boneCls[8];
            (c as CapsuleCollider).center = new Vector3(0, 0.2f, 0);
            (c as CapsuleCollider).radius = .08f;
            (c as CapsuleCollider).height = .4f;
            r = _boneRbs[8];
            r.mass = _weight * 0.0624f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
            //----
            c = _boneCls[9];
            (c as CapsuleCollider).center = new Vector3(0, 0.116f, 0);
            (c as CapsuleCollider).radius = .058f;
            (c as CapsuleCollider).height = .23f;
            r = _boneRbs[9];
            r.mass = _weight * 0.0624f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
            //----
            c = _boneCls[10];
            (c as CapsuleCollider).center = new Vector3(0, 0.2f, 0);
            (c as CapsuleCollider).radius = .08f;
            (c as CapsuleCollider).height = .4f;
            r = _boneRbs[10];
            r.mass = _weight * 0.0624f;
            r.drag = 0f;
            r.angularDrag = 0.05f;
        }

        private void Create()
        {
            foreach (string name in _boneNames)
            {
                Transform Bone = transform.FindDeepChild(NameToBone(name));


                Collider c;
                Rigidbody r;
                CharacterJoint cj;

                switch (name)
                {

                    case "Hips":
                        c = Bone.gameObject.AddComponent<BoxCollider>();
                        _boneCls[0] = c;

                        r = Bone.gameObject.AddComponent<Rigidbody>();
                        _boneRbs[0] = r;

                        _boneTransforms[0] = Bone;
                        break;

                    case "Spine":

                        c = Bone.gameObject.AddComponent<BoxCollider>();
                        _boneCls[1] = c;
                        r = Bone.gameObject.AddComponent<Rigidbody>();
                        _boneRbs[1] = r;

                        _boneTransforms[1] = Bone;

                        cj = Bone.gameObject.AddComponent<CharacterJoint>();
                        cj.axis = Vector3.right;
                        cj.swingAxis = Vector3.forward;
                        cj.connectedBody = _boneRbs[0];
                        cj.autoConfigureConnectedAnchor = true;
                        cj.enablePreprocessing = false;
                        SetLimits(cj, -20, 20, 10, 0);

                        //Limits Revisit
                        break;
                    case "Head":
                        c = Bone.gameObject.AddComponent<SphereCollider>();
                        _boneCls[2] = c;

                        r = Bone.gameObject.AddComponent<Rigidbody>();
                        _boneRbs[2] = r;

                        _boneTransforms[2] = Bone;

                        cj = Bone.gameObject.AddComponent<CharacterJoint>();
                        cj.axis = Vector3.right;
                        cj.swingAxis = Vector3.forward;
                        cj.connectedBody = _boneRbs[1];//Spine //FIX Connected body
                        cj.autoConfigureConnectedAnchor = true;
                        cj.enablePreprocessing = false;
                        SetLimits(cj, -40, 25, 25, 0);
                        //Limits Revisit

                        break;
                    case "LeftUpLeg":

                        c = Bone.gameObject.AddComponent<CapsuleCollider>();
                        _boneCls[3] = c;

                        r = Bone.gameObject.AddComponent<Rigidbody>();
                        _boneRbs[3] = r;

                        _boneTransforms[3] = Bone;

                        cj = Bone.gameObject.AddComponent<CharacterJoint>();
                        cj.axis = Vector3.left;
                        cj.swingAxis = Vector3.forward;
                        cj.connectedBody = _boneRbs[0];//hips //FIX Connected body
                        cj.autoConfigureConnectedAnchor = true;
                        cj.enablePreprocessing = false;
                        SetLimits(cj, -20, 70, 30, 0);
                        //Limits Revisit
                        break;
                    case "LeftLeg":

                        c = Bone.gameObject.AddComponent<CapsuleCollider>();
                        _boneCls[4] = c;

                        r = Bone.gameObject.AddComponent<Rigidbody>();
                        _boneRbs[4] = r;

                        _boneTransforms[4] = Bone;


                        cj = Bone.gameObject.AddComponent<CharacterJoint>();
                        cj.axis = Vector3.left;
                        cj.swingAxis = Vector3.forward;
                        cj.connectedBody = _boneRbs[3];//LeftUpLeg //FIX Connected body
                        cj.autoConfigureConnectedAnchor = true;
                        cj.enablePreprocessing = false;
                        SetLimits(cj, -80, 0, 0, 0);
                        break;
                    case "RightUpLeg":

                        c = Bone.gameObject.AddComponent<CapsuleCollider>();
                        _boneCls[5] = c;

                        r = Bone.gameObject.AddComponent<Rigidbody>();
                        _boneRbs[5] = r;
                        _boneTransforms[5] = Bone;


                        cj = Bone.gameObject.AddComponent<CharacterJoint>();
                        cj.axis = Vector3.left;
                        cj.swingAxis = Vector3.forward;
                        cj.connectedBody = _boneRbs[0];//hips //FIX Connected body
                        cj.autoConfigureConnectedAnchor = true;
                        cj.enablePreprocessing = false;
                        SetLimits(cj, -20, 70, 30, 0);
                        //Limits Revisit
                        break;
                    case "RightLeg":

                        c = Bone.gameObject.AddComponent<CapsuleCollider>();
                        _boneCls[6] = c;

                        r = Bone.gameObject.AddComponent<Rigidbody>();
                        _boneRbs[6] = r;
                        _boneTransforms[6] = Bone;


                        cj = Bone.gameObject.AddComponent<CharacterJoint>();
                        cj.axis = Vector3.left;
                        cj.swingAxis = Vector3.forward;
                        cj.connectedBody = _boneRbs[5];//RightUpLeg //FIX Connected body
                        cj.autoConfigureConnectedAnchor = true;
                        cj.enablePreprocessing = false;
                        SetLimits(cj, -80, 0, 0, 0);
                        break;
                    case "LeftArm":

                        c = Bone.gameObject.AddComponent<CapsuleCollider>();
                        _boneCls[7] = c;

                        r = Bone.gameObject.AddComponent<Rigidbody>();
                        _boneRbs[7] = r;
                        _boneTransforms[7] = Bone;


                        cj = Bone.gameObject.AddComponent<CharacterJoint>();
                        cj.axis = Vector3.back;
                        cj.swingAxis = Vector3.right;
                        cj.connectedBody = _boneRbs[1];//Spine //FIX Connected body
                        cj.autoConfigureConnectedAnchor = true;
                        cj.enablePreprocessing = false;
                        SetLimits(cj, -70, 10, 50, 0);
                        break;
                    case "LeftForeArm":
                        c = Bone.gameObject.AddComponent<CapsuleCollider>();
                        _boneCls[8] = c;

                        r = Bone.gameObject.AddComponent<Rigidbody>();
                        _boneRbs[8] = r;
                        _boneTransforms[8] = Bone;


                        cj = Bone.gameObject.AddComponent<CharacterJoint>();
                        cj.axis = Vector3.right;
                        cj.swingAxis = Vector3.back;
                        cj.connectedBody = _boneRbs[7];//LeftArm //FIX Connected body
                        cj.autoConfigureConnectedAnchor = true;
                        cj.enablePreprocessing = false;
                        SetLimits(cj, -90, 0, 0, 0);
                        break;
                    case "RightArm":

                        c = Bone.gameObject.AddComponent<CapsuleCollider>();
                        _boneCls[9] = c;

                        r = Bone.gameObject.AddComponent<Rigidbody>();
                        _boneRbs[9] = r;
                        _boneTransforms[9] = Bone;


                        cj = Bone.gameObject.AddComponent<CharacterJoint>();
                        cj.axis = Vector3.back;
                        cj.swingAxis = Vector3.left;
                        cj.connectedBody = _boneRbs[1];//Spine //FIX Connected body
                        cj.autoConfigureConnectedAnchor = true;
                        cj.enablePreprocessing = false;
                        SetLimits(cj, -70, 10, 50, 0);
                        break;
                    case "RightForeArm":
                        c = Bone.gameObject.AddComponent<CapsuleCollider>();
                        _boneCls[10] = c;

                        r = Bone.gameObject.AddComponent<Rigidbody>();
                        _boneRbs[10] = r;
                        _boneTransforms[10] = Bone;


                        cj = Bone.gameObject.AddComponent<CharacterJoint>();
                        cj.axis = Vector3.left;
                        cj.swingAxis = Vector3.back;
                        cj.connectedBody = _boneRbs[9];//RightArm //FIX Connected body
                        cj.autoConfigureConnectedAnchor = true;
                        cj.enablePreprocessing = false;
                        SetLimits(cj, -90, 0, 0, 0);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void SetLimits(CharacterJoint cj, float low, float high, float swing1, float swing2)
        {
            SoftJointLimit s = new SoftJointLimit();
            s.limit = low;
            cj.lowTwistLimit = s;
            s.limit = high;
            cj.highTwistLimit = s;
            s.limit = swing1;
            cj.swing1Limit = s;
            s.limit = swing2;
            cj.swing2Limit = s;
        }

        private void OnValidate()
        {
            int i = 0;
            foreach (string name in _boneNames)
            {
                Transform Bone = transform.FindDeepChild(NameToBone(name));

                if (Bone.TryGetComponent<Rigidbody>(out Rigidbody r))
                {
                    _boneRbs[i] = r;
                    _boneTransforms[i] = Bone;
                }
                if (Bone.TryGetComponent<Collider>(out Collider c))
                {
                    _boneCls[i] = c;
                }
                i++;


            }
        }
        string NameToBone(string name)
        {
            return "mixamorig:" + name;
        }
        #endregion

    }
}