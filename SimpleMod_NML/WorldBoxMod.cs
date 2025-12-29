using ai.behaviours;
using System.Linq;
using UnityEngine;
using NeoModLoader.api;

namespace SimpleMod_NML
{
    public class WorldBoxMod : BasicMod<WorldBoxMod>
    {
        protected override void OnModLoad()
        {
            InitAssets();
            InitUI();
        }

        void InitAssets()
        {
            //SimpleMod simpleMod = new SimpleMod();

            var projectileAsset = AssetManager.projectiles.clone("skeleton_arrow", "arrow");
            projectileAsset.trail_effect_enabled = true;
            projectileAsset.texture = "fireball";
            projectileAsset.scale_target = 0.5f;

            var equipmentAsset = AssetManager.items.clone("skeleton_bow", "$range"); // в оригинальном коде тут !range, но в новой версии игры это не работает
            equipmentAsset.base_stats["range"] = 22f;
            equipmentAsset.base_stats["critical_chance"] = 0.1f;
            equipmentAsset.base_stats["critical_damage_multiplier"] = 0.5f;
            equipmentAsset.projectile = "skeleton_arrow";

            var actorAsset = AssetManager.actor_library.clone("super_skeleton", "skeleton");
            actorAsset.default_attack = "skeleton_bow";
            actorAsset.default_weapons = null;
            actorAsset.base_stats["health"] = 100000f;
            actorAsset.base_stats["damage"] = 500f;
            actorAsset.base_stats["speed"] = 500f;
            actorAsset.job = Toolbox.a<string>(new string[] { "super_skeleton_job" });
            AssetManager.actor_library.addTrait("regeneration");
            AssetManager.actor_library.addTrait("immortal");

            var actorJob = AssetManager.job_actor.add(new ActorJob
            {
                id = "super_skeleton_job"
            });
            actorJob.addTask("mod_destroy_trees");
            actorJob.addTask("random_move");
            actorJob.addTask("wait");
            actorJob.addTask("attack_golden_brain");

            var behaviourTaskActor = AssetManager.tasks_actor.add(new BehaviourTaskActor
            {
                id = "mod_destroy_trees"
            });
            behaviourTaskActor.addBeh(new BehFindBuilding("type_tree", true, true));
            behaviourTaskActor.addBeh(new BehGoToBuildingTarget(false));
            behaviourTaskActor.addBeh(new BehResourceGatheringAnimation(1f, "", true));
            behaviourTaskActor.addBeh(new BehResourceGatheringAnimation(1f, "", true));
            behaviourTaskActor.addBeh(new BehResourceGatheringAnimation(1f, "", true));
            behaviourTaskActor.addBeh(new BehResourceGatheringAnimation(1f, "", true));
            behaviourTaskActor.addBeh(new BehExtractResourcesFromBuilding());
            behaviourTaskActor.addBeh(new BehRandomWait(1f, 2f, false));

            var power = AssetManager.powers.clone("super_skeleton", "$template_spawn_actor$");
            power.name = "Super Skeleton";
            power.actor_asset_id = "super_skeleton";
            power.path_icon = "iconSkeleton";

            LocalizedTextManager.instance._localized_text["super_skeleton"] = "Super Skeleton";
            LocalizedTextManager.instance._localized_text["super_skeleton_description"] = "Spawn a hidden Super Skeleton mob";
        }

        void InitUI()
        {
            var prefab = Resources.FindObjectsOfTypeAll<PowerButton>()
                .FirstOrDefault(b => b.name.Equals("inspect", System.StringComparison.OrdinalIgnoreCase));

            prefab.gameObject.SetActive(false);

            var button = Object.Instantiate(prefab, null);

            prefab.gameObject.SetActive(true);

            button.name = "super_skeleton";
            var icon = SpriteTextureLoader.getSprite("ui/Icons/iconSkeleton");
            button.icon.sprite = icon;
            button.icon.overrideSprite = icon;
            button.open_window_id = null;
            button.type = PowerButtonType.Active;
            button.transform.localPosition = default(Vector2);
            button.transform.localScale = Vector3.one;

            button.gameObject.SetActive(true);

            var tab = CanvasMain.instance.canvas_ui.GetComponentsInChildren<PowersTab>(true)
                .First(t => t.name == "units");

            button.transform.SetParent(tab.transform, false);
            tab._power_buttons.Add(button);
        }
    }
}
