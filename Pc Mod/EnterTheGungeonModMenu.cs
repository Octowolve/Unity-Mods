using System;
using UnityEngine;

namespace ETGModMenu
{
    public class ModMenu : MonoBehaviour
    {
        private static void Menu()
        {
            if (GUI.Button(new Rect(670f, 20f, 130f, 30f), string.Format("Mod Menu is {0}", ModMenu.showMenu ? "ON" : "OFF"), ModMenu.BtnStyle))
            {
                ModMenu.showMenu = !ModMenu.showMenu;
            }
            if (ModMenu.showMenu && !BraveUtility.isLoadingLevel && GameManager.HasInstance && GameManager.Instance.Dungeon != null)
            {
                ModMenu.windowRect = GUI.Window(0, new Rect(20f, 170f, 300f, 400f), new GUI.WindowFunction(ModMenu.WindowFunction), "Modz");
                ModMenu.windowRect2 = GUI.Window(1, new Rect(350f, 170f, 300f, 400f), new GUI.WindowFunction(ModMenu.WindowFunction), "Gun in Inventory");
                ModMenu.windowRect3 = GUI.Window(2, new Rect(680f, 170f, 500f, 400f), new GUI.WindowFunction(ModMenu.WindowFunction), "All Guns");
                ModMenu.windowRect3 = GUI.Window(3, new Rect(1210f, 170f, 500f, 400f), new GUI.WindowFunction(ModMenu.WindowFunction), "All Items");
            }
        }

