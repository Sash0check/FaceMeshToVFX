using UnityEngine;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine.VFX;
using UI = UnityEngine.UI;

namespace MediaPipe.FaceMesh
{

    public sealed class Visualizer : MonoBehaviour
    {
        #region Editable attributes

        [SerializeField] WebcamInput _webcam = null;
        [Space]
        [SerializeField] ResourceSet _resources = null;
        [SerializeField] Shader _shader = null;
        [Space]
        [SerializeField] UI.RawImage _mainUI = null;
        [SerializeField] UI.RawImage _faceUI = null;
        [SerializeField] UI.RawImage _leftEyeUI = null;
        [SerializeField] UI.RawImage _rightEyeUI = null;

        #endregion

        #region Private members

        FacePipeline _pipeline;
        Material _material;


        public GameObject new_object;
        public VisualEffect vf;

        #endregion

        #region MonoBehaviour implementation

        void Start()
        {
            _pipeline = new FacePipeline(_resources);
            _material = new Material(_shader);
        }

        void OnDestroy()
        {
            _pipeline.Dispose();
            Destroy(_material);
        }

        void Update()
        {
            // Processing on the face pipeline
            _pipeline.ProcessImage(_webcam.Texture);

        }


        //void SaveAsset(GameObject go)
        //{
        //    var mf = go.GetComponent<MeshFilter>();
        //    if (mf)
        //    {
        //        var savePath = "Assets/" + "gen_mesh" + ".asset";
        //        Debug.Log("Saved Mesh to:" + savePath);
        //        AssetDatabase.CreateAsset(mf.mesh, savePath);
        //    }
        //}
        void OnRenderObject()
        {

            //var fF = MathUtil.ScaleOffset(1f, math.float2(-0.5f, -0.25f));
            _material.SetBuffer("_Vertices", _pipeline.RefinedFaceVertexBuffer);
            _material.SetPass(1);


            //_resources.faceMeshTemplate.SetIndices(_resources.faceMeshTemplate.GetIndices(0), MeshTopology.Points, 0);


            new_object.GetComponent<MeshFilter>().mesh = _resources.faceLineTemplate;

            Vector3[] verts = new_object.GetComponent<MeshFilter>().mesh.vertices;
            Mesh m2 = new Mesh();
            m2.vertices = verts;
            vf.SetMesh("Mesh", m2);
            new_object.GetComponent<MeshFilter>().mesh.RecalculateBounds();
            var mv = float4x4.Translate(math.float3(-0.875f, -0.5f, 0));
            Graphics.DrawMeshNow(new_object.GetComponent<MeshFilter>().mesh, mv);

        }

        #endregion
    }

} // namespace MediaPipe.FaceMesh
