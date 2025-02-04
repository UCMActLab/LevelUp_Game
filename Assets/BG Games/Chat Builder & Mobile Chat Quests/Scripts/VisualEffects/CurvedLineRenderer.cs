using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.VisualEffects
{
    public class CurvedLineRenderer
    {
        private LineRenderer _lineRenderer;
        private Transform _startPoint;
        private Transform _endPoint;
        private OffsetType _offsetType;
        private int _segmentsAmount = 20;

        private Vector3 _controlPointPosition;

        public CurvedLineRenderer(LineRenderer lineRenderer, Transform startPoint, Transform endPoint, OffsetType offsetType, int segmentsAmount)
        {
            _lineRenderer = lineRenderer;
            _startPoint = startPoint;
            _endPoint = endPoint;
            _offsetType = offsetType;
            _segmentsAmount = segmentsAmount;
        }

        public void Update(Vector2 controlPointOffset)
        {
            Vector2 offset = controlPointOffset;
            bool reverseX = (_endPoint.position - _startPoint.position).x < 0;
            if (reverseX)
            {
                offset.x *= -1;
            }

            bool reverseY = (_endPoint.position - _startPoint.position).y < 0;
            if (reverseY)
            {
                offset.y *= -1;
            }

            switch (_offsetType)
            {
                case OffsetType.Center:
                    _controlPointPosition = _startPoint.position + (_endPoint.position - _startPoint.position) / 2 + (Vector3)offset;
                    break;
                case OffsetType.Start:
                    _controlPointPosition = _startPoint.position + (Vector3)offset;
                    break;
                case OffsetType.End:
                    _controlPointPosition = _endPoint.position + (Vector3)offset;
                    break;
            }

            UpdateLineRendererBezier();
        }

        private void UpdateLineRendererBezier()
        {
            Vector3[] positions = new Vector3[_segmentsAmount + 1];
            for (int i = 0; i <= _segmentsAmount; i++)
            {
                float t = (float)i / _segmentsAmount;
                positions[i] = BezierCurve(_startPoint.position, _controlPointPosition, _endPoint.position, t);
            }

            _lineRenderer.positionCount = _segmentsAmount + 1;
            _lineRenderer.SetPositions(positions);
        }

        Vector3 BezierCurve(Vector3 start, Vector3 control, Vector3 end, float t)
        {
            return Mathf.Pow(1 - t, 3) * start +
                   3 * Mathf.Pow(1 - t, 2) * t * control +
                   3 * (1 - t) * Mathf.Pow(t, 2) * end +
                   Mathf.Pow(t, 3) * end;
        }
    }
}