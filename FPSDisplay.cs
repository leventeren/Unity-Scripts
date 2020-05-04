using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace balloonCatch
{
    [RequireComponent(typeof(Text))]
    public class FPSDisplay : MonoBehaviour
    {

        [SerializeField] private Text label;
        [SerializeField] private TMPro.TextMeshProUGUI tmpLabel;
        private System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(50);
        private float fpsAVG;
        private string format = "+000.00;-000.00";

        private float gcMemory = 0F;
        private float lastGcMemory = 0F;
        private float gcMemoryInMB = 0F;

        private float gcDiff = 0F;
        private float gcDiffAVG = 0F;
        private float gcDiffAVGkB = 0F;

        private float byteToMB = 0.00000095367431640625F;   //	/ 1024 / 1024;
        private float byteTokB = 0.0009765625F;         //	/ 1024 / 1024;

        public bool richText = false;
        public bool useTextMeshPro = false;

        public float curTimeFac;

        public bool updateText;
        public void ToggleTextUpdate()
        {
            updateText = !updateText;
            label.text = tmpLabel.text = "FPS";
        }

        //	public	bool	precacheActive = true;

        //	void Awake(){
        //		if(precacheActive){
        //			FontUtils.GetPreCacheRoutine( this, label, ()=>{ precacheActive = false; });
        //		}
        //	}

        void Update()
        {

            if (!label.gameObject.activeInHierarchy /*|| precacheActive*/ ) { return; }

            //	print(1F/Time.deltaTime);

            //Total GC Mem
            gcMemory = (((float)System.GC.GetTotalMemory(false)));
            gcMemoryInMB = gcMemory * byteToMB;

            //Total GC Increase per Frame
            gcDiff = gcMemory - lastGcMemory;
            lastGcMemory = gcMemory;

            gcDiffAVG += (gcDiff - gcDiffAVG) * 0.03f;
            gcDiffAVGkB = gcDiffAVG * byteTokB;

            //frames per Second
            fpsAVG += ((Time.unscaledDeltaTime) - fpsAVG) * 0.03f;

            if (!useTextMeshPro)
            {
                if (updateText)
                {
                    if (richText) { label.text = GetRichText(); }
                    else { label.text = GetText(); }
                }
            }
            else
            {
                if (updateText)
                {
                    tmpLabel.text = GetText();
                }
            }

            if (selfdestruct) { Destroy(this.gameObject); }
        }


        private string GetText()
        {

            stringBuilder.Length = 0;
            stringBuilder.Append((1F / fpsAVG).ToString(format)); stringBuilder.Append(" FPS\n");
            stringBuilder.Append(gcMemoryInMB.ToString(format)); stringBuilder.Append(" MB Total\n");
            stringBuilder.Append(gcDiffAVGkB.ToString(format)); stringBuilder.Append(" kB/frame");

            return stringBuilder.ToString();
        }
        private string GetRichText()
        {
            return
            "<color=#00ffffff>" + (1F / fpsAVG).ToString(format) + "</color> FPS\n" +

            gcDiffAVGkB.ToString(format) + " kB Diff\n" +
            gcMemoryInMB.ToString(format) + " MB Total";
        }


        private static bool selfdestruct = false;
        public static void DestroyAllFPSLabels()
        {
            selfdestruct = true;
        }

        //	//Changing a Text every frame causes performance loss, cache per label
        //
        //	// replaced UILabel with Text:
        //	//https://gist.github.com/lonewolfwilliams/beaec7f85cb739bd770a4026336ee1ee
        //
        //	//adapted from:
        //	//http://answers.unity3d.com/questions/733570/massive-lag-due-to-fontcachefontfortext-please-hel.html
        //	public static class FontUtils
        //	{
        //		
        //		public static void GetPreCacheRoutine( MonoBehaviour coroutineTarget, Text label, System.Action callback ){
        //			coroutineTarget.StartCoroutine(GetPreCacheRoutine(label, callback));
        //		}
        //
        //		private const string ALL_GLYPHS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_+=~`[]{}|\\:;\"'<>,.?/ ";
        //
        //		private static IEnumerator GetPreCacheRoutine( Text label, System.Action callback )
        //		{
        //			return	PrecacheFontGlyphs(label.font, label.fontSize, label.fontStyle, ALL_GLYPHS, callback);
        //		}
        //		private static IEnumerator PrecacheFontGlyphs(Font theFont, int fontSize, FontStyle style, string glyphs, System.Action callback)
        //		{
        //			for (int i=0; i<glyphs.Length; i++)
        //			{
        //				theFont.RequestCharactersInTexture(glyphs[i].ToString());
        //				yield return null;
        //			}
        //			yield return new WaitForEndOfFrame();
        //
        //			callback();
        //		}
        //	}
        //
        //	//example usage with an NGUI label
        //	//
        //	// if(levelLabel 	!= null) StartCoroutine(FontUtils.GetPreCacheRoutine(levelLabel, 	() => {levelLabel.text = level.ToString();}));
        //	//
        //	//
    }
}
