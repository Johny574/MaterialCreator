using System.IO;
using UnityEditor;
using UnityEngine;

public class MaterialCreator : EditorWindow 
{
    public Shader Shader;
    SerializedObject _SO;
    string _outputPath;

    void OnEnable(){
        _SO = new SerializedObject(this);
    }

    void OnGUI(){
        EditorGUILayout.PropertyField(_SO.FindProperty("Shader"), true);
        _SO.ApplyModifiedProperties();

        GUILayout.Label("Path");
        _outputPath = GUILayout.TextField(_outputPath);

        if (GUILayout.Button("Set output"))
        {            
            _outputPath = EditorUtility.OpenFolderPanel("", "", "");
        }

        if (GUILayout.Button("Create"))
        {
            Object[] _selectedObjects = Selection.objects;
            for (int i = 0; i < _selectedObjects.Length; i++)
            {
                Texture2D texture = _selectedObjects[i] as Texture2D;

                if (texture != null)
                {
                    Material material = new Material(Shader);

                    material.mainTexture = texture;
                    AssetDatabase.CreateAsset(material, GetRelativePath(_outputPath + "/" +_selectedObjects[i].name + ".asset"));
                }
                else
                {
                    Debug.Log("No texture found for " + _selectedObjects[i].name);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

    }


    [MenuItem("Tools/Materials/Material Creator")]
    public static void OpenMaterialCreator()
    {
        MaterialCreator materialCreator = GetWindow<MaterialCreator>();
        materialCreator.titleContent = new GUIContent("Material Creator");
    }

    private string GetRelativePath(string absolutePath)
    {
        string projectPath = Application.dataPath;
        projectPath = Path.GetFullPath(projectPath);
        absolutePath = Path.GetFullPath(absolutePath);

        if (absolutePath.StartsWith(projectPath))
        {
            string relativePath = "Assets" + absolutePath.Substring(projectPath.Length);
            return relativePath.Replace("\\", "/");
        }
        else
        {
            return absolutePath;
        }
    }
}