using Newtonsoft.Json;

namespace Chem4Word.Renderer.OoXmlV3
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options
    {
        [JsonProperty]
        public bool ShowHydrogens { get; set; }

        [JsonProperty]
        public bool ColouredAtoms { get; set; }

        // Debugging
        [JsonProperty]
        public bool ClipLines { get; set; }

        // Debugging
        [JsonProperty]
        public bool ShowCharacterBoundingBoxes { get; set; }

        // Debugging
        [JsonProperty]
        public bool ShowMoleculeBoundingBoxes { get; set; }

        // Debugging
        [JsonProperty]
        public bool ShowAtomPositions { get; set; }

        // Debugging
        [JsonProperty]
        public bool ShowRingCentres { get; set; }

        // Debugging
        [JsonProperty]
        public bool PushBondToCentre { get; set; }

        public Options()
        {
            RestoreDefaults();
        }

        public Options Clone()
        {
            Options clone = new Options();

            clone.ColouredAtoms = ColouredAtoms;
            clone.ShowHydrogens = ShowHydrogens;

            // Debugging Options
            clone.ClipLines = ClipLines;
            clone.PushBondToCentre = PushBondToCentre;
            clone.ShowCharacterBoundingBoxes = ShowCharacterBoundingBoxes;
            clone.ShowMoleculeBoundingBoxes = ShowMoleculeBoundingBoxes;
            clone.ShowRingCentres = ShowRingCentres;
            clone.ShowAtomPositions = ShowAtomPositions;

            return clone;
        }

        public void RestoreDefaults()
        {
            ShowHydrogens = true;
            ColouredAtoms = true;

            // Debugging Options
            ClipLines = true;
            PushBondToCentre = false;
            ShowCharacterBoundingBoxes = false;
            ShowMoleculeBoundingBoxes = false;
            ShowRingCentres = false;
        }
    }
}