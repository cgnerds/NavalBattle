// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class CloudsToy : MonoBehaviour {

public enum TypePreset { None = 0, Stormy = 1, Sunrise = 2, Fantasy = 3 }
public TypePreset CloudPreset = TypePreset.None;
public enum TypeRender { Bright = 0, Realistic = 1 }
public TypeRender CloudRender = TypeRender.Bright;
public enum TypeDetail { Low = 0, Normal = 1, High = 2 }
public TypeDetail CloudDetail = TypeDetail.Low;
public enum Type { Nimbus1=0, Nimbus2=1, Nimbus3=2, Nimbus4=3, Cirrus1=4, Cirrus2=5, MixNimbus=6, MixCirrus=7, MixAll=8 }
public Type TypeClouds = Type.Nimbus1;
public float SizeFactorPart = 1;
public float EmissionMult = 1;
public bool  SoftClouds = false;
public Vector3 SpreadDir = new Vector3(-1, 0, 0);
public float LengthSpread = 1;
public int NumberClouds = 100;
public Vector3 Side = new Vector3(1000, 500, 1000);
public Vector3 MaximunVelocity = new Vector3(-10, 0, 0);
public float VelocityMultipier = 1;
public float DisappearMultiplier = 1.5f;
public enum TypePaintDistr { Random = 0, Below = 1 }
public TypePaintDistr PaintType = TypePaintDistr.Below;
public Color CloudColor = new Color(1, 1, 1, 1);
public Color MainColor = new Color(1, 1, 1, 1);
public Color SecondColor = new Color(0.5f, 0.5f, 0.5f, 1);
public int TintStrength = 50;
public float offset = 0.5f;
public int MaxWithCloud = 100;
public int MaxTallCloud = 40;
public int MaxDepthCloud = 100;
public bool  FixedSize = true;
public enum TypeShadow { All = 0, Most = 1, Half = 2, Some = 3, None = 4 }
public TypeShadow NumberOfShadows = TypeShadow.Some;
public Texture2D[] CloudsTextAdd = new Texture2D[6];
public Texture2D[] CloudsTextBlended = new Texture2D[6];

private Material[] CloudsMatAdditive = new Material[6];
private Material[] CloudsMatBlended = new Material[6];
private enum Axis { X = 0, Y = 1, Z = 2, XNeg = 3, YNeg = 4, ZNeg = 5 }
private Axis CloudsGenerateAxis =Axis.X;
private Transform MyTransform;
private Vector3 MyPosition;
private ArrayList MyCloudsParticles = new ArrayList();

//=================================================================================
//=================================================================================
public int MaximunClouds = 300;  // The maximun Clouds the system will manage in total.
//=================================================================================
//=================================================================================

// Private vars for detect changing parameters in inspector and execute only a piece of code.inthe Update.
private TypePreset CloudPresetAnt = TypePreset.None;
private TypeRender CloudRenderAnt = TypeRender.Bright;
private TypeDetail CloudDetailAnt = TypeDetail.Low;
private Type TypeCloudsAnt = Type.Nimbus1;
private float EmissionMultAnt = 1;
private float SizeFactorPartAnt = 1;
private bool  SoftCloudsAnt = false;
private Vector3 SpreadDirAnt = new Vector3(-1, 0, 0);
private float LengthSpreadAnt = 1;
private int NumberCloudsAnt = 10;
private Vector3 MaximunVelocityAnt;
private float VelocityMultipierAnt;
private TypePaintDistr PaintTypeAnt = TypePaintDistr.Below;
private Color CloudColorAnt = new Color(1, 1, 1, 1);
private Color MainColorAnt = new Color(1, 1, 1, 1);
private Color SecondColorAnt = new Color(0.5f, 0.5f, 0.5f, 1);
private int TintStrengthAnt = 5;
private float offsetAnt = 0;
private TypeShadow NumberOfShadowsAnt = TypeShadow.All;
private int MaxWithCloudAnt = 200;
private int MaxTallCloudAnt = 50;
private int MaxDepthCloudAnt = 200;


