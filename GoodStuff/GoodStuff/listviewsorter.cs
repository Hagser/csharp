using System;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

namespace GoodStuff
{
    /// <summary>
    /// Summary description for ListViewItemComparer.
    /// </summary>
    public class ListViewItemComparer : IComparer
    {
        private int _col;
        private SortOrder _order;
        System.TypeCode _type = System.TypeCode.String;

        public ListViewItemComparer()
        {
            _col = 0;
            _order = SortOrder.Ascending;
        }
        public ListViewItemComparer(int column, SortOrder order)
        {
            _col = column;
            _order = order;
        }
        public ListViewItemComparer(int column, SortOrder order, System.TypeCode in_type)
        {
            _col = column;
            _order = order;
            _type = in_type;
        }
        public int Compare(object x, object y)
        {
            int returnVal = -1;
            ListViewItem lvix = (ListViewItem)x;
            ListViewItem lviy = (ListViewItem)y;
            try
            {
                switch (_type)
                {

                    case System.TypeCode.String:
                        if (lvix.SubItems[_col].Tag != null && lviy.SubItems[_col].Tag != null)
                        {
                            returnVal = String.Compare(lvix.SubItems[_col].Tag.ToString(),
                                lviy.SubItems[_col].Tag.ToString());
                        }
                        else
                        {
                            returnVal = String.Compare(lvix.SubItems[_col].Text,
                                lviy.SubItems[_col].Text);
                        }
                        break;
                    case System.TypeCode.Int32:

                        if (lvix.SubItems[_col].Tag != null && lviy.SubItems[_col].Tag != null)
                        {
                            if (int.Parse(lvix.SubItems[_col].Tag.ToString()) > int.Parse(lviy.SubItems[_col].Tag.ToString()))
                            {
                                returnVal = 1;
                            }
                        }
                        else
                        {
                            if (int.Parse(lvix.SubItems[_col].Text) > int.Parse(lviy.SubItems[_col].Text))
                            {
                                returnVal = 1;
                            }
                        }


                        break;
                    case System.TypeCode.Int64:

                        if (lvix.SubItems[_col].Tag != null && lviy.SubItems[_col].Tag != null)
                        {
                            if (Int64.Parse(lvix.SubItems[_col].Tag.ToString()) > Int64.Parse(lviy.SubItems[_col].Tag.ToString()))
                            {
                                returnVal = 1;
                            }
                        }
                        else
                        {
                            if (Int64.Parse(lvix.SubItems[_col].Text) > Int64.Parse(lviy.SubItems[_col].Text))
                            {
                                returnVal = 1;
                            }
                        }


                        break;
                    case System.TypeCode.DateTime:
                        DateTimeConverter dtc = new DateTimeConverter();
                        DateTime dtStartDate = (DateTime)dtc.ConvertFromString(lvix.SubItems[_col].Text);
                        DateTime dtEndDate = (DateTime)dtc.ConvertFromString(lviy.SubItems[_col].Text);
                        returnVal = dtStartDate.CompareTo(dtEndDate);

                        break;
                    default:
                        if (lvix.SubItems[_col].Tag != null && lviy.SubItems[_col].Tag != null)
                        {
                            returnVal = String.Compare(lvix.SubItems[_col].Tag.ToString(),
                                lviy.SubItems[_col].Tag.ToString());
                        }
                        else
                        {
                            returnVal = String.Compare(lvix.SubItems[_col].Text,
                                lviy.SubItems[_col].Text);
                        }
                        break;
                }

                // Determine whether the sort order is descending.
                if (_order == SortOrder.Descending)
                    // Invert the value returned by String.Compare.
                    returnVal *= -1;
            }
            catch
            { }
            return returnVal;
        }
    }
}
