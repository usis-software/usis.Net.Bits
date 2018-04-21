//
//  @(#) JobPropertiesControl.cs
//
//  Project:    usis BITS Administration
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System;
using System.Globalization;
using System.Windows.Forms;
using usis.Platform;

namespace usis.Net.Bits.Administration
{
    //  --------------------------
    //  JobPropertiesControl class
    //  --------------------------

    internal sealed partial class JobPropertiesControl : UserControl, IInjectable<JobNode>, IInjectable<PropertyPage>
    {
        #region fields

        private bool initialized;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        public JobPropertiesControl()
        {
            InitializeComponent();

            comboBoxPriority.DataSource = Enum.GetValues(typeof(BackgroundCopyJobPriority));
            numericUpDownMinimumRetryDelay.Maximum = int.MaxValue;
            numericUpDownNoProgressTimeout.Maximum = int.MaxValue;
        }

        #endregion construction

        #region IInjectable<JobNode> implementation

        //  -------------
        //  Inject method
        //  -------------

        void IInjectable<JobNode>.Inject(JobNode node)
        {
            if (node != null)
            {
                node.Updated += (sender, e) => { Update(node.Job); };
                Initialize(node.Job);
            }
        }

        #endregion IInjectable<JobNode> implementation

        #region IInjectable<PropertyPage> implementation

        //  ---------------------
        //  PropertyPage property
        //  ---------------------

        public PropertyPage PropertyPage { get; private set; }

        //  -------------
        //  Inject method
        //  -------------

        void IInjectable<PropertyPage>.Inject(PropertyPage dependency) { PropertyPage = dependency; }

        #endregion IInjectable<PropertyPage> implementation

        #region methods

        //  -----------------
        //  Initialize method
        //  -----------------

        private void Initialize(BackgroundCopyJob job)
        {
            textBoxDisplayName.Text = job.DisplayName;
            textBoxDescription.Text = job.Description;
            comboBoxPriority.SelectedItem = job.Priority;
            numericUpDownMinimumRetryDelay.Value = job.MinimumRetryDelay;
            numericUpDownMinimumRetryDelay.Select(0, int.MaxValue);
            numericUpDownNoProgressTimeout.Value = job.NoProgressTimeout;
            numericUpDownNoProgressTimeout.Select(0, int.MaxValue);

            if (PropertyPage is ManagementConsole.PropertyPage propertyPage)
            {
                propertyPage.RecordPropertyValue(comboBoxPriority.Tag as string, comboBoxPriority.SelectedValue);
            }

            Update(job);
            initialized = true;
        }

        //  -------------
        //  Update method
        //  -------------

        private void Update(BackgroundCopyJob job)
        {
            textBoxId.Text = job.Id.ToString();
            textBoxType.Text = job.JobType.ToString();
            textBoxOwner.Text = JobNode.AccountNameFromSidString(job.Owner);
            textBoxState.Text = job.State.ToString();
            if (job.ErrorCount == 0) textBoxErrorCount.Text = job.ErrorCount.ToString(CultureInfo.CurrentCulture);
            else
            {
                textBoxErrorCount.Text = string.Format(CultureInfo.CurrentCulture, "{0} - Last Error: {1}", job.ErrorCount, job.GetErrorInfo()?.Description);
                toolTip.SetToolTip(textBoxErrorCount, textBoxErrorCount.Text);
            }

            if (job.State == BackgroundCopyJobState.Canceled)
            {
                textBoxDisplayName.ReadOnly = true;
                textBoxDescription.ReadOnly = true;
                comboBoxPriority.DataSource = new[] { job.State };
                comboBoxPriority.Enabled = false;
                numericUpDownMinimumRetryDelay.Enabled = false;
                numericUpDownNoProgressTimeout.Enabled = false;
            }
        }

        //  --------------
        //  Changed method
        //  --------------

        private void Changed(object sender, EventArgs e)
        {
            if (!initialized) return;
            if (PropertyPage is ManagementConsole.PropertyPage propertyPage)
            {
                if (sender is Control control && control.Tag is string name && !string.IsNullOrWhiteSpace(name))
                {
                    if (sender is TextBox textBox)
                    {
                        propertyPage.RecordChangedProperty(name, textBox.Text);
                    }
                    else if (sender is ComboBox comboBox)
                    {
                        propertyPage.RecordChangedProperty(name, comboBox.SelectedValue);
                    }
                    else if (sender is NumericUpDown numericUpDown)
                    {
                        propertyPage.RecordChangedProperty(name, numericUpDown.Value);
                    }
                    else return;
                }
            }
        }

        #endregion methods
    }
}

// eof "JobPropertiesControl.cs"
