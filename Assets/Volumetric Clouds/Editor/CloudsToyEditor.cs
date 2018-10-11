using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(CloudsToy))]
public class CloudsToyEditor : Editor {
	private int i = 0;
	private bool showAdvancedSettings = false;
	private bool showMaximunClouds = false;
	private CloudsToy CloudSystem;

    public override void OnInspectorGUI() {
		//EditorGUIUtility.LookLikeInspector();
		EditorGUIUtility.LookLikeControls();
		CloudSystem = (CloudsToy) target;
		if (!CloudSystem.gameObject) {
			return;
		}
		EditorGUILayout.BeginVertical();
		
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		showMaximunClouds = EditorGUILayout.Foldout(showMaximunClouds, " Maximun Clouds (DO NOT change while executing)");
		if(showMaximunClouds)
			CloudSystem.MaximunClouds = EditorGUILayout.IntField("  ", CloudSystem.MaximunClouds);
			if (GUI.changed)
				EditorUtility.SetDirty(CloudSystem);
		GUI.changed = false;
		
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		CloudSystem.CloudPreset = (CloudsToy.TypePreset)EditorGUILayout.EnumPopup("  Cloud Presets: ", CloudSystem.CloudPreset);
		if (GUI.changed) {
			if(CloudSystem.CloudPreset == CloudsToy.TypePreset.Stormy)
				CloudSystem.SetPresetStormy();
			else
			if(CloudSystem.CloudPreset == CloudsToy.TypePreset.Sunrise)
				CloudSystem.SetPresetSunrise();
			else
			if(CloudSystem.CloudPreset == CloudsToy.TypePreset.Fantasy)
				CloudSystem.SetPresetFantasy();
			EditorUtility.SetDirty(CloudSystem);
		}
		GUI.changed = false;
		
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		CloudSystem.CloudRender = (CloudsToy.TypeRender)EditorGUILayout.EnumPopup("  Cloud Render: ", CloudSystem.CloudRender);
		CloudSystem.TypeClouds = (CloudsToy.Type)EditorGUILayout.EnumPopup("  Cloud Type: ", CloudSystem.TypeClouds);
		CloudSystem.CloudDetail = (CloudsToy.TypeDetail)EditorGUILayout.EnumPopup("  Cloud Detail: ", CloudSystem.CloudDetail);
		if (GUI.changed) {
			CloudSystem.SetCloudDetailParams();
			EditorUtility.SetDirty(CloudSystem);
		}
		GUI.changed = false;
			
		EditorGUILayout.BeginHorizontal();
		showAdvancedSettings = EditorGUILayout.Foldout(showAdvancedSettings, "Particles Advanced Settings");
		EditorGUILayout.EndHorizontal();
		if(showAdvancedSettings){
			EditorGUILayout.Separator();
			//EditorGUILayout.BeginHorizontal();
			CloudSystem.SizeFactorPart = EditorGUILayout.Slider("  Size Factor: ", CloudSystem.SizeFactorPart, 0.1f, 4.0f);
			CloudSystem.EmissionMult = EditorGUILayout.Slider("  Emitter Mult: ", CloudSystem.EmissionMult, 0.1f, 4.0f);
			//EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
		}
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		Rect buttonRect = EditorGUILayout.BeginHorizontal();
		buttonRect.x = buttonRect.width / 2 - 100;
		buttonRect.width = 200;
		buttonRect.height = 30;
		//GUI.skin = terrain.guiSkin;
		if(GUI.Button(buttonRect, "Repaint Clouds")){
			CloudSystem.EditorRepaintClouds();
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		CloudSystem.SoftClouds = EditorGUILayout.Toggle("  Soft Clouds", CloudSystem.SoftClouds);
		if(CloudSystem.SoftClouds){
			CloudSystem.SpreadDir = EditorGUILayout.Vector3Field("  Spread Direction: ", CloudSystem.SpreadDir);
			CloudSystem.LengthSpread = EditorGUILayout.Slider("  Length Spread: ", CloudSystem.LengthSpread, 1, 30);
		}
		EditorGUILayout.Separator();
		CloudSystem.NumberClouds = EditorGUILayout.IntSlider("  Clouds Num: ", CloudSystem.NumberClouds, 1, CloudSystem.MaximunClouds);
		EditorGUILayout.Separator();
		CloudSystem.Side = EditorGUILayout.Vector3Field("  Cloud Creation Size: ", CloudSystem.Side);
		CloudSystem.DisappearMultiplier = EditorGUILayout.Slider("  Dissapear Mult: ", CloudSystem.DisappearMultiplier, 1, 10);
		EditorGUILayout.Separator();
		CloudSystem.MaximunVelocity = EditorGUILayout.Vector3Field("  Maximun Velocity: ", CloudSystem.MaximunVelocity);
		CloudSystem.VelocityMultipier = EditorGUILayout.Slider("  Velocity Mult: ", CloudSystem.VelocityMultipier, 0, 20);
		EditorGUILayout.Separator();
		CloudSystem.PaintType = (CloudsToy.TypePaintDistr)EditorGUILayout.EnumPopup("  Paint Type: ", CloudSystem.PaintType);
		if(CloudSystem.CloudRender == CloudsToy.TypeRender.Realistic)
			CloudSystem.CloudColor = EditorGUILayout.ColorField("  Cloud Color: ", CloudSystem.CloudColor);
		CloudSystem.MainColor = EditorGUILayout.ColorField("  Main Color: ", CloudSystem.MainColor);
		CloudSystem.SecondColor = EditorGUILayout.ColorField("  Secondary Color: ", CloudSystem.SecondColor);
		CloudSystem.TintStrength = EditorGUILayout.IntSlider("  Tint Strength: ", CloudSystem.TintStrength, 1, 100);
		if(CloudSystem.PaintType == CloudsToy.TypePaintDistr.Below)
			CloudSystem.offset = EditorGUILayout.Slider("  Offset: ", CloudSystem.offset, 0, 1);
		EditorGUILayout.Separator();
		CloudSystem.MaxWithCloud = EditorGUILayout.IntSlider("  Width: ", CloudSystem.MaxWithCloud, 10, 1000);
		CloudSystem.MaxTallCloud = EditorGUILayout.IntSlider("  Height: ", CloudSystem.MaxTallCloud, 5, 500);
		CloudSystem.MaxDepthCloud = EditorGUILayout.IntSlider("  Depth: ", CloudSystem.MaxDepthCloud, 5, 1000);
		CloudSystem.FixedSize = EditorGUILayout.Toggle("  Fixed Size", CloudSystem.FixedSize);
		EditorGUILayout.Separator();
		CloudSystem.NumberOfShadows = (CloudsToy.TypeShadow)EditorGUILayout.EnumPopup("  Shadows: ", CloudSystem.NumberOfShadows);
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		
		EditorGUILayout.LabelField("Text Additive", "Used for Bright Clouds");
		EditorGUILayout.BeginHorizontal();
		for(i = 0; i < CloudSystem.CloudsTextAdd.Length; i++){
			if(i == CloudSystem.CloudsTextAdd.Length/2){
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Separator();
			}
			else
				EditorGUILayout.Space();
			CloudSystem.CloudsTextAdd[i] = (Texture2D)EditorGUILayout.ObjectField( CloudSystem.CloudsTextAdd[i], typeof(Texture2D), GUILayout.Width(50), GUILayout.Height(50));
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.LabelField("Text Blended", "Used for Realistic Clouds");
		EditorGUILayout.BeginHorizontal();
		for(i = 0; i < CloudSystem.CloudsTextBlended.Length; i++){
			if(i == CloudSystem.CloudsTextBlended.Length/2){
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Separator();
			}
			else
				EditorGUILayout.Space();
			CloudSystem.CloudsTextBlended[i] = (Texture2D)EditorGUILayout.ObjectField(CloudSystem.CloudsTextBlended[i], typeof(Texture2D), GUILayout.Width(50), GUILayout.Height(50));
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();
		if (GUI.changed)
            EditorUtility.SetDirty (CloudSystem);
        GUI.changed = false;
    }
}
