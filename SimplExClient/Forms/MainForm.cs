﻿using SimplExClient.View;
using SimplExModel.Model;
using SimplExModel.NetworkData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SimplExClient.Forms
{
    public partial class MainForm : Form, IMainView
    {
        TimeSpan timeSpan;
        private int qustionsCount;
        private bool closingByPresenter;

        private Button disabledButton;
        private IList<Theme> themes;
        private ClientStatus clientStatus;
        private Ticket ticket;
        private ISessionInformationView sessionInformationView;
        private IChatView chatView;

        public event ViewActionHandler<IMainView> Disconnected;
        public event ViewActionHandler<IMainView> QuestionChanged;
        public event ViewActionHandler<IMainView> GoToExecution;

        public double Time { get => timeSpan.TotalSeconds; set => timeSpan = TimeSpan.FromSeconds(value); }
        public IList<Theme> Themes
        {
            get => themes;
            set
            {
                themes = value;
                tree.Nodes["Themes"].Nodes.Clear();
                for (int i = 0; i < themes.Count; i++)
                    tree.Nodes["Themes"].Nodes.Add(LoadTheme(themes[i]));
            }
        }
        public Ticket Ticket
        {
            get => ticket;
            set
            {
                ticket = value;
                qustionsCount = ticket.GetQuestions().Length;
                tree.Nodes["Ticket"].Nodes.Clear();
                tree.Nodes["Ticket"].Nodes.Add(LoadTicket(ticket));
                executedLabel.Text = $"Выполнено вопросов: 0/{qustionsCount}";
            }
        }
        public string GroupName { get => groupLabel.Text; set => groupLabel.Text = "Группа: " + value; }
        public ClientStatus ClientStatus
        {
            get => clientStatus; set
            {
                clientStatus = value;
                switch (value)
                {
                    case ClientStatus.Connected:
                        statusLabel.Text = "Статус: Ожидание начала сессии";
                        break;
                    case ClientStatus.Executing:
                        statusLabel.Text = "Статус: Выполнение";
                        break;
                    case ClientStatus.Reconnecting:
                        statusLabel.Text = "Статус: Переподключение";
                        break;
                    case ClientStatus.Executed:
                        statusLabel.Text = "Статус: Выполнено";
                        break;
                }
            }
        }

        public ISessionInformationView SessionInformationView
        {
            get => sessionInformationView; set
            {
                sessionInformationView = value;
                UserControl control = (UserControl)sessionInformationView;
                control.Parent = propertiesPanel;
                control.Size = propertiesPanel.Size;
                control.Location = new Point(0, 0);
                control.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            }
        }

        public IChatView ChatView
        {
            get => chatView; set
            {
                chatView = value;
                UserControl control = (UserControl)chatView;
                control.Parent = propertiesPanel;
                control.Size = propertiesPanel.Size;
                control.Location = new Point(0, 0);
                control.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            }
        }

        public MainForm()
        {
            InitializeComponent();
            disabledButton = sesionInfoButton;
        }
        public new void Show()
        {
            ShowDialog();
        }
        public new void Close()
        {
            closingByPresenter = true;
            base.Close();
        }
        public void ShowChatToolTip()
        {
            Control temp = ActiveControl;
            additionButton.Focus();
            Rectangle rectangle = additionButton.ClientRectangle;
            chatToolTip.Show("Прочитайте новое сообщение в чате", additionButton, rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2, 3000);
            temp.Focus();
        }
        public void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public void Invoke(Action action)
        {
            try
            {
                base.Invoke(action);
            }
            catch { }
        }

        private void TabStopClick(object sender, EventArgs e)
        {
            Button senderButton = (Button)sender;

            disabledButton.BackColor = Color.FromArgb(171, 31, 47);
            senderButton.BackColor = SystemColors.Control;

            disabledButton.Enabled = true;
            senderButton.Enabled = false;
            disabledButton = senderButton;
            switch (int.Parse(senderButton.Tag.ToString()))
            {
                case 0:
                    HideAllProperties();
                    SessionInformationView?.Show();
                    break;
                case 1:
                    HideAllProperties();
                    break;
                case 2:
                    HideAllProperties();
                    ChatView?.Show();
                    break;
            }
        }
        private void DisconnectButtonClick(object sender, EventArgs e)
        {
            AskForClosing();
        }

        private void MainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closingByPresenter)
            {
                AskForClosing();
            }
            closingByPresenter = false;
        }

        private void AskForClosing()
        {
            DialogResult dialogResult;
            if (ClientStatus == ClientStatus.Connected)
            {
                dialogResult = MessageBox.Show("Отключится от сессии?", "Отключится от сессии", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            else
            {
                dialogResult = MessageBox.Show("Сдать работу?", "Отключится от сессии", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            if (dialogResult == DialogResult.Yes)
                Disconnected?.Invoke(this);
        }

        private void HideAllProperties()
        {
            SessionInformationView?.Hide();
            ChatView.Hide();
        }
        private TreeNode LoadTheme(Theme theme)
        {
            TreeNode result = new TreeNode($"Тема '{theme.ThemeName}'");
            result.Tag = theme;
            return result;
        }
        private TreeNode LoadTicket(Ticket ticket)
        {
            TreeNode result = new TreeNode($"Билет '{ticket.TicketName}'");
            result.Tag = ticket;
            for (int i = 0; i < ticket.QuestionGroups.Count; i++)
                result.Nodes.Add(LoadQuestionGroup(ticket.QuestionGroups[i]));
            return result;
        }
        private TreeNode LoadQuestionGroup(QuestionGroup group)
        {
            TreeNode result = new TreeNode($"Группа '{group.QuestionGroupName}'");
            result.Tag = group;
            int i;
            for (i = 0; i < group.ChildQuestionGroups.Count; i++)
                result.Nodes.Add(LoadQuestionGroup(group.ChildQuestionGroups[i]));
            for (i = 0; i < group.Questions.Count; i++)
                result.Nodes.Add(LoadQuestion(group.Questions[i]));
            return result;
        }
        private TreeNode LoadQuestion(Question questionBuilder)
        {
            TreeNode result = new TreeNode($"Вопрос №{1 + questionBuilder.QuestionNumber}");
            result.Tag = questionBuilder;
            return result;
        }

        public void WarnAboutStart()
        {
            timeLabel.Text = $"Осталось времени: {timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            timer.Start();
            MessageBox.Show("Можете приступать к заданиям.", "Время выполнения началось", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TimerTick(object sender, EventArgs e)
        {
            timeSpan = TimeSpan.FromSeconds(timeSpan.TotalSeconds - 1);
            if (timeSpan.TotalSeconds >= 0)
            {
                timeLabel.Text = $"Осталось времени: {timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
                timer.Start();
            }
            else
                timeLabel.Text = $"Осталось времени: 00:00:00";
        }
    }
}
