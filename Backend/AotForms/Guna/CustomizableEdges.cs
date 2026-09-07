using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace AimBotConquer.Guna
{

    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Description("You can choose which edges will be included when styling the border radius")]
    public class CustomizableEdges
    {

        public CustomizableEdges(bool A_1, bool A_2, bool A_3, bool A_4)
        {
            this.TopLeft = A_3;
            this.TopRight = A_4;
            this.BottomLeft = A_1;
            this.BottomRight = A_2;
        }

        public CustomizableEdges()
        {
        }

        private void nqysj4YvYorHGK4MnerCLAgf5c()
        {
            if (this.TwHanDGKMGVJNJSxGLvTA3SQgH != null)
			{
                this.TwHanDGKMGVJNJSxGLvTA3SQgH.Invalidate();
            }
        }

        public override string ToString()
        {
            return string.Concat(new string[]
            {
                this.BottomLeft.ToString(),
                ", ",
                this.BottomRight.ToString(),
                ", ",
                this.TopLeft.ToString(),
                ", ",
                this.TopRight.ToString()
            });
        }

        [DefaultValue(true)]
        public bool TopLeft
        {
            get
            {
                return this.lpGGOlxcGU1eEyQBJJUuHDWMjd6;
            }
            set
            {
                if (this.lpGGOlxcGU1eEyQBJJUuHDWMjd6 == value)
                {
                    return;
                }
                this.lpGGOlxcGU1eEyQBJJUuHDWMjd6 = value;
                this.nqysj4YvYorHGK4MnerCLAgf5c();
            }
        }

        [DefaultValue(true)]
        public bool TopRight
        {
            get
            {
                return this.p8SSF4dTRidxnOBHCmbLPkjnR2;
            }
            set
            {
                if (this.p8SSF4dTRidxnOBHCmbLPkjnR2 == value)
				{
                    return;
                }
                this.p8SSF4dTRidxnOBHCmbLPkjnR2 = value;
                this.nqysj4YvYorHGK4MnerCLAgf5c();
            }
        }

        [DefaultValue(true)]
        public bool BottomLeft
        {
            get
            {
                return this.Eww86ei1jXBuIdPurJ6dPLeK6Ei;
            }
            set
            {
                if (this.Eww86ei1jXBuIdPurJ6dPLeK6Ei == value)
                {
                    return;
                }
                this.Eww86ei1jXBuIdPurJ6dPLeK6Ei = value;
                this.nqysj4YvYorHGK4MnerCLAgf5c();
            }
        }

        [DefaultValue(true)]
        public bool BottomRight
        {
            get
            {
                return this.AbdF560AAlOb17xqEjmXBxzH53O;
            }
            set
            {
                if (this.AbdF560AAlOb17xqEjmXBxzH53O == value)
                {
                    return;
                }
                this.AbdF560AAlOb17xqEjmXBxzH53O = value;
                this.nqysj4YvYorHGK4MnerCLAgf5c();
            }
        }

        public static CustomizableEdges TryParse(string input)
        {
            input = input.Replace(" ", "");
            string[] array = input.Split(new char[] { ',' });
            bool flag;
            bool flag2;
            bool flag3;
            bool flag4;
            if (bool.TryParse(array[0], out flag) && bool.TryParse(array[1], out flag2) && bool.TryParse(array[2], out flag3) && bool.TryParse(array[3], out flag4))
            {
                return new CustomizableEdges(flag3, flag4, flag, flag2);
            }
            return null;
        }

        internal Control TwHanDGKMGVJNJSxGLvTA3SQgH;

		private bool lpGGOlxcGU1eEyQBJJUuHDWMjd6 = true;

        private bool p8SSF4dTRidxnOBHCmbLPkjnR2 = true;

		private bool Eww86ei1jXBuIdPurJ6dPLeK6Ei = true;

        private bool AbdF560AAlOb17xqEjmXBxzH53O = true;
    }
}