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

        public void Notify(NotifType type, float duration = 2)
        {
            switch (type)
            {
                case NotifType.EnergyLow:
                    TweenNotif(EnergyLowUI, duration);
                    break;
                case NotifType.PlayerScore:
                    TweenNotif(PlayerScoreUI, duration);
                    break;
                case NotifType.EnemyScore:
                    TweenNotif(EnemyScoreUI, duration);
                    break;
                case NotifType.Draw:
                    TweenNotif(DrawUI, duration);
                    break;
                case NotifType.PlayerWon:
                    TweenNotif(PlayerWonUI, duration);
                    break;
                case NotifType.EnemyWon:
                    TweenNotif(EnemyWonUI, duration);
                    break;
                case NotifType.SwitchSide:
                    TweenNotif(SwitchSideUI, duration);
                    break;
                case NotifType.PenaltyTime:
                    TweenNotif(PenaltyTimeUI, duration);
                    break;
                default:
                    break;
            }
        }

        public void TweenNotif(RectTransform tweenedNotif, float duration = 2)
        {
            AudioManager.instance.PlaySFX("PopUp");
            Sequence seq = DOTween.Sequence();
            seq.Append(tweenedNotif.DOScale(0, 0)).AppendCallback(() => tweenedNotif.gameObject.SetActive(true));
            seq.Append(tweenedNotif.DOScale(1, 0.25f));
            seq.AppendInterval(duration);
            seq.Append(tweenedNotif.DOScale(0, 0.25f)).AppendCallback(() =>
            {
                tweenedNotif.localScale = Vector3.one;
                tweenedNotif.gameObject.SetActive(false);
            });


        }
    }
}

