using System;
using System.Text.RegularExpressions;

namespace ConsolePokemonStyleFight.Entities
{
    public class SizedString
    {
        public static int GetAnsiCharCount(string subject)
        {
            var replaced = Regex.Replace(subject, @"\e\[(\d+;)*(\d+)?[ABCDHJKfmsu]", "");
            return subject.Length - replaced.Length;
        }
        
        public static string Reverse(string subject)
        {
            char[] charArray = subject.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        
        public static string Max(string subject, int size, Alignment alignment)
        {
            var ansiCount = GetAnsiCharCount(subject);
            if (subject.Length - ansiCount <= size) return subject;
            switch (alignment)
            {
                case Alignment.Center:
                    for (var i = subject.Length - ansiCount; i > size; i --)
                    {
                        if (i % 2 == 0) subject = subject.Substring(0, subject.Length - 1);
                        else subject = subject.Substring(1);
                    }
                    break;
                case Alignment.Left:
                    subject = subject.Substring(0, size);
                    break;
                case Alignment.Right:
                    subject = subject.Substring(subject.Length - size);
                    break;
            }
            
            return subject;
        }

        public static string Min(string subject, int size, Alignment alignment, char fill = ' ')
        {
            var ansiCount = GetAnsiCharCount(subject);
            if (subject.Length - ansiCount >= size) return subject;
            switch (alignment)
            {
                case Alignment.Center:
                    for (var i = 0; i < size - (subject.Length - ansiCount); i ++)
                    {
                        if (i % 2 == 0) subject += fill;
                        else subject = fill + subject;
                    }
                    break;
                case Alignment.Left:
                    subject += new String(fill, size - (subject.Length - ansiCount));
                    break;
                case Alignment.Right:
                    subject = new String(fill, size - (subject.Length - ansiCount)) + subject;
                    break;
            }
            return subject;
        }

        public static string Constrain(string subject, int size, Alignment alignment, char fill = ' ')
        {
            return Min(Max(subject, size, alignment), size, alignment, fill);
        }
        
        public string Content;
        public int Size;
        public Alignment Align;
        public char Fill;

        public enum  Alignment
        {
            Center,
            Right,
            Left
        }
        
        public SizedString(string content, int size, Alignment alignment, char fill = ' ')
        {
            Content = content;
            Size = size;
            Align = alignment;
            Fill = fill;
        }

        public new string ToString()
        {
            return Constrain(Content, Size, Align, Fill);
        }
    }
}