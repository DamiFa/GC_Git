using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ChainFade : MonoBehaviour
{
	public MeshRenderer mRend;
	private Vector2 scaleTexture = Vector2.one;
	
	private void Update()
	{
		if(mRend != null)
		{
			scaleTexture.y = transform.localScale.y;
			
			if(Application.isPlaying) mRend.material.mainTextureScale = scaleTexture;
			else mRend.sharedMaterial.mainTextureScale = scaleTexture;
		}
		
	}/*Update*/
	
}/*ChainFade*/