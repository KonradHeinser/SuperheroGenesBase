using Verse;

namespace SuperHeroGenesBase
{
    public class HediffCompProperties_SpawnHumanlike : HediffCompProperties
    {
        public bool onRemoval = false;

        public FloatRange severities = new FloatRange(0f, 9999f);

        public IntRange spawnCount = new IntRange(1, 1);

        public ThingDef filthOnCompletion;

        public IntRange filthPerSpawn = new IntRange(4, 7);

        public bool sendLetters = true;

        public string letterLabelNote = "born";

        public string letterTextPawnDescription = "became a healthy baby!";

        public DevelopmentalStage developmentalStage = DevelopmentalStage.Adult;

        public DevelopmentalStage devStageForRemovalOrDeath = DevelopmentalStage.Child;

        public HediffCompProperties_SpawnHumanlike()
        {
            compClass = typeof(HediffComp_SpawnHumanlike);
        }
    }
}