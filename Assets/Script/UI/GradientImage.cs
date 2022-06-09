using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class GradientImage : BaseMeshEffect
{
    [SerializeField]
    private Gradient _gradient = new Gradient();
    [SerializeField]
    protected GridLayoutGroup.Axis _axis = GridLayoutGroup.Axis.Vertical;

    protected List<UIVertex> _vertexList = new List<UIVertex>();
    protected Vector2 _minMax;
    protected int _axisIndex;

    public override void ModifyMesh(VertexHelper helper)
    {
        _vertexList.Clear();
        helper.GetUIVertexStream(_vertexList);

        CreateGradientMesh();

        SetVertexColor();

        helper.Clear();
        helper.AddUIVertexTriangleStream(_vertexList);
    }
    protected void CreateGradientMesh()
    {
        _axisIndex = (int)_axis;
        _minMax = new Vector2(
            _vertexList.Select(v => v.position[_axisIndex]).Min(),
            _vertexList.Select(v => v.position[_axisIndex]).Max()
        );

        foreach (var key in _gradient.colorKeys)
            DivideCheck(key.time);
        foreach (var key in _gradient.alphaKeys)
            DivideCheck(key.time);

    }
    protected void SetVertexColor()
    {
        // 頂点カラーの設定
        for (var i = 0; i < _vertexList.Count; i++)
        {
            var vertex = _vertexList[i];
            vertex.color = _gradient.Evaluate(Mathf.InverseLerp(_minMax[0], _minMax[1], vertex.position[_axisIndex]));
            _vertexList[i] = vertex;
        }
    }

    private void DivideCheck(float key)
    {
        var count = _vertexList.Count;
        for (var i = 0; i < count; i += 6)
        {
            var min = _vertexList[i].position[_axisIndex];
            var max = _vertexList[i].position[_axisIndex];
            for (var j = 1; j < 6; ++j)
            {
                if (min > _vertexList[i + j].position[_axisIndex])
                    min = _vertexList[i + j].position[_axisIndex];
                if (max < _vertexList[i + j].position[_axisIndex])
                    max = _vertexList[i + j].position[_axisIndex];
            }
            var minT = Mathf.InverseLerp(_minMax[0], _minMax[1], min);
            var maxT = Mathf.InverseLerp(_minMax[0], _minMax[1], max);
            if (!(minT < key) || !(key < maxT))
                continue;
            Divide(i, Mathf.Lerp(_minMax[0], _minMax[1], key));
        }
    }
    /// <summary>
    /// Image の Mesh順 左下 左上 右上, 右上 右下 左下
    /// </summary>
    private void Divide(int startIndex, float targetValue)
    {
        var indexes = new[] { 2, 3, 4 };
        if (_axis == GridLayoutGroup.Axis.Vertical)
            indexes = new[] { 1, 2, 3 };
        var addVertexList = new UIVertex[6];
        if (_axis == GridLayoutGroup.Axis.Horizontal)
        {
            addVertexList[2] = GetVertex(startIndex + 2);
            addVertexList[3] = GetVertex(startIndex + 2);
            addVertexList[4] = GetVertex(startIndex + 4);
        }
        else
        {
            addVertexList[1] = GetVertex(startIndex + 1);
            addVertexList[2] = GetVertex(startIndex + 2);
            addVertexList[3] = GetVertex(startIndex + 2);
        }
        var minMaxUV = new Vector2(_vertexList[startIndex].uv0[_axisIndex], _vertexList[startIndex].uv0[_axisIndex]);
        var minMaxPos = new Vector2(_vertexList[startIndex].position[_axisIndex], _vertexList[startIndex].position[_axisIndex]);
        for (var i = 1; i < 6; i++)
        {
            if (minMaxUV[0] > _vertexList[startIndex + i].uv0[_axisIndex])
                minMaxUV[0] = _vertexList[startIndex + i].uv0[_axisIndex];
            if (minMaxUV[1] < _vertexList[startIndex + i].uv0[_axisIndex])
                minMaxUV[1] = _vertexList[startIndex + i].uv0[_axisIndex];
            if (minMaxPos[0] > _vertexList[startIndex + i].position[_axisIndex])
                minMaxPos[0] = _vertexList[startIndex + i].position[_axisIndex];
            if (minMaxPos[1] < _vertexList[startIndex + i].position[_axisIndex])
                minMaxPos[1] = _vertexList[startIndex + i].position[_axisIndex];
        }

        foreach (var index in indexes)
        {
            var vertex = _vertexList[startIndex + index];
            var pos = vertex.position;
            pos[_axisIndex] = targetValue;
            vertex.position = pos;
            var uv = vertex.uv0;
            uv[_axisIndex] = Mathf.Lerp(minMaxUV[0], minMaxUV[1], Mathf.InverseLerp(minMaxPos[0], minMaxPos[1], pos[_axisIndex]));
            vertex.uv0 = uv;
            _vertexList[startIndex + index] = vertex;
        }
        if (_axis == GridLayoutGroup.Axis.Horizontal)
        {
            addVertexList[0] = GetVertex(startIndex + 4);
            addVertexList[1] = GetVertex(startIndex + 2);
            addVertexList[5] = GetVertex(startIndex + 4);
        }
        else
        {
            addVertexList[0] = GetVertex(startIndex + 1);
            addVertexList[4] = GetVertex(startIndex + 2);
            addVertexList[5] = GetVertex(startIndex + 1);
        }
        _vertexList.AddRange(addVertexList);
    }
    private UIVertex GetVertex(int index)
    {
        var vertex = new UIVertex();
        vertex.position = _vertexList[index].position;
        vertex.uv0 = _vertexList[index].uv0;
        return vertex;
    }
}