using System.Text;
using GrenadeThrower.Grenades;
using TMPro;
using UnityEngine;

namespace GrenadeThrower.UI
{
    public class GrenadeUIScript : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text textComponent;

        [SerializeField]
        private GrenadeInventoryScript inventory;

        private void Update()
        {
            var stringBuilder = new StringBuilder();

            foreach (var itemStack in inventory.Inventory)
            {
                if (itemStack.Item == inventory.SelectedGrenade)
                {
                    stringBuilder.Append(">");
                }

                stringBuilder.Append(itemStack.Item.displayedName);
                stringBuilder.Append(" : ");
                stringBuilder.Append(itemStack.Count);
                stringBuilder.Append("\n");
            }

            textComponent.text = stringBuilder.ToString();
        }
    }
}
