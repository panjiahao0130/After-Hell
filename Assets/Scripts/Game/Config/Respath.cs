using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respath
{
        /// <summary>
        /// 音效音效资源路径    
        /// </summary>
        private static readonly Dictionary<string, string> AudioResPath = new Dictionary<string, string>()
        {
            
        };

        public static GameObject BulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet/Bullet");
        public static GameObject BulletTracer = Resources.Load<GameObject>("Prefabs/Bullet/BulletTracer");
        public static GameObject SharpshooterPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/Sharpshooter");
        public static GameObject RampagerPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/Rampager");
        public static GameObject RiotSoldierPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/RiotSoldier");
        public static GameObject FishManSergeantPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/Boss/FishManSergeant");
        public static GameObject WarMachinePrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/Boss/WarMachine");
        public static GameObject GeneralPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/Boss/General");
        public static GameObject WarMachinePlusPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/Boss/WarMachinePlus");
        public static GameObject SelfDemonPrefab = Resources.Load<GameObject>("Prefabs/Character/Enemy/Boss/SelfDemon");
        public static GameObject CannonballPrefab = Resources.Load<GameObject>("Prefabs/Bullet/CannonBall");
        public static GameObject WarningLocation = Resources.Load<GameObject>("Prefabs/CombatPerformance/WarningLocation");
        public static GameObject ExplosionPrefab = Resources.Load<GameObject>("Prefabs/Effects/Explosion");

        public static Sprite InitialBulletSprite = Resources.Load<Sprite>("Sprites/3关卡/道具特效/枪/紫色");
        public static Sprite BeAttackedBulletSprite = Resources.Load<Sprite>("Sprites/3关卡/道具特效/枪/蓝色");

}