        private static void WindowFunction(int windowID)
        {
            if (windowID == 0)
            {
                GUILayout.BeginArea(new Rect(5f, 15f, 290f, 400f));
                GUILayout.Label(string.Format("Current Actor Name: {0}", GameManager.Instance.PrimaryPlayer.ActorName), new GUILayoutOption[0]);
                GUILayout.Label(string.Format("Current Gun Name: {0}", GameManager.Instance.PrimaryPlayer.CurrentGun.name), new GUILayoutOption[0]);
                if (GUILayout.Button(string.Format("Stealthed {0}", GameManager.Instance.PrimaryPlayer.IsStealthed ? "On" : "Off"), new GUILayoutOption[0]))
                {
                    if (GameManager.Instance.PrimaryPlayer.IsStealthed)
                    {
                        GameManager.Instance.PrimaryPlayer.gameActor.SetIsStealthed(false, "box");
                    }
                    else
                    {
                        GameManager.Instance.PrimaryPlayer.gameActor.SetIsStealthed(true, "box");
                    }
                }
                if (GUILayout.Button(string.Format("Clear Floor", new object[0]), new GUILayoutOption[0]))
                {
                    GameManager.Instance.Dungeon.FloorCleared();
                }
                if (GUILayout.Button(string.Format("Heal All Players", new object[0]), new GUILayoutOption[0]))
                {
                    for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
                    {
                        if (GameManager.Instance.AllPlayers[i].healthHaver.IsAlive)
                        {
                            GameManager.Instance.AllPlayers[i].healthHaver.FullHeal();
                        }
                    }
                }
                if (GUILayout.Button(string.Format("Change to Random Gun", new object[0]), new GUILayoutOption[0]))
                {
                    GameManager.Instance.PrimaryPlayer.inventory.AddGunToInventory(PickupObjectDatabase.GetRandomGun(), true);
                }
                if (GUILayout.Button(string.Format("Add 100 currency", new object[0]), new GUILayoutOption[0]))
                {
                    GameStatsManager.Instance.RegisterStatChange(TrackedStats.META_CURRENCY, 100f);
                    GameManager.Instance.PrimaryPlayer.carriedConsumables.Currency += 100;
                }
                if (GUILayout.Button(string.Format("Unlimited Ammo {0}", ModMenu.unlimAmmo ? "On" : "Off"), new GUILayoutOption[0]))
                {
                    ModMenu.unlimAmmo = !ModMenu.unlimAmmo;
                }
                if (GUILayout.Button(string.Format("Destroy Enemy Bullets", new object[0]), new GUILayoutOption[0]))
                {
                    SilencerInstance.DestroyBulletsInRange(GameManager.Instance.PrimaryPlayer.transform.PositionVector2(), 100f, true, false, null, false, null, false, null);
                }
                if (GUILayout.Button(string.Format("Teleport Hax {0}", ModMenu.teleportHax ? "On" : "Off"), new GUILayoutOption[0]))
                {
                    ModMenu.teleportHax = !ModMenu.teleportHax;
                }
                if (GUILayout.Button(string.Format("Spawn Currency", new object[0]), new GUILayoutOption[0]))
                {
                    LootEngine.SpawnCurrency(GameManager.Instance.PrimaryPlayer.CenterPosition, 1000, false);
                }
                if (GUILayout.Button(string.Format("Can always steal {0}", ModMenu.canAlwaysSteal ? "On" : "Off"), new GUILayoutOption[0]))
                {
                    ModMenu.canAlwaysSteal = !ModMenu.canAlwaysSteal;
                }
                if (GUILayout.Button(string.Format("Add Ammo Automatically {0}", ModMenu.addAmmoAutomatically ? "On" : "Off"), new GUILayoutOption[0]))
                {
                    ModMenu.addAmmoAutomatically = !ModMenu.addAmmoAutomatically;
                }
                if (GUILayout.Button(string.Format("Auto Heal {0}", ModMenu.autoHeal ? "On" : "Off"), new GUILayoutOption[0]))
                {
                    ModMenu.autoHeal = !ModMenu.autoHeal;
                }
                if (GUILayout.Button(string.Format("Increase Base Ammo", new object[0]), new GUILayoutOption[0]))
                {
                    GameManager.Instance.PrimaryPlayer.CurrentGun.SetBaseMaxAmmo(10000);
                }
                GUILayout.EndArea();
            }
            if (windowID == 1)
            {
                GUILayout.BeginArea(new Rect(5f, 15f, 290f, 400f));
                ModMenu.scrollViewVector = GUILayout.BeginScrollView(ModMenu.scrollViewVector, new GUILayoutOption[]
                {
                    GUILayout.Width(280f),
                    GUILayout.Height(380f)
                });
                if (GameManager.Instance.PrimaryPlayer.inventory.AllGuns != null)
                {
                    for (int j = 0; j < GameManager.Instance.PrimaryPlayer.inventory.AllGuns.Count; j++)
                    {
                        if (GUILayout.Button(string.Format("Get Ammo for {0}", GameManager.Instance.PrimaryPlayer.inventory.AllGuns[j].DisplayName), ModMenu.BtnStyle, new GUILayoutOption[0]))
                        {
                            GameManager.Instance.PrimaryPlayer.inventory.AllGuns[j].GainAmmo(1000);
                        }
                        GUILayout.Label(string.Concat(new object[]
                        {
                            "Gun Name: ",
                            GameManager.Instance.PrimaryPlayer.inventory.AllGuns[j].name,
                            "\nDisplay Name: ",
                            GameManager.Instance.PrimaryPlayer.inventory.AllGuns[j].DisplayName,
                            "\nGun Ammo: ",
                            GameManager.Instance.PrimaryPlayer.inventory.AllGuns[j].ammo,
                            "\n================"
                        }), new GUILayoutOption[0]);
                    }
                }
                GUILayout.EndScrollView();
                GUILayout.EndArea();
            }
            if (windowID == 2)
            {
                GUILayout.BeginArea(new Rect(5f, 15f, 490f, 400f));
                ModMenu.scrollViewVector2 = GUILayout.BeginScrollView(ModMenu.scrollViewVector2, new GUILayoutOption[]
                {
                    GUILayout.Width(480f),
                    GUILayout.Height(380f)
                });
                int count = PickupObjectDatabase.Instance.Objects.Count;
                for (int k = 0; k < PickupObjectDatabase.Instance.Objects.Count; k++)
                {
                    if (PickupObjectDatabase.Instance.Objects[k] != null && PickupObjectDatabase.Instance.Objects[k] is Gun)
                    {
                        EncounterTrackable component = PickupObjectDatabase.Instance.Objects[k].GetComponent<EncounterTrackable>();
                        if (component && component.PrerequisitesMet())
                        {
                            if (GUILayout.Button(string.Format("Add {0} to your Inventory", PickupObjectDatabase.Instance.Objects[k].DisplayName), ModMenu.BtnStyle, new GUILayoutOption[0]))
                            {
                                GameManager.Instance.PrimaryPlayer.inventory.AddGunToInventory(PickupObjectDatabase.Instance.Objects[k] as Gun, true);
                            }
                            GUILayout.Label(string.Concat(new object[]
                            {
                                "Gun Name: ",
                                PickupObjectDatabase.Instance.Objects[k].name,
                                "\nDisplay Name: ",
                                PickupObjectDatabase.Instance.Objects[k].DisplayName,
                                "\nItem Quality: ",
                                PickupObjectDatabase.Instance.Objects[k].quality.ToString(),
                                "\n================"
                            }), new GUILayoutOption[0]);
                        }
                    }
                }
                GUILayout.EndScrollView();
                GUILayout.EndArea();
            }
            if (windowID == 3)
            {
                GUILayout.BeginArea(new Rect(5f, 15f, 490f, 400f));
                ModMenu.scrollViewVector3 = GUILayout.BeginScrollView(ModMenu.scrollViewVector3, new GUILayoutOption[]
                {
                    GUILayout.Width(480f),
                    GUILayout.Height(380f)
                });
                for (int l = 0; l < PickupObjectDatabase.Instance.Objects.Count; l++)
                {
                    if (PickupObjectDatabase.Instance.Objects[l] != null && PickupObjectDatabase.Instance.Objects[l] is PassiveItem)
                    {
                        EncounterTrackable component2 = PickupObjectDatabase.Instance.Objects[l].GetComponent<EncounterTrackable>();
                        if (component2 && component2.PrerequisitesMet())
                        {
                            if (GUILayout.Button(string.Format("Add {0} to your Inventory", PickupObjectDatabase.Instance.Objects[l].DisplayName), ModMenu.BtnStyle, new GUILayoutOption[0]))
                            {
                                GameManager.Instance.PrimaryPlayer.AcquirePassiveItem(PickupObjectDatabase.Instance.Objects[l] as PassiveItem);
                            }
                            GUILayout.Label(string.Concat(new object[]
                            {
                                "Item Name: ",
                                PickupObjectDatabase.Instance.Objects[l].name,
                                "\nDisplay Name: ",
                                PickupObjectDatabase.Instance.Objects[l].DisplayName,
                                "\n================"
                            }), new GUILayoutOption[0]);
                        }
                    }
                }
                GUILayout.EndScrollView();
                GUILayout.EndArea();
            }
        }

