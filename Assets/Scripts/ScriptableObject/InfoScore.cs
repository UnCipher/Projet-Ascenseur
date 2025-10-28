using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InfoScore", menuName = "ScriptableObject/InfoScore")]
public class InfoScore : ScriptableObject
{
    public int score;
    public int vague;

    public int ennemisRestants;
}