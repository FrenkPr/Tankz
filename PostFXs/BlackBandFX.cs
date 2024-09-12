using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;

namespace Tankz
{
    class BlackBandFX : PostProcessingEffect
    {
        //GLSL: OpenGl Shading Language
        private static string fragmentShader =
        @"
            #version 330 core
            
            in vec2 uv;
            uniform sampler2D tex;
            out vec4 out_color;

            void main()
            {
                vec4 texture_color = texture(tex, uv);
                
                if (uv.y < 0.1f || uv.y > 0.9f)
                {
                    out_color = vec4(0.f, 0.f, 0.f, 0.f);     
                }

                else
                {
                    out_color = texture_color;
                }
            }
        ";

        public BlackBandFX() : base(fragmentShader)
        {

        }
    }
}
