using System;
using System.Collections.Generic;
using UnityEngine;

namespace AutoPost
{
    class Main : MonoBehaviour
    {
        // GUI Stuff
        public Rect Menu = new Rect(20, 20, 300, 700);
        public bool WindowOpen = false;
        public string PlayerPositionString = "";
        public string CursorPositionString = "";
        public string ClosestTreePositionString = "";
        public string ClosestRockPositionString = "";
        public string ClosestChestPositionString = "";
        public string ClosestSnowmanPartPositionString = "";
        public string ClosestBerryBushPositionString = "";
        public string ClosestFishingSpotPositionString = "";
        public string ClosestRunestonePositionString = "";

        // Counter
        public int MinionCounter;
        public int i = 4425;
        public int TickCount;
        public int rendererDisabledCount = 0;
        public int rendererReenabledCount = 0;

        // Settings
        public bool DisableRenderingToggle = false;
        public int SetTick = 144;

        // Positions
        Player Player;
        public Vector3 CursorPosition;
        Tree ClosestTree;
        Rock ClosestRock;
        Chest ClosestChest;
        SnowmanPart ClosestSnowmanPart;
        BerryBush ClosestBerryBush;
        FishingSpot ClosestFishingSpot;
        Runestone ClosestRunestone;
        GameObject CurrentGoal;

        // Disabled Renderer
        public List<GameObject> rendererDisabled = new List<GameObject>();
        public List<GameObject> billboardRendererDisabled = new List<GameObject>();
        public List<GameObject> lineRendererDisabled = new List<GameObject>();
        public List<GameObject> meshRendererDisabled = new List<GameObject>();
        public List<GameObject> skinnedMeshRendererDisabled = new List<GameObject>();
        public List<GameObject> spriteRendererDisabled = new List<GameObject>();
        public List<GameObject> trailRendererDisabled = new List<GameObject>();


        public void Start()
        {

        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                WindowOpen = !WindowOpen;
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Loader.Unload();
            }
            AutoPostUpdate();
        }

        public void OnGUI()
        {
            GUI.backgroundColor = Color.black;
            GUI.contentColor = Color.white;
            if (WindowOpen)
            {
                Menu = GUI.Window(0, Menu, DoMyWindow, "Menu <3");
            }
        }

