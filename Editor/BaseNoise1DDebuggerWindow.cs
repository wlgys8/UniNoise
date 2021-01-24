using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MS.Noise.Editor{
    public class BaseNoise1DDebuggerWindow : EditorWindow
    {
        [MenuItem("Window/Noise/Debugger1D")]
        public static void Open(){
            var dialog = ScriptableObject.CreateInstance<BaseNoise1DDebuggerWindow>();
            dialog.titleContent = new GUIContent("1DNoiseDebugger");
            dialog.ShowUtility();
        }
       private Noise1DDebugger _debugger = new Noise1DDebugger(new PerlinNoise1D());
       

       private float _frequency = 1;
       private float _amplitude = 1;

       private NoiseType _type = NoiseType.None;

       
       void OnGUI(){
            EditorGUI.BeginChangeCheck();
            _type = (NoiseType) EditorGUILayout.EnumPopup("Type",_type);
            if(EditorGUI.EndChangeCheck()){
                _debugger = null;
            }
            if(_debugger == null){
                var noise = CreateNoise(_type);
                _debugger = new Noise1DDebugger(noise);
            }
            _frequency = EditorGUILayout.Slider("frequency",_frequency,1,100);
            _amplitude = EditorGUILayout.Slider("amplitude",_amplitude,0.1f,1);
            _debugger.frequency = _frequency;
            _debugger.amplitude = _amplitude * 0.5f;

            var lastRect = GUILayoutUtility.GetLastRect();
            var height=  this.position.height - lastRect.y;
            if (Event.current.type == EventType.Repaint) {
                _debugger.OnPostRender();
            }
       }

       private INoise1D CreateNoise(NoiseType type){
           switch(type){
               case NoiseType.None:
               return new NoneNoise1D();
               case NoiseType.Value1D:
               return new ValueNoise1D();
               case NoiseType.Perlin1D:
               return new PerlinNoise1D();
               case NoiseType.Factal1D:
               return new FractalNoise1D(new PerlinNoise1D(),new PerlinNoise1D());
           }
           return null;
       }



        internal class NoneNoise1D : INoise1D
        {
            public float Evaluate(float x)
            {
                return 0;
            }
        }

        private enum NoiseType{
           None,
           Value1D,
           Perlin1D,
           Factal1D,
       }
    }
}
