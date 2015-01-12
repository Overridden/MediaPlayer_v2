using System;
using System.Drawing;
using System.Windows.Forms;

namespace WpfApplication1
{
    public partial class TreeView_manager : Form
    {
        private System.Windows.Forms.TreeView main_treeview;
        private System.Windows.Forms.Button showCheckedNodesButton;
        private TreeViewCancelEventHandler checkForCheckedChildren;

        public void initialize()
        {
            // INITIALIZING
            main_treeview = new System.Windows.Forms.TreeView();
            showCheckedNodesButton = new Button();
            checkForCheckedChildren = new TreeViewCancelEventHandler(CheckForCheckedChildrenHandler);

            this.SuspendLayout();
            main_treeview.Location = new Point(0, 24);
            main_treeview.Size = new Size(250, 250);
            main_treeview.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            main_treeview.CheckBoxes = true;

            // ADD NODE
            TreeNode node;
            for (int x = 0; x < 3; ++x)
            {
                // Add a root node.
                node = main_treeview.Nodes.Add(String.Format("Node{0}", x * 4));
                for (int y = 1; y < 4; ++y)
                {
                    // Add a node as a child of the previously added node.
                    node = node.Nodes.Add(String.Format("Node{0}", x * 4 + y));
                }
            }

            main_treeview.Nodes[1].Nodes[0].Nodes[0].Checked = true;

            // Initialize showCheckedNodesButton.
            showCheckedNodesButton.Size = new Size(144, 24);
            showCheckedNodesButton.Text = "Show Checked Nodes";
            showCheckedNodesButton.Click +=
                new EventHandler(showCheckedNodesButton_Click);

            // Initialize the form.
            this.ClientSize = new Size(292, 273);
            this.Controls.AddRange(new System.Windows.Forms.Control[] { showCheckedNodesButton, main_treeview });

            this.ResumeLayout(false);
        }

        private void showCheckedNodesButton_Click(object sender, EventArgs e)
        {
            // Disable redrawing of treeView1 to prevent flickering 
            // while changes are made.
            main_treeview.BeginUpdate();

            // Collapse all nodes of treeView1.
            main_treeview.ExpandAll();

            // Add the checkForCheckedChildren event handler to the BeforeExpand event.
            main_treeview.BeforeCollapse += checkForCheckedChildren;

            // Expand all nodes of main_treeview. Nodes without checked children are 
            // prevented from expanding by the checkForCheckedChildren event handler.
            main_treeview.CollapseAll();

            // Remove the checkForCheckedChildren event handler from the BeforeExpand 
            // event so manual node expansion will work correctly.
            main_treeview.BeforeCollapse -= checkForCheckedChildren;

            // Enable redrawing of main_treeview.
            main_treeview.EndUpdate();
        }

        // Prevent collapse of a node that has checked child nodes.
        private void CheckForCheckedChildrenHandler(object sender,
            TreeViewCancelEventArgs e)
        {
            if (HasCheckedChildNodes(e.Node)) e.Cancel = true;
        }

        // Returns a value indicating whether the specified 
        // TreeNode has checked child nodes.
        private bool HasCheckedChildNodes(TreeNode node)
        {
            if (node.Nodes.Count == 0) return false;
            foreach (TreeNode childNode in node.Nodes)
            {
                if (childNode.Checked) return true;
                // Recursively check the children of the current child node.
                if (HasCheckedChildNodes(childNode)) return true;
            }
            return false;
        }
    }
}
