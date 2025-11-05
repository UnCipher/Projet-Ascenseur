using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InfoCompteur", menuName = "ScriptableObject/InfoCompteur")]
public class InfoCompteur : ScriptableObject
{
    public int compteur;

    public int nbVie = 3;
}