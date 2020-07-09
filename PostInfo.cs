using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VK_API
{
    public class PostInfo
    {
        public int id;
        public DateTime dateTime;
        public string text;

        public PostInfo(int id, DateTime dateTime, string text)
        {
            this.id = id;
            this.dateTime = dateTime;
            this.text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public override string ToString()
        {
            return String.Format("Пост № {0} от {1}\n{2}",
                id,
                dateTime.ToString("dd.MM.yyyy"),
                text.Substring(0, 100));
        }
    }
}
