using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace MS.Noise.Editor{

    public enum NoiseType{
        White,
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
        private PerlinNoiseOptions _perlinOptions;

        private Dictionary<NoiseType, IBaseNoiseGenerateOptions> _noiseOptions = new Dictionary<NoiseType, IBaseNoiseGenerateOptions>();



        private void OnGUI() {
            
            _noiseType = (NoiseType)EditorGUILayout.EnumPopup("NoiseType",_noiseType);

            if(_texOutputOptions == null){
                _texOutputOptions = new NoiseTextureOutputOptions();
            }

            _texOutputOptions.OnGUI();

            if(!_noiseOptions.ContainsKey(_noiseType)){
                _noiseOptions.Add(_noiseType,NoiseEditorUtil.CreateNoiseOptionsByType(_noiseType));
            }

            _noiseOptions[_noiseType].OnGUI();
        

            if(GUILayout.Button("Generate")){
                var noiseOptions = _noiseOptions[_noiseType];
                INoise2D noise = noiseOptions.CreateNoise(_texOutputOptions);
                _generatedTexture =  _texOutputOptions.ExportNoiseTo(noise);
            }

            if(_generatedTexture != null){
                var rect = EditorGUILayout.GetControlRect(GUILayout.Width(256),GUILayout.Height(256));
                GUI.DrawTexture(rect,_generatedTexture);
            }
        }
    }


    public static class NoiseEditorUtil{
        public static IBaseNoiseGenerateOptions CreateNoiseOptionsByType(NoiseType type){
            switch(type){
                case NoiseType.Perlin:
                return new PerlinNoiseOptions();
                case NoiseType.Value:
                return new ValueNoiseOptions();
                case NoiseType.Worley:
                return new WorleyNoiseOptions();
                case NoiseType.White:
                return new WhiteNoiseOptions();
            }
            return null;
        }
    }

    public interface IBaseNoiseGenerateOptions{
        NoiseType type{
            get;
        }

        void OnGUI();

        INoise2D CreateNoise(NoiseTextureOutputOptions texOptions);
    }

    public class PerlinNoiseOptions:IBaseNoiseGenerateOptions{
        
        private bool _loop;
        public NoiseType type{
            get{
                return NoiseType.Perlin;
            }
        }

        public bool loop{
            get{
                return _loop;
            }
        }

        public INoise2D CreateNoise(NoiseTextureOutputOptions texOptions)
        {
            var loopSize = new Vector2Int(0,0);
            if(loop){
                loopSize.x = Mathf.FloorToInt(texOptions.imageSize.x / texOptions.cellSize);
                loopSize.y = Mathf.FloorToInt(texOptions.imageSize.y / texOptions.cellSize);
            }
            return new PerlinNoise2D(new PerlinNoise2D.Options(){
                loopSize = loopSize
            });
        }

        public void OnGUI(){
            _loop = EditorGUILayout.Toggle("Loop",_loop);
        }

         

    }

    public class WhiteNoiseOptions:IBaseNoiseGenerateOptions{

        public NoiseType type{
            get{
                return NoiseType.White;
            }
        }

        public INoise2D CreateNoise(NoiseTextureOutputOptions texOptions)
        {
            return new WhiteNoise2D();
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
        public bool loop{
            get;private set;
        }

        public INoise2D CreateNoise(NoiseTextureOutputOptions texOptions)
        {
            var options = new ValueNoise2D.Options();
            var loopSize = new Vector2Int(0,0);
            if(loop){
                loopSize.x = Mathf.FloorToInt(texOptions.imageSize.x / texOptions.cellSize);
                loopSize.y = Mathf.FloorToInt(texOptions.imageSize.y / texOptions.cellSize);
            }
            options.loopSize = loopSize;
            return new ValueNoise2D(options);
        }

        public void OnGUI(){
            loop = EditorGUILayout.Toggle("Loop",loop);
        }

    }
    public class WorleyNoiseOptions:IBaseNoiseGenerateOptions{

        public DistanceAlgorithm distanceAlgorithm = DistanceAlgorithm.Euclidean;
        public int maxPointsInCell = 3;

        public bool loop{
            get;private set;
        }

        public NoiseType type{
            get{
                return NoiseType.Worley;
            }
        }

        public INoise2D CreateNoise(NoiseTextureOutputOptions texOptions)
        {
            var options = new WorleyNoise2D.Options();
            options.distanceAlgorithm = distanceAlgorithm;
            options.maxPointsInCell = maxPointsInCell;
            if(loop){
                options.loopSize = new Vector2Int(Mathf.FloorToInt(texOptions.imageSize.x/texOptions.cellSize)
                ,Mathf.FloorToInt(texOptions.imageSize.y/texOptions.cellSize));
            }
            return new WorleyNoise2D(options);
        }

        public void OnGUI(){
            distanceAlgorithm = (DistanceAlgorithm)EditorGUILayout.EnumPopup("distanceAlgorithm",distanceAlgorithm);
            maxPointsInCell = EditorGUILayout.IntSlider("maxPointsInCell",maxPointsInCell,1,5);
            loop = EditorGUILayout.Toggle("loop",loop);
        }
    }


 
}