public void  SetPresetStormy (){
	if(CloudPresetAnt == CloudPreset) 
		return;
	CloudRender = TypeRender.Realistic;
	CloudDetail = TypeDetail.Normal;
	SetCloudDetailParams();
	TypeClouds = Type.Nimbus2;
	SoftClouds = false;
	SpreadDir = new Vector3(-1, 0, 0);
	LengthSpread = 1;
	NumberClouds = 100;
	//Side = new Vector3(2000, 500, 2000);
	DisappearMultiplier = 2;
	MaximunVelocity = new Vector3(-10, 0, 0);
	VelocityMultipier = 0.85f;
	PaintType = TypePaintDistr.Below;
	CloudColor = new Color(1, 1, 1, 0.5f);
	MainColor = new Color(0.62f, 0.62f, 0.62f, 0.3f);
	SecondColor = new Color(0.31f, 0.31f, 0.31f, 1);
	TintStrength = 80;
	offset = 0.8f;
	MaxWithCloud = 200;
	MaxTallCloud = 50;
	MaxDepthCloud = 200;
	FixedSize = false;
	NumberOfShadows = TypeShadow.Some;
	CloudPresetAnt = CloudPreset;
}
 
 public void  SetPresetSunrise (){
 	if(CloudPresetAnt == CloudPreset) 
		return;
	CloudRender = TypeRender.Bright;
	CloudDetail = TypeDetail.Low;
	SetCloudDetailParams();
	EmissionMult = 1.6f;
	SizeFactorPart = 1.5f;
	TypeClouds = Type.Cirrus1;
	SoftClouds = true;
	SpreadDir = new Vector3(-1, 0, 0);
	LengthSpread = 4;
	NumberClouds = 135;
	//Side = new Vector3(2000, 500, 2000);
	DisappearMultiplier = 2;
	MaximunVelocity = new Vector3(-10, 0, 0);
	VelocityMultipier = 6.2f;
	PaintType = TypePaintDistr.Below;
	CloudColor = new Color(1, 1, 1, 1);
	MainColor = new Color(1, 1, 0.66f, 0.5f);
	SecondColor = new Color(1, 0.74f, 0, 1);
	TintStrength = 100;
	offset = 1;
	MaxWithCloud = 500;
	MaxTallCloud = 20;
	MaxDepthCloud = 500;
	FixedSize = true;
	NumberOfShadows = TypeShadow.None;
	CloudPresetAnt = CloudPreset;
}
 
 public void  SetPresetFantasy (){ 
 	if(CloudPresetAnt == CloudPreset) 
		return;
	CloudRender = TypeRender.Bright;
	CloudDetail = TypeDetail.Low;
	EmissionMult = 0.3f;
	SetCloudDetailParams();
	TypeClouds = Type.Nimbus4;
	SoftClouds = false;
	SpreadDir = new Vector3(-1, 0, 0);
	LengthSpread = 1;
	NumberClouds = 200;
	//Side = new Vector3(2000, 500, 2000);
	DisappearMultiplier = 2;
	MaximunVelocity = new Vector3(-10, 0, 0);
	VelocityMultipier = 0.50f;
	PaintType = TypePaintDistr.Random;
	CloudColor = new Color(1, 1, 1, 0.5f);
	MainColor = new Color(1, 0.62f, 0, 1);
	SecondColor = new Color(0.5f, 0.5f, 0.5f, 1);
	TintStrength = 50;
	offset = 0.2f;
	MaxWithCloud = 200;
	MaxTallCloud = 50;
	MaxDepthCloud = 200;
	FixedSize = true;
	NumberOfShadows = TypeShadow.Some;
	CloudPresetAnt = CloudPreset;
}

 public void  SetCloudDetailParams (){ 
	if(CloudDetailAnt == CloudDetail)
		return;
	if(CloudDetail == TypeDetail.Low){
		EmissionMult = 1;
		SizeFactorPart = 1;
	}
	else
	if(CloudDetail == TypeDetail.Normal){
		EmissionMult = 1.5f;
		SizeFactorPart = 1.2f;
	}
	else
	if(CloudDetail == TypeDetail.High){
		EmissionMult = 2.0f;
		SizeFactorPart = 1.3f;
	}
	CloudDetailAnt = CloudDetail;
}


public void PaintTheParticlesShadows(CloudParticle MyCloudParticle){
	if(PaintType == TypePaintDistr.Random)
		MyCloudParticle.PaintParticlesBelow(MainColor, SecondColor, TintStrength, offset, 0);
	else
	if(PaintType == TypePaintDistr.Below)
		MyCloudParticle.PaintParticlesBelow(MainColor, SecondColor, TintStrength, offset, 1);
}


 public void EditorRepaintClouds (){
	int i = 0;
	CloudParticle MyCloudParticle;
	 
	if(MyCloudsParticles.Count == 0)
		return;
	for(i = 0; i < MaximunClouds; i++){
		MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
		if(MyCloudParticle.IsActive()){
			// Define some main particle properties
			if( TypeClouds == Type.Nimbus1 || TypeClouds == Type.Nimbus2 || 
				TypeClouds == Type.Nimbus3 || TypeClouds == Type.Nimbus4 || 
				TypeClouds == Type.MixNimbus || TypeClouds == Type.MixAll)
					MyCloudParticle.DefineCloudProperties (i, MaxWithCloud, MaxTallCloud, MaxDepthCloud, 0, FixedSize, true ,  true);
			else
			if(TypeClouds == Type.Cirrus1 || TypeClouds == Type.Cirrus1 || TypeClouds == Type.MixCirrus)
					MyCloudParticle.DefineCloudProperties (i, MaxWithCloud, MaxTallCloud, MaxDepthCloud, 1, FixedSize, true ,  true);
			
			MyCloudParticle.UpdateCloudsPosition();
			if(CloudRender == TypeRender.Realistic)
				MyCloudParticle.SetMainColor(CloudColor);
			PaintTheParticlesShadows(MyCloudParticle);
		}
	}
}


