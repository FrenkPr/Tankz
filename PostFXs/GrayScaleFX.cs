using System;
using Aiv.Fast2D;

namespace Tankz
{
    class GrayScaleFX : PostProcessingEffect
    {
        private static string fragmentShader = @"
        #version 330 core

        in vec2 uv;
        uniform sampler2D tex;
        out vec4 out_color;

        void main() 
        {
            vec4 tex_color = texture(tex, uv);
    
            float gray = (tex_color.r + tex_color.g + tex_color.b) / 3.f;
    
            out_color = vec4(gray,gray,gray,1.f);
        }
        ";

        public GrayScaleFX() : base(fragmentShader) { }
    }
}
