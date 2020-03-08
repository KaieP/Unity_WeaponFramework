using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public interface IGun
{
    void Update(); 
    void Setup(GunInterfaceBehaviour owner);
    BulletProps TryShoot(ref Vector3 position, ref Vector3 direction);
    BulletProps TryTrigger(ref Vector3 position, ref Vector3 direction);
    void Reload();

    GameObject GetBullet();

    SoGun GetSO();
}

public class BulletProps
{
    public readonly bool _tryResult;
    public readonly GameObject _prefab;
    public readonly float _bulletVelocity;

    public BulletProps(GameObject bullet, float velocity)
    {
        _tryResult = true;
        _prefab = bullet;
        _bulletVelocity = velocity;
    }
}

namespace GunCollection
{
    //Example Guns
    public class StandardGun : IGun
{
    private GunInterfaceBehaviour _owner;
    
    private float _bulletSpeed = 36.0f;
    private float _fireRate = 3.5f;

    private float _shootCounter;
    
    private readonly GameObject _bulletPrefab;
    private readonly SoGun _type;

    public StandardGun(GameObject bulletPrefab, SoGun type)
    {
        _bulletPrefab = bulletPrefab;
        _type = type;
    }

    public SoGun GetSO()
    {
        return _type;
    }
    
    public GameObject GetBullet()
    {
        return _bulletPrefab;
    }

    public void Update()
    {
        if (_shootCounter >= 0)
        {
            _shootCounter -= Time.deltaTime;
        }
    }
    public void Setup(GunInterfaceBehaviour owner)
    {
        _owner = owner;
    }

    public BulletProps TryShoot(ref Vector3 position, ref Vector3 direction)
    {
        var timeBetweenShots = 1f / _fireRate;
        if (_shootCounter <= 0)
        {
            _shootCounter += timeBetweenShots;
            return new BulletProps(_bulletPrefab, _bulletSpeed);
        }

        return null;
    }

    public BulletProps TryTrigger(ref Vector3 position, ref Vector3 direction)
    {
        return null;
    }

    public void Reload()
    {
        //WIP
    }
}

    public class RocketLauncher : IGun
    {
        private GunInterfaceBehaviour _owner;
    
        private float _bulletSpeed = 30.0f;
        private float _fireRate = 1f;

        private float _shootCounter;

        private readonly GameObject _bulletPrefab;
        private readonly SoGun _type;

        public RocketLauncher(GameObject bulletPrefab, SoGun type)
        {
            _bulletPrefab = bulletPrefab;
            _type = type;
        }
    
        public GameObject GetBullet()
        {
            return _bulletPrefab;
        }
        
        public SoGun GetSO()
        {
            return _type;
        }
        
        public void Update()
        {
            if (_shootCounter >= 0)
            {
                _shootCounter -= Time.deltaTime;
            }
        }

        public void Setup(GunInterfaceBehaviour owner)
        {
            _owner = owner;
        }

        public BulletProps TryShoot(ref Vector3 position, ref Vector3 direction)
        {
            return null;
        }

        public BulletProps TryTrigger(ref Vector3 position, ref Vector3 direction)
        {
            var timeBetweenShots = 1f / _fireRate;
            if (_shootCounter <= 0)
            {
                _shootCounter += timeBetweenShots;
                return new BulletProps(_bulletPrefab, _bulletSpeed);
            }

            return null;
        }

        public void Reload()
        {
            //WIP
        }
    }

    public class MiniGun : IGun
    {
        private GunInterfaceBehaviour _owner;
        
        private float _bulletSpeed = 32.0f;
        
        private float _baseFireRate = 4.0f;
        private float _maxFireRate = 10.0f;
        private float _fireRate = 1.0f;

        private float _baseSpreadAngle = 4.0f;
        private float _maxSpreadAngle = 15.0f;
        private float _spreadAngle = 4.0f;
        
        private float _secToSpinUp = 4.5f;
        private float _secToSpinDown = 3.0f;

        private float _shootCounter;

        private readonly GameObject _bulletPrefab;
        private readonly SoGun _type;

        public MiniGun(GameObject bulletPrefab, SoGun type)
        {
            _bulletPrefab = bulletPrefab;
            _type = type;
        }
        
        public GameObject GetBullet()
        {
            return _bulletPrefab;
        }
        
        public SoGun GetSO()
        {
            return _type;
        }
        
        public void Update()
        {
            if (_shootCounter >= 0)
            {
                _shootCounter -= Time.deltaTime;
            }

            if (_fireRate > _baseFireRate)
            {
                _fireRate -= (_maxFireRate - _baseFireRate) * (Time.deltaTime / _secToSpinDown);
                if (_fireRate < _baseFireRate)
                {
                    _fireRate = _baseFireRate;
                }
            }

            if (_spreadAngle > _baseSpreadAngle)
            {
                _spreadAngle -= (_maxSpreadAngle - _baseSpreadAngle) * (Time.deltaTime / _secToSpinDown);
                if (_spreadAngle < _baseSpreadAngle)
                {
                    _spreadAngle = _baseSpreadAngle;
                }
            }
        }

        public void Setup(GunInterfaceBehaviour owner)
        {
            _owner = owner;
            _fireRate = _baseFireRate;
            _spreadAngle = _baseSpreadAngle;
        }

        public BulletProps TryShoot(ref Vector3 position, ref Vector3 direction)
        {
            if (_fireRate < _maxFireRate)
            {
                _fireRate += (_maxFireRate - _baseFireRate) * (Time.deltaTime / _secToSpinDown);
                _fireRate += (_maxFireRate - _baseFireRate) * (Time.deltaTime / _secToSpinUp);
                if (_fireRate > _maxFireRate)
                {
                    _fireRate = _maxFireRate;
                }
            }

            if (_spreadAngle < _maxSpreadAngle)
            {
                _spreadAngle += (_maxSpreadAngle - _baseSpreadAngle) * (Time.deltaTime / _secToSpinDown);
                _spreadAngle += (_maxSpreadAngle - _baseSpreadAngle) * (Time.deltaTime / _secToSpinUp);
                if (_spreadAngle > _maxSpreadAngle)
                {
                    _spreadAngle = _maxSpreadAngle;
                }
            }

            var timeBetweenShots = 1f / _fireRate;
            if (_shootCounter <= 0)
            {
                _shootCounter += timeBetweenShots;
                direction = Quaternion.AngleAxis(Random.Range(-_spreadAngle * 0.5f, _spreadAngle * 0.5f), Vector3.up) * direction;
                return new BulletProps(_bulletPrefab, _bulletSpeed);
            }

            return null;
        }

        public BulletProps TryTrigger(ref Vector3 position, ref Vector3 direction)
        {
            return null;
        }

        public void Reload()
        {
            //WIP
        }
    }
}

