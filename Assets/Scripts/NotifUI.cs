using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace CapedHorse.BallBattle
{
    public class NotifUI : MonoBehaviour
    {
        public static NotifUI instance;
        public enum NotifType { EnergyLow, PlayerScore, EnemyScore, Draw, PlayerWon, EnemyWon, SwitchSide, PenaltyTime }

        public RectTransform EnergyLowUI;
        public RectTransform PlayerScoreUI;
        public RectTransform EnemyScoreUI;
        public RectTransform DrawUI;
        public RectTransform PlayerWonUI;
        public RectTransform EnemyWonUI;
        public RectTransform SwitchSideUI;
        public RectTransform PenaltyTimeUI;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        public void Notify(NotifType type, UnityAction afterNotify = null)
        {
            switch (type)
            {
                case NotifType.EnergyLow:
                    TweenNotif(EnergyLowUI, afterNotify);
                    break;
                case NotifType.PlayerScore:
                    TweenNotif(PlayerScoreUI, afterNotify);
                    break;
                case NotifType.EnemyScore:
                    TweenNotif(EnemyScoreUI, afterNotify);
                    break;
                case NotifType.Draw:
                    TweenNotif(DrawUI, afterNotify);
                    break;
                case NotifType.PlayerWon:
                    TweenNotif(PlayerWonUI, afterNotify);
                    break;
                case NotifType.EnemyWon:
                    TweenNotif(EnemyWonUI, afterNotify);
                    break;
                case NotifType.SwitchSide:
                    TweenNotif(SwitchSideUI, afterNotify);
                    break;
                case NotifType.PenaltyTime:
                    TweenNotif(PenaltyTimeUI, afterNotify);
                    break;
                default:
                    break;
            }
        }

        public void TweenNotif(RectTransform tweenedNotif, UnityAction afterNotif = null)
        {
            AudioManager.instance.PlaySFX("PopUp");
            Sequence seq = DOTween.Sequence();
            seq.Append(tweenedNotif.DOScale(0, 0)).AppendCallback(() => tweenedNotif.gameObject.SetActive(true));
            seq.Append(tweenedNotif.DOScale(1, 0.25f)).AppendCallback(() => afterNotif?.Invoke());
            seq.AppendInterval(2);
            seq.Append(tweenedNotif.DOScale(0, 0.25f)).AppendCallback(() =>
            {
                tweenedNotif.localScale = Vector3.one;
                tweenedNotif.gameObject.SetActive(false);
            });


        }
    }
}

