using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YLHR
{
    class CommonTools
    {
        public class Item
        {
            public string Text;
            public int Value;

            public Item(int id, String name)
            {
                this.Value = id;
                this.Text = name;
            }

            public override string ToString()
            {
                return Text;
            }
        }

        public class Season
        {
            public string Text;
            public char Value;

            public Season(String s)
            {
                this.Text = s;
                this.Value = s[0];
            }

            public override string ToString()
            {
                return Text;
            }
        }

        public static String findItem(System.Windows.Forms.ComboBox.ObjectCollection items, int id)
        {
            foreach (Item item in items)
            {
                if (item.Value == id) { return item.Text; }
            }
            return "";
        }
    }
}
