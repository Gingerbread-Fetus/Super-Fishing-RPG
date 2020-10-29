using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

[CustomEditor(typeof(Hatchery)), CanEditMultipleObjects]
public class HatcheryBounds : Editor
{
    private BoxBoundsHandle m_BoundsHandle = new BoxBoundsHandle();

    protected virtual void OnSceneGUI()
    {
        Hatchery hatchery = (Hatchery)target;

        // copy the target object's data to the handle
        m_BoundsHandle.center = hatchery.transform.position;
        m_BoundsHandle.size = hatchery.Extents;
        // draw the handle
        EditorGUI.BeginChangeCheck();
        m_BoundsHandle.DrawHandle();
        if (EditorGUI.EndChangeCheck())
        {
            // record the target object before setting new values so changes can be undone/redone
            Undo.RecordObject(hatchery, "Change Bounds");

            // copy the handle's updated data back to the target object
            Bounds newBounds = new Bounds();
            newBounds.center = m_BoundsHandle.center;
            newBounds.size = m_BoundsHandle.size;
            hatchery.Extents = newBounds.size;
            hatchery.transform.position = newBounds.center;
        }
    }
}