void  Start (){
	MyTransform = this.transform;
	MyPosition = transform.position;
	CloudParticle MyCloudParticle;
	Vector3 MyPos;
	Vector3 SideAux;
	int i;
	
	//CloudPrefab = GameObject.Find("VolCloud Basic");
	//CloudPrefab = Resources.LoadAssetAtPath("Assets/Volumetric Clouds/Prefabs/VolCloud Basic.prefab", typeof(GameObject));
	CloudPresetAnt = CloudPreset;
	CloudRenderAnt = CloudRender;
	CloudDetailAnt = CloudDetail;
	TypeCloudsAnt = TypeClouds;
	EmissionMultAnt = EmissionMult;
	SizeFactorPartAnt = SizeFactorPart;
	SoftCloudsAnt = SoftClouds;
	SpreadDirAnt = SpreadDir;
	LengthSpreadAnt = LengthSpread;
	NumberCloudsAnt = NumberClouds;
	MaximunVelocityAnt = MaximunVelocity;
	VelocityMultipierAnt = VelocityMultipier;
	PaintTypeAnt = PaintType;
	CloudColorAnt = CloudColor;
	MainColorAnt = MainColor;
	SecondColorAnt = SecondColor;
	TintStrengthAnt = TintStrength;
	offsetAnt = offset;
	NumberOfShadowsAnt = NumberOfShadows;
	MaxWithCloudAnt = MaxWithCloud;
	MaxTallCloudAnt = MaxTallCloud;
	MaxDepthCloudAnt = MaxDepthCloud;

	// Define the axis the clouds are moving on. (Only one value X or Y or Z, must be not equal Zero).
	Vector3 MyVelocity = MaximunVelocity;
	if(MyVelocity.x > 0)
		CloudsGenerateAxis = Axis.X;
	else
	if(MyVelocity.x < 0)
		CloudsGenerateAxis = Axis.XNeg;
	else
	if(MyVelocity.y > 0)
		CloudsGenerateAxis = Axis.Y;
	else
	if(MyVelocity.y < 0)
		CloudsGenerateAxis = Axis.YNeg;
	else
	if(MyVelocity.z > 0)
		CloudsGenerateAxis = Axis.Z;
	else
	if(MyVelocity.z < 0)
		CloudsGenerateAxis = Axis.ZNeg;
	
	// Create the materials based in the textures provided by the user. maximun textures . 6
	// There are two types of materials Additive Soft for bright Clouds & Blend for more realistic ones.
	// First type of clouds. Additive - Bright Ones.
	for(i = 0; i < 6; i++){
		CloudsMatAdditive[i] = new Material(Shader.Find("Particles/Additive (Soft)"));
		CloudsMatAdditive[i].mainTexture = CloudsTextAdd[i];
	}
	// Second type of Clouds. Realistic Ones.
	for(i = 0; i < 6; i++){
		CloudsMatBlended[i] = new Material(Shader.Find("Particles/Alpha Blended Transparent+1"));
		CloudsMatBlended[i].SetColor("_TintColor", CloudColor);
		CloudsMatBlended[i].mainTexture = CloudsTextBlended[i];
	}
	
	// Generate the clouds for first time, never well be destroyed during the scene.
	// Using a cubic shape to bounds the limits of coords. creation
	SideAux =  Side/2;
	for(i = 0; i < MaximunClouds; i++){
		MyPos = MyPosition;
		MyPos.x = Random.Range (MyPos.x-SideAux.x, MyPos.x+SideAux.x);
		MyPos.y = Random.Range (MyPos.y-SideAux.y, MyPos.y+SideAux.y);
		MyPos.z = Random.Range (MyPos.z-SideAux.z, MyPos.z+SideAux.z);
		MyCloudParticle = new CloudParticle(MyPos, Quaternion.identity);
		MyCloudParticle.SetCloudParent(MyTransform);
		MyCloudsParticles.Add(MyCloudParticle);
		
		// Define some main particle properties
		if( TypeClouds == Type.Nimbus1 || TypeClouds == Type.Nimbus2 || 
			TypeClouds == Type.Nimbus3 || TypeClouds == Type.Nimbus4 || 
			TypeClouds == Type.MixNimbus || TypeClouds == Type.MixAll)
				MyCloudParticle.DefineCloudProperties (i, MaxWithCloud, MaxTallCloud, MaxDepthCloud, 0, FixedSize, true ,  true);
		else
		if(TypeClouds == Type.Cirrus1 || TypeClouds == Type.Cirrus1 || TypeClouds == Type.MixCirrus)
				MyCloudParticle.DefineCloudProperties (i, MaxWithCloud, MaxTallCloud, MaxDepthCloud, 1, FixedSize, true ,  true);
		
		AssignCloudMaterial (MyCloudParticle,  CloudRender ,  TypeClouds);
		MyCloudParticle.SetCloudEmitter (i, SpreadDir, SoftClouds, SizeFactorPart, EmissionMult, MaximunVelocity, VelocityMultipier);
		MyCloudParticle.SetCloudVelocity (MaximunVelocity, VelocityMultipier);
		MyCloudParticle.SetLengthScale(LengthSpread);
		MyCloudParticle.SetWorldVelocity(SpreadDir);
		MyCloudParticle.SoftCloud(SoftClouds);
		ManageCloudShadow(MyCloudParticle);
		// If the cloud will be active, Paint the cloud otherwise deactivate it (they are initially active, but dont emit anything)
		if(i < NumberClouds){
			MyCloudParticle.SetActive(true); // Emit the particles, because this cloud is visible
			MyCloudParticle.UpdateCloudsPosition(); // Updating the positions of particles once the Particle Emmitter emit them.
			if(CloudRender == TypeRender.Realistic)
				MyCloudParticle.SetMainColor(CloudColor);  // Set the main color of the cloud
			PaintTheParticlesShadows(MyCloudParticle); // Colorize the cloud with the Cloud Color and the Secondary Color
		}
		else
			MyCloudParticle.DesactivateRecursively();
	}

}

