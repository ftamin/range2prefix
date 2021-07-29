using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Range2Prefix
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        public List<int> CreatePrefix(int rangeStart, int rangeEnd)
        {
            var resultList = new List<int>();

            var charactersLengthDifference = rangeEnd.ToString().Length - rangeStart.ToString().Length;

            if (charactersLengthDifference > 0)
            {
                rangeStart = int.Parse(rangeStart.ToString().PadRight(rangeStart.ToString().Length + charactersLengthDifference, '0'));
            }

            // rangeEnd < rangeStart?
            if (rangeEnd < rangeStart)
            {
                return resultList;
            }

            // rangeEnd == rangeStart?
            if (rangeEnd == rangeStart)
            {
                resultList.Add(rangeStart);
                return resultList;
            }

            // now we check if rangeEnd is in the same order, ie tens, not hundreds or thousands
            int rangeDiff = rangeEnd - rangeStart;
            int rangeOrder = CalculateOrder(rangeDiff);

            // is rangeStart on the bottom range, ie ends with "0" instead of other numbers between 1 and 9
            int bottomRange = CalculateBottomRange(rangeStart, rangeOrder);
            int upperRange;

            if (rangeStart == bottomRange)
            {
                // rangeStart is on the bottom, good chance of simply removing the last digit
                
                // now we check if the rangeEnd is on the top range, 
                // ie ends with "9" instead of other numbers between 1 and 8
                // end with "99" instead of other numbers between 01 and 98
                // depends range difference order (tens, hundreds, thousands)
                upperRange = CalculateUpperRange(rangeEnd, rangeOrder); 

                if (rangeEnd == upperRange)
                {
                    // Final result: just need to cut last rangeOrder number of characters of rangeStart
                    var lengthToCut = rangeStart.ToString().Length - rangeOrder;
                    if (lengthToCut < 1)
                    {
                        resultList.Add(rangeStart);
                        return resultList;
                    }
                    resultList.Add(int.Parse(rangeStart.ToString().Substring(0, lengthToCut)));
                    return resultList;
                }

                //rangeEnd is not on the upperRange

                //see if the rangeOrder is 1, ie it's between rangeStart is xx0 and rangeEnd is between xx1 and xx8
                if (rangeOrder == 1)
                {
                    resultList.AddRange(DistributeRange(rangeStart, rangeEnd));
                    return resultList;
                }

                // reduce the rangeOrder and find a new upperRange:
                rangeOrder--;
                upperRange = CalculateBottomRange(rangeEnd, rangeOrder);

                //rangeOrder is greater than 1, the difference between rangeStart and rangeEnd is in the tens (or hundreds, or thousands etc)
                while (upperRange < rangeStart || upperRange > rangeEnd)
                {
                    rangeOrder--;
                    upperRange = CalculateBottomRange(rangeEnd, rangeOrder);
                }

                resultList.AddRange(CreatePrefix(rangeStart, upperRange-1));
                resultList.AddRange(CreatePrefix(upperRange, rangeEnd));
                return resultList;
            }

            // rangeStart is NOT on the bottom

            //see if the rangeOrder is 1, ie it's between rangeStart is xx0 and rangeEnd is between xx1 and xx8
            if (rangeOrder == 1)
            {
                resultList.AddRange(DistributeRange(rangeStart, rangeEnd));
                return resultList;
            }

            // reduce the rangeOrder and find a new upperRange:
            rangeOrder--;
            upperRange = CalculateBottomRange(rangeEnd, rangeOrder);

            //rangeOrder is greater than 1, the difference between rangeStart and rangeEnd is in the tens (or hundreds, or thousands etc)
            while (upperRange < rangeStart)
            {
                rangeOrder--;
                upperRange = CalculateBottomRange(rangeEnd, rangeOrder);
            }

            resultList.AddRange(CreatePrefix(rangeStart, upperRange - 1));
            resultList.AddRange(CreatePrefix(upperRange, rangeEnd));
            return resultList;

        }

        private List<int> DistributeRange (int rangeStart, int rangeEnd)
        {
            var resultList = new List<int>();
            var diff = rangeEnd - rangeStart;

            for (int i = 0; i <= diff; i++)
            {
                resultList.Add(rangeStart + i);
            }
            return resultList;
        }

        private int CalculateOrder(int value)
        {
            var order = (Math.Log(value) / Math.Log(10));

            return (int)Math.Floor(order) + 1;
        }

        private int CalculateBottomRange(int value, int order)
        {
            double power = Math.Pow(10, order);

            return ((int)(Math.Floor(value / power) * power));
        }

        private int CalculateUpperRange(int value, int order)
        {
            double power = Math.Pow(10, order);

            return ((int)(Math.Ceiling(value / power) * power)) - 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var resultList = CreatePrefix((int.Parse(txtRangeStart.Text)), int.Parse(txtRangeEnd.Text));
            txtResult.Text = string.Join(Environment.NewLine, resultList);
        }
    }
}
