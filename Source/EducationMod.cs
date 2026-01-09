using System.Reflection;
using HarmonyLib;
using Verse;

namespace ProgressiveChildEducation
{
    [StaticConstructorOnStartup]
    public static class EducationMod
    {
        static EducationMod()
        {
            var harmony = new Harmony("User.ProgressiveChildEducation");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Message("[Educação Progressiva] Mod carregado com sucesso.");
        }
    }
}