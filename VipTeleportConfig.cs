using Rocket.API;
using Rocket.Core.Assets;

namespace VipTeleport
{
    public class VipTeleportConfig : IRocketPluginConfiguration, IDefaultable
    {
        public int cooldownTime;

        

        public void LoadDefaults()
        {
            this.cooldownTime = 120;
        }
    }
}
