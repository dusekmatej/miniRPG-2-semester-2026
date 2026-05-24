namespace miniRPG.GameEngine.Core.Events;

public class EnemyWaveEvent
{
    public int WaveNumber { get; set; }

    public EnemyWaveEvent(int waveNumber)
    {
        WaveNumber = waveNumber;
    }
}

