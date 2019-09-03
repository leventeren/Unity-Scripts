using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Flyweight pattern main classı
namespace FlyweightPattern
{
    public class Flyweight : MonoBehaviour
    {
        //Tüm uzaylıları tutan liste
        List<Alien> allAliens = new List<Alien>();

        List<Vector3> eyePositions;
        List<Vector3> legPositions;
        List<Vector3> armPositions;


        void Start()
        {
            //Flyweight etkinleştirildiğinde kullanılan liste
            eyePositions = GetBodyPartPositions();
            legPositions = GetBodyPartPositions();
            armPositions = GetBodyPartPositions();
            
            //Tüm uzaylıları oluşturuyoruz
            for (int i = 0; i < 10000; i++)
            {
                Alien newAlien = new Alien();

                //göz ve bacak pozisyonları
                //Flyweight olmadan!!
                newAlien.eyePositions = GetBodyPartPositions();
                newAlien.armPositions = GetBodyPartPositions();
                newAlien.legPositions = GetBodyPartPositions();

                //Flyweight ile!!
                //newAlien.eyePositions = eyePositions;
                //newAlien.armPositions = legPositions;
                //newAlien.legPositions = armPositions;

                allAliens.Add(newAlien);
            }
        }


        //listeyı generate ediyoruz
        List<Vector3> GetBodyPartPositions(){//Yenı list oluşturuyoruz
            List<Vector3> bodyPartPositions = new List<Vector3>();

            //vücut parçalarını listeye ekliyoruz
            for (int i = 0; i < 1000; i++)
            {
                bodyPartPositions.Add(new Vector3());
            }

            return bodyPartPositions;
        }
    }
}
