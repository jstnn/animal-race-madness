/* 
    <copyright file="BGCcCollider2DEdgeEditor" company="BansheeGz">
        Copyright (c) 2016 All Rights Reserved
   </copyright>
*/

using BansheeGz.BGSpline.Components;
using UnityEditor;
using UnityEngine;

namespace BansheeGz.BGSpline.Editor
{
    [CustomEditor(typeof(BGCcCollider2DEdge))]
    public class BGCcCollider2DEdgeEditor : BGCcColliderAbstractEditor
    {
        private BGCcCollider2DEdge Collider2DEdge
        {
            get { return (BGCcCollider2DEdge) cc; }
        }

        [DrawGizmo(GizmoType.NotInSelectionHierarchy)]
        public static void DrawGizmos(BGCcCollider2DEdge collider2DEdge, GizmoType gizmoType)
        {
            if (!collider2DEdge.ShowIfNotSelected) return;

            var collider = collider2DEdge.GetComponent<EdgeCollider2D>();

            if (collider == null) return;

            var points = collider.points;

            if (points == null || points.Length < 2) return;

            BGEditorUtility.SwapGizmosColor(collider2DEdge.CollidersColor, () =>
            {
                for (var i = 1; i < points.Length; i++)
                {
                    Gizmos.DrawLine(points[i - 1], points[i]);
                }
            });
        }
    }
}