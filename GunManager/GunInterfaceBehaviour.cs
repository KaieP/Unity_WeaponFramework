using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInterfaceBehaviour : MonoBehaviour
{
    private Queue<SoGun> _guns;
    private IGun _equippedGun;
    public event Action EFire;

    private void Awake()
    {
        _guns = new Queue<SoGun>();
    }

    private void Update()
    {
        if(_equippedGun == null) return;
        _equippedGun.Update();
    }
    
    #region Player Functions
    
    //Call these Functions from the player controller to use your weapons
    
    public void Shoot(Vector3 position, Vector3 direction)
    {
        if(_equippedGun == null) return;
        var fired = _equippedGun.TryShoot(ref position, ref direction);
        if(fired == null) return;
        FireBullet(fired._prefab, position, fired._bulletVelocity * direction);
    }

    public void Trigger(Vector3 position, Vector3 direction)
    {
        if(_equippedGun == null) return;
        var fired = _equippedGun.TryTrigger(ref position, ref direction);
        if(fired == null) return;
        FireBullet(fired._prefab, position, direction);
    }

    private GameObject FireBullet(GameObject bullet, Vector3 position, Vector3 velocity)
    { 
        var newBullet = Instantiate(bullet, position, Quaternion.LookRotation(velocity));
        newBullet.GetComponent<Rigidbody>().velocity = velocity;
        return newBullet;
    }

    public void Reload()
    {
        if(_equippedGun == null) return;
        _equippedGun.Reload();
    }
    
    public void Switch()
    {
        if (_guns.Count < 2) return;
        var newGun = _guns.Dequeue();
        _equippedGun = newGun.getGun();
        _equippedGun.Setup(this);
        _guns.Enqueue(newGun);
    }

    #endregion

    public void PickUp(SoGun newGun)
    {
        if(_guns.Contains(newGun)) return;
        
        _guns.Enqueue(newGun);
        
        _equippedGun = newGun.getGun();
        _equippedGun.Setup(this);
    }
}
