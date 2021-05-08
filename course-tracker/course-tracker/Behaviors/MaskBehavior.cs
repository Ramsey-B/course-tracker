using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace course_tracker.Behaviors
{
    public class MaskBehavior : Behavior<Entry>
    {
        private string _mask = "";
        public string Mask
        {
            get => _mask;
            set
            {
                _mask = value;
                SetPositions();
            }
        }
        Dictionary<int, char> _positions;

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }


        void SetPositions()
        {
            if (string.IsNullOrEmpty(Mask))
            {
                _positions = null;
                return;
            }

            // create dictionary of the position of all the non-mask chars
            var list = new Dictionary<int, char>();
            for (var i = 0; i < Mask.Length; i++)
            {
                if (Mask[i] != 'X' && Mask[i] != 'x') list.Add(i, Mask[i]);
            }

            _positions = list;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var entry = sender as Entry;

            var text = entry.Text;

            // no text to change
            if (string.IsNullOrWhiteSpace(text) || _positions == null) return;

            // prevents over filling the mask
            if (text.Length > _mask.Length)
            {
                entry.Text = text.Remove(text.Length - 1);
                return;
            }

            // inserts the non-mask chars into the text
            foreach (var position in _positions)
            {
                // determines if a non-mask char needs to be added
                if (text.Length >= position.Key + 1)
                {
                    var value = position.Value.ToString();
                    // adds the non-mask char if it isn't already added
                    if (text.Substring(position.Key, 1) != value)
                    {
                        text = text.Insert(position.Key, value);
                    }
                }
            }

            // sets the text to the new text with its correct format
            if (entry.Text != text) entry.Text = text;
        }
    }
}
