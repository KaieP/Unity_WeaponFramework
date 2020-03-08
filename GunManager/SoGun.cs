using System.Collections;
using System.Collections.Generic;
using GunCollection;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun")]
public class SoGun : ScriptableObject
{
    private enum gunKey
    {
        StandardGun,
        MiniGun,
        RocketLauncher
    }
    
    [SerializeField] private gunKey gunType;
    
    public Sprite icon;
    public GameObject model;
    public GameObject bullet;

    private Dictionary<gunKey, IGun> guns;

    public IGun getGun()
    {
        switch (gunType)
        {
            case gunKey.RocketLauncher:
                return  new RocketLauncher(bullet, this);
            case gunKey.MiniGun:
                return new MiniGun(bullet, this);
            case gunKey.StandardGun:
                return  new StandardGun(bullet, this);
            default:
                return new StandardGun(bullet, this);
        }
    }
}
