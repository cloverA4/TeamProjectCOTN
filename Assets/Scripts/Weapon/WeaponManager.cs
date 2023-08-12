using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    ItemType _nowWeapon = ItemType.Weapon; // Data�� �ִ� Weapon
    WeaponType _nowEquip = WeaponType.ShortSword; // �⺻���� ShortSword�� ����ϹǷ� ShortSword�� ����


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

    public ItemType NowWeapon // ������������ Ÿ���� �����? ���� x
    {
        get { return _nowWeapon; }
        set { _nowWeapon = value; }
    }

    public WeaponType NowEquip // ��������� Ÿ��
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
