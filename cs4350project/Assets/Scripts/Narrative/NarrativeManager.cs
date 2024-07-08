using System.Collections.Generic;

public class NarrativeManager : Singleton<NarrativeManager>
{
    private Dictionary<string, bool> m_PersistentFlags;
    private Dictionary<string, bool> m_TransientFlags;
}