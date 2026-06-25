using System;
using System.Linq;
using System.Xml;
using UnityEngine;
using Verse;

namespace SuperHeroGenesBase
{
    public class PatchOperationMultSetting : PatchOperationPathed
    {
        public string setting;

        public float min = float.MinValue; // Minimum final result (i.e. compacted uranium/plasteel stop at 80 despite their lower base values taking them far lower if a 0.1 multiplier is used)

        public float mult = 1f; // A static multiplier that multiplier the result after the setting is implemented, assuming one is specified

        public int roundDigits = 0; // If it must be a whole number, leave it at 0, otherwise set it to whatever number looks nicest
        
        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (setting != null)
            {
                if (typeof(SuperheroGenes_Settings).GetField(setting)?.GetValue(typeof(SuperheroGenes_Settings)) is float value) // This happens if the setting provided exists
                {
                    if (value != 1 || mult != 1f)
                    {
                        var list = xml.SelectNodes(xpath)?.Cast<XmlNode>().ToList();
                        if (list == null) return false;
                        list.ForEach(li => li.InnerText = Math.Round(Mathf.Max(float.Parse(li.InnerText) * value * mult, min), roundDigits).ToStringSafe());
                    }
                }
                else
                    Log.Error($"A patch is using {setting}, which is either misspelled, nonexistent, or unhandled");
            }
            else if (mult != 1f)
            {
                var list = xml.SelectNodes(xpath)?.Cast<XmlNode>().ToList();
                if (list == null) return false;
                list.ForEach(li => li.InnerText = Mathf.Max(float.Parse(li.InnerText) * mult, min).ToStringSafe());
            }
            else 
                Log.Error("A patch is trying to use this mod's settings, but doesn't specify which one.");
            return true;
        }
    }
}
