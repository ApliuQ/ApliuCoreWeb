using System;
using System.Data.Common;

namespace Apliu.Core.Database
{
    public class DatabaseDataAdapter : DbDataAdapter
    {
        private Int32 updateBatchSize;

        /// <summary>
        /// Gets or sets a command used to select records in the data source.
        /// </summary>
        public new DbCommand SelectCommand { get; set; }

        /// <summary>
        /// Gets or sets a command used to insert new records into the data source.
        /// </summary>
        public new DbCommand InsertCommand { get; set; }

        /// <summary>
        /// Gets or sets a command for deleting records from the data set.
        /// </summary>
        public new DbCommand DeleteCommand { get; set; }

        /// <summary>
        /// Gets or sets a value that enables or disables batch processing support, and specifies the number of commands that can be executed in a batch.
        /// </summary>
        public override Int32 UpdateBatchSize
        {
            get => updateBatchSize;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(UpdateBatchSize));
                updateBatchSize = value;
            }
        }

        /// <summary>
        /// Gets or sets a command used to update records in the data source.
        /// </summary>
        public new DbCommand UpdateCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of a DataAdapter class.
        /// </summary>
        /// <param name="dbCommand"></param>
        public DatabaseDataAdapter(DbCommand dbCommand) : base()
        {
            SelectCommand = dbCommand;
            UpdateBatchSize = 1;
        }
    }
}
