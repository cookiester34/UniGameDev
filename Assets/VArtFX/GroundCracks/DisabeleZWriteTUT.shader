Shader "Custom/DisableZWriteTUT"
{
	Properties{}

		SubShader{

			Tags {
				"RenderType" = "Opaque"
			}

			Pass {
				ZWrite Off
			}
	}
}