        private void DoMyWindow(int windowID)
        {
            Rect Settings = new Rect(10, 20, Menu.width - 20, 200);
            DisableRenderingToggle = GUI.Toggle(new Rect(Settings.x, Settings.y, Settings.width, 25), DisableRenderingToggle, "Disable Rendering (Disabled due to performance issues)");
            GUI.Label(new Rect(Settings.x, Settings.y + 20, Settings.width, 20), "AutoPost Tickrate: ");
            SetTick = int.Parse(GUI.TextField(new Rect(Settings.x + Settings.width / 2, Settings.y + 20, Settings.width / 2, 20), SetTick.ToString(), "33"));
            Rect Information = new Rect(10, Settings.height + 20, Menu.width - 20, 300);
            GUI.Label(new Rect(Information.x, Information.y, Information.width, 25), "Minions Detected: " + MinionCounter);
            GUI.Label(new Rect(Information.x, Information.y + 20, Information.width, 25), "TickCount: " + TickCount);
            GUI.Label(new Rect(Information.x, Information.y + 40, Information.width, 30), "Cursor Position: " + CursorPosition);
            GUI.Label(new Rect(Information.x, Information.y + 60, Information.width, 30), "Player Position: " + PlayerPositionString);
            GUI.Label(new Rect(Information.x, Information.y + 80, Information.width, 30), "Closest Tree Position: " + ClosestTreePositionString);
            GUI.Label(new Rect(Information.x, Information.y + 100, Information.width, 30), "Closest Rock Position: " + ClosestRockPositionString);
            GUI.Label(new Rect(Information.x, Information.y + 120, Information.width, 30), "Closest Chest Position: " + ClosestChestPositionString);
            GUI.Label(new Rect(Information.x, Information.y + 140, Information.width, 30), "Closest Snowman Part Position: " + ClosestSnowmanPartPositionString);
            GUI.Label(new Rect(Information.x, Information.y + 160, Information.width, 30), "Closest Resource: " + ClosestFishingSpotPositionString);
            GUI.Label(new Rect(Information.x, Information.y + 160, Information.width, 30), "Closest Resource: " + ClosestRunestonePositionString);
            if (GUI.Button(new Rect(Information.x, Information.y + 190, Information.width, 30), "Move To Closest Tree"))
            {
                UpdatePositions();
                if (ClosestTree.transform.position.x != 0 && ClosestTree.transform.position.y != 0 && ClosestTree.transform.position.z != 0)
                {
                    Player.MoveTowards(ClosestTree.transform.position);
                }
            }
            if (GUI.Button(new Rect(Information.x, Information.y + 220, Information.width, 30), "Move To Closest Rock"))
            {
                UpdatePositions();
                if (ClosestRock.transform.position.x != 0 && ClosestRock.transform.position.y != 0 && ClosestRock.transform.position.z != 0)
                    Player.MoveTowards(ClosestRock.transform.position);
            }
            if (GUI.Button(new Rect(Information.x, Information.y + 250, Information.width, 30), "Move To Closest Chest"))
            {
                UpdatePositions();
                if (ClosestChest.transform.position.x != 0 && ClosestChest.transform.position.y != 0 && ClosestChest.transform.position.z != 0)
                {
                    Player.MoveTowards(ClosestChest.transform.position);
                }
            }
            if (GUI.Button(new Rect(Information.x, Information.y + 280, Information.width, 30), "Move To Closest Snowman Part"))
            {
                UpdatePositions();
                if (ClosestSnowmanPart.transform.position.x != 0 && ClosestSnowmanPart.transform.position.z != 0)
                {
                    Player.MoveTowards(ClosestSnowmanPart.transform.position);
                }
            }
            if (GUI.Button(new Rect(Information.x, Information.y + 310, Information.width, 30), "Move To Closest Berry Bush"))
            {
                UpdatePositions();
                if (ClosestBerryBush.transform.position.x != 0 && ClosestBerryBush.transform.position.y != 0 && ClosestBerryBush.transform.position.z != 0)
                {
                    Player.MoveTowards(ClosestBerryBush.transform.position);
                }
            }
            if (GUI.Button(new Rect(Information.x, Information.y + 340, Information.width, 30), "Move To Closest FishingSpot"))
            {
                UpdatePositions();
                if (ClosestFishingSpot.transform.position.x != 0 && ClosestFishingSpot.transform.position.z != 0)
                {
                    Player.MoveTowards(ClosestFishingSpot.transform.position);
                }
            }
            if (GUI.Button(new Rect(Information.x, Information.y + 370, Information.width, 30), "Move To Closest Runestone"))
            {
                UpdatePositions();
                if (ClosestRunestone.transform.position.x != 0 && ClosestRunestone.transform.position.z != 0)
                {
                    Player.MoveTowards(ClosestRunestone.transform.position);
                }
            }
            if (GUI.Button(new Rect(Information.x, Information.y + 400, Information.width, 30), "Try Harvesting"))
            {
                if (ClosestTree.CanGather())
                {
                    Gathering.StartGather(ClosestTree);
                }
            }
            GUI.DragWindow(new Rect(0, 0, 10000, 200));
        }

        public void DetectMinions()
        {
            Minion[] allMinions = FindObjectsOfType<Minion>();
            MinionCounter = allMinions.Length;
        }

