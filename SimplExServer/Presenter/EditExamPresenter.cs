﻿using SimplExServer.Builders;
using SimplExServer.Common;
using SimplExServer.View;
using System.Text.RegularExpressions;
namespace SimplExServer.Presenter
{
    class EditExamPresenter : IntegrablePresenter<ExamBuilder, IEditPropertiesView>
    {
        private static readonly Regex passwordRegex = new Regex("([a-zA-Z0-9]{4,})$");
        public EditExamPresenter(IEditPropertiesView view, IApplicationController applicationController) : base(view, applicationController)
        {
            view.ChangesCanceled += CancelProperties;
            view.ChangesSaved += SaveProperties;
            view.Changed += PropertiesChanged;
        }
        public override void Run(ExamBuilder argument)
        {
            Argument = argument;
            View.ExamName = Argument.ExamName;
            View.Discipline = Argument.Discipline;
            View.Password = Argument.Password;
            View.CreatorName = Argument.CreatorName;
            View.CreatorSurname = Argument.CreatorSurname;
            View.CreatorPatronymic = Argument.CreatorPatronymic;
            View.ExaminationTime = Argument.ExaminationTime;
            View.FirstNumber = Argument.FirstNumber;
            View.Description = Argument.Description;
            View.RepeatPassword = Argument.Password;
            View.IsSaved = true;
        }
        private void PropertiesChanged(IEditPropertiesView sender)
        {
            sender.IsSaved = false;
        }
        private void SaveProperties(IEditPropertiesView sender)
        {
            if (string.IsNullOrEmpty(sender.ExamName.Trim()))
            {
                sender.MessageWrongExamName("Введите название теста.");
                return;
            }
            if (string.IsNullOrEmpty(sender.Discipline.Trim()))
            {
                sender.MessageWrongDiscipline("Введите дисциплину.");
                return;
            }
            if (string.IsNullOrEmpty(sender.CreatorName.Trim()))
            {
                sender.MessageWrongCreatorName("Введите имя автора.");
                return;
            }
            if (string.IsNullOrEmpty(sender.CreatorSurname.Trim()))
            {
                sender.MessageWrongCreatorSurname("Введите фамилию автора.");
                return;
            }
            if (string.IsNullOrEmpty(sender.CreatorPatronymic.Trim()))
            {
                sender.MessageWrongCreatorPatronymic("Введите отчество автора.");
                return;
            }
            if (sender.Password != null && string.IsNullOrEmpty(sender.Password.Trim()))
            {
                if (!passwordRegex.IsMatch(sender.Password))
                {
                    sender.MessageWrongPassword("Пароль должен быть длиннее 3 символов и состоять из латиницы и цифр.");
                    return;
                }
                if (View.Password != sender.RepeatPassword)
                {
                    sender.MessageWrongRepeat("Пароли не совпадают.");
                    return;
                }
            }
            Argument.ExamName = sender.ExamName.Trim();
            Argument.Discipline = sender.Discipline.Trim();
            Argument.Password = sender.Password.Trim();
            Argument.CreatorName = sender.CreatorName.Trim();
            Argument.CreatorSurname = sender.CreatorSurname.Trim();
            Argument.CreatorPatronymic = sender.CreatorPatronymic.Trim();
            Argument.ExaminationTime = sender.ExaminationTime;
            Argument.FirstNumber = sender.FirstNumber;
            Argument.Description = sender.Description.Trim();

            RefreshView(sender);

            View.IsSaved = true;
        }
        private void CancelProperties(IEditPropertiesView sender)
        {
            RefreshView(sender);
            if (string.IsNullOrEmpty(sender.ExamName.Trim()))
            {
                sender.MessageWrongExamName("Введите название теста.");
                return;
            }
            if (string.IsNullOrEmpty(sender.Discipline.Trim()))
            {
                sender.MessageWrongDiscipline("Введите дисциплину.");
                return;
            }
            if (string.IsNullOrEmpty(sender.CreatorName.Trim()))
            {
                sender.MessageWrongCreatorName("Введите имя автора.");
                return;
            }
            if (string.IsNullOrEmpty(sender.CreatorSurname.Trim()))
            {
                sender.MessageWrongCreatorSurname("Введите фамилию автора.");
                return;
            }
            if (string.IsNullOrEmpty(sender.CreatorPatronymic.Trim()))
            {
                sender.MessageWrongCreatorPatronymic("Введите отчество автора.");
                return;
            }
            if (sender.Password != null)
            {
                if (!passwordRegex.IsMatch(sender.Password))
                {
                    sender.MessageWrongPassword("Пароль должен быть длиннее 3 символов и состоять из латиницы и цифр.");
                    return;
                }
                if (sender.Password != sender.RepeatPassword)
                {
                    sender.MessageWrongRepeat("Пароли не совпадают.");
                    return;
                }
            }
            sender.IsSaved = true;
        }

        private void RefreshView(IEditPropertiesView sender)
        {
            sender.ExamName = Argument.ExamName;
            sender.Description = Argument.Discipline;
            sender.Password = Argument.Password;
            sender.CreatorName = Argument.CreatorName;
            sender.CreatorSurname = Argument.CreatorSurname;
            sender.CreatorPatronymic = Argument.CreatorPatronymic;
            sender.ExaminationTime = Argument.ExaminationTime;
            sender.FirstNumber = Argument.FirstNumber;
            sender.Description = Argument.Description;
        }
    }
}
