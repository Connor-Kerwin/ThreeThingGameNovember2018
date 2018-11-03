using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image currentHealthBar;
    public Text ratioText;

    private float currenthitpoint = 100;
    private float maxHitPoint = 100;

    private void Start()
    {
        
    }
    private void Update()
    {
        Main main = Main.Instance;
        ManagerStore managerstore = main.ManagerStore;
        PlayerManager playermanager = managerstore.Get<PlayerManager>();
        Health hitpoint = playermanager.Health;
        currenthitpoint = (hitpoint.CurrentHealth / hitpoint.MaxHealth);

        currentHealthBar.rectTransform.localScale = new Vector3(currenthitpoint, 1, 1);
        ratioText.text = (currenthitpoint * 100).ToString() + "%";

    }

}
