using System.Collections.Generic;

namespace Assets.Scripts.Common
{
    public static class Tags
    {
        public static string Player { get; } = "Player";
        public static Dictionary<string, string> Enemy { get; } = new Dictionary<string, string>
        {
            {
                "Enemy_1", "Description enemy 1"
            },
            {
                "Enemy_2", "Description enemy 1"
            }
        };
    }
}

