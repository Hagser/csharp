using System;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

namespace ClipboardCatcher
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
			_col=0;
			_order = SortOrder.Ascending;
		}
		public ListViewItemComparer(int column, SortOrder order) 
		{
			_col=column;
			_order = order;
		}
		public ListViewItemComparer(int column, SortOrder order,System.TypeCode in_type) 
		{
			_col=column;
			_order = order;
			_type = in_type;
		}
		public int Compare(object x, object y) 
		{
			int returnVal= -1;
			try
			{
				switch(_type)
				{
					case System.TypeCode.String:
						returnVal = String.Compare(((ListViewItem)x).SubItems[_col].Text,
							((ListViewItem)y).SubItems[_col].Text);
						break;
					case System.TypeCode.Int32:
						if(int.Parse(((ListViewItem)x).SubItems[_col].Text)>int.Parse(((ListViewItem)y).SubItems[_col].Text))
						{
							returnVal = 1;
						}
							
						break;
					case System.TypeCode.DateTime:
						DateTimeConverter dtc = new DateTimeConverter();
						DateTime dtStartDate=(DateTime)dtc.ConvertFromString(((ListViewItem)x).SubItems[_col].Text);
						DateTime dtEndDate=(DateTime)dtc.ConvertFromString(((ListViewItem)y).SubItems[_col].Text);
						returnVal = dtStartDate.CompareTo(dtEndDate);

						break;
					default:
						returnVal = String.Compare(((ListViewItem)x).SubItems[_col].Text,
							((ListViewItem)y).SubItems[_col].Text);
					break;
				}

				// Determine whether the sort order is descending.
				if(_order == SortOrder.Descending)
					// Invert the value returned by String.Compare.
					returnVal *= -1;
			}
			catch
			{}
			return returnVal;
		}
	}
}