// Only we manage the changes of variables in the inspector of Unity, not be used in gametime when
// everything is setup.
void  Update (){
	CloudParticle MyCloudParticle;
	int i;
	
	// Change the number of visible clouds. Must activate the new particles and Update de position of the particles in the ellipse
	if(NumberCloudsAnt != NumberClouds){
		for(i = 0; i < MaximunClouds; i++){
			MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
			if(i < NumberClouds && !MyCloudParticle.IsActive()){
				MyCloudParticle.SetActive(true);
				MyCloudParticle.UpdateCloudsPosition();
				if(SoftClouds)
					SoftCloudsAnt = !SoftClouds;
			}
			else
			if(i >= NumberClouds && MyCloudParticle.IsActive())
				MyCloudParticle.DesactivateRecursively();
		}
		NumberCloudsAnt = NumberClouds;
	}
	// Actualize the particle emmitter if the density of particles emmited has changed by user
	if(CloudDetailAnt != CloudDetail){
		if(CloudDetail == TypeDetail.Low){
			EmissionMult = 1;
			SizeFactorPart = 1;
		}
		else
		if(CloudDetail == TypeDetail.Normal){
			EmissionMult = 1.5f;
			SizeFactorPart = 1.2f;
		}
		else
		if(CloudDetail == TypeDetail.High){
			EmissionMult = 2.0f;
			SizeFactorPart = 1.3f;
		}
		for(i = 0; i < NumberClouds; i++){
			MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
			MyCloudParticle.SetCloudEmitter (i, SpreadDir, SoftClouds, SizeFactorPart, EmissionMult, MaximunVelocity, VelocityMultipier);
			MyCloudParticle.SetActive(true);
			MyCloudParticle.UpdateCloudsPosition();
			if(CloudRender == TypeRender.Realistic)
				MyCloudParticle.SetMainColor(CloudColor);
			PaintTheParticlesShadows(MyCloudParticle);
		}
		CloudDetailAnt = CloudDetail;
	}
	// if change the Size or amount of particles emmitted by any Cloud, must update the partice emmitter and emit again.
	// after that, we ensure the particles are in the assigned ellipsoid of the cloud
	if(SizeFactorPartAnt != SizeFactorPart || EmissionMultAnt != EmissionMult){
		for(i = 0; i < NumberClouds; i++){
			MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
			MyCloudParticle.SetCloudEmitter (i, SpreadDir, SoftClouds, SizeFactorPart, EmissionMult, MaximunVelocity, VelocityMultipier);
			MyCloudParticle.SetActive(true);
			MyCloudParticle.UpdateCloudsPosition();
		}
		SizeFactorPartAnt = SizeFactorPart;
		EmissionMultAnt = EmissionMult;
	}
	// Are soft clouds? Update the particle emmitter and renderer to take care of the change
	if(SoftCloudsAnt != SoftClouds){
		for(i = 0; i < NumberClouds; i++){
			MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
			MyCloudParticle.SetCloudEmitter (i, SpreadDir, SoftClouds, SizeFactorPart, EmissionMult, MaximunVelocity, VelocityMultipier);
			MyCloudParticle.SoftCloud (SoftClouds);
			MyCloudParticle.SetActive(true);
			MyCloudParticle.UpdateCloudsPosition();
		}
		SoftCloudsAnt = SoftClouds;
	}
	//  this two vars, only are visibles if softClouds are true, otherwise any change will not be advised
	if(SpreadDirAnt != SpreadDir || LengthSpreadAnt != LengthSpread){
		for(i = 0; i < NumberClouds; i++){
			MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
			MyCloudParticle.SetLengthScale(LengthSpread);
			if(SpreadDirAnt != SpreadDir){
				MyCloudParticle.SetWorldVelocity(SpreadDir);
				MyCloudParticle.SetActive(true);
				MyCloudParticle.UpdateCloudsPosition();
			}
		}
		SpreadDirAnt = SpreadDir;
		LengthSpreadAnt = LengthSpread;
	}
	// Changin the clouds width or tall. Must redefine all the cloud parameters, including his name
	if(MaxWithCloud != MaxWithCloudAnt || MaxTallCloud != MaxTallCloudAnt || MaxDepthCloud != MaxDepthCloudAnt){
		for(i = 0; i < NumberClouds; i++){
			MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
			// Define some main particle properties
			if( TypeClouds == Type.Nimbus1 || TypeClouds == Type.Nimbus2 || 
				TypeClouds == Type.Nimbus3 || TypeClouds == Type.Nimbus4 || 
				TypeClouds == Type.MixNimbus || TypeClouds == Type.MixAll)
					MyCloudParticle.DefineCloudProperties (i, MaxWithCloud, MaxTallCloud, MaxDepthCloud, 0, FixedSize, true ,  true);
			else
			if(TypeClouds == Type.Cirrus1 || TypeClouds == Type.Cirrus1 || TypeClouds == Type.MixCirrus)
					MyCloudParticle.DefineCloudProperties (i, MaxWithCloud, MaxTallCloud, MaxDepthCloud, 1, FixedSize, true ,  true);
			// Change the emitter params of the cloud to adjust the new size.
			MyCloudParticle.SetCloudEmitter (i, SpreadDir, SoftClouds, SizeFactorPart, EmissionMult, MaximunVelocity, VelocityMultipier);
			// Start emit again, my friend.
			MyCloudParticle.SetActive(true); 
			//  Update the position of the particles emmitted inside the ellipsoid
			MyCloudParticle.UpdateCloudsPosition();
			// Colorize the cloud
			if(CloudRender == TypeRender.Realistic)
				MyCloudParticle.SetMainColor(CloudColor);
			PaintTheParticlesShadows(MyCloudParticle);
		}
		MaxWithCloudAnt = MaxWithCloud;
		MaxTallCloudAnt = MaxTallCloud;
		MaxDepthCloudAnt = MaxDepthCloud;
	}
	// If change the type of cloud just meaning i must change his material or render mode
	if(TypeCloudsAnt != TypeClouds || CloudRenderAnt != CloudRender){
		for(i = 0; i < MaximunClouds; i++){
			MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
			// Change the Material depending on the type defined by user
			AssignCloudMaterial(MyCloudParticle, CloudRender, TypeClouds);
		}
		TypeCloudsAnt = TypeClouds;
		CloudRenderAnt = CloudRender;
	}
	// Actualize the velocity of the cloud and take care of the direccion of the mov for the LateUpdate proccess.
	if(MaximunVelocityAnt != MaximunVelocity || VelocityMultipierAnt != VelocityMultipier){
		// Define the axis the clouds are moving on. (Only one value X or Y or Z, must be not equal Zero).
		// Used to determine the way the coulds are goig to dissapear when they move far away from the Box.
		if(MaximunVelocity.x > 0)
			CloudsGenerateAxis = Axis.X;
		else
		if(MaximunVelocity.x < 0)
			CloudsGenerateAxis = Axis.XNeg;
		else
		if(MaximunVelocity.y > 0)
			CloudsGenerateAxis = Axis.Y;
		else
		if(MaximunVelocity.y < 0)
			CloudsGenerateAxis = Axis.YNeg;
		else
		if(MaximunVelocity.z > 0)
			CloudsGenerateAxis = Axis.Z;
		else
		if(MaximunVelocity.z < 0)
			CloudsGenerateAxis = Axis.ZNeg;
		
		for(i = 0; i < MaximunClouds; i++)
			((CloudParticle)MyCloudsParticles[i]).SetCloudVelocity(MaximunVelocity, VelocityMultipier);

		MaximunVelocityAnt = MaximunVelocity;
		VelocityMultipierAnt = VelocityMultipier;
	}
	// All this just change one color or the system to colorize the cloud, just that.
	if(CloudColorAnt != CloudColor){
		for(i = 0; i < NumberClouds; i++)
			((CloudParticle)MyCloudsParticles[i]).SetMainColor(CloudColor);
		CloudColorAnt = CloudColor;
	}
	
	if(MainColorAnt != MainColor){
		for(i = 0; i < NumberClouds; i++)
			PaintTheParticlesShadows(((CloudParticle)MyCloudsParticles[i]));
		MainColorAnt = MainColor;
	}
	
	if(SecondColorAnt != SecondColor || TintStrengthAnt !=TintStrength){
		for(i = 0; i < NumberClouds; i++)
			PaintTheParticlesShadows(((CloudParticle)MyCloudsParticles[i]));
		SecondColorAnt = SecondColor;
		TintStrengthAnt = TintStrength;
	}
	
	if(offsetAnt != offset){
		for(i = 0; i < NumberClouds; i++)
			PaintTheParticlesShadows(((CloudParticle)MyCloudsParticles[i]));
		offsetAnt = offset;
	}
	
	if(PaintTypeAnt != PaintType){
		for(i = 0; i < NumberClouds; i++)
			PaintTheParticlesShadows(((CloudParticle)MyCloudsParticles[i]));
		PaintTypeAnt = PaintType;
	}
	
	// Determine if cloud shadow must be active or not, depending on user choice
	if(NumberOfShadowsAnt != NumberOfShadows){
		for(i = 0; i < NumberClouds; i++){
			MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
			ManageCloudShadow(MyCloudParticle);
		}
		NumberOfShadowsAnt = NumberOfShadows;
	}
}

