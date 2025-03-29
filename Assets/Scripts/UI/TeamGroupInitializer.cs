using UnityEngine;

public class TeamGroupInitializer : MonoBehaviour, IProvidable
{

    [SerializeField] private TeamNameUI TeamNamerPrefab;
    [SerializeField] private Transform parent;

    public void Awake()
    {
        ServiceProvider.Register(this);
    }

    public void Start()
    {
        int teamCount =GameSettings.GetTeamSize();
        for(int i = 1 ; i<= teamCount ; i++)
        {
            Instantiate(TeamNamerPrefab,parent);
            TeamNamerPrefab.SetName(i +".Takım ");
        }
    }





}