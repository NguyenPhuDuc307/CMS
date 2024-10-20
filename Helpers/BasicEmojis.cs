namespace CMS.Helpers
{
    public class BasicEmojis
    {
        public static string ParseEmojis(string content)
        {
            var emojis = new Dictionary<string, string>
            {
                { ":grinning:", "😁" },
                { ":smiling_face_with_heart_eyes:", "😍" },
                { ":face_with_tears_of_joy:", "😂" },
                { ":smiling_face_with_open_mouth:", "😃" },
                { ":grinning_face:", "😄" },
                { ":kissing_face:", "😘" },
                { ":heart_eyes:", "😍" },
                { ":smiling_face_with_smiling_eyes:", "😊" },
                { ":face_savoring_delicious_food:", "😋" },
                { ":relieved_face:", "😌" },
                { ":smiling_face_with_closed_eyes:", "😊" },
                { ":heart:", "❤️" },
                { ":broken_heart:", "💔" },
                { ":two_hearts:", "❤️❤️" },
                { ":growing_heart:", "💗" },
                { ":beating_heart:", "💖" },
                { ":ok_hand:", "👌" },
                { ":clap:", "👏" },
                { ":waving_hand:", "👋" },
                { ":v:", "✌️" },
                { ":muscle:", "💪" },
                { ":metal:", "🤘" },
                { ":prayer_hands:", "🙏" },
                { ":point_up:", "☝️" },
                { ":point_down:", "👇" },
                { ":point_left:", "👈" },
                { ":point_right:", "👉" },
                { ":point_up_2:", "👆" },
                { ":middle_finger:", "🖕" },
                { ":vulcan_salute:", "🖖" },
                { ":alien:", "👽" },
                { ":robot:", "🤖" },
                { ":smiling_face_with_3_hearts:", "😘" },
                { ":heart_with_arrow:", "💘" },
                { ":heartpulse:", "💓" },
                { ":sparkles:", "✨" },
                { ":star_2:", "⭐️" },
                { ":dizzy:", "😵" },
                { ":punch:", "🥊" },
                { ":fist:", "🤜" },
                { ":handshake:", "🤝" },
                { ":nail_care:", "💅" },
                { ":lips:", "👄" },
                { ":tongue:", "👅" },
                { ":ear:", "👂" },
                { ":eyes:", "👀" },
                { ":nose:", "👃" },
                { ":mouth:", "🤐" },
                { ":tired_face:", "😓" },
                { ":sleeping:", "😴" },
                { ":relieved:", "😌" },
                { ":sweat_smile:", "😅" },
                { ":weary:", "😩" },
                { ":pensive:", "😔" },
                { ":confused:", "😕" },
                { ":confounded:", "😖" },
                { ":kissing:", "😘" },
                { ":heart_eyes_cat:", "😻" },
                { ":smirk_cat:", "😼" },
                { ":smiling_cat_with_heart_eyes:", "😻" },
                { ":cat_face:", "🐱" },
                { ":dog:", "🐶" },
                { ":monkey_face:", "🐒" },
                { ":rooster:", "🐓" },
                { ":turtle:", "🐢" },
                { ":honeybee:", "🐝" },
                { ":lady_beetle:", "🐞" },
                { ":fish:", "🐟" },
                { ":tropical_fish:", "🐠" },
                { ":blowfish:", "🐡" },
                { ":octopus:", "🐙" },
                { ":crab:", "🦀" },
                { ":shrimp:", "🦐" },
                { ":squid:", "🦑" },
                { ":butterfly:", "🦋" },
                { ":deer:", "🦌" },
                { ":gorilla:", "🦍" },
                { ":lizard:", "🦎" },
                { ":snake:", "🦎" },
                { ":turtle_dove:", "🦆" },
                { ":eagle:", "🦅" },
                { ":duck:", "🦆" },
                { ":bat:", "🦇" },
                { ":honey_pot:", "🍯" },
                { ":pot_of_gold:", "🏵" },
                { ":money_bag:", "💸" },
                { ":yen_banknote:", "💴" },
                { ":dollar_banknote:", "💸" },
                { ":pound_banknote:", "💷" },
                { ":euro_banknote:", "💶" },
                { ":credit_card:", "💳" },
                { ":money_with_wings:", "💸" },
                { ":inbox_tray:", "📥" },
                { ":outbox_tray:", "📤" },
                { ":envelope:", "✉️" },
                { ":email:", "✉️" },
                { ":incoming_envelope:", "📨" },
                { ":envelope_with_arrow:", "📨" },
                { ":mailbox:", "📫" },
                { ":postbox:", "📦" },
                { ":package:", "📦" },
                { ":label:", "📝" },
                { ":mailbox_closed:", "📨" },
                { ":mailbox_with_mail:", "📨" },
                { ":mailbox_with_no_mail:", "📨" },
                { ":postbox_closed:", "📨" },
                { ":package_with_bow:", "🎁" },
                { ":closed_lock_with_key:", "🔒" },
                { ":key:", "🔑" },
                { ":old_key:", "🔑" },
                { ":lock:", "🔒" },
                { ":unlock:", "🔓" },
                { ":lock_with_inbox:", "📨" },
                { ":closed_lock:", "🔒" },
                { ":envelope_with_inbox:", "📨" }
            };

            foreach (var emoji in emojis)
            {
                content = content.Replace(emoji.Key, emoji.Value);
            }
            return content;
        }
    }
}