        // Call this in GameCursorController.OnGUI()
        public static void CreateMenu()
        {
            ModMenu.Update();
            ModMenu.Menu();
        }

        private static void Update()
        {
        //Doesnt quite work yet. so its your challenge to get it to work =)
            if (ModMenu.teleportHax && Input.GetMouseButtonDown(1))
            {
                Vector2 mousePosition = BraveInput.GetInstanceForPlayer(0).MousePosition;
                Vector3 position = Camera.main.ScreenToWorldPoint(mousePosition);
                if (GameManager.Instance.PrimaryPlayer)
                {
                    GameManager.Instance.PrimaryPlayer.transform.position = position;
                }
            }
            if (ModMenu.addAmmoAutomatically)
            {
                int num = GameManager.Instance.PrimaryPlayer.CurrentGun.AdjustedMaxAmmo / 2;
                if (GameManager.Instance.PrimaryPlayer.CurrentGun.CurrentAmmo <= num)
                {
                    GameManager.Instance.PrimaryPlayer.CurrentGun.GainAmmo(10000);
                }
            }
            if (ModMenu.autoHeal)
            {
                for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
                {
                    float num2 = GameManager.Instance.AllPlayers[i].healthHaver.GetMaxHealth() / 2f;
                    if (GameManager.Instance.AllPlayers[i].healthHaver.IsAlive && GameManager.Instance.AllPlayers[i].healthHaver.GetCurrentHealth() <= num2)
                    {
                        GameManager.Instance.AllPlayers[i].healthHaver.FullHeal();
                    }
                }
            }
        }

        public static Texture2D BtnTexture
        {
            get
            {
                if (ModMenu.btntexture == null)
                {
                    ModMenu.btntexture = ModMenu.NewTexture2D;
                    ModMenu.btntexture.SetPixel(0, 0, new Color32(3, 155, 229, byte.MaxValue));
                    ModMenu.btntexture.Apply();
                }
                return ModMenu.btntexture;
            }
        }

        public static Texture2D NewTexture2D
        {
            get
            {
                return new Texture2D(1, 1);
            }
        }

        //Call this in GameCursorController.Start()
        public static void Start()
        {
            if (ModMenu.BtnStyle == null)
            {
                ModMenu.BtnStyle = new GUIStyle();
                ModMenu.BtnStyle.normal.background = ModMenu.BtnTexture;
                ModMenu.BtnStyle.onNormal.background = ModMenu.BtnTexture;
                ModMenu.BtnStyle.active.background = ModMenu.BtnTexture;
                ModMenu.BtnStyle.onActive.background = ModMenu.BtnTexture;
                ModMenu.BtnStyle.normal.textColor = Color.white;
                ModMenu.BtnStyle.onNormal.textColor = Color.white;
                ModMenu.BtnStyle.active.textColor = Color.grey;
                ModMenu.BtnStyle.onActive.textColor = Color.grey;
                ModMenu.BtnStyle.fontSize = 12;
                ModMenu.BtnStyle.fontStyle = FontStyle.Normal;
                ModMenu.BtnStyle.alignment = TextAnchor.MiddleCenter;
            }
        }

        public static Rect windowRect;

        public static bool unlimAmmo;

        private static Vector2 scrollViewVector;

        private static Rect windowRect2;

        private static Rect windowRect3;

        private static Vector2 scrollViewVector2;

        private static bool teleportHax;

        private static bool showMenu;

        public static bool canAlwaysSteal;

        public static Texture2D btntexture;

        public static GUIStyle BtnStyle;

        private static bool addAmmoAutomatically;

        private static bool autoHeal;

        private static Vector2 scrollViewVector3;
    }
}
