using Enemies.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Enemies.Parameters
{
    public class Message
    {
        public int Sender { get; private set; }
        public int Receiver { get; private set; }
        public string Text { get; private set; }
        public object Attachment { get; private set; }

        public Message(int sender, int receiver, string message, object attachment = null)
        {
            Sender = sender;
            Receiver = receiver;
            Text = message;
            Attachment = attachment;
        }

        public Message(int sender, string message, object attachment = null) : this(sender, -1, message, attachment)
        {
        }
    }
}
