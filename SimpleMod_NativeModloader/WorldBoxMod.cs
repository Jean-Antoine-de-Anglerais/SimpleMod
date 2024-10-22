using UnityEngine;

namespace SimpleMod_NativeModloader
{
    public class WorldBoxMod : MonoBehaviour
    {
        static bool initialized = false;

        void Update()
        {
            if (!Config.gameLoaded) return;


            if (!initialized)
            {
                init();
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                MapBox.instance.selectedButtons.selectedButton.godPower = AssetManager.powers.get("super_skeleton");
            }

            initialized = true;
        }

        void init()
        {
            SimpleMod simpleMod = new SimpleMod();

            GodPower power = AssetManager.powers.clone("super_skeleton", SA.skeleton);
            power.name = "Super Skeleton";
            power.actor_asset_id = "super_skeleton";
            power.click_action = new PowerActionWithID(AssetManager.powers.spawnUnit);

            LocalizedTextManager.instance.localizedText.Add(power.name, power.name);
        }
    }
}