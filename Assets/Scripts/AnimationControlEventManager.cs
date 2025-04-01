using UnityEngine;

public class AnimationControlEventManager : MonoBehaviour
{
   [SerializeField] private CardUI cardUI;

     public void AnimControlButton(){
        GetComponentInParent<CardUI>().ButtonDestroyed();
     }

     public void AnimControlPanel(){
        GetComponentInParent<CardUI>().PanelOpened();
     }

      public void AfterChest1Animation(){
         if (cardUI != null)
               cardUI.AfterChest1_Animation();
      }

      public void AfterChest3Animation(){
         if (cardUI != null)
               cardUI.AfterChest3_Animation();
      }
     public void AnimControlWrongOrTrue()
     {
      if( GetComponentInParent<MultipleQuestionPopUp>() != null )
      {
        GetComponentInParent<MultipleQuestionPopUp>().AfterOnOptionPressed();
      } else if (GetComponentInParent<TrueFalseQuestionPopUp>() != null)
      {
         GetComponentInParent<TrueFalseQuestionPopUp>().AfterOnOptionPressed();
      }

     }

}
