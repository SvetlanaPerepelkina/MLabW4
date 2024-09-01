using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace L4
{
    internal class Values : IDataErrorInfo
    {
        public int start { get; set; }
        public int stop { get; set; }
        public int N { get; set; }

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "start":
                        if((start < 0) || (start > 100))
                        {
                            error = "Стартовое значение предпочтительно в диапазоне от 0 до 100";
                            MessageBox.Show(error);
                        }
                        break;
                    case "stop":
                        if (stop < start)
                        {
                            error = "Это значение не может быть меньше стортового";
                            MessageBox.Show(error);
                        }
                        break;
                    case "N":
                        if ((start < 0) || (start > 100))
                        {
                            error = "Это значение не может быть отрицательным";
                            MessageBox.Show(error);
                        }
                        break;
                }
                return error;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public Values() { }

    }

}
