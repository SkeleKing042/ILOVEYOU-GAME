using TMPro;
using UnityEngine;

namespace ILOVEYOU.MainMenu
{
    public class VersionNumber : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<TextMeshProUGUI>().text = "Version " + Application.version;
        }
    }
}