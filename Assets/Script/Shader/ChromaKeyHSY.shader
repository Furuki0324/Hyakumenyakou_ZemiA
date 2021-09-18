Shader "Custom/ChromaKeyHSY"
{
    Properties
    {
        _KeyColor("Key Color", color) = (0,1,0)
        _Near("Near", Range(0,2)) = 0.1
        _MainTex ("Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout"}

        CGPROGRAM

        #pragma surface surf Lambert alpha

        sampler2D _MainTex;
    fixed3 _KeyColor;
    fixed _Near;

    struct Input {
        float2 uv_MainTex;
    };

    fixed GetHue(fixed3 rgb) {
        fixed hue = 0;
        fixed minValue = min(rgb.r, min(rgb.g, rgb.b));
        fixed maxValue = max(rgb.r, max(rgb.g, rgb.b));
        fixed delta = maxValue - minValue;
        if (delta != 0) {
            if (maxValue == rgb.r) {
                hue = (rgb.g - rgb.b) / delta;
            }
            else if (maxValue == rgb.g) {
                hue = 2.0 + (rgb.b - rgb.g) / delta;
            }
            else {
                hue = 4.0 + (rgb.r - rgb.g) / delta;
            }

            hue /= 6.0;

            if (hue < 0) {
                hue += 1.0;
            }
        }

        return hue;
    }

    void surf(Input IN, inout SurfaceOutput o) {
        fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
        fixed distance  = GetHue(c.rgb) - GetHue(_KeyColor);
        if (distance > 0.5) {
            distance = 1.0 - distance;
        }
        else if (distance < -0.5) {
            distance = 1.0 + distance;
        }
        else {
            distance = abs(distance);
        }

        clip(distance - _Near);
        o.Albedo = c.rgb;
        o.Alpha = c.a;
    }

    ENDCG
    }

        Fallback"Transparent/Diffuse"
}
