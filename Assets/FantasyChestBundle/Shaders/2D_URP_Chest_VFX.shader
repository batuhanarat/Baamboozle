// Made with Amplify Shader Editor v1.9.8
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "2D_URP_Chest_VFX"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[Enum(UnityEngine.Rendering.CullMode)]_CullMode("CullMode", Float) = 2
		[Enum(Additive,1,AlphaBlend,10)]_BlendMode("BlendMode", Float) = 1
		[HDR]_BaseColor("BaseColor", Color) = (1,1,1,1)
		_Brightness("Brightness", Float) = 1
		_ColorContrast("ColorContrast", Range( 0.001 , 100)) = 1
		[NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
		_MainTexTilingOffset("MainTexTilingOffset", Vector) = (1,1,0,0)
		_AlphaStrength("AlphaStrength", Range( 0 , 1)) = 1
		[Header((MainTexContoller)_X.Uspeed_Y.Vspeed)][Header((MainTexContoller)_Z.RotateDir_W.RotateSpeed)][Header(.)]_MainTexController("MainTexController", Vector) = (0,0,0,0)
		[Toggle(_POLARUV_MAINTEX_ON)] _PolarUV_MainTex("PolarUV_MainTex", Float) = 0
		_PolarControl_MainTex("PolarControl_MainTex", Vector) = (0,0,0,0)
		[NoScaleOffset]_TurbulenceTex("TurbulenceTex", 2D) = "black" {}
		_TurbulenceTilingOffset("TurbulenceTilingOffset", Vector) = (1,1,0,0)
		[Header((DistortController)_X.Uspeed_Y.Vspeed)][Header((DistortController)_Z.RotateSpd_W.DistortPower)][Header(.)]_TurbulenceController("TurbulenceController", Vector) = (0,0,0,0)
		[NoScaleOffset]_Mask("Mask", 2D) = "white" {}
		_maskTilingOffset("maskTilingOffset", Vector) = (1,1,0,0)
		_MaskContrast("MaskContrast", Range( 0.001 , 100)) = 1
		[Toggle(_MASKCHANNEL_R_ON)] _MaskChannel_R("MaskChannel_R", Float) = 1
		_MaskStrength("MaskStrength", Range( 0 , 1)) = 1

		[HideInInspector][NoScaleOffset] unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
	}

	SubShader
	{
		LOD 0

		

        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "UniversalMaterialType"="Lit" "Queue"="Transparent" "ShaderGraphShader"="true" }

		Cull [_CullMode]
		Blend SrcAlpha [_BlendMode]
		ZTest LEqual
		ZWrite Off
		Offset 0 , 0
		ColorMask RGBA
		

		HLSLINCLUDE
		#pragma target 2.0
		#pragma prefer_hlslcc gles
		// ensure rendering platforms toggle list is visible

		ENDHLSL

		
		Pass
		{
			
			Name "Sprite Lit"
            Tags { "LightMode"="Universal2D" }

			HLSLPROGRAM

			#define ASE_VERSION 19800
			#define ASE_SRP_VERSION 120111


			#pragma vertex vert
			#pragma fragment frag

            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define VARYINGS_NEED_SCREENPOSITION
            #define FEATURES_GRAPH_VERTEX

			#define SHADERPASS SHADERPASS_SPRITELIT

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging2D.hlsl"

			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local _POLARUV_MAINTEX_ON
			#pragma shader_feature_local _MASKCHANNEL_R_ON


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float3 positionWS : TEXCOORD1;
				float4 color : TEXCOORD2;
				float4 screenPosition : TEXCOORD3;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

            struct SurfaceDescription
			{
				float3 BaseColor;
				float Alpha;
			};

			half4 _RendererColor;

			sampler2D _MainTex;
			sampler2D _TurbulenceTex;
			sampler2D _Mask;
			CBUFFER_START( UnityPerMaterial )
			float4 _BaseColor;
			float4 _MainTexController;
			float4 _MainTexTilingOffset;
			float4 _TurbulenceController;
			float4 _TurbulenceTilingOffset;
			float4 _maskTilingOffset;
			float4 _PolarControl_MainTex;
			float _BlendMode;
			float _CullMode;
			float _MaskContrast;
			float _MaskStrength;
			float _ColorContrast;
			float _Brightness;
			float _AlphaStrength;
			CBUFFER_END


			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			

			VertexOutput vert( VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif
				v.normal = v.normal;
				v.tangent.xyz = v.tangent.xyz;

				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.positionOS.xyz);

				o.positionCS = vertexInput.positionCS;
				o.positionWS.xyz = vertexInput.positionWS;
				o.texCoord0.xyzw = v.uv0;
				o.color.xyzw =  v.color;
				o.screenPosition.xyzw = vertexInput.positionNDC;

				return o;
			}

			half4 frag( VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				float2 appendResult136 = (float2(_MainTexTilingOffset.z , _MainTexTilingOffset.w));
				float2 texCoord10 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult135 = (float2(_MainTexTilingOffset.x , _MainTexTilingOffset.y));
				float2 temp_output_137_0 = ( appendResult136 + ( texCoord10 * appendResult135 ) );
				float2 appendResult21 = (float2(_TurbulenceController.x , _TurbulenceController.y));
				float2 appendResult131 = (float2(_TurbulenceTilingOffset.z , _TurbulenceTilingOffset.w));
				float2 texCoord20 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult130 = (float2(_TurbulenceTilingOffset.x , _TurbulenceTilingOffset.y));
				float3 rotatedValue49 = RotateAroundAxis( float3( 0.5,0.5,0 ), float3( ( appendResult131 + ( texCoord20 * appendResult130 ) ) ,  0.0 ), float3( 0,0,1 ), ( _TurbulenceController.z * _TimeParameters.x ) );
				float2 panner22 = ( 1.0 * _Time.y * appendResult21 + rotatedValue49.xy);
				float2 appendResult127 = (float2(_maskTilingOffset.z , _maskTilingOffset.w));
				float2 texCoord30 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult126 = (float2(_maskTilingOffset.x , _maskTilingOffset.y));
				float4 tex2DNode29 = tex2D( _Mask, ( appendResult127 + ( texCoord30 * appendResult126 ) ) );
				#ifdef _MASKCHANNEL_R_ON
				float staticSwitch73 = tex2DNode29.r;
				#else
				float staticSwitch73 = tex2DNode29.a;
				#endif
				float lerpResult116 = lerp( 1.0 , pow( staticSwitch73 , abs( _MaskContrast ) ) , _MaskStrength);
				float2 lerpResult32 = lerp( temp_output_137_0 , ( temp_output_137_0 + ( (tex2D( _TurbulenceTex, panner22 )).rg * _TurbulenceController.w ) ) , lerpResult116);
				float3 rotatedValue75 = RotateAroundAxis( float3( 0.5,0.5,0 ), float3( lerpResult32 ,  0.0 ), float3( 0,0,1 ), ( ( _MainTexController.w * _TimeParameters.x * sign( _MainTexController.z ) ) + _MainTexController.z ) );
				float2 panner13 = ( 1.0 * _Time.y * (_MainTexController).xy + rotatedValue75.xy);
				float2 temp_output_34_0_g1 = ( lerpResult32 - float2( 0.5,0.5 ) );
				float2 break39_g1 = temp_output_34_0_g1;
				float2 appendResult50_g1 = (float2(( _PolarControl_MainTex.x * ( length( temp_output_34_0_g1 ) * 2.0 ) ) , ( ( atan2( break39_g1.x , break39_g1.y ) * ( 1.0 / TWO_PI ) ) * _PolarControl_MainTex.y )));
				float2 appendResult86 = (float2(_PolarControl_MainTex.z , _PolarControl_MainTex.w));
				float2 panner54 = ( 1.0 * _Time.y * appendResult86 + float2( 0,0 ));
				#ifdef _POLARUV_MAINTEX_ON
				float2 staticSwitch83 = ( appendResult50_g1 + panner54 );
				#else
				float2 staticSwitch83 = panner13;
				#endif
				float4 tex2DNode12 = tex2D( _MainTex, staticSwitch83 );
				float3 temp_cast_5 = (abs( _ColorContrast )).xxx;
				
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				surfaceDescription.BaseColor = ( float4( _BaseColor.rgb , 0.0 ) * float4( pow( tex2DNode12.rgb , temp_cast_5 ) , 0.0 ) * _Brightness * IN.color ).rgb;
				surfaceDescription.Alpha = ( _BaseColor.a * tex2DNode12.a * lerpResult116 * IN.color.a * _AlphaStrength );

				half4 color = half4(surfaceDescription.BaseColor, surfaceDescription.Alpha);

				#if defined(DEBUG_DISPLAY)
				SurfaceData2D surfaceData;
				InitializeSurfaceData(color.rgb, color.a, surfaceData);
				InputData2D inputData;
				InitializeInputData(IN.positionWS.xy, half2(IN.texCoord0.xy), inputData);
				half4 debugColor = 0;

				SETUP_DEBUG_DATA_2D(inputData, IN.positionWS);

				if (CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
				{
					return debugColor;
				}
				#endif

				color *= IN.color * _RendererColor;
				return color;
			}

			ENDHLSL
		}

		
		Pass
		{
			
            Name "Sprite Normal"
            Tags { "LightMode"="NormalsRendering" }

			HLSLPROGRAM

			#define ASE_VERSION 19800
			#define ASE_SRP_VERSION 120111


			#pragma vertex vert
			#pragma fragment frag

			#define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TANGENT_WS
            #define FEATURES_GRAPH_VERTEX

			#define SHADERPASS SHADERPASS_SPRITENORMAL

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#pragma shader_feature_local _POLARUV_MAINTEX_ON
			#pragma shader_feature_local _MASKCHANNEL_R_ON


			sampler2D _MainTex;
			sampler2D _TurbulenceTex;
			sampler2D _Mask;
			CBUFFER_START( UnityPerMaterial )
			float4 _BaseColor;
			float4 _MainTexController;
			float4 _MainTexTilingOffset;
			float4 _TurbulenceController;
			float4 _TurbulenceTilingOffset;
			float4 _maskTilingOffset;
			float4 _PolarControl_MainTex;
			float _BlendMode;
			float _CullMode;
			float _MaskContrast;
			float _MaskStrength;
			float _ColorContrast;
			float _Brightness;
			float _AlphaStrength;
			CBUFFER_END


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float3 normalWS : TEXCOORD0;
				float4 tangentWS : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

            struct SurfaceDescription
			{
				float3 NormalTS;
				float Alpha;
			};

			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			

			VertexOutput vert( VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif
				v.normal = v.normal;
				v.tangent.xyz = v.tangent.xyz;

				float3 positionWS = TransformObjectToWorld(v.positionOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);

				o.positionCS = TransformWorldToHClip(positionWS);
				o.normalWS.xyz =  -GetViewForwardDir();
				o.tangentWS.xyzw =  tangentWS;
				return o;
			}

			half4 frag( VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				float2 appendResult136 = (float2(_MainTexTilingOffset.z , _MainTexTilingOffset.w));
				float2 texCoord10 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult135 = (float2(_MainTexTilingOffset.x , _MainTexTilingOffset.y));
				float2 temp_output_137_0 = ( appendResult136 + ( texCoord10 * appendResult135 ) );
				float2 appendResult21 = (float2(_TurbulenceController.x , _TurbulenceController.y));
				float2 appendResult131 = (float2(_TurbulenceTilingOffset.z , _TurbulenceTilingOffset.w));
				float2 texCoord20 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult130 = (float2(_TurbulenceTilingOffset.x , _TurbulenceTilingOffset.y));
				float3 rotatedValue49 = RotateAroundAxis( float3( 0.5,0.5,0 ), float3( ( appendResult131 + ( texCoord20 * appendResult130 ) ) ,  0.0 ), float3( 0,0,1 ), ( _TurbulenceController.z * _TimeParameters.x ) );
				float2 panner22 = ( 1.0 * _Time.y * appendResult21 + rotatedValue49.xy);
				float2 appendResult127 = (float2(_maskTilingOffset.z , _maskTilingOffset.w));
				float2 texCoord30 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult126 = (float2(_maskTilingOffset.x , _maskTilingOffset.y));
				float4 tex2DNode29 = tex2D( _Mask, ( appendResult127 + ( texCoord30 * appendResult126 ) ) );
				#ifdef _MASKCHANNEL_R_ON
				float staticSwitch73 = tex2DNode29.r;
				#else
				float staticSwitch73 = tex2DNode29.a;
				#endif
				float lerpResult116 = lerp( 1.0 , pow( staticSwitch73 , abs( _MaskContrast ) ) , _MaskStrength);
				float2 lerpResult32 = lerp( temp_output_137_0 , ( temp_output_137_0 + ( (tex2D( _TurbulenceTex, panner22 )).rg * _TurbulenceController.w ) ) , lerpResult116);
				float3 rotatedValue75 = RotateAroundAxis( float3( 0.5,0.5,0 ), float3( lerpResult32 ,  0.0 ), float3( 0,0,1 ), ( ( _MainTexController.w * _TimeParameters.x * sign( _MainTexController.z ) ) + _MainTexController.z ) );
				float2 panner13 = ( 1.0 * _Time.y * (_MainTexController).xy + rotatedValue75.xy);
				float2 temp_output_34_0_g1 = ( lerpResult32 - float2( 0.5,0.5 ) );
				float2 break39_g1 = temp_output_34_0_g1;
				float2 appendResult50_g1 = (float2(( _PolarControl_MainTex.x * ( length( temp_output_34_0_g1 ) * 2.0 ) ) , ( ( atan2( break39_g1.x , break39_g1.y ) * ( 1.0 / TWO_PI ) ) * _PolarControl_MainTex.y )));
				float2 appendResult86 = (float2(_PolarControl_MainTex.z , _PolarControl_MainTex.w));
				float2 panner54 = ( 1.0 * _Time.y * appendResult86 + float2( 0,0 ));
				#ifdef _POLARUV_MAINTEX_ON
				float2 staticSwitch83 = ( appendResult50_g1 + panner54 );
				#else
				float2 staticSwitch83 = panner13;
				#endif
				float4 tex2DNode12 = tex2D( _MainTex, staticSwitch83 );
				
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				surfaceDescription.NormalTS = float3(0.0f, 0.0f, 1.0f);
				surfaceDescription.Alpha = ( _BaseColor.a * tex2DNode12.a * lerpResult116 * IN.ase_color.a * _AlphaStrength );

				half crossSign = (IN.tangentWS.w > 0.0 ? 1.0 : -1.0) * GetOddNegativeScale();
				half3 bitangent = crossSign * cross(IN.normalWS.xyz, IN.tangentWS.xyz);
				half4 color = half4(1.0,1.0,1.0, surfaceDescription.Alpha);

				return NormalsRenderingShared(color, surfaceDescription.NormalTS, IN.tangentWS.xyz, bitangent, IN.normalWS);
			}

            ENDHLSL
        }

		
        Pass
        {
			
            Name "SceneSelectionPass"
            Tags { "LightMode"="SceneSelectionPass" }

            Cull Off
			Blend Off
			ZTest LEqual
			ZWrite On

            HLSLPROGRAM

			#define ASE_VERSION 19800
			#define ASE_SRP_VERSION 120111


			#pragma vertex vert
			#pragma fragment frag

            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define FEATURES_GRAPH_VERTEX

            #define SHADERPASS SHADERPASS_DEPTHONLY
			#define SCENESELECTIONPASS 1

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#pragma shader_feature_local _POLARUV_MAINTEX_ON
			#pragma shader_feature_local _MASKCHANNEL_R_ON


			sampler2D _MainTex;
			sampler2D _TurbulenceTex;
			sampler2D _Mask;
			CBUFFER_START( UnityPerMaterial )
			float4 _BaseColor;
			float4 _MainTexController;
			float4 _MainTexTilingOffset;
			float4 _TurbulenceController;
			float4 _TurbulenceTilingOffset;
			float4 _maskTilingOffset;
			float4 _PolarControl_MainTex;
			float _BlendMode;
			float _CullMode;
			float _MaskContrast;
			float _MaskStrength;
			float _ColorContrast;
			float _Brightness;
			float _AlphaStrength;
			CBUFFER_END


            struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

            int _ObjectId;
            int _PassValue;

            struct SurfaceDescription
			{
				float Alpha;
			};

			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			

			VertexOutput vert(VertexInput v )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_texcoord.xy = v.ase_texcoord.xy;
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				float3 positionWS = TransformObjectToWorld(v.positionOS);
				o.positionCS = TransformWorldToHClip(positionWS);

				return o;
			}

			half4 frag(VertexOutput IN) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				float2 appendResult136 = (float2(_MainTexTilingOffset.z , _MainTexTilingOffset.w));
				float2 texCoord10 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult135 = (float2(_MainTexTilingOffset.x , _MainTexTilingOffset.y));
				float2 temp_output_137_0 = ( appendResult136 + ( texCoord10 * appendResult135 ) );
				float2 appendResult21 = (float2(_TurbulenceController.x , _TurbulenceController.y));
				float2 appendResult131 = (float2(_TurbulenceTilingOffset.z , _TurbulenceTilingOffset.w));
				float2 texCoord20 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult130 = (float2(_TurbulenceTilingOffset.x , _TurbulenceTilingOffset.y));
				float3 rotatedValue49 = RotateAroundAxis( float3( 0.5,0.5,0 ), float3( ( appendResult131 + ( texCoord20 * appendResult130 ) ) ,  0.0 ), float3( 0,0,1 ), ( _TurbulenceController.z * _TimeParameters.x ) );
				float2 panner22 = ( 1.0 * _Time.y * appendResult21 + rotatedValue49.xy);
				float2 appendResult127 = (float2(_maskTilingOffset.z , _maskTilingOffset.w));
				float2 texCoord30 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult126 = (float2(_maskTilingOffset.x , _maskTilingOffset.y));
				float4 tex2DNode29 = tex2D( _Mask, ( appendResult127 + ( texCoord30 * appendResult126 ) ) );
				#ifdef _MASKCHANNEL_R_ON
				float staticSwitch73 = tex2DNode29.r;
				#else
				float staticSwitch73 = tex2DNode29.a;
				#endif
				float lerpResult116 = lerp( 1.0 , pow( staticSwitch73 , abs( _MaskContrast ) ) , _MaskStrength);
				float2 lerpResult32 = lerp( temp_output_137_0 , ( temp_output_137_0 + ( (tex2D( _TurbulenceTex, panner22 )).rg * _TurbulenceController.w ) ) , lerpResult116);
				float3 rotatedValue75 = RotateAroundAxis( float3( 0.5,0.5,0 ), float3( lerpResult32 ,  0.0 ), float3( 0,0,1 ), ( ( _MainTexController.w * _TimeParameters.x * sign( _MainTexController.z ) ) + _MainTexController.z ) );
				float2 panner13 = ( 1.0 * _Time.y * (_MainTexController).xy + rotatedValue75.xy);
				float2 temp_output_34_0_g1 = ( lerpResult32 - float2( 0.5,0.5 ) );
				float2 break39_g1 = temp_output_34_0_g1;
				float2 appendResult50_g1 = (float2(( _PolarControl_MainTex.x * ( length( temp_output_34_0_g1 ) * 2.0 ) ) , ( ( atan2( break39_g1.x , break39_g1.y ) * ( 1.0 / TWO_PI ) ) * _PolarControl_MainTex.y )));
				float2 appendResult86 = (float2(_PolarControl_MainTex.z , _PolarControl_MainTex.w));
				float2 panner54 = ( 1.0 * _Time.y * appendResult86 + float2( 0,0 ));
				#ifdef _POLARUV_MAINTEX_ON
				float2 staticSwitch83 = ( appendResult50_g1 + panner54 );
				#else
				float2 staticSwitch83 = panner13;
				#endif
				float4 tex2DNode12 = tex2D( _MainTex, staticSwitch83 );
				
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				surfaceDescription.Alpha = ( _BaseColor.a * tex2DNode12.a * lerpResult116 * IN.ase_color.a * _AlphaStrength );

				#if _ALPHATEST_ON
					float alphaClipThreshold = 0.01f;
					#if ALPHA_CLIP_THRESHOLD
						alphaClipThreshold = surfaceDescription.AlphaClipThreshold;
					#endif
					clip(surfaceDescription.Alpha - alphaClipThreshold);
				#endif

				half4 outColor = half4(_ObjectId, _PassValue, 1.0, 1.0);
				return outColor;
			}

            ENDHLSL
        }

		
        Pass
        {
			
            Name "ScenePickingPass"
            Tags { "LightMode"="Picking" }

			Cull Off
			Blend Off
			ZTest LEqual
			ZWrite On

            HLSLPROGRAM

			#define ASE_VERSION 19800
			#define ASE_SRP_VERSION 120111


			#pragma vertex vert
			#pragma fragment frag

            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define FEATURES_GRAPH_VERTEX

            #define SHADERPASS SHADERPASS_DEPTHONLY
			#define SCENEPICKINGPASS 1

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

        	#pragma shader_feature_local _POLARUV_MAINTEX_ON
        	#pragma shader_feature_local _MASKCHANNEL_R_ON


			sampler2D _MainTex;
			sampler2D _TurbulenceTex;
			sampler2D _Mask;
			CBUFFER_START( UnityPerMaterial )
			float4 _BaseColor;
			float4 _MainTexController;
			float4 _MainTexTilingOffset;
			float4 _TurbulenceController;
			float4 _TurbulenceTilingOffset;
			float4 _maskTilingOffset;
			float4 _PolarControl_MainTex;
			float _BlendMode;
			float _CullMode;
			float _MaskContrast;
			float _MaskStrength;
			float _ColorContrast;
			float _Brightness;
			float _AlphaStrength;
			CBUFFER_END


            struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

            float4 _SelectionID;

            struct SurfaceDescription
			{
				float Alpha;
			};

			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			

			VertexOutput vert(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_texcoord.xy = v.ase_texcoord.xy;
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				float3 positionWS = TransformObjectToWorld(v.positionOS);
				o.positionCS = TransformWorldToHClip(positionWS);

				return o;
			}

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				float2 appendResult136 = (float2(_MainTexTilingOffset.z , _MainTexTilingOffset.w));
				float2 texCoord10 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult135 = (float2(_MainTexTilingOffset.x , _MainTexTilingOffset.y));
				float2 temp_output_137_0 = ( appendResult136 + ( texCoord10 * appendResult135 ) );
				float2 appendResult21 = (float2(_TurbulenceController.x , _TurbulenceController.y));
				float2 appendResult131 = (float2(_TurbulenceTilingOffset.z , _TurbulenceTilingOffset.w));
				float2 texCoord20 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult130 = (float2(_TurbulenceTilingOffset.x , _TurbulenceTilingOffset.y));
				float3 rotatedValue49 = RotateAroundAxis( float3( 0.5,0.5,0 ), float3( ( appendResult131 + ( texCoord20 * appendResult130 ) ) ,  0.0 ), float3( 0,0,1 ), ( _TurbulenceController.z * _TimeParameters.x ) );
				float2 panner22 = ( 1.0 * _Time.y * appendResult21 + rotatedValue49.xy);
				float2 appendResult127 = (float2(_maskTilingOffset.z , _maskTilingOffset.w));
				float2 texCoord30 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult126 = (float2(_maskTilingOffset.x , _maskTilingOffset.y));
				float4 tex2DNode29 = tex2D( _Mask, ( appendResult127 + ( texCoord30 * appendResult126 ) ) );
				#ifdef _MASKCHANNEL_R_ON
				float staticSwitch73 = tex2DNode29.r;
				#else
				float staticSwitch73 = tex2DNode29.a;
				#endif
				float lerpResult116 = lerp( 1.0 , pow( staticSwitch73 , abs( _MaskContrast ) ) , _MaskStrength);
				float2 lerpResult32 = lerp( temp_output_137_0 , ( temp_output_137_0 + ( (tex2D( _TurbulenceTex, panner22 )).rg * _TurbulenceController.w ) ) , lerpResult116);
				float3 rotatedValue75 = RotateAroundAxis( float3( 0.5,0.5,0 ), float3( lerpResult32 ,  0.0 ), float3( 0,0,1 ), ( ( _MainTexController.w * _TimeParameters.x * sign( _MainTexController.z ) ) + _MainTexController.z ) );
				float2 panner13 = ( 1.0 * _Time.y * (_MainTexController).xy + rotatedValue75.xy);
				float2 temp_output_34_0_g1 = ( lerpResult32 - float2( 0.5,0.5 ) );
				float2 break39_g1 = temp_output_34_0_g1;
				float2 appendResult50_g1 = (float2(( _PolarControl_MainTex.x * ( length( temp_output_34_0_g1 ) * 2.0 ) ) , ( ( atan2( break39_g1.x , break39_g1.y ) * ( 1.0 / TWO_PI ) ) * _PolarControl_MainTex.y )));
				float2 appendResult86 = (float2(_PolarControl_MainTex.z , _PolarControl_MainTex.w));
				float2 panner54 = ( 1.0 * _Time.y * appendResult86 + float2( 0,0 ));
				#ifdef _POLARUV_MAINTEX_ON
				float2 staticSwitch83 = ( appendResult50_g1 + panner54 );
				#else
				float2 staticSwitch83 = panner13;
				#endif
				float4 tex2DNode12 = tex2D( _MainTex, staticSwitch83 );
				
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				surfaceDescription.Alpha = ( _BaseColor.a * tex2DNode12.a * lerpResult116 * IN.ase_color.a * _AlphaStrength );

				#if _ALPHATEST_ON
					float alphaClipThreshold = 0.01f;
					#if ALPHA_CLIP_THRESHOLD
						alphaClipThreshold = surfaceDescription.AlphaClipThreshold;
					#endif
					clip(surfaceDescription.Alpha - alphaClipThreshold);
				#endif

				half4 outColor = _SelectionID;
				return outColor;
			}

            ENDHLSL
        }

		
		Pass
		{
			
            Name "Sprite Forward"
            Tags { "LightMode"="UniversalForward" }

			HLSLPROGRAM

			#define ASE_VERSION 19800
			#define ASE_SRP_VERSION 120111


			#pragma vertex vert
			#pragma fragment frag

            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX

			#define SHADERPASS SHADERPASS_SPRITEFORWARD

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging2D.hlsl"

			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local _POLARUV_MAINTEX_ON
			#pragma shader_feature_local _MASKCHANNEL_R_ON


			sampler2D _MainTex;
			sampler2D _TurbulenceTex;
			sampler2D _Mask;
			CBUFFER_START( UnityPerMaterial )
			float4 _BaseColor;
			float4 _MainTexController;
			float4 _MainTexTilingOffset;
			float4 _TurbulenceController;
			float4 _TurbulenceTilingOffset;
			float4 _maskTilingOffset;
			float4 _PolarControl_MainTex;
			float _BlendMode;
			float _CullMode;
			float _MaskContrast;
			float _MaskStrength;
			float _ColorContrast;
			float _Brightness;
			float _AlphaStrength;
			CBUFFER_END


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float3 positionWS : TEXCOORD1;
				float4 color : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

            struct SurfaceDescription
			{
				float3 BaseColor;
				float Alpha;
				float3 NormalTS;
			};

			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			

			VertexOutput vert( VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif
				v.normal = v.normal;
				v.tangent.xyz = v.tangent.xyz;

				float3 positionWS = TransformObjectToWorld(v.positionOS);

				o.positionCS = TransformWorldToHClip(positionWS);
				o.positionWS.xyz = positionWS;
				o.texCoord0.xyzw = v.uv0;
				o.color.xyzw = v.color;

				return o;
			}

			half4 frag( VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				float2 appendResult136 = (float2(_MainTexTilingOffset.z , _MainTexTilingOffset.w));
				float2 texCoord10 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult135 = (float2(_MainTexTilingOffset.x , _MainTexTilingOffset.y));
				float2 temp_output_137_0 = ( appendResult136 + ( texCoord10 * appendResult135 ) );
				float2 appendResult21 = (float2(_TurbulenceController.x , _TurbulenceController.y));
				float2 appendResult131 = (float2(_TurbulenceTilingOffset.z , _TurbulenceTilingOffset.w));
				float2 texCoord20 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult130 = (float2(_TurbulenceTilingOffset.x , _TurbulenceTilingOffset.y));
				float3 rotatedValue49 = RotateAroundAxis( float3( 0.5,0.5,0 ), float3( ( appendResult131 + ( texCoord20 * appendResult130 ) ) ,  0.0 ), float3( 0,0,1 ), ( _TurbulenceController.z * _TimeParameters.x ) );
				float2 panner22 = ( 1.0 * _Time.y * appendResult21 + rotatedValue49.xy);
				float2 appendResult127 = (float2(_maskTilingOffset.z , _maskTilingOffset.w));
				float2 texCoord30 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult126 = (float2(_maskTilingOffset.x , _maskTilingOffset.y));
				float4 tex2DNode29 = tex2D( _Mask, ( appendResult127 + ( texCoord30 * appendResult126 ) ) );
				#ifdef _MASKCHANNEL_R_ON
				float staticSwitch73 = tex2DNode29.r;
				#else
				float staticSwitch73 = tex2DNode29.a;
				#endif
				float lerpResult116 = lerp( 1.0 , pow( staticSwitch73 , abs( _MaskContrast ) ) , _MaskStrength);
				float2 lerpResult32 = lerp( temp_output_137_0 , ( temp_output_137_0 + ( (tex2D( _TurbulenceTex, panner22 )).rg * _TurbulenceController.w ) ) , lerpResult116);
				float3 rotatedValue75 = RotateAroundAxis( float3( 0.5,0.5,0 ), float3( lerpResult32 ,  0.0 ), float3( 0,0,1 ), ( ( _MainTexController.w * _TimeParameters.x * sign( _MainTexController.z ) ) + _MainTexController.z ) );
				float2 panner13 = ( 1.0 * _Time.y * (_MainTexController).xy + rotatedValue75.xy);
				float2 temp_output_34_0_g1 = ( lerpResult32 - float2( 0.5,0.5 ) );
				float2 break39_g1 = temp_output_34_0_g1;
				float2 appendResult50_g1 = (float2(( _PolarControl_MainTex.x * ( length( temp_output_34_0_g1 ) * 2.0 ) ) , ( ( atan2( break39_g1.x , break39_g1.y ) * ( 1.0 / TWO_PI ) ) * _PolarControl_MainTex.y )));
				float2 appendResult86 = (float2(_PolarControl_MainTex.z , _PolarControl_MainTex.w));
				float2 panner54 = ( 1.0 * _Time.y * appendResult86 + float2( 0,0 ));
				#ifdef _POLARUV_MAINTEX_ON
				float2 staticSwitch83 = ( appendResult50_g1 + panner54 );
				#else
				float2 staticSwitch83 = panner13;
				#endif
				float4 tex2DNode12 = tex2D( _MainTex, staticSwitch83 );
				float3 temp_cast_5 = (abs( _ColorContrast )).xxx;
				
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				surfaceDescription.BaseColor = ( float4( _BaseColor.rgb , 0.0 ) * float4( pow( tex2DNode12.rgb , temp_cast_5 ) , 0.0 ) * _Brightness * IN.color ).rgb;
				surfaceDescription.NormalTS = float3(0.0f, 0.0f, 1.0f);
				surfaceDescription.Alpha = ( _BaseColor.a * tex2DNode12.a * lerpResult116 * IN.color.a * _AlphaStrength );


				half4 color = half4(surfaceDescription.BaseColor, surfaceDescription.Alpha);

				#if defined(DEBUG_DISPLAY)
					SurfaceData2D surfaceData;
					InitializeSurfaceData(color.rgb, color.a, surfaceData);
					InputData2D inputData;
					InitializeInputData(IN.positionWS.xy, half2(IN.texCoord0.xy), inputData);
					half4 debugColor = 0;

					SETUP_DEBUG_DATA_2D(inputData, IN.positionWS);

					if (CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
					{
						return debugColor;
					}
				#endif

				color *= IN.color;
				return color;
			}

            ENDHLSL
        }
		
	}
	CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
	FallBack "Hidden/Shader Graph/FallbackError"
	
	Fallback Off
}
/*ASEBEGIN
Version=19800
Node;AmplifyShaderEditor.Vector4Node;133;-3696.705,1020.398;Inherit;False;Property;_TurbulenceTilingOffset;TurbulenceTilingOffset;12;0;Create;True;0;0;0;True;0;False;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;130;-3472,912;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-3568,720;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;52;-2960,1040;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;14;-3024,512;Inherit;False;Property;_TurbulenceController;TurbulenceController;13;1;[Header];Create;True;3;(DistortController)_X.Uspeed_Y.Vspeed;(DistortController)_Z.RotateSpd_W.DistortPower;.;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;-3232,912;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;131;-3456,1088;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;124;-3408,-208;Inherit;False;Property;_maskTilingOffset;maskTilingOffset;15;0;Create;True;0;0;0;True;0;False;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-2704,1008;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;126;-3152,-224;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;132;-3120,1152;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;30;-3312,-448;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotateAboutAxisNode;49;-2496,928;Inherit;False;False;4;0;FLOAT3;0,0,1;False;1;FLOAT;0;False;2;FLOAT3;0.5,0.5,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;21;-2640,544;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;125;-2916.276,-223.2715;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;127;-3146.213,-44.18372;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;22;-2208,784;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;128;-2811.333,6.776276;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;138;-2480.591,257.567;Inherit;False;Property;_MainTexTilingOffset;MainTexTilingOffset;6;0;Create;True;0;0;0;True;0;False;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;72;-2432,-144;Inherit;False;Property;_MaskContrast;MaskContrast;16;0;Create;True;0;0;0;False;0;False;1;1;0.001;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;135;-2256,272;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;29;-2496,-368;Inherit;True;Property;_Mask;Mask;14;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SamplerNode;19;-2016,512;Inherit;True;Property;_TurbulenceTex;TurbulenceTex;11;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-2416,64;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SwizzleNode;23;-1712,656;Inherit;False;FLOAT2;0;1;2;3;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;35;-832,96;Inherit;False;Property;_MainTexController;MainTexController;8;1;[Header];Create;True;3;(MainTexContoller)_X.Uspeed_Y.Vspeed;(MainTexContoller)_Z.RotateDir_W.RotateSpeed;.;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;73;-2176,-352;Inherit;False;Property;_MaskChannel_R;MaskChannel_R;17;0;Create;True;0;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;108;-2071.795,-97.03088;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;-1904,144;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;136;-2224,384;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-1536,736;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SignOpNode;81;-592,256;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;79;-624,384;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;113;-2016,32;Inherit;False;Property;_MaskStrength;MaskStrength;18;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;117;-1755.381,-81.30518;Inherit;False;Constant;_Float0;Float 0;15;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;65;-1792,-304;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;137;-1808,368;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-1424,624;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;84;-768,784;Inherit;False;Property;_PolarControl_MainTex;PolarControl_MainTex;10;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;-464,128;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;116;-1520,-64;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;86;-394.6914,784.5747;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;32;-1296,336;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;82;-304,208;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;37;-528,-64;Inherit;False;FLOAT2;0;1;2;3;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;54;-208,704;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;70;-384,544;Inherit;False;Polar Coordinates;-1;;1;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;1;False;4;FLOAT;1;False;3;FLOAT2;0;FLOAT;55;FLOAT;56
Node;AmplifyShaderEditor.RotateAboutAxisNode;75;-240,288;Inherit;False;False;4;0;FLOAT3;0,0,1;False;1;FLOAT;0;False;2;FLOAT3;0.5,0.5,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PannerNode;13;-240,-112;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;71;144,672;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;83;240,496;Inherit;False;Property;_PolarUV_MainTex;PolarUV_MainTex;9;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;66;560,448;Inherit;False;Property;_ColorContrast;ColorContrast;4;0;Create;True;0;0;0;False;0;False;1;0;0.001;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;109;925.4706,498.9025;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;12;352,64;Inherit;True;Property;_MainTex;MainTex;5;1;[NoScaleOffset];Create;True;0;0;0;True;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.ColorNode;34;368,-592;Inherit;False;Property;_BaseColor;BaseColor;2;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.VertexColorNode;110;170.9943,-325.4116;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;118;832,96;Inherit;False;Property;_AlphaStrength;AlphaStrength;7;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;106;800,-288;Inherit;False;Property;_Brightness;Brightness;3;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;67;992,288;Inherit;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;1120,-208;Inherit;False;4;4;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;816,-96;Inherit;False;5;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;111;1584,48;Inherit;False;Property;_BlendMode;BlendMode;1;1;[Enum];Create;True;0;2;Additive;1;AlphaBlend;10;0;True;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;112;1587.892,-99.46808;Inherit;False;Property;_CullMode;CullMode;0;1;[Enum];Create;True;0;0;1;UnityEngine.Rendering.CullMode;True;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;119;1344,32;Float;False;False;-1;3;UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI;0;1;New Amplify Shader;ece0159bad6633944bf6b818f4dd296c;True;Sprite Lit;0;0;Sprite Lit;0;False;True;2;5;False;;10;False;;3;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;5;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;UniversalMaterialType=Lit;Queue=Transparent=Queue=0;ShaderGraphShader=true;True;0;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Universal2D;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;120;1344,32;Float;False;False;-1;3;UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI;0;1;New Amplify Shader;ece0159bad6633944bf6b818f4dd296c;True;Sprite Normal;0;1;Sprite Normal;0;False;True;2;5;False;;10;False;;3;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;5;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;UniversalMaterialType=Lit;Queue=Transparent=Queue=0;ShaderGraphShader=true;True;0;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=NormalsRendering;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;121;1344,32;Float;False;False;-1;3;UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI;0;1;New Amplify Shader;ece0159bad6633944bf6b818f4dd296c;True;SceneSelectionPass;0;2;SceneSelectionPass;0;False;True;2;5;False;;10;False;;3;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;5;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;UniversalMaterialType=Lit;Queue=Transparent=Queue=0;ShaderGraphShader=true;True;0;True;12;all;0;False;True;0;1;False;;0;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=SceneSelectionPass;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;122;1344,32;Float;False;False;-1;3;UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI;0;1;New Amplify Shader;ece0159bad6633944bf6b818f4dd296c;True;ScenePickingPass;0;3;ScenePickingPass;0;False;True;2;5;False;;10;False;;3;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;5;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;UniversalMaterialType=Lit;Queue=Transparent=Queue=0;ShaderGraphShader=true;True;0;True;12;all;0;False;True;0;1;False;;0;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=Picking;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;123;1344,32;Float;False;True;-1;3;UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI;0;17;2D_URP_Chest_VFX;ece0159bad6633944bf6b818f4dd296c;True;Sprite Forward;0;4;Sprite Forward;6;True;True;2;5;False;;0;True;_BlendMode;0;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;True;True;2;True;_CullMode;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;5;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;UniversalMaterialType=Lit;Queue=Transparent=Queue=0;ShaderGraphShader=true;True;0;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=UniversalForward;False;False;0;;0;0;Standard;2;Vertex Position;1;0;Debug Display;0;0;0;5;True;True;True;True;True;False;;False;0
WireConnection;130;0;133;1
WireConnection;130;1;133;2
WireConnection;129;0;20;0
WireConnection;129;1;130;0
WireConnection;131;0;133;3
WireConnection;131;1;133;4
WireConnection;50;0;14;3
WireConnection;50;1;52;0
WireConnection;126;0;124;1
WireConnection;126;1;124;2
WireConnection;132;0;131;0
WireConnection;132;1;129;0
WireConnection;49;1;50;0
WireConnection;49;3;132;0
WireConnection;21;0;14;1
WireConnection;21;1;14;2
WireConnection;125;0;30;0
WireConnection;125;1;126;0
WireConnection;127;0;124;3
WireConnection;127;1;124;4
WireConnection;22;0;49;0
WireConnection;22;2;21;0
WireConnection;128;0;127;0
WireConnection;128;1;125;0
WireConnection;135;0;138;1
WireConnection;135;1;138;2
WireConnection;29;1;128;0
WireConnection;19;1;22;0
WireConnection;23;0;19;0
WireConnection;73;1;29;4
WireConnection;73;0;29;1
WireConnection;108;0;72;0
WireConnection;134;0;10;0
WireConnection;134;1;135;0
WireConnection;136;0;138;3
WireConnection;136;1;138;4
WireConnection;24;0;23;0
WireConnection;24;1;14;4
WireConnection;81;0;35;3
WireConnection;65;0;73;0
WireConnection;65;1;108;0
WireConnection;137;0;136;0
WireConnection;137;1;134;0
WireConnection;33;0;137;0
WireConnection;33;1;24;0
WireConnection;77;0;35;4
WireConnection;77;1;79;0
WireConnection;77;2;81;0
WireConnection;116;0;117;0
WireConnection;116;1;65;0
WireConnection;116;2;113;0
WireConnection;86;0;84;3
WireConnection;86;1;84;4
WireConnection;32;0;137;0
WireConnection;32;1;33;0
WireConnection;32;2;116;0
WireConnection;82;0;77;0
WireConnection;82;1;35;3
WireConnection;37;0;35;0
WireConnection;54;2;86;0
WireConnection;70;1;32;0
WireConnection;70;3;84;1
WireConnection;70;4;84;2
WireConnection;75;1;82;0
WireConnection;75;3;32;0
WireConnection;13;0;75;0
WireConnection;13;2;37;0
WireConnection;71;0;70;0
WireConnection;71;1;54;0
WireConnection;83;1;13;0
WireConnection;83;0;71;0
WireConnection;109;0;66;0
WireConnection;12;1;83;0
WireConnection;67;0;12;5
WireConnection;67;1;109;0
WireConnection;18;0;34;5
WireConnection;18;1;67;0
WireConnection;18;2;106;0
WireConnection;18;3;110;0
WireConnection;61;0;34;4
WireConnection;61;1;12;4
WireConnection;61;2;116;0
WireConnection;61;3;110;4
WireConnection;61;4;118;0
WireConnection;123;0;18;0
WireConnection;123;2;61;0
ASEEND*/
//CHKSM=4016623410F5019BAD274FE9F0CD10922C874857