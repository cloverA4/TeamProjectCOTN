using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    ItemType _nowWeapon = ItemType.Weapon; // Data에 있는 Weapon
    WeaponType _nowEquip = WeaponType.ShortSword; // 기본장비는 ShortSword를 사용하므로 ShortSword로 정의


    ShortSword _shortSwordSetting = new ShortSword();
    GreatSword _greatSwordSetting = new GreatSword();
    Spear _spearSetting = new Spear();


    public static WeaponManager Instance
    {
        get
        {
            if (null == Instance)
            {
                return null;
            }
            return Instance;
        }
    }

    public ItemType NowWeapon // 먹은아이템의 타입이 장비라면? 이해 x
    {
        get { return _nowWeapon; }
        set { _nowWeapon = value; }
    }

    public WeaponType NowEquip // 현재장비한 타입
    {
        get { return _nowEquip; }
        set { _nowEquip = value; }
    }

    //public void WeaponAbility()
    //{
    //    switch (_nowWeapon)
    //    {
    //        case ItemType.Weapon:

    //            switch (_nowEquip)
    //            {
    //                case WeaponType.ShortSword:
    //                    break;
    //                case WeaponType.GreatSword:

    //                    break;
    //                case WeaponType.Spear:
    //                    _spearSetting.SpearAttackRange();
    //                    break;
    //            }
    //            break;
    //    }
    //}
}
