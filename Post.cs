using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VK_API
{
    public class Post
    {
        public int id;
        public int owner_id;
        public int from_id;
        public int created_by;
        public int date;
        public string text;
        public int reply_owner_id;
        public int reply_post_id;
        public int friends_only;
        public object comments;
        public object likes;
        public object reposts;
        public object views;
        public string post_type;
        public object post_source;
        public object attachments;
        public object geo;
        public int signer_id;
        public object copy_history;
        public int can_pin;
        public int can_delete;
        public int can_edit;
        public int is_pinned;
        public int marked_as_ads;
        public bool is_favorite;
        public int postponed_id;

        public override string ToString()
        {
            return id.ToString();
        }
    }
}
