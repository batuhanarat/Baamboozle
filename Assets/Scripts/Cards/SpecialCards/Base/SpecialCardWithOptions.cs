using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SpecialCardWithOptions : SpecialCardBase
{
    [SerializeField] public List<Button> optionButtons;

    public abstract string GetChosenTeamName();

    public abstract void Execute(string option);

}