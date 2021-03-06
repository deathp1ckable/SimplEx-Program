﻿using SimplExServer.Builders;
using SimplExServer.Common;
using SimplExServer.View;

namespace SimplExServer.Presenter
{
    class EditFiveStepMarkSystemPresenter : IntegrablePresenter<MarkSystemBuilder, IEditFiveStepMarkSystemView>, IEditMarkSystemPresenter
    {
        FiveStepMarkSystemBuilder fiveStepMarkSystem;
        public MarkSystemBuilder MarkSystemBuilder { get => fiveStepMarkSystem; set => fiveStepMarkSystem = (FiveStepMarkSystemBuilder)value; }
        public new IEditMarkSystemView View => base.View;
        public EditFiveStepMarkSystemPresenter(IEditFiveStepMarkSystemView view, IApplicationController applicationController) : base(view, applicationController)
        {
            View.SaveChanges += SaveChanges;
            View.Changed += Changed;
            View.CancelChanges += CancelChanges;
        }
        private void SaveChanges(IEditMarkSystemView sender)
        { 
            IEditFiveStepMarkSystemView editor =  (IEditFiveStepMarkSystemView)sender;
            fiveStepMarkSystem.FivePercent = editor.FivePercent;
            fiveStepMarkSystem.FourPercent = editor.FourPercent;
            fiveStepMarkSystem.ThreePercent = editor.ThreePercent;
            fiveStepMarkSystem.TwoPercent = editor.TwoPercent;
            fiveStepMarkSystem.OnePercent = editor.OnePercent;
            editor.Saved = true;
        }
        private void CancelChanges(IEditMarkSystemView sender)
        {
            IEditFiveStepMarkSystemView editor = (IEditFiveStepMarkSystemView)sender;
            editor.FivePercent = fiveStepMarkSystem.FivePercent;
            editor.FourPercent = fiveStepMarkSystem.FourPercent;
            editor.ThreePercent = fiveStepMarkSystem.ThreePercent;
            editor.TwoPercent = fiveStepMarkSystem.TwoPercent;
            editor.OnePercent = fiveStepMarkSystem.OnePercent;
            editor.Saved = true;
        }
        private void Changed(IEditMarkSystemView sender) => sender.Saved = false;
        public override void Run(MarkSystemBuilder argument)
        {
            MarkSystemBuilder = argument;
            base.View.FivePercent = fiveStepMarkSystem.FivePercent;
            base.View.FourPercent = fiveStepMarkSystem.FourPercent;
            base.View.ThreePercent = fiveStepMarkSystem.ThreePercent;
            base.View.TwoPercent = fiveStepMarkSystem.TwoPercent;
            base.View.OnePercent = fiveStepMarkSystem.OnePercent;
            base.View.Saved = true;
        }
    }
}
