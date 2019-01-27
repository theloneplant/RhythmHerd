using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DebugRaytracer))]
public class GridRaytracerEditor : Editor
{
    private void OnSceneGUI()
    {
        var tracer = target as DebugRaytracer;
        tracer.Start = Handles.PositionHandle(tracer.Start, Quaternion.identity);
        tracer.End = Handles.PositionHandle(tracer.End, Quaternion.identity);
        var direction = tracer.End - tracer.Start;
        Handles.color = Color.blue;
        Handles.DrawLine(tracer.Start, tracer.Start + direction / 2f);
        Handles.color = Color.red;
        Handles.DrawLine(tracer.Start + direction / 2f, tracer.End);
    }
}
