using UI.Event_Listeners;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    [ExecuteAlways]
    public class RoundImageCorners : MonoBehaviour {
        private static readonly int Props = Shader.PropertyToID("_WidthHeightRadius");
        private static readonly int StartColorProp = Shader.PropertyToID("_StartColor");
        private static readonly int EndColorProp = Shader.PropertyToID("_EndColor");
        private static readonly int TintColorProp = Shader.PropertyToID("_TintColor");
        private static readonly int OffsetProp = Shader.PropertyToID("_Offset");
        private static readonly int GradientAngleProp = Shader.PropertyToID("_GradientAngle");
        private static readonly int GradientSpreadProp = Shader.PropertyToID("_GradientSpread");

        [SerializeField] private Image targetGraphic;

        public float Radius = 30;
        public bool UseGradient = false;
        public Color StartColor = Color.white;
        public Color EndColor = Color.white;
        public float GradientAngle = 0;
        public float GradientSpread = 1;
        public Color TintColor = Color.white;
        public Vector2 Offset = new Vector2();

        private Material _material;

        private void GetMaterial() {
            if (_material == null)
                _material = _material = new Material(Shader.Find("ImageRounding/ImageRounding"));
        }

        private void Start() {
            GetMaterial();

            targetGraphic.material = _material;

            if (TryGetComponent<HoverEventListener>(out var listener))
                listener.ImageUpdater = this;

            Refresh();
        }

        private void OnRectTransformDimensionsChange() {
            if (enabled && _material != null) {
                Refresh();
            }
        }

        private void OnValidate() {
            Refresh();
            targetGraphic.material = _material;
        }

        public void Refresh() {
            GetMaterial();

            var rect = ((RectTransform)transform).rect;

            GradientAngle %= 2f * Mathf.PI;
            if (GradientAngle < 0)
                GradientAngle += 2f * Mathf.PI;

            GradientSpread = Mathf.Clamp(GradientSpread, 0.1f, 2f);

            _material.SetVector(Props, new Vector4(rect.width, rect.height, Radius * 2, 0));
            _material.SetColor(StartColorProp, StartColor);
            _material.SetColor(EndColorProp, UseGradient ? EndColor : StartColor);
            _material.SetFloat(GradientAngleProp, GradientAngle);
            _material.SetFloat(GradientSpreadProp, GradientSpread);
            _material.SetColor(TintColorProp, TintColor);
            _material.SetVector(OffsetProp, Offset);
        }
    }
}