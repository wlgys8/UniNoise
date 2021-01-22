
/**

reference:

https://thebookofshaders.com/10/

**/


///1D pseudo random. the return value is between (0,1)
float PseudoRandom(float x){
    return frac(sin(x) * 43758.5453123);
}

///2D pseudo random.
float PseudoRandom(float2 xy){
    return PseudoRandom(dot(xy,float2(12.9898,78.233)));
}

float2 PseudoRandom2(float2 xy){
    xy = float2( dot(xy,float2(127.1,311.7)),
              dot(xy,float2(269.5,183.3)) );
    return frac(sin(xy)*43758.5453123);
}

float WhiteNoise(float2 uv,float2 period){
    float2 uvInt = floor(uv * period);
    return PseudoRandom(uvInt);
}

float ValueNoise(float x){
    float ix = floor(x);
    float fx = frac(x);
    float v1 = PseudoRandom(ix);
    float v2 = PseudoRandom(ix + 1);
    float t = smoothstep(0,1,fx);
    return lerp(v1,v2,t);
}


float ValueNoise(float2 xy){
    float2 ixy = floor(xy);
    float2 fxy = xy - ixy;
    float v1 = PseudoRandom(ixy);
    float v2 = PseudoRandom(ixy + float2(1,0));
    float v3 = PseudoRandom(ixy + float2(0,1));
    float v4 = PseudoRandom(ixy + float2(1,1));

    fxy = smoothstep(0,1,fxy);

    float a = lerp(v1,v2,fxy.x);
    float b = lerp(v3,v4,fxy.x);
    return lerp(a,b,fxy.y);
}


///return a random grad vector
static float2 RandomGrad(float2 xy){
    xy = PseudoRandom2(xy);
    return xy * 2 - 1;
}

float PerlinNoise(float2 xy){
    float2 ixy = floor(xy);
    float2 fxy = xy - ixy;

    float2 g1 = RandomGrad(ixy);
    float2 g2 = RandomGrad(ixy + float2(1,0));
    float2 g3 = RandomGrad(ixy + float2(0,1));
    float2 g4 = RandomGrad(ixy + float2(1,1));

    float2 d1 = fxy;
    float2 d2 = fxy - float2(1,0);
    float2 d3 = fxy - float2(0,1);
    float2 d4 = fxy - float2(1,1);

    float v1 = dot(g1,d1);
    float v2 = dot(g2,d2);
    float v3 = dot(g3,d3);
    float v4 = dot(g4,d4);
    float2 txy = smoothstep(0,1,fxy);
    float a = lerp(v1,v2,txy.x);
    float b = lerp(v3,v4,txy.x);
    float c = lerp(a,b,txy.y);
    return (c + 1) * 0.5;
}

float WorleyNoise(float2 xy){
    float2 ixy = floor(xy);
    float2 fxy = xy - ixy;   
    float minDis = 100000;
    for(int i = -1;i <= 1; i ++){
        for(int j = -1;j <= 1; j ++){
            float2 ij = float2(i,j);
            float2 rxy = PseudoRandom2(ixy + ij);
            float2 d = ij + rxy - fxy;
            float dis = dot(d,d);
            minDis = min(dis,minDis);
        }
    }
    return sqrt(2 * minDis ) / 2;
}