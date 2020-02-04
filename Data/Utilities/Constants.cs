namespace Data.Utilities
{
    public static class Constants
    {
        public static class Common
        {
            public const string OrderBy = "OrderBy";
            public const string ThenBy = "ThenBy";
            public const string ModelStateErrors = "ModelStateErrors";
            public const string IdentityErrors = "IdentityErrors";
            public const string Xoauth2 = "XOAUTH2";
            public const string ModalMessage = "ModalMessage";
            public const string ModalTitle = "ModalTitle";
            public const string ModalSize = "ModalSize";
            public const string Previous = "prev";
            public const string Next = "next";
            public const string Value = "Value";
            public const string Text = "Text";
        }

        public static class AppConfig
        {
            public const string ConnectionString = "DefaultConnection";
            public const string MigrationAssembly = "Data";
        }

        public static class Message
        {
            //Modal Title
            public const string Info = "Info";
            public const string Warning = "Warning";
            public const string Error = "Error";
            public const string Confirm = "Confirm";

            //Common message
            public const string DeletePrompt = "Are you sure you want to delete the '{0}' record?";

            //Success message
            public const string RecordSuccessAdd = "Record successfully added.";
            public const string RecordSuccessUpdate = "Record successfully updated.";
            public const string RecordSuccessDelete = "Record successfully deleted.";

            //Error message
            public const string ErrorProcessing = "An error was encountered while processing the request.";
            public const string ErrorRecordExists = "Record already exists.";
            public const string ErrorRecordNotExists = "Record does not exist.";
            public const string ErrorRecordInUse = "This record is currently in use.";
            public const string ErrorRecordInvalid = "Record is not valid.";
            public const string ErrorCategoryRecordExists = "Category value does not exists.";

            //Exception Error message
            public const string ModelException = "A model issue occured while processing your request. Please contact administrator.";
            public const string NetworkTransportException = "A network transport issue occured while processing your request. Please contact administrator.";
            public const string DatabaseException = "A database connection issue occured while processing your request. Please contact administrator.";
            public const string DirectoryNotFoundException = "A directory is not found while processing your request. Please contact administrator.";
            public const string FileNotFoundException = "A file is not found while processing your request. Please contact administrator.";
            public const string NullException = "A null exception found while processing your request. Please contact administrator.";
            public const string OutOfRangeException = "An out of range exception found while processing your request. Please contact administrator.";
        }

        #region Sorting

        public static class Sort
        {
            public const string SortBy = "SortBy";
            public const string SortOrder = "SortOrder";
            public const string Page = "Page";
        }

        public static class SortDirection
        {
            public const string Ascending = "Ascending";
            public const string Descending = "Descending";
        }

        public static class Category
        {
            public const string Id = "Id";
            public const string Name = "Name";
            public const string RowVersion = "RowVersion";
        }

        public static class Product
        {
            public const string Id = "Id";
            public const string Name = "Name";
            public const string Description = "Description";
            public const string Image = "Image";
            public const string CategoryName = "CategoryName";
            public const string RowVersion = "RowVersion";
        }
        #endregion Sorting
    }
}