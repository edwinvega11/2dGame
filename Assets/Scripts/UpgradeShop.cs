using UnityEngine;
using UnityEngine.UI;

// choose one upgrade between waves
public class UpgradeShop : MonoBehaviour
{
    public WaveManager waves;
    public PlayerShooting shooting;
    public PlayerHealth player;
    public Text soulsText;
    public Button damageButton;
    public Button fireRateButton;
    public Button healthButton;

    public int Souls { get; private set; }
    public int DamageLevel { get; private set; }
    public int FireRateLevel { get; private set; }
    public int HealthLevel { get; private set; }

    void Start()
    {
        RefreshDisplay();
    }

    public void AddSoul()
    {
        Souls++;
        RefreshDisplay();
    }

    bool CanChoose()
    {
        return waves.waveClearPanel.activeSelf && !waves.WaveActive && !waves.HasWon && !player.IsDead;
    }

    public void ChooseDamage()
    {
        if (!CanChoose()) return;
        DamageLevel++;
        GameAudio.Instance?.Upgrade();
        shooting.damage++;
        waves.StartNextWave();
    }

    public void ChooseFireRate()
    {
        if (!CanChoose()) return;
        FireRateLevel++;
        GameAudio.Instance?.Upgrade();
        shooting.secondsBetweenShots *= 0.8f;
        waves.StartNextWave();
    }

    public void ChooseLife()
    {
        if (!CanChoose()) return;
        HealthLevel++;
        GameAudio.Instance?.Upgrade();
        player.maxHealth += 2;
        player.HealFully();
        waves.StartNextWave();
    }

    public void RefreshDisplay()
    {
        soulsText.text = "SOULS: " + Souls;
        damageButton.interactable = CanChoose();
        fireRateButton.interactable = CanChoose();
        healthButton.interactable = CanChoose();
    }
}
