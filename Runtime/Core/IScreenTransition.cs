using System;

namespace Erethan.ScriptableSystems.SceneLoad
{
    public interface IScreenTransition
    {
        bool Faded { get; }

        event Action FadeInComplete;
        event Action FadeOutComplete;


        void FadeIn();
        void FadeOut();
    }
}