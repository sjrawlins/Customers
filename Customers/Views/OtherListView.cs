using System;
using System.Collections.Generic;
using System.Linq;

using iFactr.Core;
using iFactr.Core.Utilities;
using iFactr.UI;
using iFactr.UI.Controls;
using Customers.ViewModels;

namespace Customers.Views
{
    // NOTE:  This View was for class demonstration purposes only.  Serves no function.
    //
    // TODO: Change the generic type to the type of your model.
    class OtherListView : ListView<CustomerViewModel>
    {
        /// <summary>
        /// Called when the view is ready to be rendered.
        /// </summary>
        protected override void OnRender()
        {
            // TODO: Set the item count to the desired number of cells in the list.
            Sections[0].ItemCount = 1;
        }

        /// <summary>
        /// Called when a cell is ready to be rendered on the screen.  Return the ICell instance
        /// that should be placed at the given index within the given section.
        /// </summary>
        /// <param name="section">The section that will contain the cell.</param>
        /// <param name="index">The index at which the cell will be placed in the section.</param>
        /// <param name="recycledCell">An already instantiated cell that is ready for reuse, or null if no cell has been recycled.</param>
        protected override ICell OnCellRequested(int section, int index, ICell recycledCell)
        {
            // On platforms that support cell recycling (Android and iOS), recycledCell is passed-in, otherwise is null.
            // In any case, a cell must be returned.
            var myCell = new GridCell();

            myCell.AddChild(new Label("We are here"));
            return myCell;
        }

        /// <summary>
        /// Called when a reuse identifier is needed for a cell or tile.  Return the identifier that should be used
        /// to determine which cells or tiles may be reused for the item at the given index within the given section.
        /// This is only called on platforms that support recycling.  See Remarks for details.
        /// </summary>
        /// <param name="section">The section that will contain the item.</param>
        /// <param name="index">The index at which the item will be placed in the section.</param>
        protected override int OnItemIdRequested(int section, int index)
        {
            return index;
        }
    }
}