// Manage the dissapearing of the partiles at the end of en Cubic Shape and move them to the begining again.
void  LateUpdate (){
	CloudParticle MyCloudParticle;
	Vector3 MyPos;
	int i;
	bool  DestroyIt = false;
	Vector3 SideAux;

	// Test if some cloud is outside the dissapear shape (that's the original shape plus a Dissapear multiplier)
	SideAux =  (Side * DisappearMultiplier)/2;
	for(i = 0; i < MyCloudsParticles.Count; i++){
		MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
		MyPos = MyCloudParticle.GetCloudPosition();
		if(MyPos.x < MyPosition.x-SideAux.x || MyPos.x > MyPosition.x+SideAux.x)
			DestroyIt = true;
		else
		if(MyPos.y < MyPosition.y-SideAux.y || MyPos.y > MyPosition.y+SideAux.y)
			DestroyIt = true;
		else
		if(MyPos.z < MyPosition.z-SideAux.z || MyPos.z > MyPosition.z+SideAux.z)
			DestroyIt = true;
	
		// If a cloud it's outside, just brig it back in the other side (the axis the clouds are moving along is needed)
		// do all that stuff depending on the shape cuadratic or spherical.
		if(DestroyIt){
			DestroyIt = false;
			SideAux =  (Side * DisappearMultiplier)/2;
			MyPos = MyPosition;
			MyPos.x = Random.Range (MyPos.x-Side.x/2, MyPos.x+Side.x/2);
			MyPos.y = Random.Range (MyPos.y-Side.y/2, MyPos.y+Side.y/2);
			MyPos.z = Random.Range (MyPos.z-Side.z/2, MyPos.z+Side.z/2);
			if(CloudsGenerateAxis == Axis.X)
				MyPos.x = MyPosition.x-SideAux.x;
			else
			if(CloudsGenerateAxis == Axis.XNeg)
				MyPos.x = MyPosition.x+SideAux.x;
			else
			if(CloudsGenerateAxis == Axis.Y)
				MyPos.y = MyPosition.y-SideAux.y;
			else
			if(CloudsGenerateAxis == Axis.YNeg)
				MyPos.y = MyPosition.y+SideAux.y;
			else
			if(CloudsGenerateAxis == Axis.Z)
				MyPos.z = MyPosition.z-SideAux.z;
			else
			if(CloudsGenerateAxis == Axis.ZNeg)
				MyPos.z = MyPosition.z+SideAux.z;
			
			MyCloudParticle.SetCloudPosition(MyPos);
		}
	}
}


