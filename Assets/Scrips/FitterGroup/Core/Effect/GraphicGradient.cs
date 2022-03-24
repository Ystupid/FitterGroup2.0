using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.FitterGroup.Layout;

namespace UnityEngine.UI.FitterGroup.Effect
{
    public class GraphicGradient : BaseMeshEffect
    {
        [SerializeField] private Gradient m_GradientColor = new Gradient();
        public Gradient GradientColor
        {
            get => m_GradientColor;
            set
            {
                m_GradientColor = value;
                Apply();
            }
        }

        [SerializeField] private FitterAxis m_GradientAxis;
        public FitterAxis GradientAxis
        {
            get => m_GradientAxis;
            set => m_GradientAxis = value;
        }

        [SerializeField] private bool m_AdditivityColor;
        public bool AdditivityColor
        {
            get => m_AdditivityColor;
            set => m_AdditivityColor = value;
        }

        private List<UIVertex> m_VertexList;
        public List<UIVertex> VertexList
        {
            get
            {
                if (m_VertexList == null)
                    m_VertexList = new List<UIVertex>();
                return m_VertexList;
            }
        }

        public void Apply()
        {
            graphic.SetVerticesDirty();
            graphic.SetMaterialDirty();
        }

        private void ModifyVertices(VertexHelper vh)
        {
            vh.GetUIVertexStream(VertexList);
            vh.Clear();

            int step = 6;
            for (int i = 0; i < VertexList.Count; i += step)
            {
                UIVertex start1, start2, end1, end2, current1, current2;

                switch (GradientAxis)
                {
                    case FitterAxis.Horizontal:
                        start1 = VertexList[i + 0];
                        start2 = VertexList[i + 1];
                        end1 = VertexList[i + 4];
                        end2 = VertexList[i + 3];
                        break;
                    case FitterAxis.Vertical:
                        start1 = VertexList[i + 0];
                        start2 = VertexList[i + 4];
                        end1 = VertexList[i + 1];
                        end2 = VertexList[i + 2];
                        break;
                    default:
                        start1 = VertexList[i + 0];
                        start2 = VertexList[i + 1];
                        end1 = VertexList[i + 4];
                        end2 = VertexList[i + 3];
                        break;
                }

                for (int j = 0; j < m_GradientColor.colorKeys.Length; j++)
                {
                    var colorKey = m_GradientColor.colorKeys[j];
                    colorKey.color.a = m_GradientColor.Evaluate(colorKey.time).a;
                    if (j == 0)
                    {
                        multiplyColor(ref start1, colorKey.color);
                        multiplyColor(ref start2, colorKey.color);
                    }
                    else if (j == m_GradientColor.colorKeys.Length - 1)
                    {
                        multiplyColor(ref end1, colorKey.color);
                        multiplyColor(ref end2, colorKey.color);

                        //right
                        vh.AddVert(start1);
                        vh.AddVert(start2);
                        vh.AddVert(end2);

                        //left
                        vh.AddVert(end2);
                        vh.AddVert(end1);
                        vh.AddVert(start1);

                    }
                    else
                    {
                        // create right
                        current2 = CreateVertexByTime(start2, end2, colorKey.time);
                        vh.AddVert(start1);
                        vh.AddVert(start2);
                        vh.AddVert(current2);

                        // create left
                        current1 = CreateVertexByTime(start1, end1, colorKey.time);
                        vh.AddVert(current2);
                        vh.AddVert(current1);
                        vh.AddVert(start1);

                        start1 = current1;
                        start2 = current2;
                    }
                }
            }

            //每个字母的顶点数量
            int stepVertCount = (m_GradientColor.colorKeys.Length - 1) * 2 * 3;
            for (int i = 0; i < vh.currentVertCount; i += stepVertCount)
            {
                for (int m = 0; m < stepVertCount; m += 3)
                {
                    vh.AddTriangle(i + m + 0, i + m + 1, i + m + 2);
                }
            }

            VertexList.Clear();
        }

        private UIVertex multiplyColor(ref UIVertex vertex, Color color)
        {
            vertex.color = m_AdditivityColor ? Multiply(vertex.color, color) : (Color32)color;
            return vertex;
        }

        public static Color32 Multiply(Color32 a, Color32 b)
        {
            a.r = (byte)((a.r * b.r) >> 8);
            a.g = (byte)((a.g * b.g) >> 8);
            a.b = (byte)((a.b * b.b) >> 8);
            a.a = (byte)((a.a * b.a) >> 8);
            return a;
        }

        private UIVertex CreateVertexByTime(UIVertex start, UIVertex end, float time)
        {
            var vertex = new UIVertex
            {
                normal = Vector3.Lerp(start.normal, end.normal, time),
                position = Vector3.Lerp(start.position, end.position, time),
                tangent = Vector4.Lerp(start.tangent, end.tangent, time),
                uv0 = Vector2.Lerp(start.uv0, end.uv0, time),
                uv1 = Vector2.Lerp(start.uv1, end.uv1, time),
                color = m_GradientColor.Evaluate(time),
            };

            if (m_AdditivityColor)
            {
                var color = Color.Lerp(start.color, end.color, time);
                vertex.color = Multiply(color, m_GradientColor.Evaluate(time));
            }
            else vertex.color = m_GradientColor.Evaluate(time);

            return vertex;
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive()) return;
            ModifyVertices(vh);
        }
    }
}