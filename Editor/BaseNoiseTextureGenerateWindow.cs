using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace MS.Noise.Editor{

    public enum NoiseType{
        Value,
        Perlin,
        Worley,
    }

    public class NoiseTextureOutputOptions{
        private string _outputDir = "./";

        private string _fileName = "Noise.jpg";

        private int _cellSize = 16;

        private Vector2Int _imageSize = new Vector2Int(512,512);

        public void OnGUI(){
            _outputDir = EditorGUILayout.TextField("OutputDir",_outputDir);
            if(_outputDir == null){
                _outputDir = "";
            }
            _fileName = EditorGUILayout.TextField("fileName",_fileName);
            _cellSize = EditorGUILayout.IntSlider("CellSize",_cellSize,1,512);
            _imageSize = EditorGUILayout.Vector2IntField("ImageSize",_imageSize);
        }

        public string outputDir{
            get{
                return _outputDir;
            }
        }

        public int cellSize{
            get{
                return _cellSize;
            }
        }

        public Vector2Int imageSize{
            get{
                return _imageSize;
            }
        }

        public string fileName{
            get{
                return _fileName;
            }set{
                _fileName = value;
            }
        }

        public Texture2D ExportNoiseTo(INoise2D noise){
            var tex = noise.CreateTexture(new NoiseTextureGenerateOptions(){
                width = imageSize.x,
                height = imageSize.y,
                cellSize = cellSize
            });
            var binary  = tex.EncodeToJPG();
            if(_fileName == null){
                _fileName = "nosie.jpg";
            }
            var outPath = Path.Combine(outputDir,_fileName);
            File.WriteAllBytes(outPath,binary);          
            return tex; 
        }
    }
    
    public class BaseNoiseTextureGenerateWindow : EditorWindow
    {



        [MenuItem("Window/Noise/Base")]
        public static void Open(){
            var dialog = ScriptableObject.CreateInstance<BaseNoiseTextureGenerateWindow>();
            dialog.titleContent = new GUIContent("BaseNoiseTextureGenerator");
            dialog.ShowUtility();
        }

        private NoiseType _noiseType;

        private NoiseTextureOutputOptions _texOutputOptions;

        private Texture2D _generatedTexture;

        private WorleyNoiseOptions _worleyOptions;



        private void OnGUI() {
            
            _noiseType = (NoiseType)EditorGUILayout.EnumPopup("NoiseType",_noiseType);

            if(_texOutputOptions == null){
                _texOutputOptions = new NoiseTextureOutputOptions();
            }

            _texOutputOptions.OnGUI();
 
            switch(_noiseType){
                case NoiseType.Worley:
                if(_worleyOptions == null){
                    _worleyOptions = new WorleyNoiseOptions();
                }
                _worleyOptions.OnGUI();
                break;
            }

            if(GUILayout.Button("Generate")){
                INoise2D noise = null;
                switch(_noiseType){
                    case NoiseType.Value:
                    noise = new ValueNoise2D();
                    break;
                    case NoiseType.Perlin:
                    noise = new PerlinNoise2D();
                    break;
                    case NoiseType.Worley:
                    noise = new WorleyNoise2D(_worleyOptions.distanceAlgorithm,_worleyOptions.maxPointsInCell);
                    break;
                }
                _generatedTexture = _texOutputOptions.ExportNoiseTo(noise);
            }

            if(_generatedTexture != null){
                var rect = EditorGUILayout.GetControlRect(GUILayout.Width(256),GUILayout.Height(256));
                GUI.DrawTexture(rect,_generatedTexture);
            }
        }
    }

    public interface IBaseNoiseGenerateOptions{
        NoiseType type{
            get;
        }

        void OnGUI();

        INoise2D CreateNoise();
    }

    public class PerlinNoiseOptions:IBaseNoiseGenerateOptions{

        public NoiseType type{
            get{
                return NoiseType.Perlin;
            }
        }

        public INoise2D CreateNoise()
        {
            return new PerlinNoise2D();
        }

        public void OnGUI(){
         }

         

    }


    public class ValueNoiseOptions:IBaseNoiseGenerateOptions{

        public NoiseType type{
            get{
                return NoiseType.Value;
            }
        }

        public INoise2D CreateNoise()
        {
            return new ValueNoise2D();
        }

        public void OnGUI(){
        }

    }
    public class WorleyNoiseOptions:IBaseNoiseGenerateOptions{

        public DistanceAlgorithm distanceAlgorithm = DistanceAlgorithm.Euclidean;
        public int maxPointsInCell = 3;

        public NoiseType type{
            get{
                return NoiseType.Worley;
            }
        }

        public INoise2D CreateNoise()
        {
            return new WorleyNoise2D(distanceAlgorithm,maxPointsInCell);
        }

        public void OnGUI(){
            distanceAlgorithm = (DistanceAlgorithm)EditorGUILayout.EnumPopup("distanceAlgorithm",distanceAlgorithm);
            maxPointsInCell = EditorGUILayout.IntSlider("maxPointsInCell",maxPointsInCell,1,5);
        }
    }


 
}
