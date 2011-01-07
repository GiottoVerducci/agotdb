using System;
using Beyond.ExtendedControls;

namespace GenericDB.Extensions
{
	public static class ExtendedCheckedListBoxExtension
	{
		/// <summary>
		/// Performs an action on the expanded items of the extended checked list box control
		/// by de-condensing it and putting it back to its original state afterwards.
		/// </summary>
		/// <param name="ecl">The extended checked list box control.</param>
		/// <param name="actionToPerform">The action to perform on the extended checked list box control once it is expanded.</param>
		public static void WorkOnExpandedItems(this ExtendedCheckedListBox ecl, Action<ExtendedCheckedListBox> actionToPerform)
		{
			bool condensed = ecl.Condensed; // we'll use it to restore its previous state
			if (condensed)
				ecl.Condensed = false; // expand ecl to get access to the items
			actionToPerform(ecl);
			if (condensed)
				ecl.Condensed = true; // restore condensed state
		}
	}
}
