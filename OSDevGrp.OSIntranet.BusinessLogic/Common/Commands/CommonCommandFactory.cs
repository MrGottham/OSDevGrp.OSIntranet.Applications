using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Commands
{
    public static class CommonCommandFactory
    {
        #region Methods

        public static ICreateNationalityCommand BuildCreateNationalityCommand(int number, string name)
        {
            return new CreateNationalityCommand(number, name);
        }

        public static IUpdateNationalityCommand BuildUpdateNationalityCommand(int number, string name)
        {
            return new UpdateNationalityCommand(number, name);
        }

        public static IDeleteNationalityCommand BuildDeleteNationalityCommand(int number)
        {
            return new DeleteNationalityCommand(number);
        }

        public static ICreateLanguageCommand BuildCreateLanguageCommand(int number, string name)
        {
            return new CreateLanguageCommand(number, name);
        }

        public static IUpdateLanguageCommand BuildUpdateLanguageCommand(int number, string name)
        {
            return new UpdateLanguageCommand(number, name);
        }

        public static IDeleteLanguageCommand BuildDeleteLanguageCommand(int number)
        {
            return new DeleteLanguageCommand(number);
        }

        #endregion
    }
}