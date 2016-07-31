using UnityEngine;
using System.Collections;

/* Scales texture tiling and applies edge highlights at runtime.
 * Also has capabilties to pulse object vibrancy and Lerp between two colors over time.
 */
public class ShaderMod : MonoBehaviour {
												// The following are applied at runtime
	public float tileFactor = 1.0f;			    // Scales the tiling frequency. Smaller values = smaller tiles
	public float highlightPower = 0.0f;			// How much of the edge to highlight (0.3 works well)
	public bool isPulse = false;				// Does this object pulsate?
	public float pulseFactor = 1.0f;			// Speed of pulse (larger values = faster pulse)
	public bool lerpColor = false;				// Does this object Lerp between two colors
	public float lerpFactor = 1.0f;				// Speed of color Lerp (larger values = faster Lerp)
	public Color color1 = Color.white;			// Lerp color 1 (set in editor)
	public Color color2 = Color.white;			// Lerp color 2 (set in editor)
	private Color lerpedColor = Color.white;	// Holds the color given by Lerp function

	void Start () {

        // Pass values to shader
        /*this.GetComponent<Renderer>().material.SetFloat("_TileFactor", tileFactor);
        this.GetComponent<Renderer>().material.SetFloat("_HighlightPower", highlightPower);
        this.GetComponent<Renderer>().material.SetFloat("_TexScaleX", transform.lossyScale.x);
        this.GetComponent<Renderer>().material.SetFloat("_TexScaleY", transform.lossyScale.y);
        this.GetComponent<Renderer>().material.SetFloat("_TexScaleZ", transform.lossyScale.z);
        */
    }

	void Update () {
		
		if (isPulse)		// Pass pulsating color to shader
			this.GetComponent<Renderer>().material.SetFloat("_PulseFactor", (Mathf.Sin(Time.time * pulseFactor) + 1.0f) / 2.0f);

		if (lerpColor) {
			lerpedColor = Color.Lerp(color1, color2, Mathf.PingPong((Time.time * lerpFactor), 1));

			// Pass Lerp'd color to shader
			this.GetComponent<Renderer>().material.SetColor("_Color", lerpedColor);
			this.GetComponent<Renderer>().material.SetColor("_HighlightColor", lerpedColor);
		}
	}
}