        public void DisableRendering()
        {
            if (DisableRenderingToggle)
            {
                GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
                foreach (GameObject gameObject in allGameObjects)
                {
                    if (!gameObject.activeInHierarchy)
                    {
                        continue;
                    }
                    if (gameObject.GetComponent<Renderer>())
                    {
                        gameObject.GetComponent<Renderer>().enabled = false;
                        rendererDisabled.Add(gameObject);
                        rendererDisabledCount++;
                    }
                    if (gameObject.GetComponent<BillboardRenderer>())
                    {
                        gameObject.GetComponent<BillboardRenderer>().enabled = false;
                        billboardRendererDisabled.Add(gameObject);
                    }
                    if (gameObject.GetComponent<LineRenderer>())
                    {
                        gameObject.GetComponent<LineRenderer>().enabled = false;
                        lineRendererDisabled.Add(gameObject);
                    }
                    if (gameObject.GetComponent<MeshRenderer>())
                    {
                        gameObject.GetComponent<MeshRenderer>().enabled = false;
                        meshRendererDisabled.Add(gameObject);
                    }
                    if (gameObject.GetComponent<SkinnedMeshRenderer>())
                    {
                        gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
                        skinnedMeshRendererDisabled.Add(gameObject);
                    }
                    if (gameObject.GetComponent<SpriteRenderer>())
                    {
                        gameObject.GetComponent<SpriteRenderer>().enabled = false;
                        spriteRendererDisabled.Add(gameObject);
                    }
                    if (gameObject.GetComponent<TrailRenderer>())
                    {
                        gameObject.GetComponent<TrailRenderer>().enabled = false;
                        spriteRendererDisabled.Add(gameObject);
                    }
                }
            }
            if (!DisableRenderingToggle)
            {
                foreach (GameObject gameObject in rendererDisabled)
                {
                    if (!gameObject.activeInHierarchy)
                    {
                        continue;
                    }
                    gameObject.GetComponent<Renderer>().enabled = true;

                }
                rendererDisabled.Clear();
                foreach (GameObject gameObject in billboardRendererDisabled)
                {
                    if (!gameObject.activeInHierarchy)
                    {
                        continue;
                    }
                    gameObject.GetComponent<BillboardRenderer>().enabled = true;
                }
                billboardRendererDisabled.Clear();
                foreach (GameObject gameObject in lineRendererDisabled)
                {
                    if (!gameObject.activeInHierarchy)
                    {
                        continue;
                    }
                    gameObject.GetComponent<LineRenderer>().enabled = true;
                }
                lineRendererDisabled.Clear();
                foreach (GameObject gameObject in meshRendererDisabled)
                {
                    if (!gameObject.activeInHierarchy)
                    {
                        continue;
                    }
                    gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
                meshRendererDisabled.Clear();
                foreach (GameObject gameObject in skinnedMeshRendererDisabled)
                {
                    if (!gameObject.activeInHierarchy)
                    {
                        continue;
                    }
                    gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
                }
                skinnedMeshRendererDisabled.Clear();
                foreach (GameObject gameObject in spriteRendererDisabled)
                {
                    if (!gameObject.activeInHierarchy)
                    {
                        continue;
                    }
                    gameObject.GetComponent<SpriteRenderer>().enabled = true;
                }
                spriteRendererDisabled.Clear();
                foreach (GameObject gameObject in trailRendererDisabled)
                {
                    if (!gameObject.activeInHierarchy)
                    {
                        continue;
                    }
                    gameObject.GetComponent<TrailRenderer>().enabled = true;
                }
                trailRendererDisabled.Clear();
            }
        }

        public void MoveThere(GameObject goal)
        {
            Player.MoveTowards(goal.transform.position); // idk man make it work
        }

        public void AutoPostUpdate()
        {
            TickCount++;
            if (TickCount >= SetTick)
            {
                DetectMinions();
                UpdatePositions();
                TickCount = 0;
            }
        }

        public void UpdatePositions()
        {
            if (MainController.GameStarted)
            {
                Player = FindObjectOfType<Player>();
                PlayerPositionString = Player.transform.position.ToString();
                CursorPosition = Util.RayCastMouse();
                Double distance;
                Tree[] allTrees = FindObjectsOfType<Tree>();
                Double closestTreeDistance = double.MaxValue;
                foreach (Tree tree in allTrees)
                {
                    distance = (Math.Sqrt(Math.Pow((tree.transform.position.x) - (Player.transform.position.x), 2) + Math.Sqrt(Math.Pow((tree.transform.position.z) - (Player.transform.position.z), 2))));
                    if (closestTreeDistance > distance)
                    {
                        closestTreeDistance = distance;
                        ClosestTree = tree;
                        ClosestTreePositionString = tree.transform.position.ToString();
                    }
                }
                Rock[] allRocks = FindObjectsOfType<Rock>();
                Double closestRockDistance = double.MaxValue;
                foreach (Rock rock in allRocks)
                {
                    distance = (Math.Sqrt(Math.Pow((rock.transform.position.x) - (Player.transform.position.x), 2) + Math.Sqrt(Math.Pow((rock.transform.position.z) - (Player.transform.position.z), 2))));
                    if (closestRockDistance > distance)
                    {
                        closestRockDistance = distance;
                        ClosestRock = rock;
                        ClosestRockPositionString = rock.transform.position.ToString();
                    }
                }
                Chest[] allChests = FindObjectsOfType<Chest>();
                Double closestChestDistance = double.MaxValue;
                foreach (Chest chest in allChests)
                {
                    distance = (Math.Sqrt(Math.Pow((chest.transform.position.x) - (Player.transform.position.x), 2) + Math.Sqrt(Math.Pow((chest.transform.position.z) - (Player.transform.position.z), 2))));
                    if (closestChestDistance > distance)
                    {
                        closestChestDistance = distance;
                        ClosestChest = chest;
                        ClosestChestPositionString = chest.transform.position.ToString();
                    }
                }
                SnowmanPart[] allSnowmanParts = FindObjectsOfType<SnowmanPart>();
                Double closestSnowmanPartDistance = double.MaxValue;
                foreach (SnowmanPart snowmanPart in allSnowmanParts)
                {
                    distance = (Math.Sqrt(Math.Pow((snowmanPart.transform.position.x) - (Player.transform.position.x), 2) + Math.Sqrt(Math.Pow((snowmanPart.transform.position.z) - (Player.transform.position.z), 2))));
                    if (closestSnowmanPartDistance > distance)
                    {
                        closestSnowmanPartDistance = distance;
                        ClosestSnowmanPart = snowmanPart;
                        ClosestSnowmanPartPositionString = snowmanPart.transform.position.ToString();
                    }
                }
                BerryBush[] allBerryBushes = FindObjectsOfType<BerryBush>();
                Double closestBerryBushDistance = double.MaxValue;
                foreach (BerryBush berryBush in allBerryBushes)
                {
                    distance = (Math.Sqrt(Math.Pow((berryBush.transform.position.x) - (Player.transform.position.x), 2) + Math.Sqrt(Math.Pow((berryBush.transform.position.z) - (Player.transform.position.z), 2))));
                    if (closestBerryBushDistance > distance)
                    {
                        closestTreeDistance = distance;
                        ClosestBerryBush = berryBush;
                        ClosestRunestonePositionString = berryBush.transform.position.ToString();
                    }
                }
                FishingSpot[] allFishingSpots = FindObjectsOfType<FishingSpot>();
                Double closestFishingSpotDistance = double.MaxValue;
                foreach (FishingSpot fishingSpot in allFishingSpots)
                {
                    distance = (Math.Sqrt(Math.Pow((fishingSpot.transform.position.x) - (Player.transform.position.x), 2) + Math.Sqrt(Math.Pow((fishingSpot.transform.position.z) - (Player.transform.position.z), 2))));
                    if (closestFishingSpotDistance > distance)
                    {
                        closestFishingSpotDistance = distance;
                        ClosestFishingSpot = fishingSpot;
                        ClosestFishingSpotPositionString = fishingSpot.transform.position.ToString();
                    }
                }
                Runestone[] allRunestone = FindObjectsOfType<Runestone>();
                Double closestRunestoneDistance = double.MaxValue;
                foreach (Runestone runestone in allRunestone)
                {
                    distance = (Math.Sqrt(Math.Pow((runestone.transform.position.x) - (Player.transform.position.x), 2) + Math.Sqrt(Math.Pow((runestone.transform.position.z) - (Player.transform.position.z), 2))));
                    if (closestRunestoneDistance > distance)
                    {
                        closestRunestoneDistance = distance;
                        ClosestRunestone = runestone;
                        ClosestRunestonePositionString = runestone.transform.position.ToString();
                    }
                }
            }
        }

        public void TryGathering()
        {
            
        }
    }
}