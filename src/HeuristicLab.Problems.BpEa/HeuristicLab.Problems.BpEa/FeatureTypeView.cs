using HeuristicLab.Core.Views;
using HeuristicLab.MainForm;

namespace HeuristicLab.Problems.BpEa
{
    [View("FeatureType View")]
    [Content(typeof(FeatureType), true)]
    public partial class FeatureTypeView : ItemView
    {
        public new FeatureType Content {
            get { return (FeatureType)base.Content; }
            set { base.Content = value; }
        }

        public override bool ReadOnly {
            get
            {
                return base.ReadOnly;
            }
            set { base.ReadOnly = value; }
        }

        public FeatureTypeView()
        {
            InitializeComponent();
        }

        protected override void OnContentChanged()
        {
            base.OnContentChanged();
            if (Content != null)
            {
                NameView.Text = Content.Name != null ? Content.Name.Value : null;
                MinTextBox.Text = Content.Min != null ? Content.Min.Value.ToString() : null;
                MaxTextBox.Text = Content.Max != null ? Content.Max.Value.ToString() : null;
            }
            else
            {
                NameView.Text = null;
                MinTextBox.Text = null;
                MaxTextBox.Text = null;
            }
        }

        protected override void SetEnabledStateOfControls()
        {
            base.SetEnabledStateOfControls();
            NameView.Enabled = Content != null;
            MinTextBox.Enabled = Content != null;
            MaxTextBox.Enabled = Content != null;
        }
    }
}

