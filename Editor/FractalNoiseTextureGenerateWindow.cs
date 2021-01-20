using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MS.Noise.Editor{
    public class FractalNoiseTextureGenerateWindow : EditorWindow
    {
        [MenuItem("Window/Noise/Fractal")]
        public static void Open(){
            var dialog = ScriptableObject.CreateInstance<FractalNoiseTextureGenerateWindow>();
            dialog.titleContent = new GUIContent("FractalNoiseTextureGenerator");
            dialog.ShowUtility();
        }


        private List<IBaseNoiseGenerateOptions> _octaves = new List<IBaseNoiseGenerateOptions>();

        private NoiseTextureOutputOptions _texOutputOptions;

        private Texture2D _generatedNoiseTexture;
        private void OnGUI() {
            
            if(_octaves == null){
                _octaves = new List<IBaseNoiseGenerateOptions>();
            }

            for(var i = 0; i < _octaves.Count; i ++){
                var oc = _octaves[i];
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Octave " + i);
                GUILayout.FlexibleSpace();
                if(GUILayout.Button("-",EditorStyles.miniButtonRight)){
                    _octaves.RemoveAt(i);
                    break;
                }
                EditorGUILayout.EndHorizontal();
                var type = (NoiseType)EditorGUILayout.EnumPopup("BaseNoiseType",oc.type);
                if(type != oc.type){
                    _octaves.RemoveAt(i);
                    oc = NoiseEditorUtil.CreateNoiseOptionsByType(type);
                    _octaves.Insert(i,oc);
                }
                oc.OnGUI();
                EditorGUILayout.EndVertical();
            }

            if(GUILayout.Button("Add Octave")){
                _octaves.Add(new ValueNoiseOptions());
            }

            if(_texOutputOptions == null){
                _texOutputOptions = new NoiseTextureOutputOptions();
            }

            _texOutputOptions.OnGUI();


            if(GUILayout.Button("Generate")){
                List<INoise2D> octaves = new List<INoise2D>();
                foreach(var o in _octaves){
                    octaves.Add(o.CreateNoise(_texOutputOptions));
                }
                var noise = new FractalNoise2D(octaves.ToArray());
                _generatedNoiseTexture = _texOutputOptions.ExportNoiseTo(noise);
            }

            if(_generatedNoiseTexture){
                var rect = EditorGUILayout.GetControlRect(GUILayout.Width(256),GUILayout.Height(256));
                GUI.DrawTexture(rect,_generatedNoiseTexture);
            }
        }



    }
}
