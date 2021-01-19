# UniNoise

Unity的噪声函数库，有如下功能:

- 支持噪声类型:
    - WhiteNoise2D
    - ValueNoise(1D,2D)
    - PerlinNoise(1D,2D)
    - WorleyNoise2D

- 支持编辑器生成以上2D类型的噪声贴图.

- 支持通过多个基础噪声，生成分形噪声.

- 支持调整噪声的频次和震幅.



# 噪声函数介绍

__Attention: 内置的噪声函数，默认的频次与振幅均为1,满足以下规范:__
- 参数分布范围为(-infinite,+infinite)
- 返回值为(-1,1)


如想获取不同频次和振幅的噪声函数，请使用`TransformedNoise2D(INoise2D source,float frequency,float amplitude)`.

## WhiteNoise

[Check wiki](https://en.wikipedia.org/wiki/White_noise)

<img src="https://raw.githubusercontent.com/wiki/wlgys8/UniNoise/.imgs/WhiteNoise.jpg" width="300"/>

Usage:

```csharp

var noise = new WhiteNoise2D();
var result = noise.Evaluate(1,1);

```

## ValueNoise

[check wiki](https://en.wikipedia.org/wiki/Value_noise)

<img src="https://raw.githubusercontent.com/wiki/wlgys8/UniNoise/.imgs/ValueNoise.jpg" width="300"/>

Usage:

```csharp

var noise = new ValueNoise2D();
var result = noise.Evaluate(1,1);

```




## PerlinNoise

[wiki](https://en.wikipedia.org/wiki/Perlin_noise)

<img src="https://raw.githubusercontent.com/wiki/wlgys8/UniNoise/.imgs/PerlinNoise.jpg" width="300"/>

```csharp

var noise = new PerlinNoise2D();
var result = noise.Evaluate(1,1);

```

## WorleyNoise

<img src="https://raw.githubusercontent.com/wiki/wlgys8/UniNoise/.imgs/WorleyNoise.jpg" width="300"/>

```csharp

var noise = new WorleyNoise2D();
var result = noise.Evaluate(1,1);

```




# 2D噪声贴图生成

在编辑器中，我们可以:
- 通过`Window/Noise/Base`来打开基础噪声贴图生成工具.
- 通过`Window/Noise/Fractal`来打开分形噪声贴图生成工具.

运行时，通过

```csharp

Texture2D INoise2D.CreateTexture(NoiseTextureGenerateOptions options);

```

来创建噪声贴图


# 1D噪声调试工具


