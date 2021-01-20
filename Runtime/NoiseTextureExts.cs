using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MS.Noise{

    public class NoiseTextureGenerateOptions{

        public int width = 256;
        public int height = 256;
        public int cellSize = 16;
        public bool markNoLongerReadable = false;
    }
    public static class NoiseTextureExts
    {

        public static Texture2D CreateTexture(this INoise2D noise,NoiseTextureGenerateOptions options){
            // tex.set
            var width = options.width;
            var height = options.height;
            var cellSize = options.cellSize;
            Color[] colors = new Color[width * height];
            var index = 0;
            for(var x = 0; x < width; x++){
                for(var y =0;y < height; y ++){
                    float fx = x *1f/cellSize;
                    float fy = y * 1f / cellSize;
                    float value = (noise.Evaluate(fx,fy) + 1) * 0.5f;
                    // value = 0.5f;
                    colors[index] = new Color(value,value,value,1);
                    index ++;
                }
            }
            var tex = new Texture2D(options.width,options.height,TextureFormat.RGB24,false,false);
            tex.filterMode = FilterMode.Point;
            tex.SetPixels(colors,0);
            tex.Apply(false,options.markNoLongerReadable);
            return tex;
        }

    }
}
