﻿using SimplExServer.View;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SimplExServer.Controls
{
    public partial class ChatControl : UserControl, IChatView
    {
        private IList<string> messages;

        public IList<string> Messages
        {
            get => messages;
            set
            {
                if (EnableChat)
                {
                    messages = value;
                    chatBox.Text = string.Empty;
                    for (int i = 0; i < messages.Count; i++)
                        chatBox.Text += messages[i] + Environment.NewLine;
                    chatBox.Text = EnableChat ? chatBox.Text : "Чат неактивен.";
                }
            }
        }
        public string Message
        {
            get => textBox.Text; set
            {
                if (EnableChat)
                    textBox.Text = value;
            }
        }
        public bool Broadcast { get => modeBox.SelectedIndex != 0; set => modeBox.SelectedIndex = !value ? 0 : 1; }
        public bool EnableChat
        {
            get => messageLabel.Enabled;
            set
            {
                chatLabel.Enabled = chatBox.Enabled = messageLabel.Enabled = modeBox.Enabled = textBox.Enabled = modeLabel.Enabled = sendButton.Enabled = chatLabel.Enabled = value;
                chatBox.Text = value ? chatBox.Text : "Чат неактивен.";
            }
        }

        public event ViewActionHandler<IChatView> Shown;
        public event ViewActionHandler<IChatView> MessageSended;
        public event ViewActionHandler<IChatView> Hiden;

        public ChatControl()
        {
            InitializeComponent();
            Broadcast = false;
        }

        public new void Show()
        {
            base.Show();
            Shown?.Invoke(this);
        }
        public new void Hide()
        {
            base.Hide();
            Hiden?.Invoke(this);
        }
        public void Close() => Dispose();

        public void Invoke(Action action)
        {
            try
            {
                base.Invoke(action);
            }
            catch { }
        }
        private void SendButtonClick(object sender, EventArgs e)
        {
            MessageSended?.Invoke(this);
            textBox.Text = string.Empty;
            sendButton.Enabled = textBox.Text.Length != 0;
        }

        private void TextBoxTextChanged(object sender, EventArgs e)
        {
            sendButton.Enabled = textBox.Text.Length != 0;
        }
    }
}
