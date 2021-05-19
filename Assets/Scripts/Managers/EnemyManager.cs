using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int EnemigosMuertos { get; private set; }

    public void EnemigoMurio()
    {
        EnemigosMuertos++;
    }
}