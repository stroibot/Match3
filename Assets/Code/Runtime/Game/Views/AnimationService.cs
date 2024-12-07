using DG.Tweening;
using System.Collections.Generic;

namespace stroibot.Match3.Views
{
	public class AnimationService
	{
		private readonly Queue<Tween> _animationQueue = new();

		public bool IsAnimating { get; private set; }

		public void EnqueueAnimation(
			Tween animation)
		{
			_animationQueue.Enqueue(animation);
		}

		public void UpdateAnimations()
		{
			if (IsAnimating || _animationQueue.Count <= 0)
			{
				return;
			}

			IsAnimating = true;
			var tween = _animationQueue.Dequeue();
			tween.onComplete += () => { IsAnimating = false; };
			tween.Play();
		}
	}
}