// Change the Material depending on the type of cloud defined by user and the renderer (Bright/Realistic)
// So assign one of the two sets of materials available (Additive or Blended).
void  AssignCloudMaterial ( CloudParticle MyCloudParticle ,   TypeRender CloudRender ,   Type TypeClouds){
	if(CloudRender == TypeRender.Bright){
		if(TypeClouds == Type.Nimbus1)
			MyCloudParticle.SetMaterial(CloudsMatAdditive[0]);
		else
		if(TypeClouds == Type.Nimbus2)
			MyCloudParticle.SetMaterial(CloudsMatAdditive[1]);
		else
		if(TypeClouds == Type.Nimbus3)
			MyCloudParticle.SetMaterial(CloudsMatAdditive[2]);
		else
		if(TypeClouds == Type.Nimbus4)
			MyCloudParticle.SetMaterial(CloudsMatAdditive[3]);
		else
		if(TypeClouds == Type.Cirrus1)
			MyCloudParticle.SetMaterial(CloudsMatAdditive[4]);
		else
		if(TypeClouds == Type.Cirrus2)
			MyCloudParticle.SetMaterial(CloudsMatAdditive[5]);
		else
		if(TypeClouds == Type.MixNimbus)
			MyCloudParticle.SetMaterial(CloudsMatAdditive[Random.Range(0, 4)]);
		else
		if(TypeClouds == Type.MixCirrus)
			MyCloudParticle.SetMaterial(CloudsMatAdditive[Random.Range(4, 6)]);
		else
		if(TypeClouds == Type.MixAll)
			MyCloudParticle.SetMaterial(CloudsMatAdditive[Random.Range(0, 6)]);
	}
	else{
		if(TypeClouds == Type.Nimbus1)
			MyCloudParticle.SetMaterial(CloudsMatBlended[0]);
		else
		if(TypeClouds == Type.Nimbus2)
			MyCloudParticle.SetMaterial(CloudsMatBlended[1]);
		else
		if(TypeClouds == Type.Nimbus3)
			MyCloudParticle.SetMaterial(CloudsMatBlended[2]);
		else
		if(TypeClouds == Type.Nimbus4)
			MyCloudParticle.SetMaterial(CloudsMatBlended[3]);
		else
		if(TypeClouds == Type.Cirrus1)
			MyCloudParticle.SetMaterial(CloudsMatBlended[4]);
		else
		if(TypeClouds == Type.Cirrus2)
			MyCloudParticle.SetMaterial(CloudsMatBlended[5]);
		else
		if(TypeClouds == Type.MixNimbus)
			MyCloudParticle.SetMaterial(CloudsMatBlended[Random.Range(0, 4)]);
		else
		if(TypeClouds == Type.MixCirrus)
			MyCloudParticle.SetMaterial(CloudsMatBlended[Random.Range(4, 6)]);
		else
		if(TypeClouds == Type.MixAll)
			MyCloudParticle.SetMaterial(CloudsMatBlended[Random.Range(0, 6)]);
	}
}

void  ManageCloudShadow (CloudParticle MyCloudParticle){
	int RandomNumber = Random.Range(0, 10);
	bool  ShowShadow = true;
	
	if(NumberOfShadows != TypeShadow.All){
		if(NumberOfShadows == TypeShadow.Most && RandomNumber > 7)
			ShowShadow = false;
		else
		if(NumberOfShadows == TypeShadow.Half && RandomNumber > 5)
			ShowShadow = false;
		else
		if(NumberOfShadows == TypeShadow.Some && RandomNumber <= 7)
			ShowShadow = false;
		else
		if(NumberOfShadows == TypeShadow.None)
			ShowShadow = false;
			
		if(!ShowShadow && MyCloudParticle.IsShadowActive())
			MyCloudParticle.SetShadowActive(false);
		else
		if(ShowShadow && !MyCloudParticle.IsShadowActive())
			MyCloudParticle.SetShadowActive(true);
	}
	else
		if(!MyCloudParticle.IsShadowActive())
			MyCloudParticle.SetShadowActive(true);
}

// Define all the Cloud props, based on the size of the ellipse that depends on size var in inspector.
// Also define de material based on the type of cloud.
/*void  DefineCloudProperties ( GameObject Cloud ,   int Num ,   bool FirstTime ,    bool isEllipsoidChanged  ){
	Vector3 Ellipsoid = Vector3.zero;
	
	// Get the Main Components that i want to configure
	ParticleEmitter MyPartEmitter = Cloud.GetComponent<ParticleEmitter>();
	//Projector Shadow = Cloud.GetComponentInChildren<Projector>();
	Projector Shadow = (Projector)MyCloudsProjectors[Num];
	CloudMovement CloudScript = Cloud.GetComponent<CloudMovement>();
	// Dont emit a single one particle, please
	MyPartEmitter.emit = false;
	
	// The first time, all things are first time defined, so all change.
	if(FirstTime)
		isEllipsoidChanged = true;
		
	// Change the Material depending on the type defined by user.
	AssignCloudMaterial(Cloud, CloudRender, TypeClouds);
	
	// Calculate the ellipsiod of the Particle, based on his size.
	if(isEllipsoidChanged){
		Ellipsoid.x = Random.Range(MaxWithCloud/4, MaxWithCloud);
		if(FixedSize){
			if(Ellipsoid.x <= 10)
				Ellipsoid.x = 10;
			else
			if(Ellipsoid.x > 10 && Ellipsoid.x <= 20)
				Ellipsoid.x = 20;
			else
			if(Ellipsoid.x > 20 && Ellipsoid.x <= 100)
				Ellipsoid.x = 100;
			else
			if(Ellipsoid.x > 100 && Ellipsoid.x <= 200)
				Ellipsoid.x = 200;
		}
		
		Ellipsoid.z = Ellipsoid.x;
		
		if( TypeClouds == Type.Nimbus1 || TypeClouds == Type.Nimbus2 || 
			TypeClouds == Type.Nimbus3 || TypeClouds == Type.Nimbus4 || 
			TypeClouds == Type.MixNimbus || TypeClouds == Type.MixAll)
				Ellipsoid.y = Random.Range(MaxTallCloud/Random.Range(2, 3), MaxTallCloud);
		else
		if(TypeClouds == Type.Cirrus1 || TypeClouds == Type.Cirrus1 || TypeClouds == Type.MixCirrus)
			Ellipsoid.y = Random.Range(1, MaxTallCloud);
		// Divide the ellipsoid by 2 to get the radious.
		Ellipsoid /= 2.0f;
		// Only the firtTime add to the array, in other case just insert the new ellipsoid in his position.
		if(FirstTime)
			MyCloudsEllipsoids.Add(Ellipsoid);
		else
			MyCloudsEllipsoids[Num] = Ellipsoid;
	}
	
	AssignCloudEmitter(Cloud, Num, (Vector3)MyCloudsEllipsoids[Num], MyPartEmitter, Shadow, CloudScript);
	
	// Assign the velocity depending on var defined by the user.
	/*if(VelocityMultipier != 1)
		CloudScript.Velocity *= VelocityMultipier;*//*
	
	// Assign the lenght Scale for use it in case of Soft Part (Streched render mode) and determine the type of render
	// mode the user selected.
	ParticleRenderer MyParticleRenderer = Cloud.GetComponent<ParticleRenderer>();
	MyParticleRenderer.lengthScale = LengthSpread;
	MyParticleRenderer.maxParticleSize = 10;
	if(SoftClouds)
		MyParticleRenderer.particleRenderMode = ParticleRenderMode.Stretch;
	else
		MyParticleRenderer.particleRenderMode = ParticleRenderMode.SortedBillboard;
	
	if(isEllipsoidChanged)
		Shadow.transform.localPosition = new Vector3(0.0f, Ellipsoid.x * 2.0f, 0.0f);
	
	// Determine if cloud shadow must be active or not, depending on user choice
	if(FirstTime){
		int RandomNumber = Random.Range(0, 10);
		bool  ShowShadow = true;
		
		Shadow.transform.localPosition = new Vector3(0.0f, Ellipsoid.x * 2.0f, 0.0f);

		if(NumberOfShadows != TypeShadow.All){
			if(NumberOfShadows == TypeShadow.Most && RandomNumber > 7)
				ShowShadow = false;
			else
			if(NumberOfShadows == TypeShadow.Half && RandomNumber > 5)
				ShowShadow = false;
			else
			if(NumberOfShadows == TypeShadow.Some && RandomNumber <= 7)
				ShowShadow = false;
			else
			if(NumberOfShadows == TypeShadow.None)
				ShowShadow = false;
				
			if(!ShowShadow && Shadow.gameObject.active)
				Shadow.gameObject.active = false;
			else
			if(ShowShadow && !Shadow.gameObject.active)
				Shadow.gameObject.active = true;
		}
		else
			if(!Shadow.gameObject.active)
				Shadow.gameObject.active = true;
	}
	
	// Emit the particles using the emitter.
	if(FirstTime || isEllipsoidChanged){
		MyPartEmitter.ClearParticles();
		MyPartEmitter.Emit();
	}
}

void  AssignCloudEmitter ( GameObject Cloud ,   int Num ,   Vector3 Ellipsoid ,   ParticleEmitter MyPartEmitter ,   Projector Shadow ,   CloudMovement CloudScript  ){
	
	if(!MyPartEmitter)
		MyPartEmitter = Cloud.GetComponent<ParticleEmitter>();
	if(!Shadow)
		Shadow = (Projector)MyCloudsProjectors[Num];
	if(!CloudScript)	
		CloudScript = Cloud.GetComponent(typeof(CloudMovement)) as CloudMovement;
	
	
	
	// Assing the world velocity of the particle emitter for streched part rendering if needed
	MyPartEmitter.worldVelocity = SpreadDir; 
	// Define the properties od the particle Emitter.
	MyPartEmitter.useWorldSpace =   false;
	MyPartEmitter.rndRotation = true;
	MyPartEmitter.minEnergy = 3;
	MyPartEmitter.maxEnergy = 5;
	// If softclouds are active, then we emit just a few particles to strech because of the FPS drop.
	// else we emit a normal amount of particles to get a nice cloud  based on his actual size.
	if(SoftClouds){
		MyPartEmitter.maxSize = Ellipsoid.x * SizeFactorPart;
		MyPartEmitter.minSize =  Ellipsoid.y  * SizeFactorPart;
		MyPartEmitter.minEmission = 1 * EmissionMult;
		MyPartEmitter.maxEmission = Random.Range(MyPartEmitter.minEmission, MyPartEmitter.minEmission + 10 * EmissionMult);
	}
	else{
		MyPartEmitter.maxSize = ((Ellipsoid.x + Mathf.Cos(Ellipsoid.x*0.1f))* SizeFactorPart);
		MyPartEmitter.minSize = MyPartEmitter.maxSize * 0.75f;
		MyPartEmitter.maxEmission = Ellipsoid.x * EmissionMult;
		MyPartEmitter.minEmission = MyPartEmitter.maxEmission * 0.75f;
	}

	Shadow.transform.localPosition = new Vector3(0, Ellipsoid.x*3, 0);
	
	if(Ellipsoid.x <= 10){
		Cloud.name = "Cloud" + Num.ToString()+" Little";
		CloudScript.Velocity = MaximunVelocity / 1;
	}
	else
	if(Ellipsoid.x > 10 && Ellipsoid.x <= 20){
		Cloud.name = "Cloud" + Num.ToString()+" Medium";
		CloudScript.Velocity = MaximunVelocity / 2;
	}
	else
	if(Ellipsoid.x > 20 && Ellipsoid.x <= 100){
		Cloud.name = "Cloud" + Num.ToString()+" Big";
		CloudScript.Velocity = MaximunVelocity / 3;
	}
	else
	if(Ellipsoid.x > 100 &&  Ellipsoid.x <= 200){
		Cloud.name = "Cloud" + Num.ToString()+" Gigant";
		CloudScript.Velocity = MaximunVelocity / 4;
	}
	else{
		Cloud.name = "Cloud" + Num.ToString()+" Massive";
		CloudScript.Velocity = MaximunVelocity / 5;
	}
	
	// Assign the velocity depending on var defined by the user.
	if(VelocityMultipier != 1)
		CloudScript.Velocity *= VelocityMultipier;
}


// Move all the emitted particles, so they will exist only in the bounds of the desired ellipsoid.
bool UpdateCloudsPosition ( GameObject Cloud ,   Vector3 Ellipsoid ){
	bool  allUpdated = true;
	int j = 0;
	Vector3 MyPos;
	Particle[] theParticles;

	
	ParticleEmitter MyPartEmitter = Cloud.GetComponent<ParticleEmitter>();
	//Array theParticles = new Array(Cloud.particleEmitter.particles); 
	theParticles = MyPartEmitter.particles;
	if(theParticles.Length > 0){
		for (j = 0; j < theParticles.Length; j++){
			/*MyPos.x = Random.Range(-Ellipsoid.x, Ellipsoid.x);
			MyPos.y = Random.Range(-Ellipsoid.y, Ellipsoid.y);
			MyPos.z = Random.Range(-Ellipsoid.z, Ellipsoid.z);*//*
			// Ellipsoid Emission.
			float alfa = Random.Range(0.0f, Mathf.PI*2);
			float beta = Random.Range(0.0f, Mathf.PI);
			float a =  Random.Range(0.0f, 1.0f);
			float b =  Random.Range(0.0f, 1.0f);
			float c =  Random.Range(0.0f, 1.0f);

			MyPos.x = a * Mathf.Cos(alfa) * Mathf.Sin(beta) * Ellipsoid.x;
			MyPos.y = b * Mathf.Sin(alfa) * Mathf.Sin(beta) * Ellipsoid.y;
			MyPos.z = c * Mathf.Cos(beta) * Ellipsoid.z;
			if(Random.value > 0.5f)
				MyPos.x = - MyPos.x;
			if(Random.value > 0.5f)
				MyPos.y = - MyPos.y;
			if(Random.value > 0.5f)
				MyPos.z = - MyPos.z;
			theParticles[j].position = MyPos;
		}
		// And write changes back
		//Cloud.particleEmitter.particles = theParticles.ToBuiltin(Particle);
		MyPartEmitter.particles = theParticles;
	}
	else
		allUpdated = false;
		
	return allUpdated;
}


// Move all the emitted particles, so they will exist only in the bounds of the desired ellipsoid.
void  PaintParticlesBelow ( GameObject Cloud ,   Vector3 MyEllipsoid ,   int TintStrength ,   Color TintColor  ){
	int i = 0;
	float particleHeight = 0;
	Particle[] theParticles;
	
	ParticleEmitter MyPartEmitter = Cloud.GetComponent<ParticleEmitter>();
	theParticles= MyPartEmitter.particles;
	if(theParticles.Length > 0){
		// get all the positions of the particles emitted
		for(i=0; i < theParticles.Length; i++){
			if(TintStrength > 1){
			   // Get the height of the particles emmited
				if(PaintType == TypePaintDistr.Below)
					if(theParticles[i].position.y > 0)
						particleHeight = ((( (MyEllipsoid.y*2) - theParticles[i].position.y) / MyEllipsoid.y) - offset);
					else
						particleHeight = (((MyEllipsoid.y + theParticles[i].position.y)  / MyEllipsoid.y) - offset);
				else
				// Random distribution of the tinted particles.
				if(PaintType == TypePaintDistr.Random)
					particleHeight = (theParticles[i].position.y - MyEllipsoid.y) / MyEllipsoid.x;
				
				// Paint the particles depending on his 'height' (below or random).
				if( i < (theParticles.Length*TintStrength)/100)				
					theParticles[i].color = Color.Lerp (TintColor, MainColor, particleHeight);
				else
					theParticles[i].color = MainColor;
			}
			else
				theParticles[i].color = MainColor;
		}
		// And write changes back
		MyPartEmitter.particles = theParticles;
	}
}*/


// Not used. Dont wanna draw the gizmos all the time, only when selecting the cloudManager.
/*void  OnDrawGizmos (){
	// Draw a blue sphere creation shape at the transform's position
	Gizmos.color = Color.blue;
	Gizmos.DrawWireCube (transform.position, Side);
	// Draw a yellow sphere at dissapear distance at the transform's position
	Gizmos.color = Color.yellow;
	Gizmos.DrawWireCube (transform.position, Side * DisappearMultiplier);
}*/

// Draw the two gizmos, blue one is the first instantiated area of the clouds.
// 2nd is the limits of their movement (dissapear from one side, appear in the opposite side).
void  OnDrawGizmosSelected (){
	// Draw a blue sphere creation shape at the transform's position
	Gizmos.color = Color.blue;
	Gizmos.DrawWireCube (transform.position, Side);
	// Draw a yellow sphere at dissapear distance at the transform's position
	Gizmos.color = Color.yellow;
	Gizmos.DrawWireCube (transform.position, Side * DisappearMultiplier);
}

}